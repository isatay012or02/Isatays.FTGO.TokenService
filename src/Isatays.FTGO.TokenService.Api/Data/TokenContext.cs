using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api.Data;

public class TokenContext(DbContextOptions<TokenContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
}