using LondonCalling.Helper.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LondonCalling.Helper
{
    public class Speechlet : ISpeechlet
    {
        public bool OnRequestValidation(AlexaResponse result, DateTime referenceTimeUtc, AlexaRequest requestEnvelope)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<bool> GetResponse(HttpRequestMessage httpRequest)
        {
            SpeechletRequestValidationResult validationResult = SpeechletRequestValidationResult.OK;
            DateTime now = DateTime.UtcNow; // reference time for this request
            bool httpResponse = true;

            if (httpRequest.Headers.Contains("Signature"))
            {

                var signatureValue = httpRequest.Headers.GetValues("Signature").First();
                if (signatureValue == "")
                {
                    validationResult = SpeechletRequestValidationResult.NoCertHeader;
                }
            }

            if (httpRequest.Headers.Contains("SignatureCertChainUrl"))
            {

                var signatureValue = httpRequest.Headers.GetValues("SignatureCertChainUrl").First();
                if (signatureValue == "")
                {
                    validationResult = SpeechletRequestValidationResult.NoCertHeader;
                }
            }




            string chainUrl = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER) ||
                String.IsNullOrEmpty(chainUrl = httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).First()))
            {
                validationResult = SpeechletRequestValidationResult.NoCertHeader;
            }

            string signature = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER) ||
                String.IsNullOrEmpty(signature = httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).First()))
            {
                validationResult = SpeechletRequestValidationResult.NoSignatureHeader;
            }

            var signatureCertChainUrl = httpRequest.Headers.GetValues("SignatureCertChainUrl").First().Replace("/../", "/");

            var certUrl = new Uri(signatureCertChainUrl);

            if (!((certUrl.Port == 443 || certUrl.IsDefaultPort)
                && certUrl.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                && certUrl.Host.Equals("s3.amazonaws.com", StringComparison.OrdinalIgnoreCase)
                && certUrl.AbsolutePath.StartsWith("/echo.api/")
                ))
                validationResult = SpeechletRequestValidationResult.NoCertHeader;

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
                    validationResult = SpeechletRequestValidationResult.NoCertHeader;



                if (!cert.Subject.Contains("CN=echo-api.amazon.com") || !cert.Issuer.Contains("CN=Symantec Class 3 Secure Server CA"))
                {
                    validationResult = SpeechletRequestValidationResult.NoCertHeader;
                }

                var signatureString = httpRequest.Headers.GetValues("Signature").First();

                byte[] signature1 = Convert.FromBase64String(signatureString);

                using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    var body = await httpRequest.Content.ReadAsStringAsync();
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    var data = sha1.ComputeHash(encoding.GetBytes(body));
                    var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                    if (rsa == null || rsa.VerifyHash(data, CryptoConfig.MapNameToOID("SHA1"), signature1))
                        validationResult = SpeechletRequestValidationResult.NoCertHeader;



                }

                if (validationResult != SpeechletRequestValidationResult.OK)
                {
                    httpResponse = false;
                }

                else
                {
                    httpResponse = true;
                }

                return httpResponse;
            }
        }
    }

}