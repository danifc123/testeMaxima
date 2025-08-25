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
import { LoginDto } from '../../models/usuario';
import { APP_CONSTANTS } from '../../constants/app.constants';
import { Validators as CustomValidators } from '../../utils/validators';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
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
export class LoginComponent {
    loginForm: FormGroup;
    loadingState: 'idle' | 'loading' | 'success' | 'error' = 'idle';

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private snackBar: MatSnackBar
    ) {
        this.loginForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            senha: ['', [Validators.required, Validators.minLength(APP_CONSTANTS.VALIDATION.MIN_LENGTH_SENHA)]]
        });
    }

    get isLoading(): boolean {
        return this.loadingState === 'loading';
    }

    getErrorMessage(controlName: string): string {
        const control = this.loginForm.get(controlName);
        return CustomValidators.getErrorMessage(control);
    }

    isFieldInvalid(controlName: string): boolean {
        const control = this.loginForm.get(controlName);
        return CustomValidators.isFieldInvalid(control);
    }

    onSubmit(): void {
        if (this.loginForm.valid) {
            this.setLoadingState('loading');

            const loginData: LoginDto = this.loginForm.value;

            this.authService.login(loginData).subscribe({
                next: (response) => {
                    setTimeout(() => {
                        this.authService.saveToken(response.data.token);
                        this.authService.saveUser(response.data.usuario);
                        this.setLoadingState('success');

                        this.snackBar.open(response.message, 'Fechar', {
                            duration: APP_CONSTANTS.SNACKBAR_DURATION,
                            horizontalPosition: 'center',
                            verticalPosition: 'top'
                        });

                        this.router.navigate([APP_CONSTANTS.ROUTES.PRODUTOS]);
                    }, 0);
                },
                error: (error) => {
                    setTimeout(() => {
                        this.setLoadingState('error');
                        const errorMessage = error.error?.message || APP_CONSTANTS.MESSAGES.ERRO_LOGIN;

                        this.snackBar.open(errorMessage, 'Fechar', {
                            duration: APP_CONSTANTS.SNACKBAR_DURATION,
                            horizontalPosition: 'center',
                            verticalPosition: 'top'
                        });
                    }, 0);
                }
            });
        } else {
            CustomValidators.markInvalidFieldsAsTouched(this.loginForm);
        }
    }

    goToRegister(): void {
        this.router.navigate([APP_CONSTANTS.ROUTES.REGISTER]);
    }

    private setLoadingState(state: 'idle' | 'loading' | 'success' | 'error'): void {
        this.loadingState = state;
    }
}
