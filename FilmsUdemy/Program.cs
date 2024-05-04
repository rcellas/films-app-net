using FilmsUdemy.Data;
using FilmsUdemy.Endpoints;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
using FilmsUdemy.Repositories.Actors;
using FilmsUdemy.Repositories.CommentsFilms;
using FilmsUdemy.Repositories.Films;
using FilmsUdemy.Service;
using Microsoft.AspNetCore.Cors;using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var originsAllowed = builder.Configuration.GetValue<string>("OriginsAllowed")!;

builder.Services.AddDbContext<ApplicationDBContext>(options =>
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

builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddHttpContextAccessor();

// Lo que hace el AddAutoMapper es registrar el servicio de AutoMapper en la aplicación y aplicar las configuraciones de AutoMapper en la aplicación
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseCors();
app.UseOutputCache();

app.UseStaticFiles();

//app.MapGet("/", [EnableCors(policyName:"free")]() => "Hello World!");
app.MapGet("/", () => "Hello World!");

app.MapGroup("/gender").MapGenders();
app.MapGroup("/actors").MapActors();
app.MapGroup("/films").MapFilms();
app.MapGroup("/films/{filmId:int}/comments").MapComments();

app.Run();

