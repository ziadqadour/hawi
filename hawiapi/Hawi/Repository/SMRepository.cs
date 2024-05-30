using Contracts;
using Microsoft.Extensions.Options;
using Hawi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

using Twilio.Rest.Api.V2010.Account;

namespace Repository
{
    public class SMRepository : ISMRepository
    {
        private readonly twillioseting _twillio;
        Random generator = new Random();
        
        public SMRepository(IOptions<twillioseting> twillio)
        {
            _twillio = twillio.Value;

        }

        public MessageResource Send(string mobileNumber, string body)
        {
            TwilioClient.Init(_twillio.AccountSID, _twillio.AuthToken);
            var result = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(_twillio.TwilioPhoneNumber),
                to: mobileNumber
                );
            return result;
        }

        //
        public short CreateVerifyCode()
        {
  
            return (short)generator.Next(1000, 9999);
        }

    }
}
