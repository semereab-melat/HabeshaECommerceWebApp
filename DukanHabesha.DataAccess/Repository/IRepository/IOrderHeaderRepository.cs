using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader obj);

    
    //we make payment status null coz we dont want to update payment status each time
    void Updatestatus(int id, string orderStatus, string? paymentStatus=null);
    void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    
}
