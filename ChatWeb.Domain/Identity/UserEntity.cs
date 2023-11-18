using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Domain.Identity;

public class UserEntity : IdentityUser<int>
{
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Image { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    public virtual ICollection<ChatGroupEntity> ChatGroups { get; set; }
    public virtual ICollection<MessageEntity> Messages { get; set; }
}
