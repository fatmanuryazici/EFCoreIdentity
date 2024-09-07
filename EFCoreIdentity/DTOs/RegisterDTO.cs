namespace EFCoreIdentity.DTOs
{
    public record RegisterDTO //kayıt islemi oldugu için record kullanıyorum
    (
      string Email,  
      string UserName,  //Identity kutuphanesi kayıt icin UserName parametresini zorunlu kılıyor
      string FirstName,
      string LastName,
      string Password
    );
    
    //username harf ve rakamdan olusabiliyor
    //email zorunlulugu yok ama istersek gecerli email ve unique email yapabiliyoruz

}
