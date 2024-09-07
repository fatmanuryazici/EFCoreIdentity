namespace EFCoreIdentity.DTOs
{
    public record ChangePasswordDTO(
        Guid Id,
        string CurrentPassword, //mevcut sifre
        string NewPassword
        );
    
}
