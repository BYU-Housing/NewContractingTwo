using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Saml;

namespace ReslifeFiveBusinessLayer.Service
{
    public class SamlService: ISamlService
    {
        private readonly string _samlEndpoint = "https://cas-stg.byu.edu/cas/idp/profile/SAML2/Redirect/SSO";
        private readonly string _entityId = "https://www.byureslife-dev.com";
        private readonly string _acsUrl = "https://www.byureslife-dev.com/SamlConsume";




       public string GenerateSAMLLoginUrl()
        {
            // AuthRequest is a class provided by your SAML library that
            // can create SAML requests
            var request = new AuthRequest(_entityId, _acsUrl);

            // Generate the full redirect URL to the IdP's login endpoint with the
            // encoded SAML request
            string samlRequestEncoded = HttpUtility.UrlEncode(request.GetRequest());
            string redirectUrl = $"{_samlEndpoint}?SAMLRequest={samlRequestEncoded}";
            return redirectUrl;
        }

    }
}
