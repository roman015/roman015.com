using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureB2C
{
    public class AzureAdB2COptions
    {
        public const string PolicyAuthenticationProperty = "Policy";

        public AzureAdB2COptions()
        {
            AzureAdB2CInstance = "https://roman015AD.b2clogin.com/tfp";//cea18c8b-419b-4cf8-bf52-33de291ed32b/B2C_1_SignUpSignIn/v2.0/
            //https://roman015AD.b2clogin.com/tfp/roman015AD.onmicrosoft.com/B2C_1_SignUpSignIn/v2.0/.well-known/openid-configuration
        }

        public string ClientId { get; set; }
        public string AzureAdB2CInstance { get; set; }
        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string SignInPolicyId { get; set; }
        public string SignUpPolicyId { get; set; }
        public string ResetPasswordPolicyId { get; set; }
        public string EditProfilePolicyId { get; set; }
        public string RedirectUri { get; set; }

        public string DefaultPolicy => SignUpSignInPolicyId;
        public string Authority => $"{AzureAdB2CInstance}/{Tenant}/{DefaultPolicy}/v2.0";

        public string ClientSecret { get; set; }
        public string ApiUrl { get; set; }
        public string ApiScopes { get; set; }

    }
}