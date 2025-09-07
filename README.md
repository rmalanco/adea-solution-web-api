# ADEA Solution Web API

API backend para la gestión de Cajas y Expedientes desarrollada para el examen técnico de ADEA MX.

## Características

- **Entidades**: Cajas y Expedientes
- **Persistencia**: Base de datos en memoria
- **Reglas de negocio**: Implementadas según especificaciones
- **CORS**: Configurado para comunicación con frontend Angular
- **Swagger**: Documentación automática de la API

## Endpoints

### Cajas (`/api/cajas`)

- `GET /api/cajas` - Obtener todas las cajas
- `GET /api/cajas/{id}` - Obtener caja por ID
- `POST /api/cajas` - Crear nueva caja
- `PUT /api/cajas/{id}` - Actualizar caja
- `DELETE /api/cajas/{id}` - Eliminar caja
- `GET /api/cajas/{id}/expedientes` - Obtener expedientes de una caja

### Expedientes (`/api/expedientes`)

- `GET /api/expedientes` - Obtener todos los expedientes
- `GET /api/expedientes/{id}` - Obtener expediente por ID
- `POST /api/expedientes` - Crear nuevo expediente
- `PUT /api/expedientes/{id}` - Actualizar expediente
- `DELETE /api/expedientes/{id}` - Eliminar expediente

### Opciones (`/api/opciones`)

- `GET /api/opciones/ubicaciones` - Obtener ubicaciones disponibles
- `GET /api/opciones/tipos-expediente` - Obtener tipos de expediente disponibles

## Reglas de Negocio Implementadas

1. ✅ Una CAJA puede tener 1 o más EXPEDIENTES
2. ✅ Mostrar el número de EXPEDIENTES por CAJA en el listado
3. ✅ No permitir borrar CAJAS que cuenten con EXPEDIENTES
4. ✅ Al borrar el último EXPEDIENTE de una CAJA, automáticamente se borra la CAJA

## Validaciones

### Cajas
- Estado: Obligatorio, exactamente 3 caracteres
- Ubicación: Obligatorio, debe ser una de las opciones válidas

### Expedientes
- Caja_Id: Obligatorio, debe existir la caja
- Nombre_Empleado: Obligatorio, máximo 100 caracteres
- Tipo_Expediente: Obligatorio, debe ser uno de los tipos válidos

## Ejecutar la API

```bash
cd adea-solution-web-api
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`

## Datos de Ejemplo

La API incluye datos de ejemplo para pruebas:
- 3 cajas con diferentes ubicaciones
- 4 expedientes distribuidos entre las cajas
