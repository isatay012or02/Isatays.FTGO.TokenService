using System.ComponentModel.DataAnnotations.Schema;

namespace Isatays.FTGO.TokenService.Api.Data;

[Table(name: "User", Schema = "public")]
public class User : BaseEntity
{
    [Column(name:"username")]
    public string? UserName { get; set; }
    
    [Column(name:"password")]
    public string? Password { get; set; }
    
    [Column(name:"roleid")]
    public int RoleId { get; set; }
}