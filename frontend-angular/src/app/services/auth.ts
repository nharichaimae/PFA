import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs'

@Injectable({ providedIn: 'root' })
export class AuthService {

  private api = 'http://localhost:8000/api';

  constructor(private http: HttpClient ) {}

  login(email: string, password: string): Observable<any>  {
    return this.http.post<any>(`${this.api}/login`, { email, password });
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }
    isAuthenticated(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
  }
}
