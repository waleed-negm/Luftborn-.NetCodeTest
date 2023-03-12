import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {GenericService} from '../../shared/services/genericCRUD.service';
import {BehaviorSubject} from 'rxjs';
import {Response} from '../../shared/interfaces/Iresponse';
import {ToastrService} from 'ngx-toastr';
@Injectable({
	providedIn: 'root',
})
export class ItemsService extends GenericService<Response> {
	constructor(http: HttpClient, private toastr: ToastrService) {
		super(http, 'Item');
	}
	dataChange: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
	loadingData: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	get loading(): boolean {
		return this.loadingData.value;
	}
	get data(): any[] {
		return this.dataChange.value;
	}
	dialogData: any;
	getDialogData() {
		return this.dialogData;
	}
	getAllItems() {
		this.http.get<Response>(this.api_url).subscribe({
			next: (data: Response) => {
				this.loadingData.next(true);
				this.dataChange.next(data.body);
			},
			error: (e) => {
				this.loadingData.next(false);
				let res: Response = e.error ?? e;
				this.toastr.error(res.message, 'loading failed');
			},
			complete: () => this.loadingData.next(false),
		});
	}
}
