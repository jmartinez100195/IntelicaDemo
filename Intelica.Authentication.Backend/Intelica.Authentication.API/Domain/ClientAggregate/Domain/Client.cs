﻿namespace Intelica.Authentication.API.Domain.ClientAggregate.Domain
{
    public class Client
    {
        public string ClientID { get; private set; }
        public string ClientName { get; private set; }
        public string ClientRedirectUrl { get; private set; }
        public bool IsValid(string clientRedirectUrl)
        {
            return clientRedirectUrl.StartsWith(ClientRedirectUrl);
        }
    }
}