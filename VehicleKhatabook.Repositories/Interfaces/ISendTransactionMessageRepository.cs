using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ISendTransactionMessageRepository
    {
        void SendTransactionMessage(string transactionType, string userId, string driverOwnerUserId, decimal? amount);
    }
}
