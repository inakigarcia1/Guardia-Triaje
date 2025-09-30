# language: en
@triaje @adulto
Feature: Activación de triaje para paciente adulto
  Para derivar pacientes adultos a una guardia con prioridad
  Como enfermero/a
  Quiero registrar motivo, signos vitales y observaciones, asignar prioridad

Background:
	Given que existe una lista de pacientes PENDIENTES de triaje
	And existe el paciente "Santino Hamada" de 30 años con un ingreso registrado
	And hay prioridades preestablecidas: "Sin urgencia", "Urgencia menor", "Urgencia", "Emergencia", "Crítica"
	And yo inicio sesión como usuario con rol "ENFERMERO"

Rule: Validación de signos vitales
    # Rangos (criterios de aceptación de Historia 1)
    #   T: 30–50 °C, SO: 60–120, FC: 60–200 ppm, TA: 4–20

@seleccion
Scenario: Seleccionar paciente PENDIENTE de triaje
	When selecciono el registro de atención PENDIENTE del paciente "Santino Hamada"
	Then veo los datos del paciente

@basico
Scenario: Crear triaje adulto con signos vitales válidos (flujo básico)
	Given seleccioné al paciente "Santino Hamada" con triaje PENDIENTE
	When registro el motivo "fiebre"
	And registro los signos vitales: FC 80 ppm, TA 120/80 mmHg, SO 95, T 38
	And registro observaciones "sin particularidades"
	And asigno la prioridad "Urgencia menor"
	Then se crea el triaje
	And el paciente deja de figurar en la lista de pendientes

@validacion
Scenario Outline: Triaje con campos no válidos
	Given selecciono el registro de atención PENDIENTE del paciente "Santino Hamada"
	When intento guardar signos vitales inválidos: <campo> = <valor_invalido>
	Then se solicita corrección del dato "<campo>"
	When corrijo "<campo>" a "<valor_valido>"
	And asigno la prioridad "Urgencia menor"
	Then se crea el triaje
	And el paciente deja de figurar en la lista de pendientes

Examples:
	| campo | valor_invalido | valor_valido |
	| FC    | -80            | 80           |
	| T     | 10             | 38           |
	| SO    | 30             | 95           |
	| TA    | 2              | 12           |
	| Motivo| ""             | "Fiebre"     |

