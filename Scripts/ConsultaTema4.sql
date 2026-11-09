
-- =========================================================
-- RETROGAMES | Etapa 5 de 6
-- Consultas e testes de integridade
-- Especificações 4.2 e 4.3; Fundamentos, tema 4
-- Banco de dados: MySQL 8
--
-- Execute as consultas e os testes separadamente.
-- Os resultados observados foram registrados após a execução.
-- =========================================================


-- ---------------------------------------------------------
-- CONSULTA 1: Catálogo de jogos ativos por nome
-- ---------------------------------------------------------
SELECT
    j.co_seq_jogo,
    j.no_jogo,
    p.no_plataforma,
    c.ds_condicao_jogo
FROM tb_jogo AS j
INNER JOIN tb_plataforma AS p
    ON p.co_seq_plataforma = j.co_plataforma
INNER JOIN tb_condicao_jogo AS c
    ON c.co_condicao_jogo = j.co_condicao_jogo
WHERE j.st_ativo = 1
ORDER BY j.no_jogo;

-- Resultado observado:
-- Executada com sucesso.
-- Retornou os jogos ativos com nome, plataforma e condição,
-- ordenados alfabeticamente pelo nome do jogo.


-- ---------------------------------------------------------
-- CONSULTA 2: Gêneros de um jogo informado
-- ---------------------------------------------------------
SET @id_jogo = 1;

SELECT
    g.co_seq_genero,
    g.no_genero
FROM tb_jogo_genero AS jg
INNER JOIN tb_genero AS g
    ON g.co_seq_genero = jg.co_genero
WHERE jg.co_jogo = @id_jogo
ORDER BY g.no_genero;

-- Resultado observado:
-- Executada com sucesso.
-- O jogo consultado possui os gêneros Ação e Aventura.


-- ---------------------------------------------------------
-- CONSULTA 3: Jogos de um gênero informado
-- ---------------------------------------------------------
SET @id_genero = 1;

SELECT
    j.co_seq_jogo,
    j.no_jogo,
    j.nu_ano_lancamento,
    j.vl_preco,
    j.qt_estoque,
    j.st_ativo
FROM tb_jogo_genero AS jg
INNER JOIN tb_jogo AS j
    ON j.co_seq_jogo = jg.co_jogo
WHERE jg.co_genero = @id_genero
ORDER BY j.no_jogo;

-- Resultado observado:
-- Executada com sucesso.
-- Retornou os jogos associados ao gênero informado,
-- com ano, preço, estoque e situação, ordenados pelo nome.


-- ---------------------------------------------------------
-- CONSULTA 4: Quantidade de jogos por plataforma
-- Inclui plataformas sem jogos.
-- ---------------------------------------------------------
SELECT
    p.co_seq_plataforma,
    p.no_plataforma,
    COUNT(j.co_seq_jogo) AS quantidade_jogos
FROM tb_plataforma AS p
LEFT JOIN tb_jogo AS j
    ON j.co_plataforma = p.co_seq_plataforma
GROUP BY
    p.co_seq_plataforma,
    p.no_plataforma
ORDER BY p.no_plataforma;

-- Resultado observado:
-- Executada com sucesso.
-- Retornou a quantidade de jogos por plataforma.
-- A consulta utiliza LEFT JOIN para incluir plataformas
-- sem jogos, caso existam.


-- ---------------------------------------------------------
-- CONSULTA 5: Busca parcial do nome, 5 itens por página
-- LIMIT 0, 5 corresponde à primeira página.
-- Para a página 2, utilize LIMIT 5, 5.
-- Para a página 3, utilize LIMIT 10, 5.
-- ---------------------------------------------------------
SELECT
    j.co_seq_jogo,
    j.no_jogo,
    j.nu_ano_lancamento,
    j.vl_preco,
    j.qt_estoque,
    j.st_ativo
FROM tb_jogo AS j
WHERE j.no_jogo LIKE '%mario%'
ORDER BY j.no_jogo
LIMIT 0, 5;

-- Resultado observado:
-- Executada com sucesso.
-- A busca parcial por 'mario' retornou 3 jogos.
-- A consulta limita o resultado a 5 itens por página.
-- =========================================================
-- TESTES DE INTEGRIDADE
-- =========================================================


-- ---------------------------------------------------------
-- TESTE 1: Inserir jogo com co_plataforma inexistente
-- Resultado esperado: Erro 1452 (chave estrangeira).
-- Resultado observado:
-- MySQL Error (1452): Cannot add or update a child row:
-- a foreign key constraint fails (`retrogames`.`tb_jogo`,
-- CONSTRAINT `fk_jogo_plataforma` FOREIGN KEY
-- (`co_plataforma`) REFERENCES `tb_plataforma`
-- (`co_seq_plataforma`)).
--
-- O banco impediu a inserção do jogo devido à referência
-- a uma plataforma inexistente.
--
-- Utilize um co_plataforma que não exista e um
-- co_condicao_jogo válido.
-- ---------------------------------------------------------
-- INSERT INTO tb_jogo
--     (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco,
--      qt_estoque, st_ativo, dt_cadastro, co_plataforma,
--      co_condicao_jogo)
-- VALUES
--     ('Teste FK plataforma', NULL, 2024, 10.00,
--      1, 1, UTC_TIMESTAMP(), 999999, 1);


-- ---------------------------------------------------------
-- TESTE 2: Inserir plataforma com nome já cadastrado
-- Resultado esperado: Erro 1062 (UNIQUE).
-- Resultado observado:
-- Erro 1062 (UNIQUE).
-- O banco impediu o cadastro de uma plataforma com nome
-- duplicado, respeitando a restrição de unicidade.
--
-- Substitua o nome abaixo por um nome de plataforma existente.
-- ---------------------------------------------------------
-- INSERT INTO tb_plataforma
--     (no_plataforma, no_fabricante, nu_ano_lancamento)
-- VALUES
--     ('[NOME JÁ CADASTRADO]', 'Fabricante teste', 2020);


-- ---------------------------------------------------------
-- TESTE 3: Excluir plataforma que possui jogos
-- Resultado esperado: Erro 1451 (chave estrangeira).
-- Resultado observado:
-- Erro 1451 (chave estrangeira).
-- O banco impediu a exclusão da plataforma porque existem
-- jogos associados a ela.
--
-- Utilize um co_seq_plataforma que possua jogos associados.
-- ---------------------------------------------------------
-- DELETE FROM tb_plataforma
-- WHERE co_seq_plataforma = 1;


-- ---------------------------------------------------------
-- TESTE 4: Inserir jogo com no_jogo NULL
-- Resultado esperado: Erro 1048 (NOT NULL).
-- Resultado observado:
-- Erro 1048 (NOT NULL).
-- O banco impediu a inserção do jogo porque o campo
-- no_jogo é obrigatório e não aceita valores nulos.
--
-- Utilize IDs existentes para plataforma e condição.
-- ---------------------------------------------------------
-- INSERT INTO tb_jogo
--     (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco,
--      qt_estoque, st_ativo, dt_cadastro, co_plataforma,
--      co_condicao_jogo)
-- VALUES
--     (NULL, NULL, 2024, 10.00, 1, 1,
--      UTC_TIMESTAMP(), 1, 1);


-- ---------------------------------------------------------
-- TESTE 5: Inserir jogo com vl_preco igual a zero
-- Resultado esperado: Aceito; a regra fica no Service (RN06).
-- Resultado observado:
-- Inserção aceita com sucesso.
-- O jogo foi cadastrado com preço igual a zero.
-- A validação dessa regra fica na camada Service (RN06).
--
-- Utilize IDs existentes para plataforma e condição.
-- Este teste insere um registro no banco.
-- ---------------------------------------------------------
-- INSERT INTO tb_jogo
--     (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco,
--      qt_estoque, st_ativo, dt_cadastro, co_plataforma,
--      co_condicao_jogo)
-- VALUES
--     ('Teste preco zero', NULL, 2024, 0.00, 1, 1,
--      UTC_TIMESTAMP(), 1, 1);

-- =========================================================
-- FIM DO ARQUIVO
-- =========================================================