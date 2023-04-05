using GitlabMonitor.Model.Statistic;
using Microsoft.EntityFrameworkCore;

namespace GitlabMonitor.Infrastructure;

public sealed class ApplicationContext : DbContext
{
    public DbSet<AssignedMergeRequest> AssignedMergeRequests { get; set; }
    
    public DbSet<Reviewer> Reviewers { get; set; }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
#if RELEASE
        Database.EnsureCreated();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reviewer>()
            .HasIndex(x => x.UserId);
    }
}