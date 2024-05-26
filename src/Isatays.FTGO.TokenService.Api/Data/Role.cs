using System.ComponentModel.DataAnnotations.Schema;

namespace Isatays.FTGO.TokenService.Api.Data;

[Table(name: "Role", Schema = "public")]
public class Role : BaseEntity
{
    [Column(name:"code")]
    public string? Code { get; set; }
}