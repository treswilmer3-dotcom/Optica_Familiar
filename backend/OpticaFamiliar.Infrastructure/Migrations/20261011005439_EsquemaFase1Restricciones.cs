using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpticaFamiliar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EsquemaFase1Restricciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_venta_sucursal_id",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_sucursal_empresa_id",
                table: "sucursal");

            migrationBuilder.DropIndex(
                name: "IX_numeracion_documento_sucursal_id",
                table: "numeracion_documento");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "venta_detalle",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "precio_unitario",
                table: "venta_detalle",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "descuento",
                table: "venta_detalle",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "total",
                table: "venta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "venta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "iva",
                table: "venta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "descuento",
                table: "venta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_esfera",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_cilindro",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_adicion",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_esfera",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_cilindro",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_adicion",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "distancia_pupilar",
                table: "receta",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "precio",
                table: "producto",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "costo",
                table: "producto",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "pago",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "movimiento_caja",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "compra_detalle",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "costo_unitario",
                table: "compra_detalle",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "total",
                table: "compra",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "compra",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "iva",
                table: "compra",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_venta_sucursal_id_numero_factura",
                table: "venta",
                columns: new[] { "sucursal_id", "numero_factura" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_username",
                table: "usuario",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sucursal_empresa_id_codigo",
                table: "sucursal",
                columns: new[] { "empresa_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rol_codigo",
                table: "rol",
                column: "codigo",
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
                name: "IX_numeracion_documento_sucursal_id_tipo_documento",
                table: "numeracion_documento",
                columns: new[] { "sucursal_id", "tipo_documento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historia_clinica_numero_historia",
                table: "historia_clinica",
                column: "numero_historia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresa_codigo",
                table: "empresa",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresa_ruc",
                table: "empresa",
                column: "ruc",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_venta_sucursal_id_numero_factura",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_usuario_username",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_sucursal_empresa_id_codigo",
                table: "sucursal");

            migrationBuilder.DropIndex(
                name: "IX_rol_codigo",
                table: "rol");

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
                name: "IX_numeracion_documento_sucursal_id_tipo_documento",
                table: "numeracion_documento");

            migrationBuilder.DropIndex(
                name: "IX_historia_clinica_numero_historia",
                table: "historia_clinica");

            migrationBuilder.DropIndex(
                name: "IX_empresa_codigo",
                table: "empresa");

            migrationBuilder.DropIndex(
                name: "IX_empresa_ruc",
                table: "empresa");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "venta_detalle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "precio_unitario",
                table: "venta_detalle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "descuento",
                table: "venta_detalle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total",
                table: "venta",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "venta",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "iva",
                table: "venta",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "descuento",
                table: "venta",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_esfera",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_cilindro",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oi_adicion",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_esfera",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_cilindro",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "od_adicion",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "distancia_pupilar",
                table: "receta",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "precio",
                table: "producto",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "costo",
                table: "producto",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "pago",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "movimiento_caja",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "compra_detalle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "costo_unitario",
                table: "compra_detalle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total",
                table: "compra",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "compra",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "iva",
                table: "compra",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateIndex(
                name: "IX_venta_sucursal_id",
                table: "venta",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "IX_sucursal_empresa_id",
                table: "sucursal",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_numeracion_documento_sucursal_id",
                table: "numeracion_documento",
                column: "sucursal_id");
        }
    }
}
