using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.Services.Interfaces
{
    public interface IValidationService
    {
        Task<bool> IsValidMessage(Message message);
    }
}
