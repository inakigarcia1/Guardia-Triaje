using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Guardia.Api.Controllers;
using Guardia.Aplicacion.Servicios;
using Guardia.Aplicacion.Validators;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Guardia.Tests;
public class AuthControllerTests
{
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        var authService = Substitute.For<IAuthService>();
        ConfigureLogin(authService);
        ConfigureRegister(authService);
        _authController = new AuthController(authService);
    }

    private static void ConfigureLogin(IAuthService authService)
    {
        authService.LoginAsync(Arg.Any<LoginDto>()).Returns(callInfo =>
        {
            var dto = callInfo.Arg<LoginDto>();

            return dto switch
            {
                { Username: "usuarioCorrecto", Password: "contraseñaCorrecta" } => Task.FromResult(
                    new AuthResponse(true, "token-de-ejemplo", dto.Username, [])),

                { Username: "usuarioCorrecto", Password: "12345678" } => Task.FromResult(
                    new AuthResponse(true, "token-de-ejemplo", dto.Username, [])),

                _ => Task.FromResult(new AuthResponse(false, string.Empty, dto.Username,
                    ["Usuario o contraseña no válidos."]))
            };
        });
    }

    private static void ConfigureRegister(IAuthService authService)
    {
        authService.RegistrarAsync(Arg.Any<RegistroUsuarioDto>(), Arg.Any<string>()).Returns(callInfo =>
        {
            var dto = callInfo.Arg<RegistroUsuarioDto>();
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                errores.Add("El nombre de usuario no puede estar vacío.");
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                errores.Add("El correo electrónico no puede estar vacío.");
            }
            else if (!new EmailAddressAttribute().IsValid(dto.Email))
            {
                errores.Add("El correo electrónico no es válido.");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                errores.Add("La contraseña no puede estar vacía.");
            }
            else if (dto.Password.Length < 8)
            {
                errores.Add("La contraseña debe tener una longitud mínima de 8 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(dto.Matricula))
            {
                errores.Add("La matrícula no puede estar vacía.");
            }
            if (string.IsNullOrWhiteSpace(dto.Cuil))
            {
                errores.Add("El CUIL no puede estar vacío.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Cuil, @"^\d+$"))
            {
                errores.Add("El CUIL solo puede contener números.");
            }
            else if (dto.Cuil.Length < 10)
            {
                errores.Add("El CUIL debe tener al menos 10 dígitos.");
            }
            else if (dto.Cuil.Length > 11)
            {
                errores.Add("El CUIL no puede tener más de 11 dígitos.");
            }

            return Task.FromResult(errores.Any() ? new RegisterResponse(false, "Error en el registro", errores) : new RegisterResponse(true, "Exito", []));
        });
    }

    [Fact]
    public async Task Login_UsuarioNoExiste_DebeDevolverUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto("usuario-inexistente", "contraseñaCorrecta");
        var resultadoNoAutorizado = new AuthResponse(false, string.Empty, "usuario-inexistente", ["Usuario o contraseña no válidos."]);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as UnauthorizedObjectResult;
        var resultadoValue = (resultado!.Value as AuthResponse)!;

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoNoAutorizado);
    }

    [Fact]
    public async Task Login_PasswordIncorrecta_DebeDevolverUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "contraseñaIncorrecta");
        var resultadoNoAutorizado = new AuthResponse(false, string.Empty, "usuarioCorrecto", ["Usuario o contraseña no válidos."]);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as UnauthorizedObjectResult;
        var resultadoValue = (resultado!.Value as AuthResponse)!;

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoNoAutorizado);

    }

    [Fact]
    public async Task Login_CredencialesValidas_DebeDevolverOkConToken()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "contraseñaCorrecta");
        var resultadoAutorizado = new AuthResponse(true, "token-de-ejemplo", "usuarioCorrecto", []);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as OkObjectResult;
        var resultadoValue = (resultado!.Value as AuthResponse)!;

        // Assert
        Assert.IsType<OkObjectResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoAutorizado);

    }

    [Fact]
    public async Task Login_UsuarioYPasswordIncorrectas_DebeDevolverUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioIncorrecto", "contraseñaIncorrecta");
        var resultadoNoAutorizado = new AuthResponse(false, string.Empty, "usuarioIncorrecto", ["Usuario o contraseña no válidos."]);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as UnauthorizedObjectResult;
        var resultadoValue = (resultado!.Value as AuthResponse)!;

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoNoAutorizado);
    }

    [Fact]
    public async Task Login_UsuarioVacio_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto(string.Empty, "contraseñaCorrecta");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Login_UsuarioNull_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto(null!, "contraseñaCorrecta");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Login_UsuarioWhitespace_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("   ", "contraseñaCorrecta");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Login_PasswordVacia_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", string.Empty);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Login_PasswordNull_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", null!);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Login_PasswordWhitespace_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "   ");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Login_UsuarioYPasswordVacios_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto(string.Empty, string.Empty);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Login_PasswordMenorA8Caracteres_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "short");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña debe tener al menos 8 caracteres.");
    }

    [Fact]
    public async Task Login_PasswordExactamente7Caracteres_DebeDevolverBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "1234567");

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña debe tener al menos 8 caracteres.");
    }

    [Fact]
    public async Task Login_PasswordExactamente8Caracteres_DebeDevolverOk()
    {
        // Arrange
        var loginDto = new LoginDto("usuarioCorrecto", "12345678");
        var resultadoAutorizado = new AuthResponse(true, "token-de-ejemplo", "usuarioCorrecto", []);

        // Act
        var resultado = await _authController.Login(loginDto, new LoginValidation()) as OkObjectResult;
        var resultadoValue = (resultado!.Value as AuthResponse)!;

        // Assert
        Assert.IsType<OkObjectResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoAutorizado);
    }

    // Tests de Register
    [Fact]
    public async Task Register_UsuarioVacio_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto(string.Empty, "test@example.com", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Register_UsuarioNull_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto(null!, "test@example.com", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Register_UsuarioWhitespace_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("   ", "test@example.com", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
    }

    [Fact]
    public async Task Register_EmailVacio_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", string.Empty, "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico es obligatorio.");
    }

    [Fact]
    public async Task Register_EmailNull_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", null!, "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico es obligatorio.");
    }

    [Fact]
    public async Task Register_EmailInvalido_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "email-invalido", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico no es válido.");
    }

    [Fact]
    public async Task Register_EmailSinArroba_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "emailinvalido.com", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico no es válido.");
    }

    [Fact]
    public async Task Register_EmailSinDominio_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "email@", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico no es válido.");
    }

    [Fact]
    public async Task Register_PasswordVacia_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", string.Empty, "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Register_PasswordNull_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", null!, "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Register_PasswordWhitespace_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "   ", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña es obligatoria.");
    }

    [Fact]
    public async Task Register_PasswordMenorA8Caracteres_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "short", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña debe tener al menos 8 caracteres.");
    }

    [Fact]
    public async Task Register_PasswordExactamente7Caracteres_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "1234567", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("La contraseña debe tener al menos 8 caracteres.");
    }

    [Fact]
    public async Task Register_TodosLosCamposVacios_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
        errores.Should().Contain("El correo electrónico es obligatorio.");
        errores.Should().Contain("La contraseña es obligatoria.");
        errores.Should().Contain("El CUIL es obligatorio.");
    }

    [Fact]
    public async Task Register_UsuarioYEmailInvalidos_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto(string.Empty, "email-invalido", "password123", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El nombre de usuario es obligatorio.");
        errores.Should().Contain("El correo electrónico no es válido.");
    }

    [Fact]
    public async Task Register_EmailYPasswordInvalidos_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "email-invalido", "short", "ENF-001", "20123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El correo electrónico no es válido.");
        errores.Should().Contain("La contraseña debe tener al menos 8 caracteres.");
    }

    [Fact]
    public async Task Register_DatosValidos_DebeDevolverCreated()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuarioValido", "test@example.com", "password123", "ENF-001", "20123456789");
        var resultadoExitoso = new RegisterResponse(true, "Exito", []);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as CreatedResult;
        var resultadoValue = (resultado!.Value as RegisterResponse)!;

        // Assert
        Assert.IsType<CreatedResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoExitoso);
    }

    [Fact]
    public async Task Register_PasswordExactamente8Caracteres_DebeDevolverCreated()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuarioValido", "test@example.com", "12345678", "ENF-001", "20123456789");
        var resultadoExitoso = new RegisterResponse(true, "Exito", []);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as CreatedResult;
        var resultadoValue = (resultado!.Value as RegisterResponse)!;

        // Assert
        Assert.IsType<CreatedResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoExitoso);
    }

    [Fact]
    public async Task Register_CuilVacio_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "password123", "ENF-001", string.Empty);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El CUIL es obligatorio.");
    }

    [Fact]
    public async Task Register_CuilNull_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "password123", "ENF-001", null!);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El CUIL es obligatorio.");
    }

    [Fact]
    public async Task Register_CuilConLetras_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "password123", "ENF-001", "20ABC56789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El CUIL solo puede contener números.");
    }

    [Fact]
    public async Task Register_CuilMenorA10Digitos_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "password123", "ENF-001", "123456789");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El CUIL debe tener al menos 10 dígitos.");
    }

    [Fact]
    public async Task Register_CuilMayorA11Digitos_DebeDevolverBadRequest()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuario", "test@example.com", "password123", "ENF-001", "201234567890");

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as BadRequestObjectResult;
        var errores = resultado!.Value as IEnumerable<string>;

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        errores.Should().Contain("El CUIL no puede tener más de 11 dígitos.");
    }

    [Fact]
    public async Task Register_CuilExactamente10Digitos_DebeDevolverCreated()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuarioValido", "test@example.com", "password123", "ENF-001", "2012345678");
        var resultadoExitoso = new RegisterResponse(true, "Exito", []);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as CreatedResult;
        var resultadoValue = (resultado!.Value as RegisterResponse)!;

        // Assert
        Assert.IsType<CreatedResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoExitoso);
    }

    [Fact]
    public async Task Register_CuilExactamente11Digitos_DebeDevolverCreated()
    {
        // Arrange
        var registerDto = new RegistroUsuarioDto("usuarioValido", "test@example.com", "password123", "ENF-001", "20123456789");
        var resultadoExitoso = new RegisterResponse(true, "Exito", []);

        // Act
        var resultado = await _authController.Registrar(registerDto, "Enfermero", new RegistroUsuarioValidator()) as CreatedResult;
        var resultadoValue = (resultado!.Value as RegisterResponse)!;

        // Assert
        Assert.IsType<CreatedResult>(resultado);
        resultadoValue.Should().BeEquivalentTo(resultadoExitoso);
    }

}

