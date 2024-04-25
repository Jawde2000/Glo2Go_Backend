namespace BaseLibrary.Responses
{
    public record LoginResponse(bool Flag, String Message = null!, string Token = null!, string RefreshToken = null!);
}
