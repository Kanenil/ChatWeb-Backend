using ChatWeb.Domain.Common;
using ChatWeb.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatWeb.Domain;

[Table("tblChats")]
public class ChatEntity : BaseEntity<int>
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Image { get; set; }

    [ForeignKey("LastEditionAuthor")]
    public int LastEditionAuthorId { get; set; }
    public virtual UserEntity LastEditionAuthor { get; set; }

    [ForeignKey("CreateAuthor")]
    public int CreateAuthorId { get; set; }
    public virtual UserEntity CreateAuthor { get; set; }

    public virtual ICollection<ChatGroupEntity> ChatGroups { get; set; }
    public virtual ICollection<MessageEntity> Messages { get; set; }
}
