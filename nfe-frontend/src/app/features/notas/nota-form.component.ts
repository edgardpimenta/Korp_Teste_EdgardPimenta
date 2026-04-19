import { Component, OnInit } from '@angular/core';
import { FaturamentoService } from '../../core/services/faturamento.service';
import { EstoqueService } from '../../core/services/estoque.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nota-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Nova Nota</h2>

    <div *ngFor="let p of produtos">
      <label>
        {{ p.nome }}
        <input type="number" [(ngModel)]="p.quantidade" name="q-{{p.id}}" />
      </label>
    </div>

    <button (click)="criar()">Criar Nota</button>
  `
}) 
export class NotaFormComponent implements OnInit {
  produtos: any[] = [];

  constructor(
    private faturamento: FaturamentoService,
    private estoque: EstoqueService,
    private router: Router
  ) {}

  ngOnInit() {
    this.estoque.listarProdutos().subscribe(res => {
      this.produtos = res.map(p => ({
        ...p,
        quantidade: 0
      }));
    });
  }

  criar() {
    const itens = this.produtos
      .filter(p => p.quantidade > 0)
      .map(p => ({
        produtoId: p.id,
        quantidade: p.quantidade
      }));

    this.faturamento.criarNota({ itens }).subscribe(() => {
      this.router.navigate(['/notas']);
    });
  }
}