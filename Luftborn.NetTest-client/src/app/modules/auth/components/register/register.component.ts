import {Component} from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl} from '@angular/forms';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Subscription} from 'rxjs';
import {Response} from 'src/app/modules/shared/interfaces/Iresponse';
import {CustomValidators} from '../../customeValidators/confirmPassword';
import {AuthServices} from '../../services/Auth.service';
@Component({
	selector: 'app-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
	subscriptions: Subscription[] = [];
	registerForm: FormGroup;
	hide = true;
	hideConfirm = true;
	logging: boolean = false;
	constructor(private router: Router, private _auth: AuthServices, private fb: FormBuilder, private toastr: ToastrService) {
		this.registerForm = this.fb.group(
			{
				firstName: ['', [Validators.required, Validators.maxLength(100)]],
				lastName: ['', [Validators.required, Validators.maxLength(100)]],
				userName: ['', [Validators.required, Validators.maxLength(100)]],
				email: ['', [Validators.required, Validators.email]],
				phoneNumber: ['', [Validators.required, Validators.pattern('01[0125][0-9]{8}')]],
				password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
				confirmPassword: ['', [Validators.required, Validators.maxLength(100)]],
			},
			{validators: CustomValidators.MatchValidator('password', 'confirmPassword')}
		);
	}
	get firstName(): FormControl {
		return this.registerForm.get('firstName') as FormControl;
	}
	get lastName(): FormControl {
		return this.registerForm.get('lastName') as FormControl;
	}
	get userName(): FormControl {
		return this.registerForm.get('userName') as FormControl;
	}
	get email(): FormControl {
		return this.registerForm.get('email') as FormControl;
	}
	get phone(): FormControl {
		return this.registerForm.get('phoneNumber') as FormControl;
	}
	get confirmPassword(): FormControl {
		return this.registerForm.get('confirmPassword') as FormControl;
	}
	get password(): FormControl {
		return this.registerForm.get('password') as FormControl;
	}
	handleLogin() {
		this.router.navigate(['auth/login']);
	}
	handleSubmit() {
		if (this.registerForm.valid) {
			this.logging = true;
			this.subscriptions.push(
				this._auth.register(this.registerForm.value).subscribe({
					next: (data: Response) => {
						this.toastr.success(data.message, 'account created sucessfully');
						this.router.navigate(['']);
					},
					error: (e) => {
						let res: Response = e.error ?? e;
						this.toastr.error(res.message, 'unauthorized');
						this.logging = false;
					},
					complete: () => {
						this.logging = false;
					},
				})
			);
		}
	}
	ngOnDestroy() {
		this.subscriptions.forEach((s) => s.unsubscribe());
	}
}
