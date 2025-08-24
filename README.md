create BDS:
CREATE DATABASE ecommerce;

\c ecommerce;

CREATE TABLE departamento (
    codigo VARCHAR(3) PRIMARY KEY,
    descricao VARCHAR(50) NOT NULL
);

INSERT INTO departamento (codigo, descricao) VALUES
('010', 'BEBIDAS'),
('020', 'CONGELADOS'),
('030', 'LATICINIOS'),
('040', 'VEGETAIS');

CREATE TABLE produto (
    id UUID PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL,
    descricao VARCHAR(100) NOT NULL,
    departamento_codigo VARCHAR(3) NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    status BOOLEAN DEFAULT TRUE,
    excluido BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (departamento_codigo) REFERENCES departamento(codigo)
);
