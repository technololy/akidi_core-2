using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace akidi_core.Utils
{
    public class EmailHandler
    {
        //public void sendGmailEmail()
        //{
        //    try
        //    {
        //        SmtpMail oMail = new SmtpMail("TryIt");

        //        // Your gmail email address
        //        oMail.From = "abdouldosso78@gmail.com";
        //        // Set recipient email address
        //        oMail.To = "abdoul.rhamane.dosso@gmail.com";

        //        // Set email subject
        //        oMail.Subject = "TEST MAIL";
        //        // Set email body
        //        oMail.TextBody = "MAIL CONTENT DDD";

        //        // Gmail SMTP server address
        //        SmtpServer oServer = new SmtpServer("smtp.gmail.com");

        //        // Gmail user authentication
        //        // For example: your email is "gmailid@gmail.com", then the user should be the same
        //        oServer.User = "abdouldosso78@gmail.com";

        //        // Create app password in Google account
        //        // https://support.google.com/accounts/answer/185833?hl=en
        //        oServer.Password = "your app password";

        //        // Set 465 port
        //        oServer.Port = 465;

        //        // detect SSL/TLS automatically
        //        oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

        //        Console.WriteLine("start to send email over SSL ...");

        //        SmtpClient oSmtp = new SmtpClient();
        //        oSmtp.SendMail(oServer, oMail);

        //        Console.WriteLine("email was sent successfully!");
        //    }
        //    catch (Exception ep)
        //    {
        //        Console.WriteLine("failed to send email with the following error:");
        //        Console.WriteLine(ep.Message);
        //    }
        //}

        public void sendGmailEmailBasic()
        {
            bool isBodyHtml = false;
            bool asAttachments = false;
            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("abdouldosso78@gmail.com", "Billdoss10$");
                using (var message = new MailMessage(
                    from: new MailAddress("abdouldosso78@gmail.com", "Abdoul Dosso"),
                    to: new MailAddress("abdoul.rhamane.dosso@gmail.com", "Bill")
                    ))
                {

                    message.Subject = "Hello from code!";

                    if (isBodyHtml)
                    {
                        message.IsBodyHtml = true;
                        message.Body = "<h1>Hello Doss!!</h1>";
                    }
                    else
                    {
                        message.IsBodyHtml = false;
                        message.Body = "Loremn ipsum dolor sit amet ...";

                    }

                    if (asAttachments)
                    {
                        message.Attachments.Add(new Attachment("C:\\file.zip"));
                    }



                    client.Send(message);
                }
            }
        }

        public void sendGmailEmailInternal()
        {
            bool isBodyHtml = false;
            bool asAttachments = false;
            using (var client = new SmtpClient())
            {
                client.Host = "38.242.231.137";
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                                
                client.Credentials = new NetworkCredential("abdoul.dosso", "akidi10$");
                using (var message = new MailMessage(
                    from: new MailAddress("abdouldosso78@gmail.com", "Abdoul Dosso"),
                    to: new MailAddress("abdoul.rhamane.dosso@gmail.com", "Bill")
                    ))
                {

                    message.Subject = "Hello from code!";

                    if (isBodyHtml)
                    {
                        message.IsBodyHtml = true;
                        message.Body = "<h1>Hello Doss!!</h1>";
                    }
                    else
                    {
                        message.IsBodyHtml = false;
                        message.Body = "Loremn ipsum dolor sit amet ...";

                    }

                    if (asAttachments)
                    {
                        message.Attachments.Add(new Attachment("C:\\file.zip"));
                    }



                    client.Send(message);
                }
            }
        }
    }
}
