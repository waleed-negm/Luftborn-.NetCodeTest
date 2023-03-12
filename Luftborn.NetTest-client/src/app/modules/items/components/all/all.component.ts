import {HttpClient} from '@angular/common/http';
import {Component, OnInit, OnDestroy, ViewChild, ElementRef} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {Subscription, fromEvent} from 'rxjs';
import {DeleteDialogComponent} from '../delete/delete.dialog.component';
import {FormDialogComponent} from '../formDialog/form.dialog.component';
import {ToastrService} from 'ngx-toastr';
import {ItemsService} from '../../services/item.service';
import {TableDataSource} from 'src/app/modules/shared/components/table/tableDataSource';
import {Item} from '../../interfaces/IItem';
@Component({
	selector: 'app-all',
	templateUrl: './all.component.html',
	styleUrls: ['./all.component.css'],
})
export class AllComponent implements OnInit, OnDestroy {
	subscriptions: Subscription[] = [];
	paginationSizes: number[] = [10, 20, 50, 100];
	constructor(public httpClient: HttpClient, public dialog: MatDialog, public _item: ItemsService, private toastr: ToastrService) {}
	@ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
	@ViewChild(MatSort, {static: true}) sort: MatSort;
	@ViewChild('filter', {static: true}) filter: ElementRef;
	displayedColumns: string[];
	tableColumns: any[] = [];
	dataSource: TableDataSource;
	database: ItemsService;
	ngOnInit() {
		this.tableColumns = [
			{
				columnDef: 'id',
				header: 'id',
				cell: (element: Item) => element.id,
			},
			{
				columnDef: 'name',
				header: 'name',
				cell: (element: Item) => element.name,
			},
			{
				columnDef: 'quantity',
				header: 'quantity',
				cell: (element: Item) => element.quantity,
			},
			{
				columnDef: 'price',
				header: 'price',
				cell: (element: Item) => element.price,
			},
			{
				columnDef: 'categoryName',
				header: 'category Name',
				cell: (element: Item) => element.categoryName,
			},
		];
		this.displayedColumns = [...this.tableColumns.map((c: any) => c.columnDef), 'actions'];
		this.database = new ItemsService(this.httpClient, this.toastr);
		this.database.getAllItems();
		this.dataSource = new TableDataSource(this.database, this.paginator, this.sort);
		this.subscriptions.push(
			fromEvent(this.filter.nativeElement, 'keyup').subscribe(() => {
				if (!this.dataSource) return;
				this.dataSource.filter = this.filter.nativeElement.value;
			})
		);
	}
	addNew() {
		const dialogRef = this.dialog.open(FormDialogComponent, {minWidth: '30%'});
		this.subscriptions.push(
			dialogRef.afterClosed().subscribe((result) => {
				if (result?.data) {
					this.database.data.push(this._item.getDialogData());
					this.refreshTable();
					this.toastr.success(result.data.message);
				}
			})
		);
	}
	startEdit(row: Item) {
		const dialogRef = this.dialog.open(FormDialogComponent, {data: row, minWidth: '30%'});
		this.subscriptions.push(
			dialogRef.afterClosed().subscribe((result) => {
				if (result?.data) {
					this.database.data[this.database.data.findIndex((x) => x.id === row.id)] = this._item.getDialogData();
					this.refreshTable();
					this.toastr.success(result.data.message);
				}
			})
		);
	}
	deleteItem(row: Item) {
		const dialogRef = this.dialog.open(DeleteDialogComponent, {data: row, minWidth: '30%'});
		this.subscriptions.push(
			dialogRef.afterClosed().subscribe((result) => {
				if (result?.data) {
					this.database.data.splice(
						this.database.data.findIndex((x) => x.id === row.id),
						1
					);
					this.refreshTable();
					this.toastr.success(result.data.message);
				}
			})
		);
	}
	clearFilter = () => (this.dataSource.filter = this.filter.nativeElement.value = '');
	private refreshTable = () => this.paginator._changePageSize(this.paginator.pageSize);
	ngOnDestroy = () => this.subscriptions.forEach((s) => s.unsubscribe());
}
