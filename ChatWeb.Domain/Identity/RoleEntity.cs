using Microsoft.AspNetCore.Identity;

namespace ChatWeb.Domain.Identity;

public class RoleEntity : IdentityRole<int>
{
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
}
