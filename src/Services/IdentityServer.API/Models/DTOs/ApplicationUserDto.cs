namespace IdentityServer.API.Models.DTOs;

public record ApplicationUserDto(Guid Id, string Name, string Email, string Username, DateTime RegistrationDate);