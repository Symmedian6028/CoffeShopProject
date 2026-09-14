import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Coffee } from '../models/coffee.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CoffeeService {
  private readonly apiUrl = `${environment.apiBaseUrl}/coffees`;

  constructor(private http: HttpClient) {}

  getCoffees(category?: string): Observable<Coffee[]> {
    let params = new HttpParams();
    if (category) {
      params = params.set('category', category);
    }

    return this.http.get<Coffee[]>(this.apiUrl, { params });
  }
}
