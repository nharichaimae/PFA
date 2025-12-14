import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders  } from '@angular/common/http';
import { AuthService } from './auth';

@Injectable({
  providedIn: 'root',
})
export class Piece {
  private api = 'http://localhost:5296';

  constructor(private http: HttpClient ,private auth: AuthService) {}

 addPiece(name: string ) {
    const token = this.auth.getToken();
    if (!token) throw new Error('Utilisateur non authentifié');

   const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`, 
      'Content-Type': 'application/json'
    });

    return this.http.post(`${this.api}/api/Pieces/Add`, { name }, { headers :{ 'X-AUTH-TOKEN': token } });

  }
}








   

