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
  displayedColumns: string[] = ['codigo', 'descricao', 'departamento', 'preco', 'status', 'acoes'];
  loading = false;
  private routerSubscription: Subscription | undefined;

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
    this.loading = true;
    this.cdr.detectChanges(); // Força detecção de mudanças

    // Timeout de segurança para evitar loading infinito
    const timeout = setTimeout(() => {
      this.loading = false;
      this.cdr.detectChanges();
    }, 10000); // 10 segundos

    this.produtoService.getProdutos().subscribe({
      next: (data) => {
        clearTimeout(timeout);
        this.produtos = data;
        this.loading = false;
        this.cdr.detectChanges(); // Força detecção de mudanças
      },
      error: (error) => {
        clearTimeout(timeout);
        console.error('Erro ao carregar produtos:', error);
        this.snackBar.open('Erro ao carregar produtos', 'Fechar', { duration: 3000 });
        this.loading = false;
        this.cdr.detectChanges(); // Força detecção de mudanças
      }
    });
  }

  editarProduto(produto: Produto): void {
    this.router.navigate(['/produtos/editar', produto.id]);
  }

  excluirProduto(produto: Produto): void {
    const dialogRef = this.dialog.open(ConfirmacaoDialogComponent, {
      width: '400px',
      data: { mensagem: `Deseja realmente excluir o produto "${produto.descricao}"?` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.produtoService.deleteProduto(produto.id).subscribe({
          next: () => {
            this.snackBar.open('Produto excluído com sucesso', 'Fechar', { duration: 3000 });
            this.carregarProdutos();
          },
          error: (error) => {
            console.error('Erro ao excluir produto:', error);
            this.snackBar.open('Erro ao excluir produto', 'Fechar', { duration: 3000 });
          }
        });
      }
    });
  }

  novoProduto(): void {
    this.router.navigate(['/produtos/novo']);
  }

  formatarPreco(preco: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(preco);
  }

  getStatusClass(status: boolean): string {
    return status ? 'status-ativo' : 'status-inativo';
  }

  getStatusText(status: boolean): string {
    return status ? 'Ativo' : 'Inativo';
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
