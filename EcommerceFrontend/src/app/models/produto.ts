export interface Produto {
  id: string;
  codigo: string;
  descricao: string;
  departamentoCodigo: string;
  departamentoDescricao: string;
  preco: number;
  status: boolean;
  dataCriacao: string;
}

export interface ProdutoDto {
  codigo: string;
  descricao: string;
  departamentoCodigo: string;
  preco: number;
  status: boolean;
}
