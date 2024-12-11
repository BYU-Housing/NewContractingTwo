using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ReslifeFiveBackEnd.Model;
using ReslifeFiveBusinessLayer.Service;
using System.Security.Claims;

namespace ReslifeFiveFrontEnd.Application.Authentication
{
    public class SamlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SamlMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public SamlMiddleware(RequestDelegate next, ILogger<SamlMiddleware> logger, IServiceScopeFactory scopefactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopefactory;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/SamlConsume")// This path should handle the SAML response
            {
                await HandleSamlResponse(context);
            }
            else if (context.Request.Path == "/Logout")
            {
                await FullLogout(context);
            }
            else
            {
                // If the request is not for the SAML response, 
                ///just call the next middleware in the pipeline
                await _next(context);
            }
        }





        private async Task HandleSamlResponse(HttpContext context)
        {
            string samlCertificate = @"-----BEGIN CERTIFICATE-----
MIIDHTCCAgWgAwIBAgIUKGn04Kl01bFUug79PAw2A0sPDz0wDQYJKoZIhvcNAQELBQAwGjEYMBYGA1UEAwwPY2FzLXN0Zy5ieXUuZWR1MB4XDTIwMTExNjIzMTMyNFoXDTQwMTExNjIzMTMyNFowGjEYMBYGA1UEAwwPY2FzLXN0Zy5ieXUuZWR1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAw3i9NSh/Ks4HZSk2OXsTPR4EYtgS4F3BvT5BT9UK06OswZSoa2Iw8sdGFQxunhtCOl6FqIRzG4U+3guOgKze55IJin7bdTDuNiLhSaWynOAIvDdIHFHxgrHhleJENtVmtDUnsLkGPhXQStYpHDuNG0p0EobdoinAUWIwdH2Iw0L64F4u3GXOLiXd1hJ7qSTrgoeSPh73qQWPcHqHDjMaX5gtJDR7/HG9OKqdQXiYzSiS2oG16y9ntjj0hpzdlQYQMImv/r3JK7foezkpQx/ZKB498xYp+e/M8rFO40IHY/RtfAylF0XlSWpQovF9D1bxayJ0+bi7Ikqgyn+m+GTkbwIDAQABo1swWTAdBgNVHQ4EFgQUhY6mSZ6u/kJCmdGj5y1x7+rmuPowOAYDVR0RBDEwL4IPY2FzLXN0Zy5ieXUuZWR1hhxjYXMtc3RnLmJ5dS5lZHUvaWRwL21ldGFkYXRhMA0GCSqGSIb3DQEBCwUAA4IBAQBx/Ihjsdbs9ryiGjHgNR/TG9gRPEwCe04WuD4Mb45iSvwaBfr2L9QonCC9x/d/QQpnEOYMIXoqZvDH+ZnGS5FmInoUUmIJ0DT9+WxIvmVwAWflQ+nUx+S3jxdfYxIVRiWtlzOswz5rIKGNxgyz7QywQhzs+ygsH9HAK2o13MHHoM1wgXq6Zj6nUnqQ7WJJq8biR2TV44Nhi5k4AFVHJQnhYAN1jt/IfPP9W9+5YihRJ/l6iRMnw99CwlhVSHUc3CIdpEyEmIeDdd3Ig3DKy3hMx32gEhZZ85DJonQRx8rts/YjqCk1avNMv7SPC076thrwbtF/WvjcBrNk5WE1btcE
-----END CERTIFICATE-----";

            var samlResponse = new Saml.Response(samlCertificate, context.Request.Form["SAMLResponse"]);

            if (samlResponse.IsValid())
            {
                var byuId = samlResponse.GetCustomAttribute("byuId");
                var netId = samlResponse.GetCustomAttribute("netId");
                _logger.LogInformation($"extracted netid {netId}");


               


                using(var scope = _scopeFactory.CreateScope())
                {
                    var genService = scope.ServiceProvider.GetRequiredService<IGenService>();

                    var user = genService.GetModel<User>().FirstOrDefault(x => x.NetID == netId);

                    var username = samlResponse.GetNameID() ?? user?.FName;


                    if (user != null)
                    {
                        var roleName = genService.GetModel<Role>().FirstOrDefault(x => x.Id == user.RoleID && x.Active == true)?.Name;



                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username ?? "No Name From SAML Or DB"),
                            new Claim(ClaimTypes.Role, roleName ?? "Guest")
                        };





                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        //complete login
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        //optionally redirect after login
                        context.Response.Redirect("/");
                    }
                    else
                    {
                        throw new NullReferenceException($"No User Found with netid {netId}");
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 401; //Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid SAML Response");
            }


        }

        private async Task FullLogout(HttpContext context)
        {
            var netId = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? "Unknown";
            _logger.LogInformation($"Fully logging out user with netId: {netId}");

            // Sign the user out of your application
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the CAS logout page (logging them out of CAS entirely)
            context.Response.Redirect("https://cas.byu.edu/cas/logout");
        }
    }
}
