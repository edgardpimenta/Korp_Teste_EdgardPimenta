import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

const API = 'http://localhost:5182';

@Injectable({ providedIn: 'root' })
export class FaturamentoService {
  constructor(private http: HttpClient) {}

  listarNotas() {
    return this.http.get<any[]>(`${API}/notas`);
  }

  criarNota(nota: any) {
    return this.http.post(`${API}/notas`, nota);
  }

  imprimirNota(id: number) {
    return this.http.post(`${API}/notas/${id}/imprimir`, {});
  }
}