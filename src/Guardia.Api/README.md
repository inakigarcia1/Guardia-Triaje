# Sistema de Guardia Hospitalaria - API

## Descripción
Sistema de guardia priorizada con triaje para hospitales que permite registrar admisiones de pacientes y gestionar la cola de atención según niveles de emergencia.

## Características Principales

### Niveles de Emergencia
- **Crítico (Rojo)**: 5 minutos máximo
- **Emergencia (Naranja)**: 30 minutos máximo  
- **Urgencia (Amarillo)**: 60 minutos máximo
- **Urgencia Menor (Verde)**: 120 minutos máximo
- **Sin Urgencia (Azul)**: 240 minutos máximo

### Estados de Ingreso
- **PENDIENTE**: Paciente en espera de atención
- **EN_PROCESO**: Paciente siendo atendido
- **FINALIZADO**: Atención completada

## Endpoints de la API

### POST /api/ingreso/registrar
Registra un nuevo ingreso de paciente.

**Request Body:**
```json
{
  "dniPaciente": 12345678,
  "informe": "Dolor de pecho intenso",
  "nivelEmergencia": "Critico",
  "temperatura": 37.5,
  "frecuenciaCardiaca": 120,
  "frecuenciaRespiratoria": 20,
  "tensionSistolica": 140,
  "tensionDiastolica": 90,
  "matriculaEnfermero": "ENF001"
}
```

**Response (Success):**
```json
{
  "message": "Ingreso registrado correctamente",
  "ingreso": {
    "id": "guid",
    "fechaIngreso": "2024-01-01T10:00:00",
    "paciente": {
      "dni": 12345678,
      "nombre": "Juan Pérez"
    },
    "nivelEmergencia": {
      "prioridad": "Critico",
      "color": "Rojo",
      "tiempoMaximo": 5
    },
    "estado": "PENDIENTE",
    "enfermero": {
      "matricula": "ENF001",
      "nombre": "María González"
    }
  }
}
```

**Response (Error):**
```json
{
  "error": "El paciente no existe en el sistema. Se debe crear el paciente antes de proceder al registro del ingreso."
}
```

### GET /api/ingreso/cola-atencion
Obtiene la cola de atención ordenada por prioridad.

**Response:**
```json
[
  {
    "id": "guid",
    "fechaIngreso": "2024-01-01T10:00:00",
    "paciente": {
      "dni": 12345678,
      "nombre": "Juan Pérez"
    },
    "nivelEmergencia": {
      "prioridad": "Critico",
      "color": "Rojo",
      "tiempoMaximo": 5
    },
    "estado": "PENDIENTE",
    "informe": "Dolor de pecho intenso",
    "frecuenciaCardiaca": 120,
    "frecuenciaRespiratoria": 20,
    "tensionArterial": "140/90",
    "temperatura": 37.5
  }
]
```

## Validaciones

### Datos Mandatorios
- Informe (string)
- Matrícula del enfermero (string)
- Frecuencia cardíaca (float, no negativo)
- Frecuencia respiratoria (float, no negativo)
- Tensión sistólica (float, no negativo)
- Tensión diastólica (float, no negativo)

### Reglas de Negocio
1. El paciente debe existir en el sistema antes del registro
2. Los valores de frecuencia cardíaca y respiratoria no pueden ser negativos
3. Los valores de tensión arterial no pueden ser negativos
4. La cola se ordena por prioridad (crítico > emergencia > urgencia > urgencia menor > sin urgencia)
5. Para el mismo nivel de prioridad, se ordena por fecha de ingreso (FIFO)

## Cómo Ejecutar

1. **Ejecutar la aplicación:**
   ```bash
   dotnet run --project Guardia.Consola
   ```

2. **Acceder a Swagger UI:**
   ```
   https://localhost:7000/swagger
   ```

3. **Ejecutar tests:**
   ```bash
   dotnet test
   ```

4. **Ejecutar tests BDD:**
   ```bash
   dotnet test --project Guardia.Bdd
   ```

## Arquitectura

### Capas
- **Controllers**: Endpoints de la Web API
- **Services**: Lógica de negocio
- **Repositories**: Acceso a datos (en memoria)
- **Entities**: Modelos del dominio

### Patrones Utilizados
- **Repository Pattern**: Para abstracción de datos
- **Dependency Injection**: Para inyección de dependencias
- **Service Layer**: Para lógica de negocio

## Tests

### Unit Tests
- Tests para `IngresoService`
- Validación de reglas de negocio
- Tests de repositorios en memoria

### BDD Tests
- Feature: `RegistrarAdmision.feature`
- Step Definitions: `RegistrarAdmisionStepDefinitions.cs`
- Cobertura de todos los criterios de aceptación
