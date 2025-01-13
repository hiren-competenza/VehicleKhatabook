using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;
using FirebaseAdmin.Messaging;

namespace VehicleKhatabook.Services.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly FirebaseSettings _firebaseSettings;

        public FirebaseService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IVehicleRepository vehicleRepository,
            IMapper mapper,
            IOptions<FirebaseSettings> firebaseSettings)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _firebaseSettings = firebaseSettings.Value;
        }



        public async Task<bool> SendPushNotificationToDevice(string? deviceToken, string? notificationType, string? message)
        {
            if (string.IsNullOrEmpty(deviceToken) || string.IsNullOrEmpty(notificationType) || string.IsNullOrEmpty(message))
            {
                return false;
            }

            try
            {
                // Create the notification message
                var notificationMessage = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationType, // Notification title
                        Body = message // Notification body
                    },
                    Token = deviceToken, // Target device token
                    Android = new AndroidConfig()
                    {
                        Notification = new AndroidNotification()
                        {
                            Title = notificationType,
                            Body = message,
                        }
                    },
                    Data = new Dictionary<string, string>() // Additional data payload
                    {
                        ["NotificationType"] = notificationType,
                        ["Message"] = message
                    }
                };

                var result = await FirebaseMessaging.DefaultInstance.SendAsync(notificationMessage);
                Console.WriteLine(result);
                return !string.IsNullOrEmpty(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                return false;
            }
        }
    }
    }
