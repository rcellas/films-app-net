using FilmsUdemy.Entity;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Data;

public class ApplicationDBContext: DbContext
{
    // el DbContextOptions nos permite configurar la conexión a la base de datos
    public ApplicationDBContext(DbContextOptions options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //no podemos borrar la llamada a la base, ya que es la que se encarga de crear las tablas
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GenderFilms>().Property(p => p.Name).HasMaxLength(50);
        modelBuilder.Entity<Actor>().Property(p=>p.Name).HasMaxLength(150);
        
        // para la foto lo haremos con unicode ya que de esa forma podemos guardar cualquier tipo de caracter
        modelBuilder.Entity<Actor>().Property(p=>p.Photo).IsUnicode();
        
        //Films
        modelBuilder.Entity<Film>().Property(p=>p.Title).HasMaxLength(300);
        modelBuilder.Entity<Film>().Property(p => p.Poster).IsUnicode();
    }

    // DbSet es una colección de entidades que se pueden consultar, agregar, modificar y eliminar
    public DbSet<GenderFilms> Genders { get; set; }
    public DbSet<Actor> Actors { get; set; }
    
    public DbSet<Film> Films { get; set; }
    public DbSet<Comment?> Comments { get; set; }
}