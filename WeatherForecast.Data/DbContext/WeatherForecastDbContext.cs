using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.Entities;


namespace WeatherForecast.Data.DbContext;

public class WeatherForecastDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Entities.WeatherForecast> WeatherForecasts  { get; set; }
    public DbSet<Coordinate> Coordinates  { get; set; }
    
    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
    {
    }

 //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 //   {
 //       optionsBuilder.UseSqlite("Data Source=..\\WeatherForecast.Data\\WeatherForecast.db");
 //   }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Coordinate>()
            .HasMany(c => c.WeatherForecasts)
            .WithOne(w => w.Coordinate)
            .HasForeignKey(k => k.CoordinateId);


    }
}