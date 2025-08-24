import { Routes } from '@angular/router';
import { ProdutoListComponent } from './components/produto-list/produto-list';
import { ProdutoFormComponent } from './components/produto-form/produto-form';

export const routes: Routes = [
  { path: '', redirectTo: '/produtos', pathMatch: 'full' },
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'produtos/novo', component: ProdutoFormComponent },
  { path: 'produtos/editar/:id', component: ProdutoFormComponent },
  { path: '**', redirectTo: '/produtos' }
];
