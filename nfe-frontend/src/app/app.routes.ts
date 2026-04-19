import { Routes } from '@angular/router';
import { NotaListComponent } from './features/notas/nota-list.component';
import { NotaFormComponent } from './features/notas/nota-form.component';
import { ProdutoListComponent } from './features/produtos/produto-list.component';
import { ProdutoFormComponent } from './features/produtos/produto-form.component';

export const routes: Routes = [
  { path: 'notas', component: NotaListComponent },
  { path: 'notas/nova', component: NotaFormComponent },
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'produtos/novo', component: ProdutoFormComponent },
  { path: '', redirectTo: 'notas', pathMatch: 'full' }
];
