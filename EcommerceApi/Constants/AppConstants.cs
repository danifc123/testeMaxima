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
            public const string NomeObrigatorio = "Nome é obrigatório";
            public const string EmailObrigatorio = "Email é obrigatório";
            public const string EmailInvalido = "Email inválido";
            public const string SenhaObrigatoria = "Senha é obrigatória";
            public const string SenhaMinima = "Senha deve ter pelo menos 6 caracteres";
            public const string EmailJaExiste = "Email já cadastrado";
            public const string CredenciaisInvalidas = "Email ou senha inválidos";
        }

        // Mensagens de sucesso
        public static class SuccessMessages
        {
            public const string ProdutoCriado = "Produto criado com sucesso";
            public const string ProdutoAtualizado = "Produto atualizado com sucesso";
            public const string ProdutoExcluido = "Produto excluído com sucesso";
            public const string UsuarioCriado = "Usuário criado com sucesso";
            public const string LoginRealizado = "Login realizado com sucesso";
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
            public const int MaxLengthNome = 100;
            public const int MaxLengthEmail = 100;
            public const int MinLengthSenha = 6;
        }

        // Departamentos
        public static class Departamentos
        {
            public const string Bebidas = "010";
            public const string Congelados = "020";
            public const string Laticinios = "030";
            public const string Vegetais = "040";
        }

        public static class Auth
        {
            public const string JwtSecret = "sua_chave_secreta_muito_segura_aqui_2024";
            public const string JwtIssuer = "EcommerceApi";
            public const string JwtAudience = "EcommerceFrontend";
            public const int TokenExpirationHours = 24;
        }
    }
} 