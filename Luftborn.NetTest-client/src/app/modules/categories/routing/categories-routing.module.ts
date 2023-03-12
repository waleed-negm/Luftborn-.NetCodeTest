import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginGuard} from '../../auth/guards/login.guard';
import {CategoriesComponent} from '../categories.component';
import {AllComponent} from '../components/all/all.component';

const routes: Routes = [
	{
		path: '',
		component: CategoriesComponent,
		canActivate: [LoginGuard],
		canActivateChild: [LoginGuard],
		children: [
			{path: 'all', component: AllComponent, title: 'all categories'},
			{path: '', redirectTo: 'all', pathMatch: 'full'},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CategoriesRoutingModule {}
