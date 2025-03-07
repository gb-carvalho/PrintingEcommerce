import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs'

interface Product {
  id?: number;
  name: string;
  price: number;
  description: string;
}

@Injectable({
  providedIn: 'root'
})  
export class ProductService {
  private apiUrl = 'https://localhost:7086/Product'

  constructor(private http: HttpClient) { }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }
}
