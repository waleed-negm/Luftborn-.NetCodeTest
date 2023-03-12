using Core.Domain.Entities;
using Core.Domain.LKP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Context
{
    public class LuftbornContext: IdentityDbContext<User>
    {
        protected string? _username;
        public LuftbornContext( DbContextOptions<LuftbornContext> options ,IHttpContextAccessor accessor):base(options)
        {
            if(accessor.HttpContext != null)
            {
                _username = accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }
        public LuftbornContext(DbContextOptions<LuftbornContext> options) : base(options)
        {}

        #region entities
        public virtual DbSet<Category> Categories { get; set; } 
        public virtual DbSet<Item> Items { get; set; } 
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationShip in builder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
                relationShip.DeleteBehavior = DeleteBehavior.NoAction;
            builder.Entity<Category>().HasQueryFilter(f => !f.IsDeleted);
            builder.Entity<Item>().HasQueryFilter(f => !f.IsDeleted);
            base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken= default)
        {
            foreach (var entity in ChangeTracker.Entries<BaseModel>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedOn = DateTime.Now.ToLocalTime();
                        entity.Entity.CreatedBy = _username;
                        break;
                    case EntityState.Modified:
                        entity.Entity.ModifiedOn = DateTime.Now.ToLocalTime();
                        entity.Entity.ModifiedBy = _username;
                        break;
                    case EntityState.Deleted:
                        entity.Entity.DeletedOn = DateTime.Now.ToLocalTime();
                        entity.Entity.DeletedBy = _username;
                        entity.Entity.IsDeleted = true;
                        break;
                }
            }
            foreach (var entity in ChangeTracker.Entries<User>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedOn = DateTime.Now;
                        entity.Entity.CreatedBy = _username;
                        entity.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entity.Entity.ModifiedOn = DateTime.Now;
                        entity.Entity.ModifiedBy = _username;
                        break;
                    case EntityState.Deleted:
                        entity.State = EntityState.Modified;
                        entity.Entity.DeletedOn = DateTime.Now;
                        entity.Entity.DeletedBy = _username;
                        entity.Entity.IsDeleted = true;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
