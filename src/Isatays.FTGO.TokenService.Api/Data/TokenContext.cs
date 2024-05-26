using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api.Data;

public class TokenContext : DbContext
{
    public TokenContext(DbContextOptions<TokenContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
}