using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task<bool> SendPushNotificationToDevice(string firebaseToken, string? notificationType, string? message);
    }
}
