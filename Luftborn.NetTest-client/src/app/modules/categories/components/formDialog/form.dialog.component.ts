import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl, FormArray} from '@angular/forms';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ToastrService} from 'ngx-toastr';
import {Subscription} from 'rxjs';
import {Categry} from '../../interfaces/Icategory';
import {CategoriesService} from '../../services/categories.service';
import {Response} from './../../../shared/interfaces/Iresponse';

@Component({
	selector: 'app-form.dialog',
	templateUrl: './form.dialog.html',
	styleUrls: ['./form.dialog.css'],
})
export class FormDialogComponent implements OnInit, OnDestroy {
	subscriptions: Subscription[] = [];
	loading: boolean = false;
	form: FormGroup;
	isSubmitting: boolean = false;
	deletedItems: string[] = [];

	constructor(
		public dialogRef: MatDialogRef<FormDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: Categry,
		private _category: CategoriesService,
		private fb: FormBuilder,
		private toastr: ToastrService
	) {
		this.form = this.createFormItem('init');
	}

	get id(): FormControl {
		return this.form.get('id') as FormControl;
	}

	get name(): FormControl {
		return this.form.get('name') as FormControl;
	}

	get items(): FormArray {
		return this.form.get('items') as FormArray;
	}

	getItemId = (index: number): FormControl => this.items.at(index).get('id') as FormControl;

	getName = (index: number): FormControl => this.items.at(index).get('name') as FormControl;

	getQuantity = (index: number): FormControl => this.items.at(index).get('quantity') as FormControl;

	getPrice = (index: number): FormControl => this.items.at(index).get('price') as FormControl;

	ngOnInit() {
		if (this.data) {
			this.data.items.forEach(() => this.items.push(this.createFormItem('item')));
			this.form.patchValue(this.data);
		}
	}

	createFormItem(type: string): FormGroup {
		let formItem: FormGroup = this.fb.group({});
		switch (type) {
			case 'init':
				formItem = this.fb.group({
					id: [null],
					name: ['', [Validators.required, Validators.maxLength(100)]],
					items: this.fb.array([]),
				});
				break;
			case 'item':
				formItem = this.fb.group({
					id: [null],
					categoryId: [this.id.value],
					name: ['', [Validators.required, Validators.maxLength(100)]],
					quantity: [1, [Validators.required, Validators.min(1)]],
					price: [0, [Validators.min(0)]],
				});
				break;
		}
		return formItem;
	}

	handleNewItem = () => this.items.push(this.createFormItem('item'));

	handleDeleteItem = (index: number) => {
		if (this.getItemId(index).value) this.deletedItems.push(this.getItemId(index).value);
		this.items.removeAt(index);
	};

	onCancel() {
		this.dialogRef.close();
	}

	handleSubmit() {
		if (this.form.valid) {
			if (this.id.value) this.update();
			else this.add();
		}
	}

	update() {
		this.isSubmitting = true;

		if (this.deletedItems.length)
			this.subscriptions.push(
				this._category.deleteItems(this.deletedItems).subscribe({
					error: (e) => {
						this.isSubmitting = false;
						let res: Response = e.error ?? e;
						this.toastr.error(res.message, 'an error occurred');
					},
				})
			);

		this.subscriptions.push(
			this._category.update(this.id.value, this.form.value).subscribe({
				next: (res) => {
					this._category.dialogData = res.body;
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

	add() {
		this.subscriptions.push(
			this._category.add(this.form.value).subscribe({
				next: (res) => {
					this.isSubmitting = true;
					this._category.dialogData = res.body;
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

	ngOnDestroy() {
		this.subscriptions.forEach((s) => s.unsubscribe());
	}
}
