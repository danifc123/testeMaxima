
-- Criar banco de dados (execute como superuser)
-- CREATE DATABASE ecommerce;

-- Conectar ao banco ecommerce e executar:

-- Criar tabela de produtos
CREATE TABLE IF NOT EXISTS produto (
    id UUID PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL UNIQUE,
    descricao VARCHAR(255) NOT NULL,
    departamento_codigo VARCHAR(10) NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    status BOOLEAN NOT NULL DEFAULT true,
    excluido BOOLEAN NOT NULL DEFAULT false,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP,
    data_exclusao TIMESTAMP
);

-- Criar índices para melhor performance
CREATE INDEX IF NOT EXISTS idx_produto_codigo ON produto(codigo);
CREATE INDEX IF NOT EXISTS idx_produto_excluido ON produto(excluido);
CREATE INDEX IF NOT EXISTS idx_produto_departamento ON produto(departamento_codigo);

--  alguns produtos 
INSERT INTO produto (id, codigo, descricao, departamento_codigo, preco, status) VALUES
    (gen_random_uuid(), 'PROD001', 'Coca-Cola 2L', '010', 8.50, true),
    (gen_random_uuid(), 'PROD002', 'Pizza Congelada Margherita', '020', 15.90, true),
    (gen_random_uuid(), 'PROD003', 'Queijo Minas Frescal 500g', '030', 12.75, true),
    (gen_random_uuid(), 'PROD004', 'Alface Americana', '040', 3.50, true),
    (gen_random_uuid(), 'PROD005', 'Pepsi 1L', '010', 4.25, true);

-- - id: UUID único para cada produto
-- - codigo: Código apresentável ao usuário (único)
-- - descricao: Descrição do produto
-- - departamento_codigo: Código do departamento (010, 020, 030, 040)
-- - preco: Preço do produto
-- - status: Ativo/Inativo
-- - excluido: Flag para exclusão lógica
-- - data_criacao: Data de criação do registro
-- - data_atualizacao: Data da última atualização
-- - data_exclusao: Data da exclusão lógica 