using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Animerch.Models
{
    public class EmailSender2 : IEmailSender
    {
        public EmailSender2()
        {
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.FromResult(0);
        }
    }
}
