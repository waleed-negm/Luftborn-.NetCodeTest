import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ConfirmEmailComponent} from '../components/confirm-email/confirm-email.component';
import {ForgetPasswordComponent} from '../components/forget-password/forget-password.component';
import {LoginComponent} from '../components/login/login.component';
import {RegisterComponent} from '../components/register/register.component';
import {ResetPasswordComponent} from '../components/reset-password/reset-password.component';
import {AccessLoginPageGuard} from '../guards/AccessLoginPage.guard';
const routes: Routes = [
	{path: 'login', component: LoginComponent, title: 'login', canActivate: [AccessLoginPageGuard]},
	{path: 'register', component: RegisterComponent, title: 'register'},
	{path: 'confirmEmail', component: ConfirmEmailComponent, title: 'confirm email'},
	{path: 'resetPassword', component: ResetPasswordComponent, title: 'reset password'},
	{path: 'forgetPassword', component: ForgetPasswordComponent, title: 'forget password', canActivate: [AccessLoginPageGuard]},
	{path: 'auth', redirectTo: 'login', pathMatch: 'full'},
	{path: '', redirectTo: '/', pathMatch: 'full'},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AuthRoutingModule {}
