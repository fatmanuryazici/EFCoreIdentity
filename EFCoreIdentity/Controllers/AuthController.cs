using EFCoreIdentity.DTOs;
using EFCoreIdentity.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EFCoreIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request, CancellationToken cancellationToken)
        {
            //Db kayıt islemi

            AppUser appUser = new() //default bu 4 parametreyi vermem yeterli Passwordu UserManager kendi hallediyor
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            IdentityResult result=await userManager.CreateAsync(appUser,request.Password); //bunu verince passwordu otomatik hashliyor
             if (!result.Succeeded)      //hata fırlatmadıgı için if ile sorguluyoruz                                                  //kaydedebilirse kaydediyor,kaydedemezse bana geriye result döndürüyor hata fırlatmıyor
            {
                return BadRequest(result.Errors.Select(s=>s.Description)); //bunla beraber bize hangi alanda patladıgını söylüyor
                                                                          //basarılıysa bisey yapmamıza gerek yok
            }
            

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            AppUser? appuser = await userManager.FindByIdAsync(request.Id.ToString()); //bulamazsa null bulabilirse kullancıyı dönecegi
                                                                                       //bir nullable user dönüyor
              if (appuser is null)                                                     
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }
            IdentityResult result = await userManager.ChangePasswordAsync(appuser, request.CurrentPassword,
                request.NewPassword); 
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s=>s.Description));
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
        {
            AppUser? appuser=await userManager.FindByEmailAsync(email);
            if (appuser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(appuser); //şifre sıfırlamada eski şifre olmazsa sıfırlama yapamayız
            return Ok(new {Token= token});

            //kullanıcıyı bulursa bize özel token üretiyor.
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordUsingToken(ChangePasswordUsingTokenDTO request, CancellationToken cancellationToken)
        {
            AppUser? appuser = await userManager.FindByEmailAsync(request.Email);
            if (appuser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

           IdentityResult result= await userManager.ResetPasswordAsync(appuser, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request,CancellationToken cancellationToken)
        {
            AppUser? appuser = 
                await userManager.Users
                .FirstOrDefaultAsync(p=>
                p.Email==request.UserNameOrEmail || 
                p.UserName== request.UserNameOrEmail, cancellationToken);
            // .Users diyerek default tüm Linque metotlarına erişiyorum. Ve hem userName hem maile bakabiliyorum.

            if (appuser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

            bool result=await userManager.CheckPasswordAsync(appuser,request.Password);
            if (!result)
            {
                return BadRequest(new { Message = "Sifre yanlis" });
            }
            return Ok(new { Token = "Token" });

        }

        [HttpPost]
        public async Task<IActionResult> LoginWithSignInManager(LoginDTO request, CancellationToken cancellationToken)
        {
            AppUser? appuser =
                await userManager.Users
                .FirstOrDefaultAsync(p =>
                p.Email == request.UserNameOrEmail ||
                p.UserName == request.UserNameOrEmail, cancellationToken);
            // .Users diyerek default tüm Linque metotlarına erişiyorum. Ve hem userName hem maile bakabiliyorum.

            if (appuser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

            SignInResult result=await signInManager.CheckPasswordSignInAsync(appuser,request.Password,true);

            if (result.IsLockedOut)
            {
                TimeSpan? timeSpan = appuser.LockoutEnd - DateTime.Now;
                if (timeSpan is not null)
                {
                    return StatusCode(500, $"Sifrenizi 3 kere yanlıs girdiginiz icin kullanıcınız {timeSpan.Value.TotalSeconds} " +
                        $"saniye girise yasaklanmıstır.Sure bitiminde tekrar giris yapabilirsiniz ");
                }
                else
                {
                    return StatusCode(500, $"Sifrenizi 3 kere yanlıs girdiginiz icin kullanıcınız 30 saniye girise yasaklanmıstır.Sure bitiminde tekrar giris yapabilirsiniz ");
                }
            }
           
            if (result.Succeeded)
            {
                return StatusCode(500, "Sifreniz yanlıs");
            }
            if (result.IsNotAllowed)
            {
                return StatusCode(500, "Mail adresiniz onaylı degil");
            }
            return Ok(new { Token = "Token" });
            
       

        }
    }
}
