import {Injectable} from '@angular/core';
import {CanActivate, CanActivateChild, CanLoad, Router} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
	providedIn: 'root',
})
export class LoginGuard implements CanActivate, CanLoad, CanActivateChild {
	constructor(private _router: Router) {}
	jwtHelper = new JwtHelperService();
	canActivateChild = () => this.Auth();
	canActivate = () => this.Auth();
	canLoad = () => this.Auth();
	Auth() {
		var token = localStorage.getItem('token');
		if (token && token !== 'undefined' && !this.jwtHelper.isTokenExpired(token)) return true;
		this._router.navigate(['/auth/login']);
		localStorage.clear();
		return false;
	}
}
