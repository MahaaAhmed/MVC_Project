using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Helper
{
    public static class EmailsSettings
    {
        public static void SendEmail(Email email)
         {
            var Client = new SmtpClient("smtp.sendgrid.net" , 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("apikey", "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" );
            Client.Send("aliaatarek25@gmail.com", email.ReciverEmail ,email.Tittle ,email.Body);
        }
    }
}
