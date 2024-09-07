
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
                options.Password.RequiredLength = 1;    //bu ayarlarý yapmazsak ýdentity kutuphanesi
                                                        //defaultta 6 karakter buyukharf kucukharf ve özel karakter istiyor
                options.User.RequireUniqueEmail = true; //bunu verince gecerli bir email girmemiz gerekiyor.
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3; //þifreyi 3 defa ard arda yanlýs girerse kitle
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//1 dakikalýðýna giriþi kitle

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();  //dependencyInjection ile userManager ýn neler yapacagýný özelleþtirmemiz gerekiyor
                                                                                     //AddDefaultTokeProviders demezsek token üretme iþlemini yapmýyor

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
