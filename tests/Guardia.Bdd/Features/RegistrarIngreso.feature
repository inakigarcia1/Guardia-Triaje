Feature: Registrar Ingreso
  Como enfermera
  Quiero poder registrar las admisiones de los pacientes a urgencias
  Para determinar que pacientes tienen mayor prioridad de atención

Background:
	Given que existe un sistema de guardia con niveles de emergencia:
		| Nivel         | Color    | Tiempo Maximo |
		| Critico       | Rojo     | 5             |
		| Emergencia    | Naranja  | 30            |
		| Urgencia      | Amarillo | 60            |
		| UrgenciaMenor | Verde    | 120           |
		| SinUrgencia   | Azul     | 240           |

Scenario: Ingresar paciente existente con datos válidos
	Given que existe un paciente con CUIL "20123456789" y nombre "Juan Pérez"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	When la enfermera registra un ingreso para el paciente con:
		| Campo                   | Valor                  |
		| Informe                 | Dolor de pecho intenso |
		| Nivel de Emergencia     | Critico                |
		| Frecuencia Cardiaca     | 120                    |
		| Frecuencia Respiratoria | 20                     |
		| Tensión Sistólica       | 140                    |
		| Tensión Diastólica      | 90                     |
		| Temperatura             | 38                     |
	Then el ingreso se registra correctamente
	And el paciente queda en estado PENDIENTE
	And el ingreso se agrega a la cola de atención

Scenario: Ingresar paciente inexistente con datos válidos
	Given que no existe un paciente con CUIL "22123456780" y nombre "Santino Hamada"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	When la enfermera registra un ingreso para el paciente con:
		| Campo                   | Valor           |
		| Informe                 | Dolor de cabeza |
		| Nivel de Emergencia     | Emergencia      |
		| Frecuencia Cardiaca     | 100             |
		| Frecuencia Respiratoria | 18              |
		| Tensión Sistólica       | 120             |
		| Tensión Diastólica      | 80              |
		| Temperatura             | 37              |
	Then se debe crear el paciente antes de proceder al registro del ingreso
	And el ingreso se registra correctamente
	And el paciente queda en estado PENDIENTE
	And el ingreso se agrega a la cola de atención

Scenario: Ingresar paciente con datos mandatorios faltantes
	Given que existe un paciente con CUIL "20123456789" y nombre "Juan Pérez"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	When la enfermera intenta registrar un ingreso para el paciente omitiendo el informe
	Then se emite un mensaje de error indicando que el informe es obligatorio

Scenario: Ingresar paciente con frecuencia cardíaca negativa
	Given que existe un paciente con CUIL "20123456789" y nombre "Juan Pérez"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	When la enfermera intenta registrar un ingreso con frecuencia cardíaca "-10"
	Then se emite un mensaje de error indicando que la frecuencia cardíaca no puede ser negativa

Scenario: Ingresar paciente con frecuencia respiratoria negativa
	Given que existe un paciente con CUIL "20123456789" y nombre "Juan Pérez"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	When la enfermera intenta registrar un ingreso con frecuencia respiratoria "-5"
	Then se emite un mensaje de error indicando que la frecuencia respiratoria no puede ser negativa

Scenario: Verificar prioridad de atención - paciente crítico vs emergencia
	Given que existe un paciente A con CUIL "22451954275" y nombre "Ana García"
	And que existe un paciente B con CUIL "22451954275" y nombre "Carlos López"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	And el paciente B está en espera con nivel de emergencia "Emergencia"
	When la enfermera registra un ingreso para el paciente A con nivel "Critico"
	Then el paciente A debe ser atendido antes que el paciente B

Scenario: Verificar prioridad de atención - mismo nivel, orden por fecha
	Given que existe un paciente A con CUIL "22451954275" y nombre "Ana García"
	And que existe un paciente B con CUIL "20123456789" y nombre "Carlos López"
	And que existe una enfermera con matrícula "ENF001" y nombre "María González"
	And el paciente B está en espera con nivel de emergencia "Critico" desde hace 10 minutos
	When la enfermera registra un ingreso para el paciente A con nivel "Critico"
	Then el paciente B debe ser atendido antes que el paciente A
