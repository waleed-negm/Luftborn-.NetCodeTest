import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginGuard} from '../../auth/guards/login.guard';
import {NotfoundComponent} from '../components/notfound/notfound.component';
import {ProfileComponent} from '../components/profile/profile.component';
import {SharedComponent} from '../shared.component';

const routes: Routes = [
	{
		path: '',
		component: SharedComponent,
		title: 'dashboard',
		children: [
			{path: 'categories', loadChildren: () => import('../../categories/categories.module').then((m) => m.CategoriesModule), canLoad: [LoginGuard], canActivate: [LoginGuard]},
			{path: 'items', loadChildren: () => import('../../items/items.module').then((m) => m.ItemsModule), canLoad: [LoginGuard], canActivate: [LoginGuard]},
			{path: 'profile', component: ProfileComponent, title: 'Profile', canActivate: [LoginGuard]},
			{path: '', redirectTo: 'categories', pathMatch: 'full'},
		],
	},
	{path: '**', component: NotfoundComponent, title: '404 - not found'},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SharedRoutingModule {}
