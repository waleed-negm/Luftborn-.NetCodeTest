import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {LoginGuard} from '../../auth/guards/login.guard';
import {AllComponent} from '../components/all/all.component';
import {ItemsComponent} from '../items.component';

const routes: Routes = [
	{
		path: '',
		component: ItemsComponent,
		canActivate: [LoginGuard],
		canActivateChild: [LoginGuard],
		children: [
			{path: 'all', component: AllComponent, title: 'all items'},
			{path: '', redirectTo: 'all', pathMatch: 'full'},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class ItemsRoutingModule {}
