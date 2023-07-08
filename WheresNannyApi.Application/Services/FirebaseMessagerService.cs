﻿using FirebaseAdmin.Messaging;
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
        public void SendNotification(Message message)
        {
            _ = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
        }
    }
}
