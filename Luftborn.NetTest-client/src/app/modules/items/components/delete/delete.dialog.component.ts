import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Component, Inject} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {Item} from '../../interfaces/IItem';
import {ItemsService} from '../../services/item.service';
@Component({
	selector: 'app-delete.dialog',
	templateUrl: './delete.dialog.html',
	styleUrls: ['./delete.dialog.css'],
})
export class DeleteDialogComponent {
	isSubmitting: boolean = false;
	constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: Item, public _item: ItemsService, private toastr: ToastrService) {}
	onNoClick(): void {
		this.dialogRef.close();
	}
	confirmDelete(): void {
		this._item.delete(this.data.id).subscribe({
			next: (res) => {
				this.isSubmitting = true;
				this.dialogRef.close({data: res});
			},
			error: (e) => {
				this.isSubmitting = false;
				this.toastr.error(e.erorr.message, 'an error occurred');
			},
			complete: () => {
				this.isSubmitting = false;
			},
		});
	}
}
