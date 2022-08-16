using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Globo.PIC.Infra.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_CATEGORIA",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_categoria = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    nm_categoria = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CATEGORIA", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_CATEGORIA_TB_CATEGORIA_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "TB_CATEGORIA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_CONTEUDO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_codigo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_nome = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_status = table.Column<string>(type: "VARCHAR(1)", maxLength: 1, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_sigiloso = table.Column<sbyte>(type: "TINYINT(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTEUDO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_DEPARTAMENTO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_departamento = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DEPARTAMENTO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ds_titulo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_criacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    ds_link = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_STATUS_PEDIDO_ARTE",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_status = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_STATUS_PEDIDO_ARTE", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_STATUS_PEDIDO_ITEM_ARTE",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_status = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_STATUS_PEDIDO_ITEM_ARTE", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_STATUS_PEDIDO_ITEM_VEICULO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_status = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_STATUS_PEDIDO_ITEM_VEICULO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_STATUS_PEDIDO_VEICULO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_status = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_STATUS_PEDIDO_VEICULO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_UNIDADE_NEGOCIO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_codigo = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_codigo_oi = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_uf = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_UNIDADE_NEGOCIO", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ITEM",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_tipo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_subcategoria = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    vl_valor = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    nm_fornecedor = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_fornecedor = table.Column<string>(type: "VARCHAR(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_descricao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_unidade_medida = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_nro_acordo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_cod_item = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nr_linha_acordo = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_acordo_juridico = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_tipo_negociacao = table.Column<long>(type: "BIGINT(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ITEM", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_TB_CATEGORIA_id_subcategoria",
                        column: x => x.id_subcategoria,
                        principalTable: "TB_CATEGORIA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_TB_CATEGORIA_id_tipo",
                        column: x => x.id_tipo,
                        principalTable: "TB_CATEGORIA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO_ASSOCIADOS",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ds_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_role = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_notificacao = table.Column<long>(type: "BIGINT(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO_ASSOCIADOS", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_ASSOCIADOS_TB_NOTIFICACAO_id_notificacao",
                        column: x => x.id_notificacao,
                        principalTable: "TB_NOTIFICACAO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO_LIDAS",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ds_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_notificacao = table.Column<long>(type: "BIGINT(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO_LIDAS", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_LIDAS_TB_NOTIFICACAO_id_notificacao",
                        column: x => x.id_notificacao,
                        principalTable: "TB_NOTIFICACAO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO_VIZUALIZADAS",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ds_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_notificacao = table.Column<long>(type: "BIGINT(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO_VIZUALIZADAS", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_VIZUALIZADAS_TB_NOTIFICACAO_id_notificacao",
                        column: x => x.id_notificacao,
                        principalTable: "TB_NOTIFICACAO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_nome = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_apelido = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_sobrenome = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_email = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_departamento = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    id_unidade_negocio = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    st_ativo = table.Column<sbyte>(type: "TINYINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.nm_login);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_DEPARTAMENTO_id_departamento",
                        column: x => x.id_departamento,
                        principalTable: "TB_DEPARTAMENTO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_UNIDADE_NEGOCIO_id_unidade_negocio",
                        column: x => x.id_unidade_negocio,
                        principalTable: "TB_UNIDADE_NEGOCIO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ITEM_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ITEM_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_ANEXO_TB_ITEM_id_item",
                        column: x => x.id_item,
                        principalTable: "TB_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ITEM_CATALOGO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_conteudo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    st_bloqueado = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_justificativa_bloqueio = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_ativo_ate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_inicio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_fim = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_ativo = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ITEM_CATALOGO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_CATALOGO_TB_ITEM_id_item",
                        column: x => x.id_item,
                        principalTable: "TB_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dt_pedido = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_itens = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    vl_total = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    nm_titulo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_conteudo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_projeto = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_tarefa = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_tarefa_descricao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_local_entrega = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_criacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_criado_por = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_atualizado_por = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_ativo = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    nm_finalidade = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_centro_custo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_conta = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_devolucao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa_devolucao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_cancelamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_cancelamento = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa_cancelamento = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_tipo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_tag = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_organizacao = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_codigo_organizacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_localizacao = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_codigo_localizacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_TB_CONTEUDO_id_conteudo",
                        column: x => x.id_conteudo,
                        principalTable: "TB_CONTEUDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_TB_USUARIO_nm_atualizado_por",
                        column: x => x.nm_atualizado_por,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_TB_USUARIO_nm_criado_por",
                        column: x => x.nm_criado_por,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_TB_USUARIO_nm_login_cancelamento",
                        column: x => x.nm_login_cancelamento,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_TB_USUARIO_nm_login_devolucao",
                        column: x => x.nm_login_devolucao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ROLES_USUARIO",
                columns: table => new
                {
                    nm_login = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_role = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ROLES_USUARIO", x => new { x.nm_role, x.nm_login });
                    table.ForeignKey(
                        name: "FK_TB_ROLES_USUARIO_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_CONTEUDOS",
                columns: table => new
                {
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_conteudo = table.Column<long>(type: "BIGINT(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_CONTEUDOS", x => new { x.id_conteudo, x.nm_login });
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_CONTEUDOS_TB_CONTEUDO_id_conteudo",
                        column: x => x.id_conteudo,
                        principalTable: "TB_CONTEUDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_CONTEUDOS_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ANEXO_TB_PEDIDO_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ARTE",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    dt_necessario = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_local_utilizacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_cena = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    dt_solicitacao_cancelamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_confirmacao_compra = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_base = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_vinculo_base = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_reenvio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_edicao_reenvio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_pedido_alimentos = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    st_fastpass = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ARTE", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ARTE_TB_PEDIDO_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ARTE_TB_STATUS_PEDIDO_ARTE_id_status",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ARTE_TB_USUARIO_nm_login_base",
                        column: x => x.nm_login_base,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_EQUIPE",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_EQUIPE", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_EQUIPE_TB_PEDIDO_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_EQUIPE_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    cd_numero = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true, defaultValue: "0")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    id_item = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    nr_qtd = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    vl_valor_itens = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    vl_valor = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    vl_valor_unitario = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    dt_necessario = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_local_entrega = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_entrega = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_item = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_descricao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_unidade_medida = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa_cancelamento = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa_devolucao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_login_cancelamento = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_cancelamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_devolucao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_aprovacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_aprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_reprovacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_reprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_ITEM_id_item",
                        column: x => x.id_item,
                        principalTable: "TB_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_PEDIDO_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_USUARIO_nm_login_aprovacao",
                        column: x => x.nm_login_aprovacao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_USUARIO_nm_login_cancelamento",
                        column: x => x.nm_login_cancelamento,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_USUARIO_nm_login_devolucao",
                        column: x => x.nm_login_devolucao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_TB_USUARIO_nm_login_reprovacao",
                        column: x => x.nm_login_reprovacao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_VEICULO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login_acionamento = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_acionamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_chegada = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_local_faturamento = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login_comprador = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_pre_producao = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    dt_envio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_VEICULO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_VEICULO_TB_PEDIDO_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_VEICULO_TB_STATUS_PEDIDO_VEICULO_id_status",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_VEICULO_TB_USUARIO_nm_login_acionamento",
                        column: x => x.nm_login_acionamento,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_VEICULO_TB_USUARIO_nm_login_comprador",
                        column: x => x.nm_login_comprador,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ANEXO_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nr_qtd_pendente_compra = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nr_qtd_pendente_entrega = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nr_qtd_entregue = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nr_qtd_comprada = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nr_qtd_devolvida = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    st_marcacao_cena = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_referencias = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_sugestao_fornecedor = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_solicitacao_dirigida = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    nm_login_comprador = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_vinculo_comprador = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_visualizacao_comprador = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    id_tipo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    dt_reenvio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_edicao_reenvio = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nr_qtd_aprovacao_compra = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    dt_entrega_prevista = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_observacao_aprovacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_devolvido_base = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    st_devolvido_comprador = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    st_item_nao_encontrado = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TB_STATUS_PEDIDO_ITEM_ARTE_id_status",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TB_USUARIO_nm_login_comprador",
                        column: x => x.nm_login_comprador,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_CONVERSA",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoitem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    ds_conversa = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_conversa = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    id_pedidoitem_conversa_pai = table.Column<long>(type: "BIGINT(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_CONVERSA", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_CONVERSA_TB_PEDIDO_ITEM_id_pedidoitem",
                        column: x => x.id_pedidoitem,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_CONVERSA_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_VEICULO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_personagem_utilizacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_modelo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_tipo_veiculos = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_subcategoria_veiculos = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nr_opcoes = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nr_ano = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_cor = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_chegada = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_continuidade = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    st_cena_acao = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobrecenaacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_origem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_localgravacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nr_passageiros = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_horasvoo = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_horasparado = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_tag = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_necessidades = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vl_valor_maximo = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    nm_local_faturamento = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_devolver = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_justificativa_devolver = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_VEICULO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TB_CATEGORIA_id_subcategoria_veiculos",
                        column: x => x.id_subcategoria_veiculos,
                        principalTable: "TB_CATEGORIA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TB_CATEGORIA_id_tipo_veiculos",
                        column: x => x.id_tipo_veiculos,
                        principalTable: "TB_CATEGORIA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TB_STATUS_PEDIDO_ITEM_VEICULO_id_stat~",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_ITEM_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_RC",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    cd_oracle_head_id = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_oracle_requisition = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_oracle_lineid = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_oracle_bu = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_catalog_item_key = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    cd_acordo = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_acordo = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    st_acordo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_linha_acordo = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    st_linha_acordo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_line_requisition = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_requisition = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_ordem_compra = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_fornecedor = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_fornecedor = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    ds_moeda = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_categoria = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_categoria = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    vl_valor = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    ds_tipo_documento = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_imagem_url = table.Column<string>(type: "VARCHAR(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_item = table.Column<long>(type: "BIGINT(20)", nullable: true),
                    cd_item = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_uom = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sg_uom = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_titulo = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_completa = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_RC", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_RC_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ACIONAMENTO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_veiculos = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_roteiro = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_local_entrega = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_saida = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_hora_higienizacao_set = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_inicio_gravacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_termino_gravacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_cancelamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_justificativa_cancelamento = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_outra_justificativa = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ACIONAMENTO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_TB_PEDIDO_VEICULO_id_pedido_veiculos",
                        column: x => x.id_pedido_veiculos,
                        principalTable: "TB_PEDIDO_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_ATRIBUICAO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoarteitem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_tipo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_comprador = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_comprador_anterior = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_atribuicao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_justificativa = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_TB_PEDIDO_ITEM_ARTE_id_pedido~",
                        column: x => x.id_pedidoarteitem,
                        principalTable: "TB_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_TB_USUARIO_nm_comprador",
                        column: x => x.nm_comprador,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_TB_USUARIO_nm_comprador_anter~",
                        column: x => x.nm_comprador_anterior,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoarteitem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_compra = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_qtd = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    vl_valor_compra = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    nm_observacoes = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cd_nro_documento = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_COMPRA", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_COMPRA_TB_PEDIDO_ITEM_ARTE_id_pedidoarte~",
                        column: x => x.id_pedidoarteitem,
                        principalTable: "TB_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_COMPRA_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_DEVOLUCAO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoarteitem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_pedidoarteitemOriginal = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_tipo = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_comprador = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nr_qtd = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nm_justificativa = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_DEVOLUCAO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_DEVOLUCAO_TB_PEDIDO_ITEM_ARTE_id_pedidoa~",
                        column: x => x.id_pedidoarteitem,
                        principalTable: "TB_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_DEVOLUCAO_TB_USUARIO_nm_comprador",
                        column: x => x.nm_comprador,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_ENTREGA",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoarteitem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_entrega = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_qtd = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    nm_local_entrega = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_recebedor = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_ENTREGA", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_ENTREGA_TB_PEDIDO_ITEM_ARTE_id_pedidoart~",
                        column: x => x.id_pedidoarteitem,
                        principalTable: "TB_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_ENTREGA_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_TRACKING",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    dt_tracking = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_ordem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    st_ativo = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    ds_alterado_por = table.Column<string>(type: "VARCHAR(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_TRACKING", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TRACKING_TB_PEDIDO_ITEM_ARTE_id_pedido_i~",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TRACKING_TB_STATUS_PEDIDO_ITEM_ARTE_id_s~",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_ITEM_ARTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_TRACKING_TB_USUARIO_ds_alterado_por",
                        column: x => x.ds_alterado_por,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_CONVERSA_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedidoitem_conversa = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_CONVERSA_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_CONVERSA_ANEXO_TB_PEDIDO_ITEM_CONVERSA_id_ped~",
                        column: x => x.id_pedidoitem_conversa,
                        principalTable: "TB_PEDIDO_ITEM_CONVERSA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ITEM_VEICULO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_veiculos_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_placa = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_cidade = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_chegada = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_devolucao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_observacao_comprador = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_observacao_ep = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_tipo_pagamento = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_modelo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nr_ano = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_cor = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_periodo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_ativo_ate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_aprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_reprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_justificativa_reprovacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_hora_fechamento = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_envio_aprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_bloqueio_emprestimos = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_justificativa_bloqueio = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ITEM_VEICULO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_VEICULO_TB_ITEM_id_item",
                        column: x => x.id_item,
                        principalTable: "TB_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ITEM_VEICULO_TB_PEDIDO_ITEM_VEICULO_id_pedido_veiculos_it~",
                        column: x => x.id_pedido_veiculos_item,
                        principalTable: "TB_PEDIDO_ITEM_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_VEICULO_TRACKING",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    dt_tracking = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_ordem = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_status = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    st_ativo = table.Column<sbyte>(type: "TINYINT(4)", nullable: false),
                    ds_alterado_por = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_VEICULO_TRACKING", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TRACKING_TB_PEDIDO_ITEM_VEICULO_id_pe~",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TRACKING_TB_STATUS_PEDIDO_ITEM_VEICUL~",
                        column: x => x.id_status,
                        principalTable: "TB_STATUS_PEDIDO_ITEM_VEICULO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_VEICULO_TRACKING_TB_USUARIO_ds_alterado_por",
                        column: x => x.ds_alterado_por,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ACIONAMENTO_ITEM",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_acionamento = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    id_pedido_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    st_cena_acao = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_cena_acao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_local_gravacao = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nr_passageiros = table.Column<int>(type: "INT", nullable: false),
                    nm_horas_voo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_horas_parado = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    st_taxat = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_local_embarque = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_periodo_utilizacao = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_cena_acao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_insulfilm = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_insulfilm = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_insulfilm = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_adesivagem = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_adesivagem = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_adesivagem = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_mecanica = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_mecanica = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_mecanica = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_motorista_cena = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_motorista_cena = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_motorista_cena = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    st_reboque = table.Column<sbyte>(type: "TINYINT(4)", nullable: false, defaultValue: (sbyte)0),
                    nm_sobre_reboque = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_confirmacao_reboque = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    dt_data_aprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_aprovacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_data_reprovacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_login_reprovacao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_justificativa_reprovacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_cancelamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    nm_justificativa_cancelamento = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_outra_justificativa = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ACIONAMENTO_ITEM", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_ITEM_TB_ACIONAMENTO_id_acionamento",
                        column: x => x.id_acionamento,
                        principalTable: "TB_ACIONAMENTO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_ITEM_TB_PEDIDO_ITEM_id_pedido_item",
                        column: x => x.id_pedido_item,
                        principalTable: "TB_PEDIDO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_ITEM_TB_USUARIO_nm_login_aprovacao",
                        column: x => x.nm_login_aprovacao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_ITEM_TB_USUARIO_nm_login_reprovacao",
                        column: x => x.nm_login_reprovacao,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_compra = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_login = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_documento = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    nr_qtd = table.Column<long>(type: "BIGINT(20)", nullable: false, defaultValue: 0L),
                    vl_valor_compra = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, defaultValue: 0m),
                    nm_fornecedor = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_observacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_TB_PEDIDO_ITEM_ARTE_COM~",
                        column: x => x.id_compra,
                        principalTable: "TB_PEDIDO_ITEM_ARTE_COMPRA",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_TB_USUARIO_nm_login",
                        column: x => x.nm_login,
                        principalTable: "TB_USUARIO",
                        principalColumn: "nm_login",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ACIONAMENTO_ITEM_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_acionamento_item = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ACIONAMENTO_ITEM_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_ACIONAMENTO_ITEM_ANEXO_TB_ACIONAMENTO_ITEM_id_acionamento~",
                        column: x => x.id_acionamento_item,
                        principalTable: "TB_ACIONAMENTO_ITEM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO",
                columns: table => new
                {
                    id = table.Column<long>(type: "BIGINT(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_documentos = table.Column<long>(type: "BIGINT(20)", nullable: false),
                    nm_arquivo = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nm_original = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ds_tipo = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dt_arquivo = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO_TB_PEDIDO_ITEM_AR~",
                        column: x => x.id_documentos,
                        principalTable: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_id_pedido_veiculos",
                table: "TB_ACIONAMENTO",
                column: "id_pedido_veiculos");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_ITEM_id_acionamento",
                table: "TB_ACIONAMENTO_ITEM",
                column: "id_acionamento");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_ITEM_id_pedido_item",
                table: "TB_ACIONAMENTO_ITEM",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_ITEM_nm_login_aprovacao",
                table: "TB_ACIONAMENTO_ITEM",
                column: "nm_login_aprovacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_ITEM_nm_login_reprovacao",
                table: "TB_ACIONAMENTO_ITEM",
                column: "nm_login_reprovacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ACIONAMENTO_ITEM_ANEXO_id_acionamento_item",
                table: "TB_ACIONAMENTO_ITEM_ANEXO",
                column: "id_acionamento_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CATEGORIA_id_categoria",
                table: "TB_CATEGORIA",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_id_subcategoria",
                table: "TB_ITEM",
                column: "id_subcategoria");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_id_tipo",
                table: "TB_ITEM",
                column: "id_tipo");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_ANEXO_id_item",
                table: "TB_ITEM_ANEXO",
                column: "id_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_CATALOGO_id_item",
                table: "TB_ITEM_CATALOGO",
                column: "id_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_VEICULO_id_item",
                table: "TB_ITEM_VEICULO",
                column: "id_item",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_ITEM_VEICULO_id_pedido_veiculos_item",
                table: "TB_ITEM_VEICULO",
                column: "id_pedido_veiculos_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_ASSOCIADOS_id_notificacao",
                table: "TB_NOTIFICACAO_ASSOCIADOS",
                column: "id_notificacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_LIDAS_id_notificacao",
                table: "TB_NOTIFICACAO_LIDAS",
                column: "id_notificacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_VIZUALIZADAS_id_notificacao",
                table: "TB_NOTIFICACAO_VIZUALIZADAS",
                column: "id_notificacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_id_conteudo",
                table: "TB_PEDIDO",
                column: "id_conteudo");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_nm_atualizado_por",
                table: "TB_PEDIDO",
                column: "nm_atualizado_por");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_nm_criado_por",
                table: "TB_PEDIDO",
                column: "nm_criado_por");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_nm_login_cancelamento",
                table: "TB_PEDIDO",
                column: "nm_login_cancelamento");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_nm_login_devolucao",
                table: "TB_PEDIDO",
                column: "nm_login_devolucao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ANEXO_id_pedido",
                table: "TB_PEDIDO_ANEXO",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ARTE_id_pedido",
                table: "TB_PEDIDO_ARTE",
                column: "id_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ARTE_id_status",
                table: "TB_PEDIDO_ARTE",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ARTE_nm_login_base",
                table: "TB_PEDIDO_ARTE",
                column: "nm_login_base");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_EQUIPE_id_pedido",
                table: "TB_PEDIDO_EQUIPE",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_EQUIPE_nm_login",
                table: "TB_PEDIDO_EQUIPE",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_id_item",
                table: "TB_PEDIDO_ITEM",
                column: "id_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_id_pedido",
                table: "TB_PEDIDO_ITEM",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_id_pedido_item",
                table: "TB_PEDIDO_ITEM",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_nm_login_aprovacao",
                table: "TB_PEDIDO_ITEM",
                column: "nm_login_aprovacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_nm_login_cancelamento",
                table: "TB_PEDIDO_ITEM",
                column: "nm_login_cancelamento");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_nm_login_devolucao",
                table: "TB_PEDIDO_ITEM",
                column: "nm_login_devolucao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_nm_login_reprovacao",
                table: "TB_PEDIDO_ITEM",
                column: "nm_login_reprovacao");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ANEXO_id_pedido_item",
                table: "TB_PEDIDO_ITEM_ANEXO",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_id_pedido_item",
                table: "TB_PEDIDO_ITEM_ARTE",
                column: "id_pedido_item",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_id_status",
                table: "TB_PEDIDO_ITEM_ARTE",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_nm_login_comprador",
                table: "TB_PEDIDO_ITEM_ARTE",
                column: "nm_login_comprador");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_id_pedidoarteitem",
                table: "TB_PEDIDO_ITEM_ARTE_ATRIBUICAO",
                column: "id_pedidoarteitem");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_nm_comprador",
                table: "TB_PEDIDO_ITEM_ARTE_ATRIBUICAO",
                column: "nm_comprador");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_ATRIBUICAO_nm_comprador_anterior",
                table: "TB_PEDIDO_ITEM_ARTE_ATRIBUICAO",
                column: "nm_comprador_anterior");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_COMPRA_id_pedidoarteitem",
                table: "TB_PEDIDO_ITEM_ARTE_COMPRA",
                column: "id_pedidoarteitem");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_COMPRA_nm_login",
                table: "TB_PEDIDO_ITEM_ARTE_COMPRA",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_id_compra",
                table: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_nm_login",
                table: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO_id_documentos",
                table: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO",
                column: "id_documentos");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_DEVOLUCAO_id_pedidoarteitem",
                table: "TB_PEDIDO_ITEM_ARTE_DEVOLUCAO",
                column: "id_pedidoarteitem");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_DEVOLUCAO_nm_comprador",
                table: "TB_PEDIDO_ITEM_ARTE_DEVOLUCAO",
                column: "nm_comprador");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_ENTREGA_id_pedidoarteitem",
                table: "TB_PEDIDO_ITEM_ARTE_ENTREGA",
                column: "id_pedidoarteitem");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_ENTREGA_nm_login",
                table: "TB_PEDIDO_ITEM_ARTE_ENTREGA",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_TRACKING_ds_alterado_por",
                table: "TB_PEDIDO_ITEM_ARTE_TRACKING",
                column: "ds_alterado_por");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_TRACKING_id_pedido_item",
                table: "TB_PEDIDO_ITEM_ARTE_TRACKING",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_ARTE_TRACKING_id_status",
                table: "TB_PEDIDO_ITEM_ARTE_TRACKING",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_CONVERSA_id_pedidoitem",
                table: "TB_PEDIDO_ITEM_CONVERSA",
                column: "id_pedidoitem");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_CONVERSA_nm_login",
                table: "TB_PEDIDO_ITEM_CONVERSA",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_CONVERSA_ANEXO_id_pedidoitem_conversa",
                table: "TB_PEDIDO_ITEM_CONVERSA_ANEXO",
                column: "id_pedidoitem_conversa");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_id_pedido_item",
                table: "TB_PEDIDO_ITEM_VEICULO",
                column: "id_pedido_item",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_id_status",
                table: "TB_PEDIDO_ITEM_VEICULO",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_id_subcategoria_veiculos",
                table: "TB_PEDIDO_ITEM_VEICULO",
                column: "id_subcategoria_veiculos");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_id_tipo_veiculos",
                table: "TB_PEDIDO_ITEM_VEICULO",
                column: "id_tipo_veiculos");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_TRACKING_ds_alterado_por",
                table: "TB_PEDIDO_ITEM_VEICULO_TRACKING",
                column: "ds_alterado_por");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_TRACKING_id_pedido_item",
                table: "TB_PEDIDO_ITEM_VEICULO_TRACKING",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_ITEM_VEICULO_TRACKING_id_status",
                table: "TB_PEDIDO_ITEM_VEICULO_TRACKING",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_VEICULO_id_pedido",
                table: "TB_PEDIDO_VEICULO",
                column: "id_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_VEICULO_id_status",
                table: "TB_PEDIDO_VEICULO",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_VEICULO_nm_login_acionamento",
                table: "TB_PEDIDO_VEICULO",
                column: "nm_login_acionamento");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PEDIDO_VEICULO_nm_login_comprador",
                table: "TB_PEDIDO_VEICULO",
                column: "nm_login_comprador");

            migrationBuilder.CreateIndex(
                name: "IX_TB_RC_id_pedido_item",
                table: "TB_RC",
                column: "id_pedido_item");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ROLES_USUARIO_nm_login",
                table: "TB_ROLES_USUARIO",
                column: "nm_login");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_id_departamento",
                table: "TB_USUARIO",
                column: "id_departamento");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_id_unidade_negocio",
                table: "TB_USUARIO",
                column: "id_unidade_negocio");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_CONTEUDOS_nm_login",
                table: "TB_USUARIO_CONTEUDOS",
                column: "nm_login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ACIONAMENTO_ITEM_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_ITEM_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_ITEM_CATALOGO");

            migrationBuilder.DropTable(
                name: "TB_ITEM_VEICULO");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO_ASSOCIADOS");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO_LIDAS");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO_VIZUALIZADAS");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ARTE");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_EQUIPE");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_ATRIBUICAO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_DEVOLUCAO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_ENTREGA");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_TRACKING");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_CONVERSA_ANEXO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_VEICULO_TRACKING");

            migrationBuilder.DropTable(
                name: "TB_RC");

            migrationBuilder.DropTable(
                name: "TB_ROLES_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_CONTEUDOS");

            migrationBuilder.DropTable(
                name: "TB_ACIONAMENTO_ITEM");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO");

            migrationBuilder.DropTable(
                name: "TB_STATUS_PEDIDO_ARTE");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_CONVERSA");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_VEICULO");

            migrationBuilder.DropTable(
                name: "TB_ACIONAMENTO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE_COMPRA");

            migrationBuilder.DropTable(
                name: "TB_STATUS_PEDIDO_ITEM_VEICULO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_VEICULO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM_ARTE");

            migrationBuilder.DropTable(
                name: "TB_STATUS_PEDIDO_VEICULO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO_ITEM");

            migrationBuilder.DropTable(
                name: "TB_STATUS_PEDIDO_ITEM_ARTE");

            migrationBuilder.DropTable(
                name: "TB_ITEM");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO");

            migrationBuilder.DropTable(
                name: "TB_CATEGORIA");

            migrationBuilder.DropTable(
                name: "TB_CONTEUDO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_DEPARTAMENTO");

            migrationBuilder.DropTable(
                name: "TB_UNIDADE_NEGOCIO");
        }
    }
}
