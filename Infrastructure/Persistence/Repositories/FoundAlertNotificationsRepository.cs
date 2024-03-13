using Application.Common.Interfaces.Repositories;
using Domain.Entities.Alerts.Notifications;
using Infrastructure.Persistence.DataContext;
using Application.Common.Entities;
using Domain.Entities;
using Domain.Entities.Alerts;
using Domain.Entities.Alerts.UserPreferences;
using Infrastructure.Persistence.QueryLogics;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Persistence.Repositories;

public class FoundAlertNotificationsRepository : GenericRepository<FoundAnimalAlertNotifications>,
	IFoundAlertNotificationsRepository
{
	private readonly AppDbContext _dbContext;

	public FoundAlertNotificationsRepository(AppDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public List<FoundAnimalUserPreferences> GetUsersThatMatchPreferences(FoundAnimalAlertMessage alertMessage)
	{
		Point alertLocation = CoordinatesCalculator.CreatePointBasedOnCoordinates(alertMessage.FoundLocationLatitude,
			alertMessage.FoundLocationLongitude);

		// TODO: Filter based on a percentage and not all preferences, around 85% (value to be analyzed and decided)
		// of what the user set as preference
		return _dbContext.FoundAnimalUserPreferences
			.Include(preferences => preferences.User)
			.Include(preferences => preferences.Colors)
			.Where(preferences => preferences.Location == null ||
			                      preferences.Location.Distance(alertLocation) <=
			                      preferences.RadiusDistanceInKm * 1000 &&
			                      preferences.Gender == null || preferences.Gender == alertMessage.Gender &&
			                      preferences.Breed == null || preferences.BreedId == alertMessage.BreedId &&
			                      preferences.Species == null || preferences.SpeciesId == alertMessage.SpeciesId &&
			                      !preferences.Colors.Any() ||
			                      preferences.Colors.Any(color => alertMessage.ColorIds.Contains(color.Id)))
			.ToList();
	}

	public void SaveNotifications(List<User> usersThatMatchPreferences, FoundAnimalAlertMessage messageData)
	{
		FoundAnimalAlert? alert = GetAlert(messageData.Id);
		if (alert is null)
		{
			throw new Exception("Could not get alert.");
		}

		FoundAnimalAlertNotifications notification = new()
		{
			Users = usersThatMatchPreferences,
			TimeStampUtc = DateTime.UtcNow,
			FoundAnimalAlert = alert,
			FoundAnimalAlertId = alert.Id
		};

		_dbContext.Add(notification);
		_dbContext.SaveChanges();
	}

	private FoundAnimalAlert? GetAlert(Guid alertId)
	{
		return _dbContext.FoundAnimalAlerts
			.SingleOrDefault(alert => alert.Id == alertId);
	}
}