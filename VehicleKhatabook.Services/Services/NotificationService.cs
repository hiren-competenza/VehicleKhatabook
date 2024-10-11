using AutoMapper;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync(userId);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
        }

        public async Task<NotificationDTO> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.MarkNotificationAsReadAsync(notificationId);
            return _mapper.Map<NotificationDTO>(notification);
        }
    }
}
