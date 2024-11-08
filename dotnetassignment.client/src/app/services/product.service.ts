import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }

  saveProducts(products: any[]): Observable<any> {
    return this.http.post<any>(`https://localhost:7298/api/Product/SaveProduct`, products, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  getProducts(): Observable<any> {
    return this.http.get<any>(`https://localhost:7298/api/Product/GetAllProducts`)
  };

}
