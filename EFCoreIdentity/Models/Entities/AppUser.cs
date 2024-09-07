using Microsoft.AspNetCore.Identity;

namespace EFCoreIdentity.Models.Entities
{
    public class AppUser : IdentityUser<Guid> //burdan inherite ediyoruz kolay yönetmek için
    {
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => string.Join(" ", FirstName,LastName); //database de bir alan olarak görünmeyecek
                                                                        //sadece newlendiğinde gelecek FullName diye cagırdıgımda
    }
}
