﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Contracts
{
    public interface ISMRepository
    {
        MessageResource Send(string mobileNumber, string body);
        short CreateVerifyCode();
    }
}
