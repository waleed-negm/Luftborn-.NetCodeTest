import {Component, OnDestroy} from '@angular/core';
import {OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Auth} from '../../interfaces/IAuth';
import {Subscription} from 'rxjs';
import {AuthServices} from '../../services/Auth.service';
import {Response} from 'src/app/modules/shared/interfaces/Iresponse';

@Component({
	selector: 'app-confirm-email',
	templateUrl: './confirm-email.component.html',
	styleUrls: ['./confirm-email.component.css'],
})
export class ConfirmEmailComponent implements OnInit, OnDestroy {
	constructor(private _auth: AuthServices, private route: ActivatedRoute, private toastr: ToastrService, private router: Router) {}
	subscriptions: Subscription[] = [];
	ngOnInit(): void {
		this.subscriptions.push(
			this.route.queryParams.subscribe((res: any) => {
				this.subscriptions.push(
					this._auth.confirmEmail(res.userid, res.token).subscribe({
						next: (data: Response) => {
							let auth: Auth = data.body;
							this._auth.setLocalStorage(auth);
							this._auth.username.next(auth.userName);
							this._auth.isLogged = true;
							this.toastr.success(data.message, 'logged in');
						},
						error: (e) => {
							this._auth.isLogged = false;
							this._auth.username.next(null);
							localStorage.clear();
							let res: Response = e.error ?? e;
							this.toastr.error(res.message, 'unauthorized');
						},
						complete: () => {
							this.router.navigate(['']);
						},
					})
				);
			})
		);
	}
	ngOnDestroy = () => this.subscriptions.forEach((sub) => sub.unsubscribe());
}
