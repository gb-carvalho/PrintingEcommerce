import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs'
import { AuthService } from '../../services/auth/auth.service'


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

  constructor(private http: HttpClient, private authService: AuthService) { }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  createProduct(product: Product): Observable<Product> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}` // Adiciona o token
    });

    return this.http.post<Product>(this.apiUrl, product, { headers });
  }

  deleteProduct(id: number): Observable<void> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}` // Adiciona o token
    });

    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }
}
