using Domain.Entities;
using Domain.Entities.Alerts;
using Domain.Entities.Alerts.UserPreferences;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataContext;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<FoundAnimalAlert> FoundAnimalAlerts { get; set; } = null!;
	public DbSet<FoundAnimalUserPreferences> FoundAnimalUserPreferences { get; set; } = null!;
	public DbSet<Breed> Breeds { get; set; } = null!;
	public DbSet<Color> Colors { get; set; } = null!;
	public DbSet<Species> Species { get; set; } = null!;
}