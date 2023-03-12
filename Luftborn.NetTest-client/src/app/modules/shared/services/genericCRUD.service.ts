import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from 'src/environments/environment';

export abstract class GenericService<T> {
	constructor(public http: HttpClient, private controller: string) {}
	api_url: string = `${environment.apiUrl}${this.controller}`;
	headers = new HttpHeaders({'Content-Type': 'application/json'});

	getAll = () => this.http.get<T>(`${this.api_url}`, {headers: this.headers});

	getOne = (id: string) => this.http.get<T>(`${this.api_url}/GetById?id=${id}`, {headers: this.headers});

	add = (model: T) => this.http.post<T>(`${this.api_url}`, model, {headers: this.headers});

	update = (id: string, model: T) => this.http.put<T>(`${this.api_url}?id=${id}`, {...model, id}, {headers: this.headers});

	delete = (id: string) => this.http.delete<T>(`${this.api_url}?id=${id}`, {headers: this.headers});
}
