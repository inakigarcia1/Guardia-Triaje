# language: en
@login @auth
Feature: Autenticación de usuarios (Login)
  Para acceder a la aplicación de guardia
  Como usuario registrado
  Quiero iniciar sesión

Background:
	Given que existe un usuario "enfermero@mail.com" con contraseña "SoyEnfemer0!"

@feliz @ui @api
Scenario: Login exitoso
	When ingreso email "enfermero@mail.com" y contraseña "SoyEnfemer0!"
	Then ingreso correctamente a la aplicación

@validacion @seguridad
Scenario Outline: Credenciales inválidas
	When ingreso email "<email>" y contraseña "<password>"
	And hago clic en "Iniciar sesión"
	Then veo el error "Credenciales inválidas"
	And no ingreso a la aplicación

Examples:
	| email                   | password     |
	| enfermero@mail.com      | WrongPass    |
	| usuario_inexistente@foo | SoyEnfemer0! |
	|                         | SoyEnfemer0! |
	| enfermero@mail.com      |              |

@validacion
Scenario: Email y contraseña obligatorios
	When dejo vacío el email y la contraseña
	And hago clic en "Iniciar sesión"
	Then veo el error "El email es obligatorio"
	And veo el error "La contraseña es obligatoria"
	And el login no se realiza

