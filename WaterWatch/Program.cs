using Microsoft.EntityFrameworkCore;
using WaterWatch.Data;
using Npgsql;
using WaterWatch.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Using App Secrets to manage DB Password
NpgsqlConnectionStringBuilder npgConnStrBuilder = new NpgsqlConnectionStringBuilder();
npgConnStrBuilder.Host = "localhost";
npgConnStrBuilder.Port = 5433;
npgConnStrBuilder.Database = "cptwater";
npgConnStrBuilder.Username = "waterwatch";
npgConnStrBuilder.Password = builder.Configuration["DbPassword"];

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(npgConnStrBuilder.ConnectionString)); 
builder.Services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
builder.Services.AddScoped<IWaterConsumptionRepository,WaterConsumptionRepository>();
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
