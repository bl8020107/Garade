import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Garage } from '../models/garage.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GarageService {
  private apiUrl = `${environment.apiUrl}/garages`;

  constructor(private http: HttpClient) { }

  getGaragesFromGovernment(): Observable<Garage[]> {
    return this.http.get<Garage[]>(`${this.apiUrl}/from-government`);
  }

  getGarages(): Observable<Garage[]> {
    return this.http.get<Garage[]>(this.apiUrl);
  }

  addGarages(garages: Garage[]): Observable<Garage[]> {
    if (!garages || garages.length === 0) {
      throw new Error('Garages array cannot be empty');
    }
    return this.http.post<Garage[]>(`${this.apiUrl}/bulk`, garages);
  }
}
