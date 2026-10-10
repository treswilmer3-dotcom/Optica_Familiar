using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpticaFamiliar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria_producto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria_producto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configuracion_sistema",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    valor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    tipo_dato = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    editable = table.Column<bool>(type: "boolean", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracion_sistema", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "empresa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    razon_social = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    nombre_comercial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ruc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sitio_web = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marca",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marca", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parametro_catalogo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametro_catalogo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permiso",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    modulo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permiso", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo_identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    numero_identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "proveedor",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ruc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    razon_social = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    nombre_comercial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    contacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proveedor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "empresa_configuracion",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    empresa_id = table.Column<long>(type: "bigint", nullable: false),
                    logo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    color_primario = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    color_secundario = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo_notificaciones = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telefono_contacto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    direccion_matriz = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    sitio_web = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    mensaje_factura = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresa_configuracion", x => x.id);
                    table.ForeignKey(
                        name: "FK_empresa_configuracion_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sucursal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    empresa_id = table.Column<long>(type: "bigint", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ciudad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    provincia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sucursal", x => x.id);
                    table.ForeignKey(
                        name: "FK_sucursal_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoria_id = table.Column<long>(type: "bigint", nullable: false),
                    marca_id = table.Column<long>(type: "bigint", nullable: true),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    codigo_barras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    costo = table.Column<decimal>(type: "numeric", nullable: false),
                    precio = table.Column<decimal>(type: "numeric", nullable: false),
                    stock_minimo = table.Column<int>(type: "integer", nullable: false),
                    stock_maximo = table.Column<int>(type: "integer", nullable: false),
                    requiere_formula = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.id);
                    table.ForeignKey(
                        name: "FK_producto_categoria_producto_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categoria_producto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_producto_marca_marca_id",
                        column: x => x.marca_id,
                        principalTable: "marca",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    persona_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id);
                    table.ForeignKey(
                        name: "FK_cliente_persona_persona_id",
                        column: x => x.persona_id,
                        principalTable: "persona",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    persona_id = table.Column<long>(type: "bigint", nullable: false),
                    ocupacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    empresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    alergias = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    antecedentes_medicos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    observaciones_generales = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paciente", x => x.id);
                    table.ForeignKey(
                        name: "FK_paciente_persona_persona_id",
                        column: x => x.persona_id,
                        principalTable: "persona",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rol_permiso",
                columns: table => new
                {
                    rol_id = table.Column<long>(type: "bigint", nullable: false),
                    permiso_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_permiso", x => new { x.rol_id, x.permiso_id });
                    table.ForeignKey(
                        name: "FK_rol_permiso_permiso_permiso_id",
                        column: x => x.permiso_id,
                        principalTable: "permiso",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rol_permiso_rol_rol_id",
                        column: x => x.rol_id,
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "caja",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caja", x => x.id);
                    table.ForeignKey(
                        name: "FK_caja_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proveedor_id = table.Column<long>(type: "bigint", nullable: false),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    numero_documento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_compra = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    iva = table.Column<decimal>(type: "numeric", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compra", x => x.id);
                    table.ForeignKey(
                        name: "FK_compra_proveedor_proveedor_id",
                        column: x => x.proveedor_id,
                        principalTable: "proveedor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_compra_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "numeracion_documento",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_documento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    serie = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    numero_actual = table.Column<int>(type: "integer", nullable: false),
                    numero_final = table.Column<int>(type: "integer", nullable: false),
                    reinicio_anual = table.Column<bool>(type: "boolean", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeracion_documento", x => x.id);
                    table.ForeignKey(
                        name: "FK_numeracion_documento_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sucursal_configuracion",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    nombre_impresora = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    impresora_facturas = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    impresora_etiquetas = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    correo_sucursal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telefono_sucursal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    direccion_sucursal = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sucursal_configuracion", x => x.id);
                    table.ForeignKey(
                        name: "FK_sucursal_configuracion_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    persona_id = table.Column<long>(type: "bigint", nullable: true),
                    rol_id = table.Column<long>(type: "bigint", nullable: false),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ultimo_acceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_persona_persona_id",
                        column: x => x.persona_id,
                        principalTable: "persona",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_usuario_rol_rol_id",
                        column: x => x.rol_id,
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventario",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    producto_id = table.Column<long>(type: "bigint", nullable: false),
                    stock_actual = table.Column<int>(type: "integer", nullable: false),
                    stock_reservado = table.Column<int>(type: "integer", nullable: false),
                    ultima_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventario", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventario_producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "producto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventario_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historia_clinica",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    numero_historia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_apertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historia_clinica", x => x.id);
                    table.ForeignKey(
                        name: "FK_historia_clinica_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compra_detalle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    compra_id = table.Column<long>(type: "bigint", nullable: false),
                    producto_id = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    costo_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compra_detalle", x => x.id);
                    table.ForeignKey(
                        name: "FK_compra_detalle_compra_compra_id",
                        column: x => x.compra_id,
                        principalTable: "compra",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_compra_detalle_producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "producto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id = table.Column<long>(type: "bigint", nullable: true),
                    tabla = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registro_id = table.Column<long>(type: "bigint", nullable: true),
                    accion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    valor_anterior = table.Column<string>(type: "text", nullable: true),
                    valor_nuevo = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fecha_evento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_auditoria_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "movimiento_caja",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    caja_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    valor = table.Column<decimal>(type: "numeric", nullable: false),
                    concepto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_movimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimiento_caja", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimiento_caja_caja_caja_id",
                        column: x => x.caja_id,
                        principalTable: "caja",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movimiento_caja_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optometrista",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    persona_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_id = table.Column<long>(type: "bigint", nullable: true),
                    numero_registro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    especialidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    firma = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optometrista", x => x.id);
                    table.ForeignKey(
                        name: "FK_optometrista_persona_persona_id",
                        column: x => x.persona_id,
                        principalTable: "persona",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_optometrista_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transferencia",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero_transferencia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sucursal_origen_id = table.Column<long>(type: "bigint", nullable: false),
                    sucursal_destino_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_solicita_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_aprueba_id = table.Column<long>(type: "bigint", nullable: true),
                    fecha_solicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_aprobacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_envio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_recepcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transferencia", x => x.id);
                    table.ForeignKey(
                        name: "FK_transferencia_sucursal_sucursal_destino_id",
                        column: x => x.sucursal_destino_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transferencia_sucursal_sucursal_origen_id",
                        column: x => x.sucursal_origen_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transferencia_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_transferencia_usuario_usuario_aprueba_id",
                        column: x => x.usuario_aprueba_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transferencia_usuario_usuario_solicita_id",
                        column: x => x.usuario_solicita_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    numero_factura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_venta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    descuento = table.Column<decimal>(type: "numeric", nullable: false),
                    iva = table.Column<decimal>(type: "numeric", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.id);
                    table.ForeignKey(
                        name: "FK_venta_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venta_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venta_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movimiento_inventario",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    inventario_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    stock_anterior = table.Column<int>(type: "integer", nullable: false),
                    stock_nuevo = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_movimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimiento_inventario", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimiento_inventario_inventario_inventario_id",
                        column: x => x.inventario_id,
                        principalTable: "inventario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movimiento_inventario_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cita",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    optometrista_id = table.Column<long>(type: "bigint", nullable: false),
                    sucursal_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hora_inicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    hora_fin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cita", x => x.id);
                    table.ForeignKey(
                        name: "FK_cita_optometrista_optometrista_id",
                        column: x => x.optometrista_id,
                        principalTable: "optometrista",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cita_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cita_sucursal_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transferencia_detalle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transferencia_id = table.Column<long>(type: "bigint", nullable: false),
                    producto_id = table.Column<long>(type: "bigint", nullable: false),
                    cantidad_solicitada = table.Column<int>(type: "integer", nullable: false),
                    cantidad_aprobada = table.Column<int>(type: "integer", nullable: true),
                    cantidad_enviada = table.Column<int>(type: "integer", nullable: true),
                    cantidad_recibida = table.Column<int>(type: "integer", nullable: true),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transferencia_detalle", x => x.id);
                    table.ForeignKey(
                        name: "FK_transferencia_detalle_producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "producto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transferencia_detalle_transferencia_transferencia_id",
                        column: x => x.transferencia_id,
                        principalTable: "transferencia",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venta_id = table.Column<long>(type: "bigint", nullable: false),
                    metodo_pago = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    valor = table.Column<decimal>(type: "numeric", nullable: false),
                    referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_pago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pago", x => x.id);
                    table.ForeignKey(
                        name: "FK_pago_venta_venta_id",
                        column: x => x.venta_id,
                        principalTable: "venta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venta_detalle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venta_id = table.Column<long>(type: "bigint", nullable: false),
                    producto_id = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    descuento = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta_detalle", x => x.id);
                    table.ForeignKey(
                        name: "FK_venta_detalle_producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "producto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venta_detalle_venta_venta_id",
                        column: x => x.venta_id,
                        principalTable: "venta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "consulta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    historia_clinica_id = table.Column<long>(type: "bigint", nullable: false),
                    optometrista_id = table.Column<long>(type: "bigint", nullable: false),
                    cita_id = table.Column<long>(type: "bigint", nullable: true),
                    fecha_consulta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    motivo_consulta = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    diagnostico = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    recomendaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consulta", x => x.id);
                    table.ForeignKey(
                        name: "FK_consulta_cita_cita_id",
                        column: x => x.cita_id,
                        principalTable: "cita",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_consulta_historia_clinica_historia_clinica_id",
                        column: x => x.historia_clinica_id,
                        principalTable: "historia_clinica",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consulta_optometrista_optometrista_id",
                        column: x => x.optometrista_id,
                        principalTable: "optometrista",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<long>(type: "bigint", nullable: false),
                    od_esfera = table.Column<decimal>(type: "numeric", nullable: true),
                    od_cilindro = table.Column<decimal>(type: "numeric", nullable: true),
                    od_eje = table.Column<int>(type: "integer", nullable: true),
                    od_adicion = table.Column<decimal>(type: "numeric", nullable: true),
                    oi_esfera = table.Column<decimal>(type: "numeric", nullable: true),
                    oi_cilindro = table.Column<decimal>(type: "numeric", nullable: true),
                    oi_eje = table.Column<int>(type: "integer", nullable: true),
                    oi_adicion = table.Column<decimal>(type: "numeric", nullable: true),
                    distancia_pupilar = table.Column<decimal>(type: "numeric", nullable: true),
                    observacion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    fecha_emision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receta", x => x.id);
                    table.ForeignKey(
                        name: "FK_receta_consulta_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "consulta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orden_trabajo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venta_id = table.Column<long>(type: "bigint", nullable: true),
                    receta_id = table.Column<long>(type: "bigint", nullable: true),
                    numero_orden = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_entrega_estimada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_entrega_real = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orden_trabajo", x => x.id);
                    table.ForeignKey(
                        name: "FK_orden_trabajo_receta_receta_id",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_orden_trabajo_venta_venta_id",
                        column: x => x.venta_id,
                        principalTable: "venta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_usuario_id",
                table: "auditoria",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_caja_sucursal_id",
                table: "caja",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_cita_optometrista_id",
                table: "cita",
                column: "optometrista_id");

            migrationBuilder.CreateIndex(
                name: "IX_cita_paciente_id",
                table: "cita",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_cita_sucursal_id",
                table: "cita",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_persona_id",
                table: "cliente",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_compra_proveedor_id",
                table: "compra",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_sucursal_id",
                table: "compra",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_detalle_compra_id",
                table: "compra_detalle",
                column: "compra_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_detalle_producto_id",
                table: "compra_detalle",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_consulta_cita_id",
                table: "consulta",
                column: "cita_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consulta_historia_clinica_id",
                table: "consulta",
                column: "historia_clinica_id");

            migrationBuilder.CreateIndex(
                name: "IX_consulta_optometrista_id",
                table: "consulta",
                column: "optometrista_id");

            migrationBuilder.CreateIndex(
                name: "IX_empresa_configuracion_empresa_id",
                table: "empresa_configuracion",
                column: "empresa_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historia_clinica_paciente_id",
                table: "historia_clinica",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_producto_id",
                table: "inventario",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_sucursal_id",
                table: "inventario",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_caja_caja_id",
                table: "movimiento_caja",
                column: "caja_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_caja_usuario_id",
                table: "movimiento_caja",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_inventario_inventario_id",
                table: "movimiento_inventario",
                column: "inventario_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_inventario_usuario_id",
                table: "movimiento_inventario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_numeracion_documento_sucursal_id",
                table: "numeracion_documento",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_optometrista_persona_id",
                table: "optometrista",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_optometrista_usuario_id",
                table: "optometrista",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_orden_trabajo_receta_id",
                table: "orden_trabajo",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_orden_trabajo_venta_id",
                table: "orden_trabajo",
                column: "venta_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paciente_persona_id",
                table: "paciente",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pago_venta_id",
                table: "pago",
                column: "venta_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_categoria_id",
                table: "producto",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_marca_id",
                table: "producto",
                column: "marca_id");

            migrationBuilder.CreateIndex(
                name: "IX_receta_consulta_id",
                table: "receta",
                column: "consulta_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rol_permiso_permiso_id",
                table: "rol_permiso",
                column: "permiso_id");

            migrationBuilder.CreateIndex(
                name: "IX_sucursal_empresa_id",
                table: "sucursal",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_sucursal_configuracion_sucursal_id",
                table: "sucursal_configuracion",
                column: "sucursal_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_sucursal_destino_id",
                table: "transferencia",
                column: "sucursal_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_sucursal_origen_id",
                table: "transferencia",
                column: "sucursal_origen_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_usuario_aprueba_id",
                table: "transferencia",
                column: "usuario_aprueba_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_usuario_solicita_id",
                table: "transferencia",
                column: "usuario_solicita_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_UsuarioId",
                table: "transferencia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_detalle_producto_id",
                table: "transferencia_detalle",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_detalle_transferencia_id",
                table: "transferencia_detalle",
                column: "transferencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_persona_id",
                table: "usuario",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_rol_id",
                table: "usuario",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_sucursal_id",
                table: "usuario",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_cliente_id",
                table: "venta",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_sucursal_id",
                table: "venta",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_usuario_id",
                table: "venta",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_detalle_producto_id",
                table: "venta_detalle",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_detalle_venta_id",
                table: "venta_detalle",
                column: "venta_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "compra_detalle");

            migrationBuilder.DropTable(
                name: "configuracion_sistema");

            migrationBuilder.DropTable(
                name: "empresa_configuracion");

            migrationBuilder.DropTable(
                name: "movimiento_caja");

            migrationBuilder.DropTable(
                name: "movimiento_inventario");

            migrationBuilder.DropTable(
                name: "numeracion_documento");

            migrationBuilder.DropTable(
                name: "orden_trabajo");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "parametro_catalogo");

            migrationBuilder.DropTable(
                name: "rol_permiso");

            migrationBuilder.DropTable(
                name: "sucursal_configuracion");

            migrationBuilder.DropTable(
                name: "transferencia_detalle");

            migrationBuilder.DropTable(
                name: "venta_detalle");

            migrationBuilder.DropTable(
                name: "compra");

            migrationBuilder.DropTable(
                name: "caja");

            migrationBuilder.DropTable(
                name: "inventario");

            migrationBuilder.DropTable(
                name: "receta");

            migrationBuilder.DropTable(
                name: "permiso");

            migrationBuilder.DropTable(
                name: "transferencia");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "proveedor");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "consulta");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "categoria_producto");

            migrationBuilder.DropTable(
                name: "marca");

            migrationBuilder.DropTable(
                name: "cita");

            migrationBuilder.DropTable(
                name: "historia_clinica");

            migrationBuilder.DropTable(
                name: "optometrista");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "persona");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "sucursal");

            migrationBuilder.DropTable(
                name: "empresa");
        }
    }
}
