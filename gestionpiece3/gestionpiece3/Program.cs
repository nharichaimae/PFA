using gestionpiece3.Data;
using Microsoft.EntityFrameworkCore;
using gestionpiece3.Services;

var builder = WebApplication.CreateBuilder(args);

//base de donne 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));


//appler les service que j'ai utiliser 
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<SymfonyAuthService>();
builder.Services.AddScoped<PieceService>();

//=> se connecte avec angular 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:54552")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseCors("AllowAngular");

app.UseAuthorization();


app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pieces}/{action=Add}/{id?}"
);

app.Run();
