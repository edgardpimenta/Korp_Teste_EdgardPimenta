import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EstoqueService } from '../../core/services/estoque.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h2>Novo Produto</h2>

    <input [(ngModel)]="produto.nome" placeholder="Nome" />
    <input [(ngModel)]="produto.saldo" type="number" placeholder="Saldo" />

    <button (click)="salvar()">Salvar</button>
  `
})
export class ProdutoFormComponent {
  produto: any = {};

  constructor(
    private service: EstoqueService,
    private router: Router
  ) {}

  salvar() {
    this.service.criarProduto(this.produto).subscribe(() => {
      this.router.navigate(['/produtos']);
    });
  }
}