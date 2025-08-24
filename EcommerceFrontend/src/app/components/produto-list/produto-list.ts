import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, NavigationEnd } from '@angular/router';
import { ProdutoService } from '../../services/produto';
import { Produto } from '../../models/produto';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { filter, Subscription } from 'rxjs';
import { APP_CONSTANTS } from '../../constants/app.constants';
import { Formatters } from '../../utils/formatters';
import { LoadingState } from '../../models/api-response';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './produto-list.html',
  styleUrl: './produto-list.scss'
})
export class ProdutoListComponent implements OnInit, OnDestroy {
  produtos: Produto[] = [];
  readonly displayedColumns: string[] = ['codigo', 'descricao', 'departamento', 'preco', 'status', 'acoes'];
  loadingState: LoadingState = 'idle';
  private routerSubscription: Subscription | undefined;

  // Getters para melhor legibilidade
  get isLoading(): boolean {
    return this.loadingState === 'loading';
  }

  get hasProdutos(): boolean {
    return this.produtos.length > 0;
  }

  constructor(
    private produtoService: ProdutoService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.carregarProdutos();

    // Escutar mudanças de rota para recarregar produtos quando voltar do formulário
    this.routerSubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.carregarProdutos();
    });
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

      carregarProdutos(): void {
    this.setLoadingState('loading');

    // Timeout de segurança para evitar loading infinito
    const timeout = setTimeout(() => {
      this.setLoadingState('error');
    }, APP_CONSTANTS.LOADING_TIMEOUT);

    this.produtoService.getProdutos().subscribe({
      next: (data) => {
        clearTimeout(timeout);
        this.produtos = data;
        this.setLoadingState('success');
      },
      error: (error) => {
        clearTimeout(timeout);
        this.handleError('Erro ao carregar produtos', error);
        this.setLoadingState('error');
      }
    });
  }

  private setLoadingState(state: LoadingState): void {
    this.loadingState = state;
    this.cdr.detectChanges();
  }

  private handleError(message: string, error: any): void {
    console.error(message, error);
    this.snackBar.open(message, 'Fechar', { duration: APP_CONSTANTS.SNACKBAR_DURATION });
  }

  editarProduto(produto: Produto): void {
    this.router.navigate([APP_CONSTANTS.ROUTES.PRODUTO_EDITAR, produto.id]);
  }

  excluirProduto(produto: Produto): void {
    const dialogRef = this.dialog.open(ConfirmacaoDialogComponent, {
      width: '400px',
      data: { mensagem: `Deseja realmente excluir o produto "${produto.descricao}"?` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.excluirProdutoConfirmado(produto.id);
      }
    });
  }

  private excluirProdutoConfirmado(produtoId: string): void {
    this.produtoService.deleteProduto(produtoId).subscribe({
      next: () => {
        this.snackBar.open(APP_CONSTANTS.MESSAGES.PRODUTO.EXCLUIDO, 'Fechar', { duration: APP_CONSTANTS.SNACKBAR_DURATION });
        this.carregarProdutos();
      },
      error: (error) => {
        this.handleError(APP_CONSTANTS.MESSAGES.PRODUTO.ERRO_EXCLUIR, error);
      }
    });
  }

  novoProduto(): void {
    this.router.navigate([APP_CONSTANTS.ROUTES.PRODUTO_NOVO]);
  }

  // Métodos de formatação usando utilitários
  formatarPreco(preco: number): string {
    return Formatters.formatCurrency(preco);
  }

  getStatusClass(status: boolean): string {
    return Formatters.getStatusClass(status);
  }

  getStatusText(status: boolean): string {
    return Formatters.getStatusText(status);
  }
}

@Component({
  selector: 'app-confirmacao-dialog',
  template: `
    <h2 mat-dialog-title>Confirmar Exclusão</h2>
    <mat-dialog-content>{{ data.mensagem }}</mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancelar</button>
      <button mat-button [mat-dialog-close]="true" color="warn">Excluir</button>
    </mat-dialog-actions>
  `,
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule]
})
export class ConfirmacaoDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { mensagem: string }) {}
}
