-- Script completo para configurar o banco de dados
-- Execute este script no PostgreSQL para criar todas as tabelas necessárias

-- 1. Criar tabela de produtos (se não existir)
CREATE TABLE IF NOT EXISTS produto (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    codigo VARCHAR(50) NOT NULL UNIQUE,
    descricao VARCHAR(200) NOT NULL,
    departamento_codigo VARCHAR(10) NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    status BOOLEAN DEFAULT TRUE,
    excluido BOOLEAN DEFAULT FALSE,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP,
    data_exclusao TIMESTAMP
);

-- 2. Criar tabela de usuários (se não existir)
CREATE TABLE IF NOT EXISTS usuario (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL,
    excluido BOOLEAN DEFAULT FALSE,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP,
    data_exclusao TIMESTAMP
);

-- 3. Criar índices para melhor performance
CREATE INDEX IF NOT EXISTS idx_produto_codigo ON produto(codigo) WHERE excluido = FALSE;
CREATE INDEX IF NOT EXISTS idx_produto_excluido ON produto(excluido);
CREATE INDEX IF NOT EXISTS idx_usuario_email ON usuario(email) WHERE excluido = FALSE;
CREATE INDEX IF NOT EXISTS idx_usuario_excluido ON usuario(excluido);

-- 4. Inserir dados de exemplo

-- Produtos de exemplo
INSERT INTO produto (id, codigo, descricao, departamento_codigo, preco, status) VALUES 
    ('11111111-1111-1111-1111-111111111111', 'PROD001', 'Coca-Cola 2L', '010', 8.50, true),
    ('22222222-2222-2222-2222-222222222222', 'PROD002', 'Sorvete de Chocolate', '020', 12.90, true),
    ('33333333-3333-3333-3333-333333333333', 'PROD003', 'Queijo Minas', '030', 15.75, true),
    ('44444444-4444-4444-4444-444444444444', 'PROD004', 'Alface Crespa', '040', 3.50, true)
ON CONFLICT (codigo) DO NOTHING;

-- Usuário de teste (senha: 123456)
INSERT INTO usuario (id, nome, email, senha_hash) VALUES (
    '550e8400-e29b-41d4-a716-446655440000',
    'Administrador',
    'admin@teste.com',
    'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg='
) ON CONFLICT (email) DO NOTHING;

-- Verificar se as tabelas foram criadas
SELECT 'Tabelas criadas com sucesso!' as status;
SELECT COUNT(*) as total_produtos FROM produto WHERE excluido = FALSE;
SELECT COUNT(*) as total_usuarios FROM usuario WHERE excluido = FALSE; 