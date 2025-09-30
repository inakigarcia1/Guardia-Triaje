# language: en
@medico @guardia
Feature: Atención en guardia por prioridad
  Para asegurar atención oportuna
  Como médico de guardia
  Quiero ver la lista de pacientes con triaje por prioridad y registrar Atenciones

Background:
	Given estoy autenticado como médico de guardia
	And hay pacientes con triaje en espera con prioridades: "Crítica", "Emergencia", "Urgencia", "Urgencia menor", "Sin urgencia"

@orden
Scenario: Ver la lista ordenada por prioridad
	Given existen los pacientes "Ana" (Crítica), "Bruno" (Urgencia), "Carla" (Emergencia)
	When consulto la lista de pacientes con triaje
	Then la lista se presenta en orden: "Ana", "Carla", "Bruno"

@desempate
Scenario: Mismo nivel de prioridad, se atiende por mayor antigüedad de espera
	Given "Dora" (Emergencia, espera 40 min) y "Ezequiel" (Emergencia, espera 15 min)
	When consulto la lista de pacientes con triaje
	Then la lista se presenta en orden: "Ana", "Ezequiel"

@registro
Scenario: Registrar Atención completa
	Given el paciente "Mateo" (Urgencia) está en espera
	When inicio la atención de "Mateo"
	And registro diagnóstico presuntivo "Estreñimiento"
	And registro procedimientos realizados:
		"""
		Se realizó una manometría anorrectal
		"""
	Then queda registrada la Atención en Guardia de "Mateo" con todos los datos ingresados
	And "Mateo" deja de figurar en la lista de espera
