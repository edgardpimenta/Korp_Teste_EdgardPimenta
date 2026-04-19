import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API = 'http://localhost:8081';

@Injectable({ providedIn: 'root' })
export class EstoqueService {
  constructor(private http: HttpClient) {}

  listarProdutos(): Observable<any[]> {
    return this.http.get<any[]>(`${API}/produtos`);
  }

  criarProduto(produto: any) {
    return this.http.post(`${API}/produtos`, produto);
  }

  atualizarProduto(id: number, produto: any) {
    return this.http.put(`${API}/produtos/${id}`, produto);
  }

  deletarProduto(id: number) {
    return this.http.delete(`${API}/produtos/${id}`);
  }
}