import {AuthServices} from '../../../auth/services/Auth.service';
import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {Router} from '@angular/router';
import {DatePipe} from '@angular/common';
import {ToastrService} from 'ngx-toastr';
@Component({
	selector: 'app-navbar',
	templateUrl: './navbar.component.html',
	styleUrls: ['./navbar.component.css'],
	// encapsulation: ViewEncapsulation.None,
})
export class NavbarComponent implements OnInit {
	constructor(public data: AuthServices, private router: Router) {}
	hasNewNotifications: boolean;
	notifications: Notification[] = [];
	notificationVisible: boolean;
	ngOnInit() {
		this.data.username.next(localStorage.getItem('uname'));
	}
	handleLogout() {
		localStorage.clear();
		this.router.navigate(['/auth/login']);
	}
	handleLogin() {
		this.router.navigate(['/auth/login']);
	}
}
