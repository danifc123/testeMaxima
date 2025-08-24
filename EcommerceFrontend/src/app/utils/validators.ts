import { AbstractControl } from '@angular/forms';
import { APP_CONSTANTS } from '../constants/app.constants';

// Utilitários para validação
export class Validators {
  /**
   * Obtém mensagem de erro para um campo
   */
  static getErrorMessage(control: AbstractControl | null): string {
    if (!control) return '';

    if (control.hasError('required')) {
      return APP_CONSTANTS.MESSAGES.VALIDATION.CAMPO_OBRIGATORIO;
    }

    if (control.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `Máximo de ${maxLength} caracteres`;
    }

    if (control.hasError('min')) {
      return APP_CONSTANTS.MESSAGES.VALIDATION.PRECO_MINIMO;
    }

    return '';
  }

  /**
   * Verifica se um campo é inválido e foi tocado
   */
  static isFieldInvalid(control: AbstractControl | null): boolean {
    return !!(control && control.invalid && control.touched);
  }

  /**
   * Marca todos os campos inválidos como tocados
   */
  static markInvalidFieldsAsTouched(form: AbstractControl): void {
    Object.keys(form.value).forEach(key => {
      const control = form.get(key);
      if (control?.invalid) {
        control.markAsTouched();
      }
    });
  }
}
