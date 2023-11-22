using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace akidi_core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, List<Attachment> attachments);
    }
}
