namespace adea_solution_web_api.Constants
{
    public static class ErrorMessages
    {
        public const string InternalServerError = "Error interno del servidor";
        public const string CajaNotFound = "Caja con ID {0} no encontrada";
        public const string ExpedienteNotFound = "Expediente con ID {0} no encontrado";
        public const string EstadoRequired = "El estado es obligatorio";
        public const string EstadoLength = "El estado debe tener exactamente 3 caracteres";
        public const string UbicacionRequired = "La ubicación es obligatoria";
        public const string UbicacionInvalid = "Ubicación inválida. Valores permitidos: {0}";
        public const string CajaIdRequired = "El ID de la caja es obligatorio";
        public const string NombreEmpleadoRequired = "El nombre del empleado es obligatorio";
        public const string NombreEmpleadoLength = "El nombre del empleado no puede exceder 100 caracteres";
        public const string TipoExpedienteRequired = "El tipo de expediente es obligatorio";
        public const string TipoExpedienteInvalid = "Tipo de expediente inválido. Valores permitidos: {0}";
        public const string CannotDeleteCajaWithExpedientes = "No se puede eliminar una caja que contiene expedientes";
        public const string IdMismatch = "El ID de la URL no coincide con el ID del cuerpo de la petición";
    }
}
