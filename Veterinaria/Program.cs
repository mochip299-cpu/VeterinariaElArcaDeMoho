using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Data;
using Veterinaria.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Razor Pages ───────────────────────────────────────────────────────────────
builder.Services.AddRazorPages(options =>
{
    // Proteger TODO por defecto
    options.Conventions.AuthorizeFolder("/");
    // Permitir acceso anónimo solo a Login y Registro
    options.Conventions.AllowAnonymousToFolder("/Auth");
});

// ── Entity Framework Core con SQLite ─────────────────────────────────────────
builder.Services.AddDbContext<VeterinariaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── ASP.NET Core Identity ─────────────────────────────────────────────────────
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Contraseña
    options.Password.RequireDigit           = true;
    options.Password.RequiredLength         = 8;
    options.Password.RequireUppercase       = true;
    options.Password.RequireLowercase       = true;
    options.Password.RequireNonAlphanumeric = false;

    // Bloqueo por intentos fallidos
    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers      = true;

    // Usuario
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<VeterinariaDbContext>()
.AddDefaultTokenProviders();

// ── Configurar cookie de autenticación ────────────────────────────────────────
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath        = "/Auth/Login";
    options.LogoutPath       = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccesoDenegado";
    options.ExpireTimeSpan   = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// ── Servicios de aplicación ───────────────────────────────────────────────────
builder.Services.AddScoped<VeterinariaService>();

var app = builder.Build();

// ── Aplicar migraciones automáticamente ──────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VeterinariaDbContext>();
    db.Database.Migrate();
}

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// ORDEN IMPORTANTE: Authentication antes que Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
