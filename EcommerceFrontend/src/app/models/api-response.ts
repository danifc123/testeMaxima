// Interfaces para respostas da API
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  error: string;
  statusCode?: number;
}

// Estados de loading
export type LoadingState = 'idle' | 'loading' | 'success' | 'error';

// Operações CRUD
export type CrudOperation = 'create' | 'read' | 'update' | 'delete';
