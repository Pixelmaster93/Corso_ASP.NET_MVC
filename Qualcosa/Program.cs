using CityInfoAPI;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using CityInfoAPI.DbContexts;
using CityInfoAPI.Services;
using Serilog;
using CityInfo.API.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)//Qua imposto il path
    .CreateLogger(); //Log personalizzato per scrivere un file di log


var builder = WebApplication.CreateBuilder(args);
//I 2 qua sotto posso rimuoverli perchè ora utilizzo il mio log!
//builder.Logging.ClearProviders(); 
//builder.Logging.AddConsole();

builder.Host.UseSerilog();// Utilizzo il log creato sopra in Log.Logger

// Add services to the container.

builder.Services.AddControllers( options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();// Registro il Local Mail Service ussando l'indipencece injection
//Ora può essere igniettata un'istanza di LocalMailService!!
#else
builder.Services.AddTransient<IMailService, CloudMailService>();// Qua implaemnto il cloud mail service se non sono in debug userà questo
#endif

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(
    dbContextOpions => dbContextOpions.UseSqlite(
        builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));// inserisco il path del DB in appsettings.Develpment.json cosi posso mettere questo "path" 

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
