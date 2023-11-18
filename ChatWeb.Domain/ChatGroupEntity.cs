using ChatWeb.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatWeb.Domain;

[Table("tblChatGroups")]
public class ChatGroupEntity
{
    [ForeignKey("Chat")]
    public int ChatId { get; set; }
    public virtual ChatEntity Chat { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
}
