namespace EFCoreIdentity.DTOs
{
    public record ChangePasswordUsingTokenDTO(
       string Email,
       string NewPassword,
       string Token
        );
    
}
