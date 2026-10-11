using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpticaFamiliar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Multiempresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuario_username",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_producto_codigo",
                table: "producto");

            migrationBuilder.DropIndex(
                name: "IX_persona_numero_identificacion",
                table: "persona");

            migrationBuilder.DropIndex(
                name: "IX_orden_trabajo_numero_orden",
                table: "orden_trabajo");

            migrationBuilder.DropIndex(
                name: "IX_historia_clinica_numero_historia",
                table: "historia_clinica");

            migrationBuilder.DropIndex(
                name: "IX_empresa_ruc",
                table: "empresa");

            migrationBuilder.RenameColumn(
                name: "ruc",
                table: "empresa",
                newName: "identificacion_fiscal");

            migrationBuilder.AlterColumn<string>(
                name: "identificacion_fiscal",
                table: "empresa",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "venta_detalle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "venta",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "usuario",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "transferencia_detalle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "transferencia",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "sucursal_configuracion",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "receta",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "proveedor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "producto",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "persona",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "parametro_catalogo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "pago",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "paciente",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "orden_trabajo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "optometrista",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "numeracion_documento",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "movimiento_inventario",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "movimiento_caja",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "marca",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "inventario",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "historia_clinica",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "idioma",
                table: "empresa",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "iva_porcentaje",
                table: "empresa",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "moneda",
                table: "empresa",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "pais",
                table: "empresa",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "zona_horaria",
                table: "empresa",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "consulta",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "configuracion_sistema",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "compra_detalle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "compra",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "cliente",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "cita",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "categoria_producto",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "caja",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "empresa_id",
                table: "auditoria",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // --- Backfill: todo dato previo pertenece a la (única) empresa existente ---
            migrationBuilder.Sql("UPDATE empresa SET pais = 'EC', moneda = 'USD', zona_horaria = 'America/Guayaquil', idioma = 'es', iva_porcentaje = 15 WHERE pais = '';");
            migrationBuilder.Sql("UPDATE venta_detalle SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE venta SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE usuario SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE transferencia_detalle SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE transferencia SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE sucursal_configuracion SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE receta SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE proveedor SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE producto SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE persona SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE parametro_catalogo SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE pago SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE paciente SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE orden_trabajo SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE optometrista SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE numeracion_documento SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE movimiento_inventario SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE movimiento_caja SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE marca SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE inventario SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE historia_clinica SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE consulta SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE configuracion_sistema SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE compra_detalle SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE compra SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE cliente SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE cita SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE categoria_producto SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE caja SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");
            migrationBuilder.Sql("UPDATE auditoria SET empresa_id = (SELECT MIN(id) FROM empresa) WHERE empresa_id = 0;");

            migrationBuilder.CreateIndex(
                name: "IX_venta_detalle_empresa_id",
                table: "venta_detalle",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_empresa_id",
                table: "venta",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_empresa_id_username",
                table: "usuario",
                columns: new[] { "empresa_id", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_detalle_empresa_id",
                table: "transferencia_detalle",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_transferencia_empresa_id",
                table: "transferencia",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_sucursal_configuracion_empresa_id",
                table: "sucursal_configuracion",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_receta_empresa_id",
                table: "receta",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_proveedor_empresa_id",
                table: "proveedor",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_empresa_id_codigo",
                table: "producto",
                columns: new[] { "empresa_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persona_empresa_id_numero_identificacion",
                table: "persona",
                columns: new[] { "empresa_id", "numero_identificacion" },
                unique: true,
                filter: "numero_identificacion IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_parametro_catalogo_empresa_id",
                table: "parametro_catalogo",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_pago_empresa_id",
                table: "pago",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_empresa_id",
                table: "paciente",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_orden_trabajo_empresa_id_numero_orden",
                table: "orden_trabajo",
                columns: new[] { "empresa_id", "numero_orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_optometrista_empresa_id",
                table: "optometrista",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_numeracion_documento_empresa_id",
                table: "numeracion_documento",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_inventario_empresa_id",
                table: "movimiento_inventario",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimiento_caja_empresa_id",
                table: "movimiento_caja",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_marca_empresa_id",
                table: "marca",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_empresa_id",
                table: "inventario",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_historia_clinica_empresa_id_numero_historia",
                table: "historia_clinica",
                columns: new[] { "empresa_id", "numero_historia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresa_pais_identificacion_fiscal",
                table: "empresa",
                columns: new[] { "pais", "identificacion_fiscal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consulta_empresa_id",
                table: "consulta",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_configuracion_sistema_empresa_id",
                table: "configuracion_sistema",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_detalle_empresa_id",
                table: "compra_detalle",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_empresa_id",
                table: "compra",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_empresa_id",
                table: "cliente",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_cita_empresa_id",
                table: "cita",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_categoria_producto_empresa_id",
                table: "categoria_producto",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_caja_empresa_id",
                table: "caja",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_empresa_id",
                table: "auditoria",
                column: "empresa_id");

            migrationBuilder.AddForeignKey(
                name: "FK_auditoria_empresa_empresa_id",
                table: "auditoria",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_caja_empresa_empresa_id",
                table: "caja",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_categoria_producto_empresa_empresa_id",
                table: "categoria_producto",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cita_empresa_empresa_id",
                table: "cita",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_empresa_empresa_id",
                table: "cliente",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_compra_empresa_empresa_id",
                table: "compra",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_compra_detalle_empresa_empresa_id",
                table: "compra_detalle",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_configuracion_sistema_empresa_empresa_id",
                table: "configuracion_sistema",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_consulta_empresa_empresa_id",
                table: "consulta",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_historia_clinica_empresa_empresa_id",
                table: "historia_clinica",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_inventario_empresa_empresa_id",
                table: "inventario",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_marca_empresa_empresa_id",
                table: "marca",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movimiento_caja_empresa_empresa_id",
                table: "movimiento_caja",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movimiento_inventario_empresa_empresa_id",
                table: "movimiento_inventario",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_numeracion_documento_empresa_empresa_id",
                table: "numeracion_documento",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_optometrista_empresa_empresa_id",
                table: "optometrista",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orden_trabajo_empresa_empresa_id",
                table: "orden_trabajo",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_paciente_empresa_empresa_id",
                table: "paciente",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pago_empresa_empresa_id",
                table: "pago",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_parametro_catalogo_empresa_empresa_id",
                table: "parametro_catalogo",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_persona_empresa_empresa_id",
                table: "persona",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_producto_empresa_empresa_id",
                table: "producto",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_proveedor_empresa_empresa_id",
                table: "proveedor",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_receta_empresa_empresa_id",
                table: "receta",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sucursal_configuracion_empresa_empresa_id",
                table: "sucursal_configuracion",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transferencia_empresa_empresa_id",
                table: "transferencia",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transferencia_detalle_empresa_empresa_id",
                table: "transferencia_detalle",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_empresa_empresa_id",
                table: "usuario",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_venta_empresa_empresa_id",
                table: "venta",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_venta_detalle_empresa_empresa_id",
                table: "venta_detalle",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auditoria_empresa_empresa_id",
                table: "auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_caja_empresa_empresa_id",
                table: "caja");

            migrationBuilder.DropForeignKey(
                name: "FK_categoria_producto_empresa_empresa_id",
                table: "categoria_producto");

            migrationBuilder.DropForeignKey(
                name: "FK_cita_empresa_empresa_id",
                table: "cita");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_empresa_empresa_id",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_compra_empresa_empresa_id",
                table: "compra");

            migrationBuilder.DropForeignKey(
                name: "FK_compra_detalle_empresa_empresa_id",
                table: "compra_detalle");

            migrationBuilder.DropForeignKey(
                name: "FK_configuracion_sistema_empresa_empresa_id",
                table: "configuracion_sistema");

            migrationBuilder.DropForeignKey(
                name: "FK_consulta_empresa_empresa_id",
                table: "consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_historia_clinica_empresa_empresa_id",
                table: "historia_clinica");

            migrationBuilder.DropForeignKey(
                name: "FK_inventario_empresa_empresa_id",
                table: "inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_marca_empresa_empresa_id",
                table: "marca");

            migrationBuilder.DropForeignKey(
                name: "FK_movimiento_caja_empresa_empresa_id",
                table: "movimiento_caja");

            migrationBuilder.DropForeignKey(
                name: "FK_movimiento_inventario_empresa_empresa_id",
                table: "movimiento_inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_numeracion_documento_empresa_empresa_id",
                table: "numeracion_documento");

            migrationBuilder.DropForeignKey(
                name: "FK_optometrista_empresa_empresa_id",
                table: "optometrista");

            migrationBuilder.DropForeignKey(
                name: "FK_orden_trabajo_empresa_empresa_id",
                table: "orden_trabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_paciente_empresa_empresa_id",
                table: "paciente");

            migrationBuilder.DropForeignKey(
                name: "FK_pago_empresa_empresa_id",
                table: "pago");

            migrationBuilder.DropForeignKey(
                name: "FK_parametro_catalogo_empresa_empresa_id",
                table: "parametro_catalogo");

            migrationBuilder.DropForeignKey(
                name: "FK_persona_empresa_empresa_id",
                table: "persona");

            migrationBuilder.DropForeignKey(
                name: "FK_producto_empresa_empresa_id",
                table: "producto");

            migrationBuilder.DropForeignKey(
                name: "FK_proveedor_empresa_empresa_id",
                table: "proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_receta_empresa_empresa_id",
                table: "receta");

            migrationBuilder.DropForeignKey(
                name: "FK_sucursal_configuracion_empresa_empresa_id",
                table: "sucursal_configuracion");

            migrationBuilder.DropForeignKey(
                name: "FK_transferencia_empresa_empresa_id",
                table: "transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_transferencia_detalle_empresa_empresa_id",
                table: "transferencia_detalle");

            migrationBuilder.DropForeignKey(
                name: "FK_usuario_empresa_empresa_id",
                table: "usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_empresa_empresa_id",
                table: "venta");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_detalle_empresa_empresa_id",
                table: "venta_detalle");

            migrationBuilder.DropIndex(
                name: "IX_venta_detalle_empresa_id",
                table: "venta_detalle");

            migrationBuilder.DropIndex(
                name: "IX_venta_empresa_id",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_usuario_empresa_id_username",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_transferencia_detalle_empresa_id",
                table: "transferencia_detalle");

            migrationBuilder.DropIndex(
                name: "IX_transferencia_empresa_id",
                table: "transferencia");

            migrationBuilder.DropIndex(
                name: "IX_sucursal_configuracion_empresa_id",
                table: "sucursal_configuracion");

            migrationBuilder.DropIndex(
                name: "IX_receta_empresa_id",
                table: "receta");

            migrationBuilder.DropIndex(
                name: "IX_proveedor_empresa_id",
                table: "proveedor");

            migrationBuilder.DropIndex(
                name: "IX_producto_empresa_id_codigo",
                table: "producto");

            migrationBuilder.DropIndex(
                name: "IX_persona_empresa_id_numero_identificacion",
                table: "persona");

            migrationBuilder.DropIndex(
                name: "IX_parametro_catalogo_empresa_id",
                table: "parametro_catalogo");

            migrationBuilder.DropIndex(
                name: "IX_pago_empresa_id",
                table: "pago");

            migrationBuilder.DropIndex(
                name: "IX_paciente_empresa_id",
                table: "paciente");

            migrationBuilder.DropIndex(
                name: "IX_orden_trabajo_empresa_id_numero_orden",
                table: "orden_trabajo");

            migrationBuilder.DropIndex(
                name: "IX_optometrista_empresa_id",
                table: "optometrista");

            migrationBuilder.DropIndex(
                name: "IX_numeracion_documento_empresa_id",
                table: "numeracion_documento");

            migrationBuilder.DropIndex(
                name: "IX_movimiento_inventario_empresa_id",
                table: "movimiento_inventario");

            migrationBuilder.DropIndex(
                name: "IX_movimiento_caja_empresa_id",
                table: "movimiento_caja");

            migrationBuilder.DropIndex(
                name: "IX_marca_empresa_id",
                table: "marca");

            migrationBuilder.DropIndex(
                name: "IX_inventario_empresa_id",
                table: "inventario");

            migrationBuilder.DropIndex(
                name: "IX_historia_clinica_empresa_id_numero_historia",
                table: "historia_clinica");

            migrationBuilder.DropIndex(
                name: "IX_empresa_pais_identificacion_fiscal",
                table: "empresa");

            migrationBuilder.DropIndex(
                name: "IX_consulta_empresa_id",
                table: "consulta");

            migrationBuilder.DropIndex(
                name: "IX_configuracion_sistema_empresa_id",
                table: "configuracion_sistema");

            migrationBuilder.DropIndex(
                name: "IX_compra_detalle_empresa_id",
                table: "compra_detalle");

            migrationBuilder.DropIndex(
                name: "IX_compra_empresa_id",
                table: "compra");

            migrationBuilder.DropIndex(
                name: "IX_cliente_empresa_id",
                table: "cliente");

            migrationBuilder.DropIndex(
                name: "IX_cita_empresa_id",
                table: "cita");

            migrationBuilder.DropIndex(
                name: "IX_categoria_producto_empresa_id",
                table: "categoria_producto");

            migrationBuilder.DropIndex(
                name: "IX_caja_empresa_id",
                table: "caja");

            migrationBuilder.DropIndex(
                name: "IX_auditoria_empresa_id",
                table: "auditoria");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "venta_detalle");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "transferencia_detalle");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "transferencia");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "sucursal_configuracion");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "receta");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "proveedor");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "persona");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "parametro_catalogo");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "pago");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "orden_trabajo");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "optometrista");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "numeracion_documento");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "movimiento_inventario");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "movimiento_caja");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "marca");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "inventario");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "historia_clinica");

            migrationBuilder.DropColumn(
                name: "identificacion_fiscal",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "idioma",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "iva_porcentaje",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "moneda",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "pais",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "zona_horaria",
                table: "empresa");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "consulta");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "configuracion_sistema");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "compra_detalle");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "compra");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "cliente");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "cita");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "categoria_producto");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "caja");

            migrationBuilder.DropColumn(
                name: "empresa_id",
                table: "auditoria");

            migrationBuilder.AddColumn<string>(
                name: "ruc",
                table: "empresa",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_username",
                table: "usuario",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_producto_codigo",
                table: "producto",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persona_numero_identificacion",
                table: "persona",
                column: "numero_identificacion",
                unique: true,
                filter: "numero_identificacion IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_orden_trabajo_numero_orden",
                table: "orden_trabajo",
                column: "numero_orden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historia_clinica_numero_historia",
                table: "historia_clinica",
                column: "numero_historia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresa_ruc",
                table: "empresa",
                column: "ruc",
                unique: true);
        }
    }
}
