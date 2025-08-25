// Constantes da aplicação
export const APP_CONSTANTS = {
  // API
  API_BASE_URL: 'http://localhost:7001/api',

  // Timeouts
  LOADING_TIMEOUT: 10000, // 10 segundos
  SNACKBAR_DURATION: 3000, // 3 segundos

  // Rotas
  ROUTES: {
    PRODUTOS: '/produtos',
    PRODUTO_NOVO: '/produtos/novo',
    PRODUTO_EDITAR: '/produtos/editar',
    LOGIN: '/login',
    REGISTER: '/register'
  },

  // Departamentos
  DEPARTAMENTOS: {
    BEBIDAS: { codigo: '010', descricao: 'BEBIDAS' },
    CONGELADOS: { codigo: '020', descricao: 'CONGELADOS' },
    LATICINIOS: { codigo: '030', descricao: 'LATICINIOS' },
    VEGETAIS: { codigo: '040', descricao: 'VEGETAIS' }
  },

  // Validações
  VALIDATION: {
    CODIGO_MAX_LENGTH: 50,
    DESCRICAO_MAX_LENGTH: 255,
    PRECO_MIN: 0.01,
    MIN_LENGTH_SENHA: 6
  },

  // Mensagens
  MESSAGES: {
    PRODUTO: {
      CRIADO: 'Produto criado com sucesso',
      ATUALIZADO: 'Produto atualizado com sucesso',
      EXCLUIDO: 'Produto excluído com sucesso',
      ERRO_CARREGAR: 'Erro ao carregar produtos',
      ERRO_CRIAR: 'Erro ao criar produto',
      ERRO_ATUALIZAR: 'Erro ao atualizar produto',
      ERRO_EXCLUIR: 'Erro ao excluir produto',
      ERRO_DEPARTAMENTOS: 'Erro ao carregar departamentos'
    },
    VALIDATION: {
      CAMPO_OBRIGATORIO: 'Este campo é obrigatório',
      PRECO_MINIMO: 'Valor deve ser maior que zero',
      CODIGO_EXISTE: 'Código já existe'
    },
    ERRO_LOGIN: 'Erro ao realizar login'
  }
} as const;
