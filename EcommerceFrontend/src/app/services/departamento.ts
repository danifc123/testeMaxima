import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Departamento } from '../models/departamento';
import { APP_CONSTANTS } from '../constants/app.constants';

@Injectable({
  providedIn: 'root'
})
export class DepartamentoService {
  private readonly apiUrl = `${APP_CONSTANTS.API_BASE_URL}/departamento`;

  constructor(private http: HttpClient) { }

  getDepartamentos(): Observable<Departamento[]> {
    return this.http.get<Departamento[]>(this.apiUrl);
  }
}
