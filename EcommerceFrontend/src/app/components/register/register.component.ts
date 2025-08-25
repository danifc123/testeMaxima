import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UsuarioDto } from '../../models/usuario';
import { APP_CONSTANTS } from '../../constants/app.constants';
import { Validators as CustomValidators } from '../../utils/validators';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatProgressSpinnerModule
    ]
})
export class RegisterComponent {
    registerForm: FormGroup;
    loadingState: 'idle' | 'loading' | 'success' | 'error' = 'idle';

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private snackBar: MatSnackBar
    ) {
        this.registerForm = this.fb.group({
            nome: ['', [Validators.required, Validators.maxLength(100)]],
            email: ['', [Validators.required, Validators.email]],
            senha: ['', [Validators.required, Validators.minLength(APP_CONSTANTS.VALIDATION.MIN_LENGTH_SENHA)]],
            confirmarSenha: ['', [Validators.required]]
        }, { validators: this.passwordMatchValidator });
    }

    get isLoading(): boolean {
        return this.loadingState === 'loading';
    }

    getErrorMessage(controlName: string): string {
        const control = this.registerForm.get(controlName);
        return CustomValidators.getErrorMessage(control);
    }

    isFieldInvalid(controlName: string): boolean {
        const control = this.registerForm.get(controlName);
        return CustomValidators.isFieldInvalid(control);
    }

    passwordMatchValidator(form: FormGroup): { [key: string]: any } | null {
        const senha = form.get('senha');
        const confirmarSenha = form.get('confirmarSenha');

        if (senha && confirmarSenha && senha.value !== confirmarSenha.value) {
            return { passwordMismatch: true };
        }

        return null;
    }

    onSubmit(): void {
        if (this.registerForm.valid) {
            this.setLoadingState('loading');

            const registerData: UsuarioDto = {
                nome: this.registerForm.get('nome')?.value,
                email: this.registerForm.get('email')?.value,
                senha: this.registerForm.get('senha')?.value
            };

            this.authService.register(registerData).subscribe({
                next: (response) => {
                    setTimeout(() => {
                        this.setLoadingState('success');

                        this.snackBar.open(response.message, 'Fechar', {
                            duration: APP_CONSTANTS.SNACKBAR_DURATION,
                            horizontalPosition: 'center',
                            verticalPosition: 'top'
                        });

                        this.router.navigate([APP_CONSTANTS.ROUTES.LOGIN]);
                    }, 0);
                },
                error: (error) => {
                    setTimeout(() => {
                        this.setLoadingState('error');
                        const errorMessage = error.error?.message || 'Erro ao criar conta';

                        this.snackBar.open(errorMessage, 'Fechar', {
                            duration: APP_CONSTANTS.SNACKBAR_DURATION,
                            horizontalPosition: 'center',
                            verticalPosition: 'top'
                        });
                    }, 0);
                }
            });
        } else {
            CustomValidators.markInvalidFieldsAsTouched(this.registerForm);
        }
    }

    goToLogin(): void {
        this.router.navigate([APP_CONSTANTS.ROUTES.LOGIN]);
    }

    private setLoadingState(state: 'idle' | 'loading' | 'success' | 'error'): void {
        this.loadingState = state;
    }
}
