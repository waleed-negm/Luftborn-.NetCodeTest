import {AfterViewInit, Component, ElementRef, OnInit} from '@angular/core';
@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, AfterViewInit {
	constructor(private elementRef: ElementRef) {}
	ngOnInit() {}
	ngAfterViewInit() {
		this.elementRef.nativeElement.ownerDocument.body.style.backgroundColor = '#FFF';
	}
}
