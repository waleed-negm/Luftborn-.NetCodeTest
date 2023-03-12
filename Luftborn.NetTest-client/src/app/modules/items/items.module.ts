import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {LoginGuard} from '../auth/guards/login.guard';
import {MatComponentsModule} from '../matComponents/matComponents.module';
import {SharedModule} from '../shared/shared.module';
import {AllComponent} from './components/all/all.component';
import {DeleteDialogComponent} from './components/delete/delete.dialog.component';
import {FormDialogComponent} from './components/formDialog/form.dialog.component';
import {ItemsComponent} from './items.component';
import {ItemsRoutingModule} from './routing/item-routing.module';

@NgModule({
	declarations: [ItemsComponent, AllComponent, DeleteDialogComponent, FormDialogComponent],
	imports: [CommonModule, ItemsRoutingModule, MatComponentsModule, SharedModule],
	providers: [LoginGuard],
})
export class ItemsModule {}
