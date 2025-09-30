# language: en
@triaje @alertas @tiempos
Feature: Alertas por tiempo de espera excedido según prioridad
  Para cumplir tiempos máximos por nivel de triaje
  Como médico
  Quiero recibir alertas visibles y sonoras cuando el tiempo se exceda

Background:
	Given que existen tiempos máximos configurados por prioridad:
		| prioridad      | max_espera_min |
		| Crítica        | 0              |
		| Emergencia     | 30             |
		| Urgencia       | 60             |
		| Urgencia menor | 120            |
		| Sin urgencia   | 240            |
	And existe el paciente "María Flores" con prioridad "Urgencia" en espera desde hace 65 minutos
	And estoy viendo la lista de pacientes con triaje asignado

@alerta
Scenario: Generar alerta
	When el tiempo de espera de "María Flores" supera el máximo de su prioridad
	Then se dispara una alerta en el panel
	And la alerta muestra: nombre "María Flores", prioridad "Urgencia" y tiempo excedido "5 min"
	And "María Flores" pasa a ser la primer paciente en la lista

Scenario: Excesos de tiempo por antigüedad
  Given existe el paciente "Juan Pérez" con prioridad "Urgencia"
  When el paciente "Juan Pérez" supera el tiempo máximo por 2 minutos
  Then "Juan Pérez" aparece en la lista con menos prioridad que "María Flores"
