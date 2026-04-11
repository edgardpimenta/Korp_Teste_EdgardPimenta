-- Tabela de Produtos (gerenciada pelo Serviço de Estoque)
CREATE TABLE IF NOT EXISTS produtos (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL UNIQUE,
    descricao VARCHAR(255) NOT NULL,
    saldo INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Tabela de Notas Fiscais (gerenciada pelo Serviço de Faturamento)
CREATE TABLE IF NOT EXISTS notas_fiscais (
    id SERIAL PRIMARY KEY,
    numeracao INTEGER NOT NULL UNIQUE,
    status VARCHAR(10) NOT NULL DEFAULT 'Aberta' CHECK (status IN ('Aberta', 'Fechada')),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Tabela de Itens da Nota
CREATE TABLE IF NOT EXISTS itens_nota (
    id SERIAL PRIMARY KEY,
    nota_fiscal_id INTEGER NOT NULL REFERENCES notas_fiscais(id),
    produto_id INTEGER NOT NULL REFERENCES produtos(id),
    quantidade INTEGER NOT NULL CHECK (quantidade > 0),
    created_at TIMESTAMP DEFAULT NOW()
);

-- Sequência para numeração automática das notas
CREATE SEQUENCE IF NOT EXISTS seq_nota_fiscal START 1;

-- Índices para performance
CREATE INDEX IF NOT EXISTS idx_itens_nota_nota_id ON itens_nota(nota_fiscal_id);
CREATE INDEX IF NOT EXISTS idx_itens_nota_produto_id ON itens_nota(produto_id);