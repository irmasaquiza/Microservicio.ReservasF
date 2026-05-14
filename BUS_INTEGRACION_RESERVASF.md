# Integracion Bus - Microservicio ReservasF

Este documento reemplaza la guia anterior con datos levantados directamente del codigo actual de `Microservicio.ReservasF`. Su objetivo es darle al Bus de Integracion la informacion exacta para configurar puertos, reenviar JWT, consumir endpoints y mapear modelos sin adivinar nombres de campos.

## Datos base confirmados

- Microservicio: `Microservicio.ReservasF`
- API principal: `Microservicio.ReservasF.Api`
- Version de rutas: `api/v1`
- Puerto HTTP dev: `5248`
- Puerto HTTPS dev: `7280`
- `sslPort` IIS Express dev: `44370`
- Autenticacion: JWT Bearer, emitido por MS Seguridad.
- Roles usados por la API: `ADMINISTRADOR`, `AEROLINEA`, `CLIENTE`.
- Base de datos: PostgreSQL.
- Esquema confirmado: `ventas`.

El `sslPort` pedido para configuracion estilo `44xxx` es `44370`, tomado de `Microservicio.ReservasF.Api/Properties/launchSettings.json`.

## Configuracion de Base de Datos

La conexion esta configurada en `Microservicio.ReservasF.Api/appsettings.json` bajo:

```json
"ConnectionStrings": {
  "MicroservicioReservasFDb": "<connection-string-postgresql>"
}
```

No se copia la credencial real en este documento. El codigo actual apunta a PostgreSQL/Supabase y todas las entidades internas del dominio se mapean al esquema `ventas`.

Tablas confirmadas por EF Core:

- `ventas.reservas`
- `ventas.reserva_detalle`
- `ventas.facturas`
- `ventas.boleto`
- `ventas.equipaje`

## Relaciones Reales

La estructura real no es exactamente la del analisis inicial. Queda asi:

```text
Reserva
  ├── tiene muchos ReservaDetalle
  ├── tiene una Factura por regla de BD (indice unico en facturas.id_reserva)
  ├── tiene muchos Boletos
  └── Factura tiene muchos Boletos

ReservaDetalle
  └── tiene cero o un Boleto (boleto.id_detalle es unico)

Boleto
  └── tiene muchos Equipajes
```

Detalles importantes:

- `ReservaDetalle` no tiene muchos `Boleto`; en el modelo real es `ReservaDetalle 1 : 0..1 Boleto`.
- `Factura` esta configurada en EF como `Reserva.WithMany(r => r.Facturas)`, pero tiene indice unico `uq_facturas_reserva` sobre `id_reserva`, por lo que en BD funciona como una factura por reserva.
- `id_cliente`, `id_pasajero`, `id_vuelo` e `id_asiento` son referencias logicas a otros microservicios; no son FK hacia tablas externas.

## Interfaces DataManagement

No existe `IReservaDetalleDataService.cs` separado en `DataManagement/Interfaces/`. Los detalles de reserva se manejan dentro de `IReservaDataService` y sus modelos relacionados.

### `IReservaDataService.cs`

```csharp
Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(ReservaFiltroDataModel filtro);
Task<ReservaDataModel?> GetByIdAsync(int id);
Task<ReservaDataModel?> GetByGuidAsync(Guid guidReserva);
Task<ReservaDataModel?> GetByCodigoAsync(string codigoReserva);
Task<IReadOnlyCollection<ReservaDataModel>> GetByClienteAsync(int idCliente);
Task<IReadOnlyCollection<ReservaDataModel>> GetByPasajeroAsync(int idPasajero);
Task<IReadOnlyCollection<ReservaDataModel>> GetByVueloAsync(int idVuelo);
Task<ReservaDataModel?> GetReservaActivaPorAsientoAsync(int idAsiento);
Task<ReservaDataModel?> GetPorVueloYAsientoAsync(int idVuelo, int idAsiento);
Task<ReservaDataModel?> GetPorVueloYPasajeroAsync(int idVuelo, int idPasajero);
Task<bool> ExistePorIdAsync(int idReserva);
Task<bool> ExistePorGuidAsync(Guid guidReserva);
Task<bool> ExistePorCodigoAsync(string codigoReserva);
Task<bool> ExistePorVueloYAsientoAsync(int idVuelo, int idAsiento);
Task<bool> ExistePorVueloYPasajeroAsync(int idVuelo, int idPasajero);
Task<ReservaDataModel> CreateAsync(ReservaDataModel model);
Task<ReservaDataModel?> UpdateAsync(ReservaDataModel model);
Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
```

### `IBoletoDataService.cs`

```csharp
Task<DataPagedResult<BoletoDataModel>> GetPagedAsync(BoletoFiltroDataModel filtro);
Task<BoletoDataModel?> GetByIdAsync(int id);
Task<BoletoDataModel?> GetByCodigoAsync(string codigoBoleto);
Task<IReadOnlyCollection<BoletoDataModel>> GetByReservaAsync(int idReserva);
Task<IReadOnlyCollection<BoletoDataModel>> GetByVueloAsync(int idVuelo);
Task<IReadOnlyCollection<BoletoDataModel>> GetByFacturaAsync(int idFactura);
Task<bool> ExistePorIdAsync(int idBoleto);
Task<bool> ExistePorCodigoAsync(string codigoBoleto);
Task<BoletoDataModel> CreateAsync(BoletoDataModel model);
Task<BoletoDataModel?> UpdateAsync(BoletoDataModel model);
Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
```

### `IFacturaDataService.cs`

```csharp
Task<DataPagedResult<FacturaDataModel>> GetPagedAsync(FacturaFiltroDataModel filtro);
Task<FacturaDataModel?> GetByIdAsync(int id);
Task<FacturaDataModel?> GetByGuidAsync(Guid guidFactura);
Task<FacturaDataModel?> GetByNumeroAsync(string numeroFactura);
Task<IReadOnlyCollection<FacturaDataModel>> GetByClienteAsync(int idCliente);
Task<IReadOnlyCollection<FacturaDataModel>> GetByReservaAsync(int idReserva);
Task<bool> ExistePorIdAsync(int idFactura);
Task<bool> ExistePorGuidAsync(Guid guidFactura);
Task<bool> ExistePorNumeroAsync(string numeroFactura);
Task<FacturaDataModel> CreateAsync(FacturaDataModel model);
Task<FacturaDataModel?> UpdateAsync(FacturaDataModel model);
Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
```

### `IEquipajeDataService.cs`

```csharp
Task<DataPagedResult<EquipajeDataModel>> GetPagedAsync(EquipajeFiltroDataModel filtro);
Task<EquipajeDataModel?> GetByIdAsync(int id);
Task<EquipajeDataModel?> GetByNumeroEtiquetaAsync(string numeroEtiqueta);
Task<IReadOnlyCollection<EquipajeDataModel>> GetByBoletoAsync(int idBoleto);
Task<IReadOnlyCollection<EquipajeDataModel>> GetByTipoAsync(string tipo);
Task<bool> ExistePorIdAsync(int idEquipaje);
Task<bool> ExistePorNumeroEtiquetaAsync(string numeroEtiqueta);
Task<EquipajeDataModel> CreateAsync(EquipajeDataModel model);
Task<EquipajeDataModel?> UpdateAsync(EquipajeDataModel model);
Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
```

## Modelos DataManagement

Los modelos estan en `Microservicio.ReservasF.DataManagement/Models/`, no en una subcarpeta `Models/Reservas/`.

### `ReservaDataModel.cs`

```csharp
public int IdReserva { get; set; }
public Guid GuidReserva { get; set; }
public string CodigoReserva { get; set; } = null!;
public int IdCliente { get; set; }
public int IdVuelo { get; set; }
public DateTime FechaReservaUtc { get; set; }
public DateTime FechaInicio { get; set; }
public DateTime FechaFin { get; set; }
public decimal SubtotalReserva { get; set; }
public decimal ValorIva { get; set; }
public decimal TotalReserva { get; set; }
public string OrigenCanalReserva { get; set; } = null!;
public string EstadoReserva { get; set; } = null!;
public DateTime? FechaConfirmacionUtc { get; set; }
public DateTime? FechaCancelacionUtc { get; set; }
public string? MotivoCancelacion { get; set; }
public bool EsEliminado { get; set; }
public string CreadoPorUsuario { get; set; } = null!;
public DateTime FechaRegistroUtc { get; set; }
public string? ModificadoPorUsuario { get; set; }
public DateTime? FechaModificacionUtc { get; set; }
public string? ModificacionIp { get; set; }
public string ServicioOrigen { get; set; } = null!;
public string? ContactoEmail { get; set; }
public string? ContactoTelefono { get; set; }
public string? Observaciones { get; set; }
public DateTime? FechaInhabilitacionUtc { get; set; }
public string? MotivoInhabilitacion { get; set; }
public byte[] RowVersion { get; set; } = null!;
public IReadOnlyCollection<ReservaDetalleDataModel> Detalles { get; set; } = Array.Empty<ReservaDetalleDataModel>();
public int CantidadPasajeros => Detalles.Count;
```

### `ReservaDetalleDataModel.cs`

```csharp
public int IdDetalle { get; set; }
public byte[] RowVersion { get; set; } = null!;
public int IdReserva { get; set; }
public int IdPasajero { get; set; }
public int IdAsiento { get; set; }
public decimal SubtotalLinea { get; set; }
public decimal ValorIvaLinea { get; set; }
public decimal TotalLinea { get; set; }
public string EstadoDetalle { get; set; } = null!;
public bool EsEliminado { get; set; }
public string Estado { get; set; } = null!;
public string CreadoPorUsuario { get; set; } = null!;
public DateTime FechaRegistroUtc { get; set; }
public string? ModificadoPorUsuario { get; set; }
public DateTime? FechaModificacionUtc { get; set; }
public string? ModificacionIp { get; set; }
```

### `BoletoDataModel.cs`

```csharp
public int IdBoleto { get; set; }
public byte[] RowVersion { get; set; } = null!;
public int IdReserva { get; set; }
public int IdDetalle { get; set; }
public int IdVuelo { get; set; }
public int IdAsiento { get; set; }
public int IdFactura { get; set; }
public string CodigoBoleto { get; set; } = null!;
public string Clase { get; set; } = null!;
public decimal PrecioVueloBase { get; set; }
public decimal PrecioAsientoExtra { get; set; }
public decimal ImpuestosBoleto { get; set; }
public decimal CargoEquipaje { get; set; }
public decimal PrecioFinal { get; set; }
public string EstadoBoleto { get; set; } = null!;
public DateTime FechaEmision { get; set; }
public bool EsEliminado { get; set; }
public string Estado { get; set; } = null!;
public string CreadoPorUsuario { get; set; } = null!;
public DateTime FechaRegistroUtc { get; set; }
public string? ModificadoPorUsuario { get; set; }
public DateTime? FechaModificacionUtc { get; set; }
public string? ModificacionIp { get; set; }
```

### `FacturaDataModel.cs`

```csharp
public int IdFactura { get; set; }
public Guid GuidFactura { get; set; }
public int IdCliente { get; set; }
public int IdReserva { get; set; }
public string NumeroFactura { get; set; } = null!;
public DateTime FechaEmision { get; set; }
public decimal Subtotal { get; set; }
public decimal ValorIva { get; set; }
public decimal CargoServicio { get; set; }
public decimal Total { get; set; }
public string? ObservacionesFactura { get; set; }
public string? OrigenCanalFactura { get; set; }
public string EstadoFactura { get; set; } = null!;
public string Estado { get => EstadoFactura; set => EstadoFactura = value; }
public DateTime? FechaInhabilitacionUtc { get; set; }
public bool EsEliminado { get; set; }
public string CreadoPorUsuario { get; set; } = null!;
public DateTime FechaRegistroUtc { get; set; }
public string? ModificadoPorUsuario { get; set; }
public DateTime? FechaModificacionUtc { get; set; }
public string? ModificacionIp { get; set; }
public string ServicioOrigen { get; set; } = null!;
public string? MotivoInhabilitacion { get; set; }
public byte[] RowVersion { get; set; } = null!;
```

### `EquipajeDataModel.cs`

```csharp
public int IdEquipaje { get; set; }
public byte[] RowVersion { get; set; } = null!;
public int IdBoleto { get; set; }
public string Tipo { get; set; } = null!;
public decimal PesoKg { get; set; }
public string? DescripcionEquipaje { get; set; }
public decimal PrecioExtra { get; set; }
public string? DimensionesCm { get; set; }
public string NumeroEtiqueta { get; set; } = null!;
public string EstadoEquipaje { get; set; } = null!;
public bool EsEliminado { get; set; }
public string Estado { get; set; } = null!;
public string CreadoPorUsuario { get; set; } = null!;
public DateTime FechaRegistroUtc { get; set; }
public string? ModificadoPorUsuario { get; set; }
public DateTime? FechaModificacionUtc { get; set; }
public string? ModificacionIp { get; set; }
```

## Endpoints Reales con Roles

Todas estas rutas requieren JWT porque los controladores tienen `[Authorize]`.

### Reservas

```text
GET    /api/v1/reservas                       Roles: ADMINISTRADOR, AEROLINEA
GET    /api/v1/reservas/{id_reserva}          Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
POST   /api/v1/reservas                       Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
PATCH  /api/v1/reservas/{id_reserva}/estado   Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
PATCH  /api/v1/reservas/{id_reserva}/pagar    Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
DELETE /api/v1/reservas/{id_reserva}          Roles: ADMINISTRADOR, AEROLINEA
```

Notas para el Bus:

- Si el rol es `CLIENTE`, el API toma `id_cliente` desde el claim `id_cliente` del JWT.
- `PATCH /pagar` es el flujo mas importante para integracion porque procesa el pago de la reserva y emision relacionada.
- El controlador lee el usuario desde `User.Identity.Name`, claim `username` o usa `SYSTEM`.

### Boletos

```text
GET    /api/v1/boletos                        Roles: ADMINISTRADOR, AEROLINEA
GET    /api/v1/boletos/{id_boleto}            Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
POST   /api/v1/boletos                        Roles: ADMINISTRADOR, AEROLINEA
PATCH  /api/v1/boletos/{id_boleto}/estado     Roles: ADMINISTRADOR, AEROLINEA
DELETE /api/v1/boletos/{id_boleto}            Roles: ADMINISTRADOR, AEROLINEA
```

### Facturas

```text
GET    /api/v1/facturas                       Roles: ADMINISTRADOR, AEROLINEA
GET    /api/v1/facturas/{id_factura}          Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
POST   /api/v1/facturas                       Roles: ADMINISTRADOR, AEROLINEA
PATCH  /api/v1/facturas/{id_factura}/anular   Roles: ADMINISTRADOR
PATCH  /api/v1/facturas/{id_factura}/aprobar  Roles: ADMINISTRADOR, AEROLINEA
POST   /api/v1/facturas/{id_factura}/pagar    Roles: ADMINISTRADOR, AEROLINEA
DELETE /api/v1/facturas/{id_factura}          Roles: ADMINISTRADOR
```

Nota: `POST /api/v1/facturas/{id_factura}/pagar` no permite `CLIENTE` segun el controlador actual, aunque internamente el servicio recibe `id_cliente` y rol.

### Equipaje

```text
GET    /api/v1/boletos/{id_boleto}/equipaje                         Roles: ADMINISTRADOR, AEROLINEA
POST   /api/v1/boletos/{id_boleto}/equipaje                         Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
PATCH  /api/v1/boletos/{id_boleto}/equipaje/{id_equipaje}/estado     Roles: ADMINISTRADOR, AEROLINEA
DELETE /api/v1/boletos/{id_boleto}/equipaje/{id_equipaje}            Roles: ADMINISTRADOR, AEROLINEA
```

## JWT que debe reenviar el Bus

El Bus debe reenviar el JWT original cuando actue en nombre de un usuario, porque la API aplica reglas por rol y por `id_cliente`.

Claims leidos por los controladores:

- Rol: `ClaimTypes.Role` o `role`.
- Cliente: `id_cliente`.
- Usuario: `User.Identity.Name` o `username`.

Implicacion:

- Para llamadas de usuario final, reenviar el token del usuario.
- Para procesos internos del Bus, usar un JWT de servicio con rol permitido, preferiblemente `ADMINISTRADOR` o `AEROLINEA` segun el caso de uso.
- No fabricar `id_cliente` en el body para rol `CLIENTE`; el API lo sobrescribe desde el JWT en creacion de reserva.

## Puntos de Integracion para el Bus

### Crear reserva

Endpoint:

```text
POST /api/v1/reservas
```

Roles:

```text
ADMINISTRADOR, AEROLINEA, CLIENTE
```

Uso esperado:

- Validar/coordinar datos con MS Clientes y MS Vuelos antes o durante la orquestacion.
- Crear la reserva con sus pasajeros/asientos.
- Para `CLIENTE`, el `id_cliente` real debe venir del JWT.

### Pagar reserva

Endpoint:

```text
PATCH /api/v1/reservas/{id_reserva}/pagar
```

Roles:

```text
ADMINISTRADOR, AEROLINEA, CLIENTE
```

Uso esperado:

- Es el candidato principal para orquestacion del Bus.
- Debe coordinar reserva, factura, boleto, equipaje si aplica y disponibilidad de asientos en MS Vuelos.
- El Bus debe tener estrategia de compensacion si falla una llamada externa despues de cambiar estado local.

### Registrar equipaje

Endpoint:

```text
POST /api/v1/boletos/{id_boleto}/equipaje
```

Roles:

```text
ADMINISTRADOR, AEROLINEA, CLIENTE
```

Uso esperado:

- Asociar equipaje a un boleto existente.
- Si el rol es `CLIENTE`, el servicio valida contra el cliente del JWT.

## Resumen para `appsettings.json` del Bus

Valores sugeridos para configurar el cliente HTTP del Bus:

```json
{
  "ServiciosExternos": {
    "ReservasFBaseUrl": "https://localhost:7280",
    "ReservasFHttpBaseUrl": "http://localhost:5248"
  },
  "PuertosDev": {
    "ReservasFHttp": 5248,
    "ReservasFHttps": 7280,
    "ReservasFSslPort": 44370
  }
}
```

Si el Bus corre en desarrollo con certificados locales no confiables, ajustar el handler HTTP solo en entorno Development.

## Checklist de Integracion

- Usar `https://localhost:7280` como base URL preferida en dev.
- Reenviar JWT original en llamadas de usuario.
- Usar token de servicio con rol permitido para procesos internos.
- Mapear modelos con los nombres exactos de `DataManagement/Models`.
- No asumir que existe `IReservaDetalleDataService`.
- No asumir `ReservaDetalle -> muchos Boletos`; la relacion real es `ReservaDetalle -> cero o un Boleto`.
- Confirmar en runtime que la BD usa esquema `ventas`.
- Mantener `id_cliente`, `id_pasajero`, `id_vuelo`, `id_asiento` como referencias logicas entre microservicios.
