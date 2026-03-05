import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtPayload, jwtDecode } from 'jwt-decode';

export interface User {
  id: string;
  name: string;
  email: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7182/User/login';
  private apiUrlUsers = 'https://localhost:7182/User';
  private tokenKey = 'auth_token';

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post(this.apiUrl, { email, password })
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return token !== null;  
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/']);
  }

  getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getRole(): string {
    const token = this.getToken();
    if (token == null) return '';

    const decodedToken = jwtDecode<{ [key: string]: any }>(token);
    const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]

    return role;
  }

  hasRole(requiredRole: string): boolean
  {
    return this.getRole() === requiredRole;
  }

  getUsers(): Observable<any[]> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}` // Adiciona o token
    });

    return this.http.get<any[]>(this.apiUrlUsers, { headers });
  }

  getUser(id: string): Observable<User> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}` // Adiciona o token
    });

    return this.http.get<User>(this.apiUrlUsers + "/" + id, { headers });
  }

  updateUser(user: User): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}` // Adiciona o token
    });

    return this.http.patch(this.apiUrlUsers + "/" + user.id, user, { headers })
  }
}
