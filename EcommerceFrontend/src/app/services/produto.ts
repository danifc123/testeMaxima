import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto, ProdutoDto } from '../models/produto';
import { APP_CONSTANTS } from '../constants/app.constants';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
  private readonly apiUrl = `${APP_CONSTANTS.API_BASE_URL}/produto`;

  constructor(private http: HttpClient) { }

  getProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  getProduto(id: string): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`);
  }

  createProduto(produto: ProdutoDto): Observable<any> {
    return this.http.post(this.apiUrl, produto);
  }

  updateProduto(id: string, produto: ProdutoDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, produto);
  }

  deleteProduto(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
