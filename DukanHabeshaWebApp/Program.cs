
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using DukanHabeshaDataAccess.Data;
using DukanHabeshaUtility;
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaDataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//this line binds the properties in StripeSettings.cs attributes with the Stripe in appsetting.json file
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//add the identity to the service to the container(After Scaffoling the identity this created)........options => options.SignIn.RequireConfirmedAccount = true

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();;

// Add services to the container.
builder.Services.AddControllersWithViews();

//we are connection with our server, the GetConnection method will find the "ConnectionStrings"
//in the appsetting.json
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")
//    ));

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
//(builder.Configuration.GetConnectionString("DefaultConnection"),
//b => b.MigrationsAssembly("BookStoreWebApp")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//fake email implementation
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1219018082193396";
    options.AppSecret = "5fbee9178f4043332c05a581e270de5c";
});




//these are a custumized route for these paths, coz if we dont do like this the web doesnt work good
//example, when we tried to add an item into cart, we run into some kind of bad request
//we are adding this faeture to customers cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


//this below code helps to configure the session of the web
builder.Services.AddDistributedMemoryCache();
//this below code helps to configure the session of the web to the container
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//adding Stripe API in global level -- .Get<string> means get the return value which is string
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//should always come before authorization(after scaffolding)
app.UseAuthentication();;

app.UseAuthorization();

app.UseSession();


app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();