using AutoMapper;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync(userId);
            return _mapper.Map<IEnumerable<Notification>>(notifications);
        }

        public async Task<NotificationDTO> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.MarkNotificationAsReadAsync(notificationId);
            return _mapper.Map<NotificationDTO>(notification);
        }

        public async Task AddNotificationsAsync(IEnumerable<Notification> notifications)
        {
            await _notificationRepository.AddNotificationsAsync(notifications);
        }
        /// <summary>
        /// Checks for expiring subscriptions, licenses, vehicle insurance, and registrations
        /// and creates notifications for users.
        /// </summary>
        public async Task CheckForExpirationsAndNotifyAsync()
        {
            var today = DateTime.UtcNow.Date;

            // Get users
            var users = await _userRepository.GetAllUsersAsync();

            foreach (var user in users)
            {
                var notifications = new List<Notification>();

                // Check subscription expirations
                if (user.PremiumExpiryDate != null)
                {
                    if (user.PremiumExpiryDate <= today.AddDays(7) && user.PremiumExpiryDate >= today)
                    {
                        notifications.Add(new Notification
                        {
                            NotificationID = Guid.NewGuid(),
                            UserID = user.UserId,
                            Message = $"Your subscription will expire on {user.PremiumExpiryDate:dd-MM-yyyy}.",
                            NotificationType = "SubscriptionExpiry",
                            NotificationDate = DateTime.UtcNow,
                            IsRead = false
                        });
                    }
                    else if (user.PremiumExpiryDate < today)
                    {
                        notifications.Add(new Notification
                        {
                            NotificationID = Guid.NewGuid(),
                            UserID = user.UserId,
                            Message = $"Your subscription expired on {user.PremiumExpiryDate:dd-MM-yyyy}.",
                            NotificationType = "SubscriptionExpiry",
                            NotificationDate = DateTime.UtcNow,
                            IsRead = false
                        });
                    }
                }

                // Check vehicle-related expirations
                var (isUserActive, hasVehicles, vehicles) = await _vehicleRepository.GetAllVehiclesAsync(user.UserId);
                if (vehicles != null)
                {
                    foreach (var vehicle in vehicles)
                    {
                        AddVehicleExpirationNotification(vehicle.InsuranceExpiry, "InsuranceExpiry", "insurance", vehicle, user, notifications, today);
                        AddVehicleExpirationNotification(vehicle.PollutionExpiry, "PollutionExpiry", "pollution", vehicle, user, notifications, today);
                        AddVehicleExpirationNotification(vehicle.FitnessExpiry, "FitnessExpiry", "fitness", vehicle, user, notifications, today);
                        AddVehicleExpirationNotification(vehicle.RoadTaxExpiry, "RoadTaxExpiry", "road tax", vehicle, user, notifications, today);
                        AddVehicleExpirationNotification(vehicle.RCPermitExpiry, "RCPermitExpiry", "RC permit", vehicle, user, notifications, today);
                        AddVehicleExpirationNotification(vehicle.NationalPermitExpiry, "NationalPermitExpiry", "national permit", vehicle, user, notifications, today);
                    }
                }
                // Add notifications to database
                if (notifications.Any())
                {
                    await _notificationRepository.AddNotificationsAsync(notifications);
                }
            }
        }

        /// <summary>
        /// Helper method to handle vehicle expiration notifications.
        /// </summary>
        private void AddVehicleExpirationNotification(DateTime? expiryDate, string notificationType, string description, Vehicle vehicle, UserDTO user, List<Notification> notifications, DateTime today)
        {
            if (expiryDate != null)
            {
                if (expiryDate <= today.AddDays(7) && expiryDate >= today)
                {
                    notifications.Add(new Notification
                    {
                        NotificationID = Guid.NewGuid(),
                        UserID = user.UserId,
                        Message = $"The {description} for your vehicle {vehicle.RegistrationNumber} will expire on {expiryDate:dd-MM-yyyy}.",
                        NotificationType = notificationType,
                        NotificationDate = DateTime.UtcNow,
                        IsRead = false
                    });
                }
                else if (expiryDate < today)
                {
                    notifications.Add(new Notification
                    {
                        NotificationID = Guid.NewGuid(),
                        UserID = user.UserId,
                        Message = $"The {description} for your vehicle {vehicle.RegistrationNumber} expired on {expiryDate:dd-MM-yyyy}.",
                        NotificationType = notificationType,
                        NotificationDate = DateTime.UtcNow,
                        IsRead = false
                    });
                }
            }
        }

        public async Task DeleteAllNotificationsAsync()
        {
            await _notificationRepository.DeleteAllNotificationsAsync();
        }

        public async Task DeleteAllNotificationsForUserAsync(Guid userId)
        {
            await _notificationRepository.DeleteAllNotificationsForUserAsync(userId);
        }

    }
}
