// Utilitários para formatação
export class Formatters {
  /**
   * Formata preço para moeda brasileira
   */
  static formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  }

  /**
   * Obtém a descrição do departamento pelo código
   */
  static getDepartamentoDescricao(codigo: string): string {
    const departamentos = {
      '010': 'BEBIDAS',
      '020': 'CONGELADOS',
      '030': 'LATICINIOS',
      '040': 'VEGETAIS'
    };

    return departamentos[codigo as keyof typeof departamentos] || 'NÃO DEFINIDO';
  }

  /**
   * Obtém a classe CSS para o status
   */
  static getStatusClass(status: boolean): string {
    return status ? 'status-ativo' : 'status-inativo';
  }

  /**
   * Obtém o texto do status
   */
  static getStatusText(status: boolean): string {
    return status ? 'Ativo' : 'Inativo';
  }
}
