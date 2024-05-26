using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isatays.FTGO.TokenService.Api.Data;

public class BaseEntity
{
    [Key]
    [Column(name:"id")]
    public int Id { get; set; }
}