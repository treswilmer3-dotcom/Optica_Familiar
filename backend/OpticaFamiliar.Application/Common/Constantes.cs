namespace OpticaFamiliar.Application.Common;

public static class Roles
{
    public const string Administrador = "ADMIN";
    public const string Vendedor = "VENDEDOR";
    public const string Optometrista = "OPTOMETRISTA";

    public const string AdminOVendedor = Administrador + "," + Vendedor;
    public const string AdminOOptometrista = Administrador + "," + Optometrista;
    public const string Todos = Administrador + "," + Vendedor + "," + Optometrista;
}

public static class Estados
{
    public const string Activo = "ACTIVO";
    public const string Inactivo = "INACTIVO";

    // Venta
    public const string VentaPendiente = "PENDIENTE";
    public const string VentaPagada = "PAGADA";
    public const string VentaAnulada = "ANULADA";

    // Orden de trabajo (docs/05-Modelo-Logico.md)
    public const string OrdenCreada = "CREADA";
    public const string OrdenEnProduccion = "EN_PRODUCCION";
    public const string OrdenEnLaboratorio = "EN_LABORATORIO";
    public const string OrdenTerminada = "TERMINADA";
    public const string OrdenEntregada = "ENTREGADA";

    public static readonly string[] Ordenes =
        [OrdenCreada, OrdenEnProduccion, OrdenEnLaboratorio, OrdenTerminada, OrdenEntregada];
}

public static class TiposDocumento
{
    public const string Venta = "VENTA";
    public const string OrdenTrabajo = "ORDEN_TRABAJO";
    public const string HistoriaClinica = "HISTORIA_CLINICA";
}
