using EFCoreIdentity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIdentity.AppContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid> 
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    //Identity kutuphanesinin dahil olabilmesi için IdentityDbContext e inherite ediyoruz
    //bunun içine de user classını vermemiz lazım,ama tek basına veremeyiz role back key i de vermemiz lazım
    //bir de en son key veriyoruz(Guid) IExecutable yapı için gerekli
    

    //IdentityRole classı kendi içinde rolunu ister
}
