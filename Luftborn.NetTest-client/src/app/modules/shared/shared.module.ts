import {LoginGuard} from './../auth/guards/login.guard';
import {MatComponentsModule} from './../matComponents/matComponents.module';
import {NotfoundComponent} from './components/notfound/notfound.component';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SharedRoutingModule} from './routing/shared-routing.module';
import {NavbarComponent} from './components/navbar/navbar.component';
import {SharedComponent} from './shared.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {LoadingBarRouterModule} from '@ngx-loading-bar/router';
import {ProfileComponent} from './components/profile/profile.component';
@NgModule({
	declarations: [SharedComponent, NavbarComponent, NotfoundComponent, ProfileComponent],
	imports: [CommonModule, LoadingBarRouterModule, SharedRoutingModule, MatComponentsModule, ReactiveFormsModule, FormsModule],
	exports: [FormsModule, ReactiveFormsModule],
	providers: [LoginGuard],
})
export class SharedModule {}
