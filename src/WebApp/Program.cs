using Alanyang.DotNetEmail.Infrastructure.Data;
using Alanyang.DotNetEmail.Infrastructure.Options;
using Alanyang.DotNetEmail.WebApp.Configuration;
using Alanyang.DotNetEmail.WebApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

// Add background services
builder.Services.AddHostedService<TimedEmailSendingService>();

builder.Services.AddCoreServices();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
