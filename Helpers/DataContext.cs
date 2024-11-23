namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entidades;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("TestDb");
    }

    // database de exemplo
    // public DbSet<User> Users { get; set; }
}