import { Component, OnInit } from '@angular/core';
import { FaturamentoService } from '../../core/services/faturamento.service';
import { CommonModule } from '@angular/common'; 


@Component({
  selector: 'app-nota-list',
  imports: [CommonModule],
  standalone: true,
  template: `
    <h2>Notas Fiscais</h2>

    <button routerLink="/notas/nova">Nova Nota</button>

    <ul>
      <li *ngFor="let n of notas">
        Nota #{{ n.numero }} - {{ n.status }}

        <button (click)="imprimir(n.id)">Imprimir</button>
      </li>
    </ul>
  `
})
export class NotaListComponent implements OnInit {
  notas: any[] = [];

  constructor(private service: FaturamentoService) {}

  ngOnInit() {
    this.service.listarNotas().subscribe(res => {
      this.notas = res;
    });
  }

  imprimir(id: number) {
    this.service.imprimirNota(id).subscribe({
      next: () => alert('Nota impressa'),
      error: () => alert('Erro ao imprimir (estoque offline?)')
    });
  }
}