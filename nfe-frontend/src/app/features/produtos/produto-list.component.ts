import { Component, OnInit } from '@angular/core';
import { EstoqueService } from '../../core/services/estoque.service';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [MatTableModule, MatButtonModule],
  template: `
    <h2>Produtos</h2>

    <button mat-raised-button color="primary" routerLink="/produtos/novo">
      Novo Produto
    </button>

    <table mat-table [dataSource]="produtos" class="mat-elevation-z8">

      <ng-container matColumnDef="nome">
        <th mat-header-cell *matHeaderCellDef> Nome </th>
        <td mat-cell *matCellDef="let p"> {{p.nome}} </td>
      </ng-container>

      <ng-container matColumnDef="saldo">
        <th mat-header-cell *matHeaderCellDef> Saldo </th>
        <td mat-cell *matCellDef="let p"> {{p.saldo}} </td>
      </ng-container>

      <tr mat-header-row *matHeaderRowDef="['nome','saldo']"></tr>
      <tr mat-row *matRowDef="let row; columns: ['nome','saldo'];"></tr>

    </table>
  `
})
export class ProdutoListComponent implements OnInit {
  produtos: any[] = [];

  constructor(private service: EstoqueService) {}

  ngOnInit() {
    this.service.listarProdutos().subscribe(res => {
      this.produtos = res;
    });
  }
}