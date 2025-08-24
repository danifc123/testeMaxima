import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Departamento } from '../models/departamento';

@Injectable({
  providedIn: 'root'
})
export class DepartamentoService {
  private apiUrl = 'http://localhost:7001/api/departamento';

  constructor(private http: HttpClient) { }

  getDepartamentos(): Observable<Departamento[]> {
    return this.http.get<Departamento[]>(this.apiUrl);
  }
}
