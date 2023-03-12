import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl} from '@angular/forms';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ToastrService} from 'ngx-toastr';
import {Subscription} from 'rxjs';
import {Item} from '../../interfaces/IItem';
import {ItemsService} from '../../services/item.service';
import {Response} from './../../../shared/interfaces/Iresponse';
import {Categry} from './../../../categories/interfaces/Icategory';
import {CategoriesService} from './../../../categories/services/categories.service';

@Component({
	selector: 'app-form.dialog',
	templateUrl: './form.dialog.html',
	styleUrls: ['./form.dialog.css'],
})
export class FormDialogComponent implements OnInit, OnDestroy {
	subscriptions: Subscription[] = [];
	form: FormGroup;
	isSubmitting: boolean = false;
	CategoriesDataSource: Categry[] = [];

	constructor(
		public dialogRef: MatDialogRef<FormDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: Item,
		private _item: ItemsService,
		private _category: CategoriesService,
		private fb: FormBuilder,
		private toastr: ToastrService
	) {
		this.form = this.fb.group({
			id: [null],
			name: ['', [Validators.required, Validators.maxLength(100)]],
			quantity: [1, [Validators.required, Validators.min(1)]],
			price: [0, [Validators.min(0)]],
			categoryId: [null, [Validators.required]],
		});
	}
	get id(): FormControl {
		return this.form.get('id') as FormControl;
	}
	get name(): FormControl {
		return this.form.get('name') as FormControl;
	}
	get quantity(): FormControl {
		return this.form.get('quantity') as FormControl;
	}
	get price(): FormControl {
		return this.form.get('price') as FormControl;
	}

	get categoryId(): FormControl {
		return this.form.get('categoryId') as FormControl;
	}

	ngOnInit() {
		this.getAllCategories();
	}

	getAllCategories() {
		this.subscriptions.push(
			this._category.getAll().subscribe({
				next: (data) => (this.CategoriesDataSource = data.body),
				error: (e) => {
					this.isSubmitting = false;
					let res: Response = e.error ?? e;
					this.toastr.error(res.message, 'an error occurred');
				},
				complete: () => {
					if (this.data) this.form.patchValue(this.data);
				},
			})
		);
	}
	onNoClick() {
		this.dialogRef.close();
	}
	handleSubmit() {
		if (this.form.valid) {
			if (this.id.value) {
				this.subscriptions.push(
					this._item.update(this.id.value, this.form.value).subscribe({
						next: (res) => {
							this.isSubmitting = true;
							this._item.dialogData = res.body;
							this.dialogRef.close({data: res});
						},
						error: (e) => {
							this.isSubmitting = false;
							let res: Response = e.error ?? e;
							this.toastr.error(res.message, 'an error occurred');
						},
						complete: () => {
							this.isSubmitting = false;
						},
					})
				);
			} else {
				this.subscriptions.push(
					this._item.add(this.form.value).subscribe({
						next: (res) => {
							this.isSubmitting = true;
							this._item.dialogData = res.body;
							this.dialogRef.close({data: res});
						},
						error: (e) => {
							this.isSubmitting = false;
							let res: Response = e.error ?? e;
							this.toastr.error(res.message, 'an error occurred');
						},
						complete: () => {
							this.isSubmitting = false;
						},
					})
				);
			}
		}
	}
	ngOnDestroy = () => this.subscriptions.forEach((s) => s.unsubscribe());
}
