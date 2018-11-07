﻿using Braintree;
using proptaxprotest.Models;

namespace proptaxprotest
{
    
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        public string Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        private IBraintreeGateway BraintreeGateway { get; set; }
      

        public IBraintreeGateway CreateGateway()
        {
            Environment = System.Environment.GetEnvironmentVariable("BraintreeEnvironment");
            MerchantId = System.Environment.GetEnvironmentVariable("BraintreeMerchantId");
            PublicKey = System.Environment.GetEnvironmentVariable("BraintreePublicKey");
            PrivateKey = System.Environment.GetEnvironmentVariable("BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = "sandbox";
                MerchantId = "kg73b8bp8h39svd3";
                PublicKey = "t3349q4s29wkbwyp";
                PrivateKey = "3f34375c4a08dd66d854ad2538b63277";
            }

            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }



        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }

       
    }
}