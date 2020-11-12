using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;

namespace PR.Client
{
    //#3
    class Auth
    {
        private static IPublicClientApplication app;

        public static async Task<bool> InitializeAuthentication()
        {

            app = PublicClientApplicationBuilder.Create("67dd9cfb-4344-4cc8-a2ca-573f6bb4422f")
              .WithAuthority("https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0")
              .WithDefaultRedirectUri()
              .Build();

            var token =  GetToken().ConfigureAwait(false);

            return true;
        }

        public static async Task<string> GetToken()
        {
            var accounts = (await app.GetAccountsAsync()).ToList();

            AuthenticationResult result = null;

            try
            {
                result = await app.AcquireTokenSilent(new string[] { "api://67dd9cfb-4344-4cc8-a2ca-573f6bb4422f/.default" }, accounts.FirstOrDefault())
                      .ExecuteAsync()
                      .ConfigureAwait(false);
            }

            catch (MsalUiRequiredException)
            {
                result = await app.AcquireTokenInteractive(new string[] { "api://67dd9cfb-4344-4cc8-a2ca-573f6bb4422f/.default" })
                               .ExecuteAsync();
            }

            return result.AccessToken;
        }
    }
}
