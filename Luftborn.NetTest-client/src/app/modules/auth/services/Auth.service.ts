import {shareReplay, BehaviorSubject} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {Register} from '../interfaces/IRegister';
import {Response} from '../../shared/interfaces/Iresponse';
import {Login} from '../interfaces/Ilogin';
import {Auth} from '../interfaces/IAuth';
import {ResetPassword} from '../interfaces/IResetPassword';
import {User} from '../interfaces/IUser';
import {UpdatePassword} from '../interfaces/IUpdatePassword';

@Injectable({
	providedIn: 'root',
})
export class AuthServices {
	constructor(private http: HttpClient) {}

	public username: BehaviorSubject<any> = new BehaviorSubject(null);
	public isLogged = !!localStorage.getItem('token');
	api_url: string = `${environment.apiUrl}Auth/`;

	login = (login: Login) => this.http.post<Response>(`${this.api_url}Login`, login).pipe(shareReplay());

	register(reister: Register) {
		return this.http.post<Response>(`${this.api_url}register`, reister).pipe(shareReplay());
	}

	forgetPassword(email: string) {
		return this.http.post<Response>(`${this.api_url}forgetPassword?email=${email}`, null);
	}

	resetPassword(model: ResetPassword) {
		return this.http.post<Response>(`${this.api_url}resetPassword`, model);
	}

	changePassword(id: string, model: UpdatePassword) {
		return this.http.put<Response>(`${this.api_url}changePassword?id=${id}`, model);
	}

	confirmEmail(userId: string, token: string) {
		return this.http.get<Response>(`${this.api_url}confirmEmail`, {params: {userid: userId, token: token}});
	}

	getUserById(userId: string) {
		return this.http.get<Response>(`${this.api_url}profile`, {params: {id: userId}});
	}

	UpdateUser(userId: string, model: User) {
		return this.http.put<Response>(`${this.api_url}?id=${userId}`, model);
	}

	public setLocalStorage(auth: Auth) {
		localStorage.setItem('token', auth.token);
		localStorage.setItem('uname', auth.userName);
		localStorage.setItem('uid', auth.userId);
	}
}
