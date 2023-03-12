import {LoginComponent} from './components/login/login.component';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AuthRoutingModule} from './routing/auth-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RegisterComponent} from './components/register/register.component';
import {MatComponentsModule} from '../matComponents/matComponents.module';
import {ConfirmEmailComponent} from './components/confirm-email/confirm-email.component';
import {ForgetPasswordComponent} from './components/forget-password/forget-password.component';
import {ResetPasswordComponent} from './components/reset-password/reset-password.component';

@NgModule({
	declarations: [LoginComponent, RegisterComponent, ResetPasswordComponent, ForgetPasswordComponent, ConfirmEmailComponent],
	imports: [CommonModule, AuthRoutingModule, MatComponentsModule, FormsModule, ReactiveFormsModule],
	providers: [],
})
export class AuthModule {}
