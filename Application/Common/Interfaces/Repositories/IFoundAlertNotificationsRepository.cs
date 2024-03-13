using Application.Common.Entities;
using Domain.Entities;
using Domain.Entities.Alerts.Notifications;
using Domain.Entities.Alerts.UserPreferences;

namespace Application.Common.Interfaces.Repositories;

public interface IFoundAlertNotificationsRepository : IGenericRepository<FoundAnimalAlertNotifications>
{
	List<FoundAnimalUserPreferences> GetUsersThatMatchPreferences(FoundAnimalAlertMessage alertMessage);
	void SaveNotifications(List<User> usersThatMatchPreferences, FoundAnimalAlertMessage messageData);
}