using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
}