import {CommonModule} from '@angular/common';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {TokenInterceptor} from './token.interceptor';
import {LoginGuard} from './modules/auth/guards/login.guard';
import {JwtModule} from '@auth0/angular-jwt';
import {environment} from 'src/environments/environment';
import {ServiceWorkerModule, SwRegistrationOptions} from '@angular/service-worker';
import {BrowserModule} from '@angular/platform-browser';
import {ToastrModule} from 'ngx-toastr';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule} from '@angular/core';

export function tokenGetter() {
	return localStorage.getItem('token');
}
@NgModule({
	declarations: [AppComponent],
	imports: [
		HttpClientModule,
		BrowserAnimationsModule,
		AppRoutingModule,
		CommonModule,
		ToastrModule.forRoot({preventDuplicates: true, positionClass: 'toast-bottom-left', progressBar: true, newestOnTop: true, progressAnimation: 'decreasing'}),
		BrowserModule,
		JwtModule.forRoot({
			config: {
				tokenGetter: tokenGetter,
				allowedDomains: [environment.host],
				disallowedRoutes: [],
			},
		}),
		ServiceWorkerModule.register('ngsw-worker.js'),
	],
	bootstrap: [AppComponent],
	providers: [
		{
			provide: SwRegistrationOptions,
			useFactory: () => ({enabled: true, registrationStrategy: 'registerWhenStable:30000'}),
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: TokenInterceptor,
			multi: true,
		},
		LoginGuard,
	],
})
export class AppModule {}
