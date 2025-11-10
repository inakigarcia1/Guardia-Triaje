using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Aplicacion.DTOs;
public record LoginDto(string Username, string Password);
public record RegisterDto(string Username, string Email, string Password);
public record AuthResponse(
    string Token,
    string Username
);
