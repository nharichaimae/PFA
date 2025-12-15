import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EquipementService {

  private apiUrl = 'http://localhost:5296/api/equipement';

  constructor(private http: HttpClient) {}

  addEquipement(equipement: any) {
       return this.http.post(this.apiUrl, equipement, {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
