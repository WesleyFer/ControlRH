﻿namespace ControlRH.Core.Application
{
    public class KeycloakOptions
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
