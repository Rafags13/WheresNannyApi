using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IFirebaseMessagerService
    {
        public void SendNotification(Message message);
    }
}
