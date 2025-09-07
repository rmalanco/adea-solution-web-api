namespace adea_solution_web_api.Models
{
    public class Caja
    {
        public int Caja_Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Ubicacion_Id { get; set; } = string.Empty;
        public int ExpedientesCount { get; set; } = 0;
    }

    public class CreateCajaRequest
    {
        public string Estado { get; set; } = string.Empty;
        public string Ubicacion_Id { get; set; } = string.Empty;
    }

    public class UpdateCajaRequest
    {
        public int Caja_Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Ubicacion_Id { get; set; } = string.Empty;
    }
}
