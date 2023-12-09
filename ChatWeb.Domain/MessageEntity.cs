using ChatWeb.Domain.Common;
using ChatWeb.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatWeb.Domain;

[Table("tblMessages")]
public class MessageEntity : BaseEntity<int>
{
    [StringLength(500)]
    public string Content { get; set; } = string.Empty;

    [StringLength(100)]
    public string FileName { get; set; } = string.Empty;

    [ForeignKey("Chat")]
    public int ChatId { get; set; }
    public virtual ChatEntity Chat { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
}
