import {Injectable} from '@angular/core';
import {CanActivate, CanActivateChild, CanLoad, Router} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
	providedIn: 'root',
})
export class AccessLoginPageGuard implements CanActivate, CanLoad, CanActivateChild {
	constructor(private _router: Router) {}
	jwtHelper = new JwtHelperService();
	canActivateChild() {
		return this.Auth();
	}
	canActivate() {
		return this.Auth();
	}
	canLoad() {
		return this.Auth();
	}
	Auth() {
		var token = localStorage.getItem('token');
		if (token && !this.jwtHelper.isTokenExpired(token)) {
			this._router.navigate(['']);
			return false;
		}
		localStorage.clear();
		return true;
	}
}
