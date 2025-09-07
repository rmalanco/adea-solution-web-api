using adea_solution_web_api.Models;

namespace adea_solution_web_api.Services
{
    public interface IDataService
    {
        // Cajas
        Task<IEnumerable<Caja>> GetAllCajasAsync();
        Task<Caja?> GetCajaByIdAsync(int id);
        Task<Caja> CreateCajaAsync(CreateCajaRequest request);
        Task<Caja?> UpdateCajaAsync(UpdateCajaRequest request);
        Task<bool> DeleteCajaAsync(int id);

        // Expedientes
        Task<IEnumerable<Expediente>> GetAllExpedientesAsync();
        Task<IEnumerable<Expediente>> GetExpedientesByCajaIdAsync(int cajaId);
        Task<Expediente?> GetExpedienteByIdAsync(int id);
        Task<Expediente> CreateExpedienteAsync(CreateExpedienteRequest request);
        Task<Expediente?> UpdateExpedienteAsync(UpdateExpedienteRequest request);
        Task<bool> DeleteExpedienteAsync(int id);

        // Utilidades
        Task<int> GetExpedientesCountByCajaIdAsync(int cajaId);
        Task<IEnumerable<string>> GetUbicacionesAsync();
        Task<IEnumerable<string>> GetTiposExpedienteAsync();
    }

    public class InMemoryDataService : IDataService
    {
        private readonly List<Caja> _cajas;
        private readonly List<Expediente> _expedientes;
        private int _nextCajaId = 1;
        private int _nextExpedienteId = 1;

        public InMemoryDataService()
        {
            _cajas = new List<Caja>();
            _expedientes = new List<Expediente>();

            // Datos de ejemplo para pruebas
            SeedData();
        }

        private void SeedData()
        {
            // Crear algunas cajas de ejemplo
            var caja1 = new Caja { Caja_Id = _nextCajaId++, Estado = "ACT", Ubicacion_Id = "Norte" };
            var caja2 = new Caja { Caja_Id = _nextCajaId++, Estado = "INA", Ubicacion_Id = "Sur" };
            var caja3 = new Caja { Caja_Id = _nextCajaId++, Estado = "ACT", Ubicacion_Id = "Centro" };

            _cajas.AddRange(new[] { caja1, caja2, caja3 });

            // Crear algunos expedientes de ejemplo
            _expedientes.AddRange(new[]
            {
                new Expediente { Expediente_Id = _nextExpedienteId++, Caja_Id = caja1.Caja_Id, Nombre_Empleado = "Juan Pérez", Tipo_Expediente = "Histórico" },
                new Expediente { Expediente_Id = _nextExpedienteId++, Caja_Id = caja1.Caja_Id, Nombre_Empleado = "María García", Tipo_Expediente = "Día a Día" },
                new Expediente { Expediente_Id = _nextExpedienteId++, Caja_Id = caja2.Caja_Id, Nombre_Empleado = "Carlos López", Tipo_Expediente = "Guarda" },
                new Expediente { Expediente_Id = _nextExpedienteId++, Caja_Id = caja3.Caja_Id, Nombre_Empleado = "Ana Martínez", Tipo_Expediente = "Histórico" }
            });

            // Actualizar conteos
            UpdateExpedientesCount();
        }

        private void UpdateExpedientesCount()
        {
            foreach (var caja in _cajas)
            {
                caja.ExpedientesCount = _expedientes.Count(e => e.Caja_Id == caja.Caja_Id);
            }
        }

        // Implementación de métodos para Cajas
        public async Task<IEnumerable<Caja>> GetAllCajasAsync()
        {
            return await Task.FromResult(_cajas.ToList());
        }

        public async Task<Caja?> GetCajaByIdAsync(int id)
        {
            return await Task.FromResult(_cajas.FirstOrDefault(c => c.Caja_Id == id));
        }

        public async Task<Caja> CreateCajaAsync(CreateCajaRequest request)
        {
            var caja = new Caja
            {
                Caja_Id = _nextCajaId++,
                Estado = request.Estado,
                Ubicacion_Id = request.Ubicacion_Id,
                ExpedientesCount = 0
            };

            _cajas.Add(caja);
            return await Task.FromResult(caja);
        }

        public async Task<Caja?> UpdateCajaAsync(UpdateCajaRequest request)
        {
            var caja = _cajas.FirstOrDefault(c => c.Caja_Id == request.Caja_Id);
            if (caja == null) return null;

            caja.Estado = request.Estado;
            caja.Ubicacion_Id = request.Ubicacion_Id;

            return await Task.FromResult(caja);
        }

        public async Task<bool> DeleteCajaAsync(int id)
        {
            var caja = _cajas.FirstOrDefault(c => c.Caja_Id == id);
            if (caja == null) return false;

            // Regla de negocio: No permitir borrar CAJAS que tengan EXPEDIENTES
            var tieneExpedientes = _expedientes.Any(e => e.Caja_Id == id);
            if (tieneExpedientes) return false;

            _cajas.Remove(caja);
            return await Task.FromResult(true);
        }

        // Implementación de métodos para Expedientes
        public async Task<IEnumerable<Expediente>> GetAllExpedientesAsync()
        {
            return await Task.FromResult(_expedientes.ToList());
        }

        public async Task<IEnumerable<Expediente>> GetExpedientesByCajaIdAsync(int cajaId)
        {
            return await Task.FromResult(_expedientes.Where(e => e.Caja_Id == cajaId).ToList());
        }

        public async Task<Expediente?> GetExpedienteByIdAsync(int id)
        {
            return await Task.FromResult(_expedientes.FirstOrDefault(e => e.Expediente_Id == id));
        }

        public async Task<Expediente> CreateExpedienteAsync(CreateExpedienteRequest request)
        {
            // Validar que la caja existe
            var caja = _cajas.FirstOrDefault(c => c.Caja_Id == request.Caja_Id);
            if (caja == null)
                throw new ArgumentException("La caja especificada no existe");

            var expediente = new Expediente
            {
                Expediente_Id = _nextExpedienteId++,
                Caja_Id = request.Caja_Id,
                Nombre_Empleado = request.Nombre_Empleado,
                Tipo_Expediente = request.Tipo_Expediente
            };

            _expedientes.Add(expediente);
            UpdateExpedientesCount();

            return await Task.FromResult(expediente);
        }

        public async Task<Expediente?> UpdateExpedienteAsync(UpdateExpedienteRequest request)
        {
            var expediente = _expedientes.FirstOrDefault(e => e.Expediente_Id == request.Expediente_Id);
            if (expediente == null) return null;

            // Validar que la nueva caja existe
            var caja = _cajas.FirstOrDefault(c => c.Caja_Id == request.Caja_Id);
            if (caja == null)
                throw new ArgumentException("La caja especificada no existe");

            expediente.Caja_Id = request.Caja_Id;
            expediente.Nombre_Empleado = request.Nombre_Empleado;
            expediente.Tipo_Expediente = request.Tipo_Expediente;

            UpdateExpedientesCount();
            return await Task.FromResult(expediente);
        }

        public async Task<bool> DeleteExpedienteAsync(int id)
        {
            var expediente = _expedientes.FirstOrDefault(e => e.Expediente_Id == id);
            if (expediente == null) return false;

            var cajaId = expediente.Caja_Id;
            _expedientes.Remove(expediente);
            UpdateExpedientesCount();

            // Regla de negocio: Al borrar el último EXPEDIENTE de una CAJA, automáticamente se borra la CAJA
            var expedientesRestantes = _expedientes.Count(e => e.Caja_Id == cajaId);
            if (expedientesRestantes == 0)
            {
                var caja = _cajas.FirstOrDefault(c => c.Caja_Id == cajaId);
                if (caja != null)
                {
                    _cajas.Remove(caja);
                }
            }

            return await Task.FromResult(true);
        }

        // Métodos de utilidad
        public async Task<int> GetExpedientesCountByCajaIdAsync(int cajaId)
        {
            return await Task.FromResult(_expedientes.Count(e => e.Caja_Id == cajaId));
        }

        public async Task<IEnumerable<string>> GetUbicacionesAsync()
        {
            return await Task.FromResult(Ubicaciones.Valores);
        }

        public async Task<IEnumerable<string>> GetTiposExpedienteAsync()
        {
            return await Task.FromResult(TiposExpediente.Valores);
        }
    }
}
