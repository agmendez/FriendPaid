﻿using System;
using System.Text;
using Domain.Utils;

namespace Domain.Notifications
{
    public class PaymentNotification : Notification
    {

        #region Methods

         public PaymentNotification()
         {
             
         }

        public PaymentNotification(Payment payment)
        {
            StringBuilder unMensaje = new StringBuilder("El miembro ");

            unMensaje.Append(payment.debtor.name).Append(" del grupo ")
                     .Append(payment.group.name).Append(" ha registrado un pago de $")
                     .Append(payment.amount.ToString()).Append(" en relacion a ")
                     .Append(payment.description);

            message = unMensaje.ToString();

            status = NotificationStatus.Unread;

            date = DateTime.Now.Date;

        }

        #endregion

    }
}
