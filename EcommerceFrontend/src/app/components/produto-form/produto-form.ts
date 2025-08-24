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
import { APP_CONSTANTS } from '../../constants/app.constants';
import { Validators as CustomValidators } from '../../utils/validators';
import { LoadingState } from '../../models/api-response';

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
  produtoForm!: FormGroup;
  departamentos: Departamento[] = [];
  isEditMode = false;
  produtoId: string | null = null;
  loadingState: LoadingState = 'idle';

  // Getters para melhor legibilidade
  get isLoading(): boolean {
    return this.loadingState === 'loading';
  }

  get isEditando(): boolean {
    return this.isEditMode;
  }

  constructor(
    private fb: FormBuilder,
    private produtoService: ProdutoService,
    private departamentoService: DepartamentoService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.inicializarFormulario();
  }

  private inicializarFormulario(): void {
    this.produtoForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(APP_CONSTANTS.VALIDATION.CODIGO_MAX_LENGTH)]],
      descricao: ['', [Validators.required, Validators.maxLength(APP_CONSTANTS.VALIDATION.DESCRICAO_MAX_LENGTH)]],
      departamentoCodigo: ['', Validators.required],
      preco: ['', [Validators.required, Validators.min(APP_CONSTANTS.VALIDATION.PRECO_MIN)]],
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
        this.handleError(APP_CONSTANTS.MESSAGES.PRODUTO.ERRO_DEPARTAMENTOS, error);
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

    this.setLoadingState('loading');
    this.produtoService.getProduto(this.produtoId).subscribe({
      next: (produto) => {
        this.preencherFormulario(produto);
        this.setLoadingState('success');
      },
      error: (error) => {
        this.handleError('Erro ao carregar produto', error);
        this.setLoadingState('error');
      }
    });
  }

  private preencherFormulario(produto: Produto): void {
    this.produtoForm.patchValue({
      codigo: produto.codigo,
      descricao: produto.descricao,
      departamentoCodigo: produto.departamentoCodigo,
      preco: produto.preco,
      status: produto.status
    });
  }

  private setLoadingState(state: LoadingState): void {
    this.loadingState = state;
  }

  private handleError(message: string, error: any): void {
    console.error(message, error);
    this.snackBar.open(message, 'Fechar', { duration: APP_CONSTANTS.SNACKBAR_DURATION });
  }

  onSubmit(): void {
    if (this.produtoForm.valid) {
      this.setLoadingState('loading');
      const produtoData: ProdutoDto = this.produtoForm.value;

      if (this.isEditando && this.produtoId) {
        this.atualizarProduto(produtoData);
      } else {
        this.criarProduto(produtoData);
      }
    } else {
      this.marcarCamposInvalidos();
    }
  }

  private atualizarProduto(produtoData: ProdutoDto): void {
    if (!this.produtoId) return;

    this.produtoService.updateProduto(this.produtoId, produtoData).subscribe({
      next: () => {
        this.snackBar.open(APP_CONSTANTS.MESSAGES.PRODUTO.ATUALIZADO, 'Fechar', { duration: APP_CONSTANTS.SNACKBAR_DURATION });
        this.navegarParaListagem();
      },
      error: (error) => {
        this.handleError(APP_CONSTANTS.MESSAGES.PRODUTO.ERRO_ATUALIZAR, error);
        this.setLoadingState('error');
      }
    });
  }

  private criarProduto(produtoData: ProdutoDto): void {
    this.produtoService.createProduto(produtoData).subscribe({
      next: () => {
        this.snackBar.open(APP_CONSTANTS.MESSAGES.PRODUTO.CRIADO, 'Fechar', { duration: APP_CONSTANTS.SNACKBAR_DURATION });
        this.navegarParaListagem();
      },
      error: (error) => {
        this.handleError(APP_CONSTANTS.MESSAGES.PRODUTO.ERRO_CRIAR, error);
        this.setLoadingState('error');
      }
    });
  }

  private navegarParaListagem(): void {
    this.router.navigate([APP_CONSTANTS.ROUTES.PRODUTOS], { replaceUrl: true });
  }

  marcarCamposInvalidos(): void {
    CustomValidators.markInvalidFieldsAsTouched(this.produtoForm);
  }

  cancelar(): void {
    this.navegarParaListagem();
  }

  getErrorMessage(controlName: string): string {
    const control = this.produtoForm.get(controlName);
    return CustomValidators.getErrorMessage(control);
  }
}
