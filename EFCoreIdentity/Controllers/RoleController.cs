using EFCoreIdentity.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIdentity.Controllers
{
    [Route("api/[controller] / [action]")]
    [ApiController]
    public class RoleController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Create(string name, CancellationToken cancellationToken)
        {
            AppRole approle = new()
            {
                Name = name,
            };

           IdentityResult result= await roleManager.CreateAsync(approle);
            //geriye identity result döndüğü için hata fırlatmaz,request döner.
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles=await roleManager.Roles.ToListAsync(cancellationToken);
            return Ok(roles);
        }
    }
}
