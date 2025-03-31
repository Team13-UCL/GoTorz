using Microsoft.AspNetCore.Authentication; // Authentication handling
using Microsoft.AspNetCore.Authentication.Cookies; // Manages authentication using cookies
using System.Collections.Concurrent; // Provides a thread-safe dictionary for storing logged-in users
using System.Security.Claims; // Provides claims-based identity

namespace GoTorz.Components.Middleware //THE TOOOOOOOOOOOOOOOOOOOOOOOOOOOBE OF WARE TO THE MIDDLE :DDDDDDDDDDDDDD
{

    public class AuthMiddleware // Authenticates requests
    {
        private readonly RequestDelegate next;

        // Logins are thread-safe disctonary which stores users authentication details the key is Guid and value is ClaimsPrincipal (Whicvh stores the user's identity hopefully
        public static IDictionary<Guid, ClaimsPrincipal> Logins { get; private set; } = new ConcurrentDictionary<Guid, ClaimsPrincipal>();

        // Middleware is a pipeline structure where ''next'' represents the next middleware in the pipeline, ''next'' is required to ensure the continuation of the pipeline if login is not required
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Checks if the request path is /login and contains a key
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]); // Extracts the key from the query string and converts it to a Guid
                var claim = Logins[key]; // Retrieves the Corresponding ClaimsPrincipal from the Logins dictionary
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim); // Signs in the User using cookies
                context.Response.Redirect("/"); // Redirects the user to the home page
            }
            else 
            {
                await next(context); // Continues the pipeline (If the request is not for /login, it passes the request to the next middleware)
            }
        }
    }
}