using Microsoft.AspNetCore.Identity;

namespace EFCoreIdentity.Models.Entities
{
    //eklemek zorunda degiliz ama eklersek IdentityRole class ı daha rahat calisir
    public class AppRole :IdentityRole<Guid>
    {
    }
}
