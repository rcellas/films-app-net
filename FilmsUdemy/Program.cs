using FilmsUdemy.Data;
using FilmsUdemy.Endpoints;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
using FilmsUdemy.Repositories.Actors;
using FilmsUdemy.Repositories.CommentsFilms;
using FilmsUdemy.Repositories.Errors;
using FilmsUdemy.Repositories.Films;
using FilmsUdemy.Repositories.Genders;
using FilmsUdemy.Service;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var originsAllowed = builder.Configuration.GetValue<string>("OriginsAllowed")!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(originsAllowed).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// lo que hacemos es registrar la interfaz y la clase que implementa la interfaz, pero también nos sirve para llamar este servicio en cualquier parte de la aplicación

builder.Services.AddScoped<IRespostoryGenderFilm, RepositoriesGender>();
builder.Services.AddScoped<IRepositoryActors, RepositoriesActors>();
builder.Services.AddScoped<IRepositoryFilms, RepositoriesFilms>();
builder.Services.AddScoped<IRepositoriesCommentsFilms, RepositoriesCommentsFilms>();
builder.Services.AddScoped<IRepositoriesErrors, RepositoriesErrors>();

builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddHttpContextAccessor();

// Lo que hace el AddAutoMapper es registrar el servicio de AutoMapper en la aplicación y aplicar las configuraciones de AutoMapper en la aplicación
builder.Services.AddAutoMapper(typeof(Program));

//Para construir las validaciones de FluentValidation (debemos instalar el paquete de FluentValidation.AspNetCore)
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}
// con esto lo que hacemos es que si hay un error en la aplicación, lo que haremos es que se ejecute el código que está dentro del UseExceptionHandler. Que a su vez lo que hace es que obtiene el error y lo guarda en la base de datos
app.UseExceptionHandler(exceptionHandlerApp=>exceptionHandlerApp.Run(async context =>
{
    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionHandlerFeature?.Error!;
    var error = new Error();
    
    error.Date = DateTime.UtcNow;
    error.Message = exception.Message;
    error.StackTrace = exception.StackTrace;
    
    var repositoriesErrors = context.RequestServices.GetRequiredService<IRepositoriesErrors>();
    await repositoriesErrors.CreateError(error);
}));
app.UseStatusCodePages();

app.UseCors();
app.UseOutputCache();

 
app.UseStaticFiles();

//app.MapGet("/", [EnableCors(policyName:"free")]() => "Hello World!");
app.MapGet("/", () => "Hello World!");
// ejemplo de error y el funcionamiento de la excepción
// app.MapGet("/error", () =>
// {
//     throw new InvalidOperationException("error de prueba");
// });
app.MapGroup("/gender").MapGenders();
app.MapGroup("/actors").MapActors();
app.MapGroup("/films").MapFilms();
app.MapGroup("/films/{filmId:int}/comments").MapComments();

app.Run();

