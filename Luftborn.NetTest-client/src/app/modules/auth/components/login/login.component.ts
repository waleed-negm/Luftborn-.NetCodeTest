import {Component} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Response} from 'src/app/modules/shared/interfaces/Iresponse';
import {Auth} from '../../interfaces/IAuth';
import {AuthServices} from '../../services/Auth.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css'],
})
export class LoginComponent {
	loginForm: FormGroup;
	hide = true;
	logging: boolean = false;
	constructor(private route: ActivatedRoute, private router: Router, private _login: AuthServices, private fb: FormBuilder, private toastr: ToastrService) {
		this.loginForm = this.fb.group({
			username: ['', [Validators.required, Validators.maxLength(100)]],
			password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
		});
	}
	get username(): FormControl {
		return this.loginForm.get('username') as FormControl;
	}
	get password(): FormControl {
		return this.loginForm.get('password') as FormControl;
	}
	handleRegister() {
		this.router.navigate(['auth/register']);
	}
	handleForgetPassword() {
		this.router.navigate(['auth/forgetPassword']);
	}
	handleSubmit() {
		if (this.loginForm.valid) {
			this.logging = true;
			this._login.login(this.loginForm.value).subscribe({
				next: (data: Response) => {
					let auth: Auth = data.body;
					this._login.setLocalStorage(auth);
					this._login.username.next(auth.userName);
					this._login.isLogged = true;
				},
				error: (e) => {
					this.logging = false;
					this._login.isLogged = false;
					this._login.username.next(null);
					localStorage.clear();
					let res: Response = e.error ?? e;
					this.toastr.error(res.message, 'unauthorized');
				},
				complete: () => {
					this.logging = false;
					this.toastr.success('loged in sucessfully', 'logged in');
					this.router.navigate(['']);
				},
			});
		}
	}
}
