namespace hotel_erp.Api.Services;

public sealed class SecuritySessionRevokedException() : Exception("La sesión fue revocada; vuelva a iniciar sesión");
public sealed class PasswordChangeRejectedException(string message) : Exception(message);
