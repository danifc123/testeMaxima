import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { ProdutoService } from '../../services/produto';
import { DepartamentoService } from '../../services/departamento';
import { Produto, ProdutoDto } from '../../models/produto';
import { Departamento } from '../../models/departamento';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatCardModule,
    MatSlideToggleModule
  ],
  templateUrl: './produto-form.html',
  styleUrl: './produto-form.scss'
})
export class ProdutoFormComponent implements OnInit {
  produtoForm: FormGroup;
  departamentos: Departamento[] = [];
  isEditMode = false;
  produtoId: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private produtoService: ProdutoService,
    private departamentoService: DepartamentoService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.produtoForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(50)]],
      descricao: ['', [Validators.required, Validators.maxLength(255)]],
      departamentoCodigo: ['', Validators.required],
      preco: ['', [Validators.required, Validators.min(0.01)]],
      status: [true]
    });
  }

  ngOnInit(): void {
    this.carregarDepartamentos();
    this.verificarModoEdicao();
  }

  carregarDepartamentos(): void {
    this.departamentoService.getDepartamentos().subscribe({
      next: (data) => {
        this.departamentos = data;
      },
      error: (error) => {
        console.error('Erro ao carregar departamentos:', error);
        this.snackBar.open('Erro ao carregar departamentos', 'Fechar', { duration: 3000 });
      }
    });
  }

  verificarModoEdicao(): void {
    this.produtoId = this.route.snapshot.paramMap.get('id');
    if (this.produtoId) {
      this.isEditMode = true;
      this.carregarProduto();
    }
  }

  carregarProduto(): void {
    if (!this.produtoId) return;

    this.loading = true;
    this.produtoService.getProduto(this.produtoId).subscribe({
      next: (produto) => {
        this.produtoForm.patchValue({
          codigo: produto.codigo,
          descricao: produto.descricao,
          departamentoCodigo: produto.departamentoCodigo,
          preco: produto.preco,
          status: produto.status
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar produto:', error);
        this.snackBar.open('Erro ao carregar produto', 'Fechar', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.produtoForm.valid) {
      this.loading = true;
      const produtoData: ProdutoDto = this.produtoForm.value;

      if (this.isEditMode && this.produtoId) {
        this.produtoService.updateProduto(this.produtoId, produtoData).subscribe({
          next: () => {
            this.snackBar.open('Produto atualizado com sucesso', 'Fechar', { duration: 3000 });
            this.router.navigate(['/produtos']);
          },
          error: (error) => {
            console.error('Erro ao atualizar produto:', error);
            this.snackBar.open('Erro ao atualizar produto', 'Fechar', { duration: 3000 });
            this.loading = false;
          }
        });
      } else {
        this.produtoService.createProduto(produtoData).subscribe({
          next: () => {
            this.snackBar.open('Produto criado com sucesso', 'Fechar', { duration: 3000 });
            this.router.navigate(['/produtos']);
          },
          error: (error) => {
            console.error('Erro ao criar produto:', error);
            this.snackBar.open('Erro ao criar produto', 'Fechar', { duration: 3000 });
            this.loading = false;
          }
        });
      }
    } else {
      this.marcarCamposInvalidos();
    }
  }

  marcarCamposInvalidos(): void {
    Object.keys(this.produtoForm.controls).forEach(key => {
      const control = this.produtoForm.get(key);
      if (control?.invalid) {
        control.markAsTouched();
      }
    });
  }

  cancelar(): void {
    this.router.navigate(['/produtos']);
  }

  getErrorMessage(controlName: string): string {
    const control = this.produtoForm.get(controlName);
    if (control?.hasError('required')) {
      return 'Este campo é obrigatório';
    }
    if (control?.hasError('maxlength')) {
      return `Máximo de ${control.errors?.['maxlength'].requiredLength} caracteres`;
    }
    if (control?.hasError('min')) {
      return 'Valor deve ser maior que zero';
    }
    return '';
  }
}
