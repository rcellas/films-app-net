using FilmsUdemy.Data;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
using Microsoft.AspNetCore.Cors;
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

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseCors();
app.UseOutputCache();

//app.MapGet("/", [EnableCors(policyName:"free")]() => "Hello World!");
app.MapGet("/", () => "Hello World!");
app.MapGet("/gender", async ( IRespostoryGenderFilm repository)  =>
{
    return await repository.GetAll();
}).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("gender-get"));

app.MapGet("/gender/${id:int}", async (IRespostoryGenderFilm repository, int id) =>
{
     var gender = await repository.GetById(id);
     
     // si no existe el registro, devolverá un 404
     if (gender is null)
     {
         return Results.NotFound();
     }
    
     // aquí no ponemos solo gender ya que tenemoss que devolver un objeto de tipo Results para que devuelva un 200
     // esto ocurre pq nosotros arriba estamos devolviendo un Results.NotFound() y necesitamos devolver un Results.Ok
     return Results.Ok(gender);
});

// el IOutputCacheStore nos permite borrar la cache de un tag en concreto
app.MapPost("/gender", async (GenderFilms gender, IRespostoryGenderFilm repository, IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(gender);
    // con el await le decimos que espere a que se ejecute el EvictByTagAsync y que al momento de crear el registro, borre la cache
    await outputCacheStore.EvictByTagAsync("gender-get",default);
    return Results.Created($"/gender/{id}", gender);
});

app.MapPut("/gender/{id:int}",
    async (int id, GenderFilms gender, IRespostoryGenderFilm repostory, IOutputCacheStore outputCacheStore) =>
    {
        //miramos si existe la id
        var exist = await repostory.Exist(id);

        if (!exist)
        {
            return Results.NotFound();
        }

        await repostory.Update(gender);
        await outputCacheStore.EvictByTagAsync("gender-get", default);
        
        // con el Results.NoContent() le decimos que no devuelva nada, pero que el registro se ha actualizado correctamente
        return Results.NoContent();
    });
app.MapDelete("/gender/{id:int}", async (int id, IRespostoryGenderFilm repository, IOutputCacheStore outputCacheStore) =>
{
    var exist = await repository.Exist(id);

    if (!exist)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("gender-get", default);
    return Results.NoContent();
});
app.Run();
