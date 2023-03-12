import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {LoginGuard} from '../auth/guards/login.guard';
import {MatComponentsModule} from '../matComponents/matComponents.module';
import {SharedModule} from '../shared/shared.module';
import {CategoriesComponent} from './categories.component';
import {AllComponent} from './components/all/all.component';
import {DeleteDialogComponent} from './components/delete/delete.dialog.component';
import {FormDialogComponent} from './components/formDialog/form.dialog.component';
import {CategoriesRoutingModule} from './routing/categories-routing.module';

@NgModule({
	declarations: [CategoriesComponent, AllComponent, DeleteDialogComponent, FormDialogComponent],
	imports: [CommonModule, CategoriesRoutingModule, MatComponentsModule, SharedModule],
	providers: [LoginGuard],
})
export class CategoriesModule {}
