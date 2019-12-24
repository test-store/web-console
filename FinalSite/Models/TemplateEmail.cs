using System;
using System.Collections.Generic;
using System.Text;
using EASendMail; 
using FinalSite.DAL;
using Microsoft.AspNet.Identity;

namespace FinalSite.Models
{
   public class TTemplateEmail
    {
        String email1;
        String email2;
        public void EnviarCorreo(string email)
        {
            
          
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Your gmail email address
                oMail.From = "soportembww@gmail.com";
                // Set recipient email address

                oMail.To = email;
            
                // Set email subject
                oMail.Subject = "Compra de productos";
                // Set email body
                oMail.TextBody = "Gracias por su compra en orionPeru.azure ";

                // Gmail SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                // Gmail user authentication
                // For example: your email is "gmailid@gmail.com", then the user should be the same
                oServer.User = "soportembww@gmail.com";
                oServer.Password = "altron123@";

                // Set 587 port, if you want to use 25 port, please change 587 5o 25
                oServer.Port = 587;

                // detect SSL/TLS automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                Console.WriteLine("start to send email over SSL ...");

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
    }
}