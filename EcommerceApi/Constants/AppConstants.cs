namespace EcommerceApi.Constants
{
    public static class AppConstants
    {
        // Mensagens de validação
        public static class ValidationMessages
        {
            public const string CampoObrigatorio = "Este campo é obrigatório";
            public const string PrecoMinimo = "O preço deve ser maior que zero";
            public const string CodigoJaExiste = "Código já existe";
            public const string ProdutoNaoEncontrado = "Produto não encontrado";
        }

        // Mensagens de sucesso
        public static class SuccessMessages
        {
            public const string ProdutoCriado = "Produto criado com sucesso";
            public const string ProdutoAtualizado = "Produto atualizado com sucesso";
            public const string ProdutoExcluido = "Produto excluído com sucesso";
        }

        // Mensagens de erro
        public static class ErrorMessages
        {
            public const string ErroInterno = "Erro interno do servidor";
            public const string ErroCarregarProdutos = "Erro ao carregar produtos";
            public const string ErroCriarProduto = "Erro ao criar produto";
            public const string ErroAtualizarProduto = "Erro ao atualizar produto";
            public const string ErroExcluirProduto = "Erro ao excluir produto";
        }

        // Validações
        public static class Validation
        {
            public const int CodigoMaxLength = 50;
            public const int DescricaoMaxLength = 255;
            public const decimal PrecoMinimo = 0.01m;
        }

        // Departamentos
        public static class Departamentos
        {
            public const string Bebidas = "010";
            public const string Congelados = "020";
            public const string Laticinios = "030";
            public const string Vegetais = "040";
        }
    }
} 