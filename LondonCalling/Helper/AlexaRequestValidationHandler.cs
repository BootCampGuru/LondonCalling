using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LondonCalling.Helper
{
    public class AlexaRequestValidationHandler : System.Net.Http.DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation)
        {

            if (request.Headers.Contains("Signature"))
            {

                var signatureValue = request.Headers.GetValues("Signature").First();
                if (signatureValue == "")
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }

            if (request.Headers.Contains("SignatureCertChainUrl"))
            {

                var signatureValue = request.Headers.GetValues("SignatureCertChainUrl").First();
                if (signatureValue == "")
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }




            string chainUrl = null;
            if (!request.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER) ||
                String.IsNullOrEmpty(chainUrl = request.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).First()))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            string signature = null;
            if (!request.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER) ||
                String.IsNullOrEmpty(signature = request.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).First()))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            var signatureCertChainUrl = request.Headers.GetValues("SignatureCertChainUrl").First().Replace("/../", "/");

            var certUrl = new Uri(signatureCertChainUrl);

            if (!((certUrl.Port == 443 || certUrl.IsDefaultPort)
                && certUrl.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                && certUrl.Host.Equals("s3.amazonaws.com", StringComparison.OrdinalIgnoreCase)
                && certUrl.AbsolutePath.StartsWith("/echo.api/")
                ))
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

            using (var web = new System.Net.WebClient())
            {

                var certificate = web.DownloadData(certUrl);
                var cert = new X509Certificate2(certificate);

                var effectiveDate = DateTime.MinValue;
                var expiryDate = DateTime.MinValue;

                if (!((DateTime.TryParse(cert.GetExpirationDateString(), out expiryDate)
                    && expiryDate > DateTime.UtcNow)
                    && (DateTime.TryParse(cert.GetEffectiveDateString(), out effectiveDate)
                    && effectiveDate < DateTime.UtcNow)))
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);



                if (!cert.Subject.Contains("CN=echo-api.amazon.com") || !cert.Issuer.Contains("CN=Symantec Class 3 Secure Server CA"))
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }

                var signatureString = request.Headers.GetValues("Signature").First();

                byte[] signature1 = Convert.FromBase64String(signatureString);

                using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    var body = await request.Content.ReadAsStringAsync();
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    var data = sha1.ComputeHash(encoding.GetBytes(body));
                    var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                    if (rsa == null || rsa.VerifyHash(data, CryptoConfig.MapNameToOID("SHA1"), signature1))
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);



                }

                //if (!SpeechletRequestSignatureVerifier.VerifyCertificateUrl(signatureString))
                //{
                //    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                //}

                if (SpeechletRequestSignatureVerifier.RetrieveAndVerifyCertificate(signatureCertChainUrl) == null)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }


            }


            return await base.SendAsync(request, cancellation);



        }
    }
}