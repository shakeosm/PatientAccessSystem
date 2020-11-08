using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.UI.Models.Email
{
    public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
