namespace adea_solution_web_api.Models
{
    public class Expediente
    {
        public int Expediente_Id { get; set; }
        public int Caja_Id { get; set; }
        public string Nombre_Empleado { get; set; } = string.Empty;
        public string Tipo_Expediente { get; set; } = string.Empty;
    }

    public class CreateExpedienteRequest
    {
        public int Caja_Id { get; set; }
        public string Nombre_Empleado { get; set; } = string.Empty;
        public string Tipo_Expediente { get; set; } = string.Empty;
    }

    public class UpdateExpedienteRequest
    {
        public int Expediente_Id { get; set; }
        public int Caja_Id { get; set; }
        public string Nombre_Empleado { get; set; } = string.Empty;
        public string Tipo_Expediente { get; set; } = string.Empty;
    }

    public static class TiposExpediente
    {
        public const string Historico = "Histórico";
        public const string DiaADia = "Día a Día";
        public const string Guarda = "Guarda";

        public static readonly string[] Valores = { Historico, DiaADia, Guarda };
    }

    public static class Ubicaciones
    {
        public const string Norte = "Norte";
        public const string Sur = "Sur";
        public const string Noreste = "Noreste";
        public const string Poniente = "Poniente";
        public const string Centro = "Centro";

        public static readonly string[] Valores = { Norte, Sur, Noreste, Poniente, Centro };
    }
}
