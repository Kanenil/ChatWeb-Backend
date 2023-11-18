using ChatWeb.Domain;
using ChatWeb.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ChatWeb.Presistence;

public class ChatDbContext : IdentityDbContext<UserEntity, RoleEntity, int,
        IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasKey(ur => new { ur.UserId, ur.RoleId });

            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
            .IsRequired();
        });

        builder.Entity<ChatGroupEntity>(cg =>
        {
            cg.HasKey(cg => new { cg.UserId, cg.ChatId });

            cg.HasOne(cg => cg.User)
              .WithMany(u => u.ChatGroups)
              .HasForeignKey(u => u.UserId)
              .IsRequired();

            cg.HasOne(c => c.Chat)
              .WithMany(c => c.ChatGroups)
              .HasForeignKey(c => c.ChatId)
              .IsRequired();
        });
    }

    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<ChatGroupEntity> ChatGroups { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
}
