using Microservicio.ReservasF.Business.Exceptions;

namespace Microservicio.ReservasF.Business.Services.Policies;

public static class EquipajePricingPolicy
{
    private const decimal PrecioFijoBodega = 45.00m;
    private const decimal PesoMaximoBodegaKg = 23.00m;
    private const decimal PesoMaximoManoKg = 10.00m;
    private const string DimensionesEstandarMano = "55x40x20";
    private const string DimensionesEstandarBodega = "158 cm lineales max";

    public static decimal CalcularPrecioExtra(string tipoEquipaje, decimal pesoKg)
    {
        var tipo = (tipoEquipaje ?? string.Empty).Trim().ToUpperInvariant();

        if (pesoKg <= 0)
            throw new ValidationException("El peso del equipaje debe ser mayor que 0.");

        return tipo switch
        {
            "MANO" => CalcularMano(pesoKg),
            "BODEGA" => CalcularBodega(pesoKg),
            _ => throw new ValidationException("El tipo de equipaje no es válido.")
        };
    }

    public static string ObtenerDimensionesEstandar(string tipoEquipaje)
    {
        var tipo = (tipoEquipaje ?? string.Empty).Trim().ToUpperInvariant();

        return tipo switch
        {
            "MANO" => DimensionesEstandarMano,
            "BODEGA" => DimensionesEstandarBodega,
            _ => throw new ValidationException("El tipo de equipaje no es válido.")
        };
    }

    private static decimal CalcularMano(decimal pesoKg)
    {
        if (pesoKg > PesoMaximoManoKg)
            throw new BusinessException("El equipaje de mano no puede superar 10 kg.");

        return 0m;
    }

    private static decimal CalcularBodega(decimal pesoKg)
    {
        if (pesoKg > PesoMaximoBodegaKg)
            throw new BusinessException("El equipaje de bodega no puede superar 23 kg.");

        return PrecioFijoBodega;
    }
}