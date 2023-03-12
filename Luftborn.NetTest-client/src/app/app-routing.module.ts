import {NgModule} from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';
import {LoginGuard} from './modules/auth/guards/login.guard';
const routes: Routes = [
	{path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then((m) => m.AuthModule)},
	{path: '', loadChildren: () => import('./modules/shared/shared.module').then((m) => m.SharedModule), canLoad: [LoginGuard]},
];
@NgModule({
	imports: [
		RouterModule.forRoot(routes, {
			preloadingStrategy: PreloadAllModules,
		}),
	],
	exports: [RouterModule],
})
export class AppRoutingModule {}
