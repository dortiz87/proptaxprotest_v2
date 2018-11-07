using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proptaxprotest.Models
{
    public class AppSettings
    {
        public string BraintreeEnvironment { get; set; }

        public string BraintreeMerchantId { get; set; }

        public string BraintreePublicKey { get; set; }

        public string BraintreePrivateKey { get; set; }
    }
}

