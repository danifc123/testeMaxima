namespace EcommerceApi.Utils
{
    public static class FormatUtils
    {
        // Obtém a descrição do departamento pelo código
        public static string GetDepartamentoDescricao(string codigo)
        {
            return codigo switch
            {
                "010" => "BEBIDAS",
                "020" => "CONGELADOS",
                "030" => "LATICINIOS",
                "040" => "VEGETAIS",
                _ => "NÃO DEFINIDO"
            };
        }

        // Formata preço para exibição
        public static string FormatPreco(decimal preco)
        {
            return preco.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
        }

        // Obtém o texto do status
        public static string GetStatusText(bool status)
        {
            return status ? "Ativo" : "Inativo";
        }
    }
} 