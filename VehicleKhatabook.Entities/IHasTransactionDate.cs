using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Entities
{
    public interface IHasTransactionDate
    {
        DateTime TransactionDate { get; }
    }
}
