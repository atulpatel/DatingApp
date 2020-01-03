using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {            
        }
        public DbSet<value> Values {get; set;}
        public DbSet<WeatherForecast> WeatherForecasts {get; set;}
    }
}