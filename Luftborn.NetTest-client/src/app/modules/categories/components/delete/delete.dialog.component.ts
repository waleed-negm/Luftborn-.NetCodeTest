import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Component, Inject} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {Categry} from './../../interfaces/Icategory';
import {CategoriesService} from '../../services/categories.service';
@Component({
	selector: 'app-delete.dialog',
	templateUrl: './delete.dialog.html',
	styleUrls: ['./delete.dialog.css'],
})
export class DeleteDialogComponent {
	isSubmitting: boolean = false;
	constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: Categry, public _category: CategoriesService, private toastr: ToastrService) {}
	onNoClick(): void {
		this.dialogRef.close();
	}
	confirmDelete(): void {
		this._category.delete(this.data.id).subscribe({
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
