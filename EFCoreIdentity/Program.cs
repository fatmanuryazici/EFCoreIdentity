
using EFCoreIdentity.AppContext;
using EFCoreIdentity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString"));
            });

            builder.Services.AddIdentity<AppUser,AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;    //bu ayarlar� yapmazsak �dentity kutuphanesi
                                                        //defaultta 6 karakter buyukharf kucukharf ve �zel karakter istiyor
                options.User.RequireUniqueEmail = true; //bunu verince gecerli bir email girmemiz gerekiyor.
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3; //�ifreyi 3 defa ard arda yanl�s girerse kitle
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//1 dakikal���na giri�i kitle

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();  //dependencyInjection ile userManager �n neler yapacag�n� �zelle�tirmemiz gerekiyor
                                                                                     //AddDefaultTokeProviders demezsek token �retme i�lemini yapm�yor

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
