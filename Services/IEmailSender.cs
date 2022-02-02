using LabManAPI.Data;
using LabManAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LabManAPI.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }

}

