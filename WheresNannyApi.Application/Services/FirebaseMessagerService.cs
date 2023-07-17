using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Application.Interfaces;

namespace WheresNannyApi.Application.Services
{
    public class FirebaseMessagerService : IFirebaseMessagerService
    {
        public string SendNotification(Message message)
        {
            var response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            return response;
        }
    }
}
