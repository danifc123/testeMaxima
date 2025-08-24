import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';
import { ProdutoService } from '../../services/produto';
import { Produto } from '../../models/produto';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';

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
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './produto-list.html',
  styleUrl: './produto-list.scss'
})
export class ProdutoListComponent implements OnInit {
  produtos: Produto[] = [];
  displayedColumns: string[] = ['codigo', 'descricao', 'departamento', 'preco', 'status', 'acoes'];
  loading = false;

  constructor(
    private produtoService: ProdutoService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.loading = true;
    this.produtoService.getProdutos().subscribe({
      next: (data) => {
        this.produtos = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar produtos:', error);
        this.snackBar.open('Erro ao carregar produtos', 'Fechar', { duration: 3000 });
        this.loading = false;
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
