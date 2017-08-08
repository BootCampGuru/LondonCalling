using LondonCalling.Data.APIModels;
using LondonCalling.Data.APIModels.RoadStar;
using LondonCalling.Data.Models;
using LondonCalling.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace LondonCalling.Controllers
{
    public class AlexaAltTranController : ApiController
    {
        static HttpClient client = new HttpClient();
        private static string host = "https://developer.nrel.gov";
        private string applicationId = "amzn1.ask.skill.d0b88736-a309-44ed-b003-3c7465481774";

        #region Fuel Stations
        private static string allFuelStations = @"/api/alt-fuel-stations/v1.json?api_key={0}&limit={1}&fuel_type={2}";
        private static string allNearbyFuelStations = @"/api/alt-fuel-stations/v1.json?api_key={0}&limit={1}&zip={2}&fuel_type={3}&city={4}";

        private static string allNearbyFuelStationsNoCity = @"/api/alt-fuel-stations/v1.json?api_key={0}&limit={1}&zip={2}&fuel_type={3}";

        private static string allNearbyFuelStationsByState = @"/api/alt-fuel-stations/v1.json?api_key={0}&limit={1}&state={2}&fuel_type={3}";

        private static string allFuelStationsCount = @"/api/alt-fuel-stations/v1.json?api_key={0}&fuel_type={1}";
        private static string allNearbyFuelStationsCount = @"/api/alt-fuel-stations/v1.json?api_key={0}&state={1}&fuel_type={2}";
        #endregion

        private static string client_id = "NJiRLfpeHsI5CPHVFq4W10WO3IZYGssuxTqeSw2V";

        #region Policy and Incentives

        private static string locationBasedPolicies = @"/api/transportation-incentives-laws/v1.json?api_key={0}&limit={1}&jurisdiction={2}&incentive_type={3}";
        private static string idBasedPolicy = @"/api/transportation-incentives-laws/v1/{0}.json?api_key={1}";
        private static string categoryBasedPolicy = @"/api/transportation-incentives-laws/v1/category-list.json?api_key={0}";
        private static string contactBasedPolicy = @"api/transportation-incentives-laws/v1/pocs.json?api_key={0}&jurisdiction={1}";

        private static string amazonHost = @"https://api.amazonalexa.com";
        private static string amazonAddress = @"/v1/devices/{0}/settings/address/countryAndPostalCode";
        private static string amazonFullAddress = @"/v1/devices/{0}/settings/address";
        #endregion

        #region GetAddress
        static PostalAddress GetAddress(string deviceId, string auth)
        {
            client = new HttpClient();
            PostalAddress address = new PostalAddress();
            try
            {
                string path = string.Format(amazonFullAddress, deviceId);
                client.BaseAddress = new Uri(amazonHost);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                HttpResponseMessage response = client.GetAsync(path).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;

                    address = Newtonsoft.Json.JsonConvert.DeserializeObject<PostalAddress>(result);
                }

            }
            catch
            {
                return null;
            }
            return address;
        }

        #endregion

        #region Policy and Incentive calls


        public static PolicyRootObject GetAllPoliciesByLocation(string state, string category)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            PolicyRootObject allPolicies = null;


            string path = string.Format(locationBasedPolicies, client_id, 1000, state, category);


            HttpResponseMessage response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allPolicies = Newtonsoft.Json.JsonConvert.DeserializeObject<PolicyRootObject>(result);
            }


            return allPolicies;
        }

        public static SinglePolicyRootObject GetAllPolicyById(string id)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            SinglePolicyRootObject allPolicies = null;

            string path = string.Format(idBasedPolicy, id, client_id);


            HttpResponseMessage response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allPolicies = Newtonsoft.Json.JsonConvert.DeserializeObject<SinglePolicyRootObject>(result);
            }


            return allPolicies;
        }

        static CategoryRootObject GetAllCategories()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Random random = new Random();
            int randomNumber = random.Next(0, 100);


            CategoryRootObject allPolicies = null;

            string path = string.Format(categoryBasedPolicy, client_id);


            HttpResponseMessage response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allPolicies = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryRootObject>(result);


            }


            return allPolicies;
        }


        static ContactRootObject GetContactBasedPolicy(string state)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            ContactRootObject allContactPolicies = null;

            string path = string.Format(contactBasedPolicy, client_id, state);


            HttpResponseMessage response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allContactPolicies = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactRootObject>(result);
            }


            return allContactPolicies;
        }




        #endregion


        #region Alternative Fuel Locations

        static int GetAllNearbyStationsCountByFuelType(string state, string type, string city)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string path = string.Format(allNearbyFuelStationsCount, client_id, state, type, city);

            AllStationRootObject allNearbyStations = null;


            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allNearbyStations = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStationRootObject>(result);
            }

            return allNearbyStations.fuel_stations.Count;
        }



        static int GetAllStationsCountByFuelType(string type)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allFuelStationsCount, client_id, type);


            AllStationRootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStationRootObject>(result);
            }

            return allStations.fuel_stations.Count;


        }

        static RootObject GetAllStationsAsync(string zip, string type)
        {

            client = new HttpClient();
            //// New code:
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allFuelStations, client_id, 10, zip, type);


            RootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(result);
            }



            return allStations;
        }


        static AllStationRootObject GetNearbyStationsAsync(string zip, string type, string city)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Empty;

            if (string.IsNullOrEmpty(city))
            {
                path = string.Format(allNearbyFuelStations, client_id, 10, zip, type, city);
            }
            else
            {
                path = string.Format(allNearbyFuelStationsNoCity, client_id, 10, zip, type);
            }


            AllStationRootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStationRootObject>(result);
            }



            return allStations;
        }


        #region GetNearbyStationbyState
        static AllStationRootObject GetNearbyStationbyState(string state, string type)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allNearbyFuelStationsByState, client_id, 10, state, type);


            AllStationRootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStationRootObject>(result);
            }



            return allStations;
        }

        #endregion

        #endregion

        #region Road Star Commands
        [HttpPost, Route("API/ALEXAAltTran/AltTran")]
        public dynamic AltTran(AlexaRequest alexaRequest)
        {


            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("Signature"))
            {

                var signatureValue = headers.GetValues("Signature").First();
                if (signatureValue == "")
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }

            if (headers.Contains("SignatureCertChainUrl"))
            {

                var signatureValue = headers.GetValues("SignatureCertChainUrl").First();
                if (signatureValue == "")
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }



            if (headers.Count() == 0)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Empty Url"
                };
            }

            if (headers.Contains("Signature"))
            {

                var signatureValue = headers.GetValues("Signature").First();
                if (signatureValue == "" || signatureValue == null)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Empty Url"
                    };
                }
            }

            if (headers.Contains("SignatureCertChainUrl"))
            {

                var signatureValue = headers.GetValues("SignatureCertChainUrl").First();
                if (signatureValue == "" || signatureValue == null)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Empty Url"
                    };
                }
            }



            if (!headers.Contains("Signature") || !headers.Contains("SignatureCertChainUrl"))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Empty Url"
                };

            }

            var signatureCertChainUrl = headers.GetValues("SignatureCertChainUrl").First().Replace("/../", "/");


            if (!String.IsNullOrEmpty(signatureCertChainUrl))
            {
                if ((signatureCertChainUrl.Length >= 8) && (signatureCertChainUrl.Substring(0, 8).ToLower() != "https://"))
                {
                    throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                }
                if ((signatureCertChainUrl.Length >= 24) && (signatureCertChainUrl.Substring(0, 24).ToLower() != "https://s3.amazonaws.com"))
                {
                    throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                }
                if (!signatureCertChainUrl.Contains("/echo.api/echo-api-cert"))
                {
                    throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                }

                string portTest = signatureCertChainUrl.ToLower().Split(':', '/')[1];
                if (!String.IsNullOrEmpty(portTest))
                {
                    if (portTest != "443")
                    {
                        throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                    }
                }
            }




            //Validate application Id
            if (alexaRequest.Session.Application.ApplicationId != applicationId)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            //not longer than 150 seconds
            var totalSeconds = (DateTime.Now - alexaRequest.Request.Timestamp).TotalSeconds;
            if (totalSeconds <= 0 || totalSeconds > 150)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));


            try
            {
                dynamic response = null;

                var request = new Requests().Create(new LondonCalling.Data.Models.Request
                {
                    MemberId = (alexaRequest.Session.Attributes == null) ? 0 : alexaRequest.Session.Attributes.MemberId,
                    Timestamp = alexaRequest.Request.Timestamp,
                    Intent = (alexaRequest.Request.Intent == null) ? "" : alexaRequest.Request.Intent.Name,
                    AppId = alexaRequest.Session.Application.ApplicationId,
                    RequestId = alexaRequest.Request.RequestId,
                    SessionId = alexaRequest.Session.SessionId,
                    UserId = alexaRequest.Session.User.UserId,
                    IsNew = alexaRequest.Session.New,
                    Version = alexaRequest.Version,
                    Type = alexaRequest.Request.Type,
                    Reason = alexaRequest.Request.Reason,
                    consentToken = alexaRequest.Session.User != null ? alexaRequest.Session.User.Permission.consentToken : "0",
                    deviceId = alexaRequest.context != null ? alexaRequest.context.System.device.deviceId : "0",
                    SlotsList = alexaRequest.Request.Intent.GetSlots(),
                    DateCreated = DateTime.UtcNow
                });

                switch (request.Type)
                {
                    case "LaunchRequest":
                        response = LaunchRequestHandler(request);
                        break;
                    case "IntentRequest":
                        response = IntentRequestHandler(request);
                        break;
                    case "SessionEndedRequest":
                        response = SessionEndedRequestHandler(request);
                        break;
                }

                return response;
            }
            catch (Exception ex)
            {

                return new
                {
                    version = "1.0",
                    sessionAttributes = new { },
                    response = new

                    {

                        outputSpeech = new
                        {

                            type = "PlainText",
                            text = "Road Star was unable to understand your voice command, please try your voice command again with a different set of parameters"
                        },
                        Card = new
                        {

                            type = "Simple",
                            title = "Road Star Error Message.",
                            content = "Road Star is currently having voice issues."
                        },
                        shouldEndSession = false
                    }

                };

            }

        }
        #endregion
        #region IntentRequestHandler

        private dynamic IntentRequestHandler(Request request)
        {
            dynamic response = null;

            switch (request.Intent)
            {
                case "HelloIntent":
                    response = HelloIntentHandler(request);
                    break;
                case "NearbyStationIntent":
                    response = NeareststationIntentHandler(request);
                    break;

                case "NearbyStationByZipIntent":
                    response = NearbyStationByZipIntentHandler(request);
                    break;

                case "NearbyStationNearMeIntent":
                    response = NearbyStationNearMeIntentHandler(request);
                    break;


                case "NearbyStationCountIntent":
                    response = NearestStationCountIntentHandler(request);
                    break;

                case "AllStationCountIntent":
                    response = AllStationCountIntentHandler(request);
                    break;

                case "CategoryBasedPolicyIntent":
                    response = CategoryBasedPolicyIntentHandler(request);
                    break;
                case "FuelBasedIntent":
                    response = FuelBasedIntentHandler(request);
                    break;
                case "AllPoliciesByLocationIntent":
                    response = AllPoliciesByLocationIntentHandler(request);
                    break;
                case "IDBasedPolicyIntent":
                    response = IDBasedPolicyIntentHandler(request);
                    break;

                case "ContactBasedPolicyIntent":
                    response = ContactBasedPolicyIntentHandler(request);
                    break;

                case "AMAZON.CancelIntent":
                    response = CancelOrStopIntentHandler(request);
                    break;
                case "AMAZON.StopIntent":
                    response = StopIntentHandler(request);
                    break;
                case "AMAZON.HelpIntent":
                    response = HelpIntent(request);
                    break;


            }

            return response;
        }

        #endregion
        #region Policy Handlers

        public dynamic IDBasedPolicyIntentHandler(Request request)
        {

            string speech = "";
            string id = "";
            var card = "ID based policy intent";

            if (request.SlotsList.Any())
            {

                id = request.SlotsList.FirstOrDefault(s => s.Key == "id").Value;


            }

            try
            {
                string _policy = string.Empty;

                SinglePolicyRootObject policy = GetAllPolicyById(id);
                if (policy != null)
                {


                    _policy = _policy + "<prosody rate=\"slow\">" + Helper.StateConversion.RemoveHTMLTags(policy.result.text) + "</prosody>";

                    card = _policy + Helper.StateConversion.RemoveHTMLTags(policy.result.text);
                    speech = _policy + "Do you want to find Alternative fule policy contacts in each state? You can find Alternative Fuel policy contacts in each state by saying, Get me All Contacts for Virginia";

                }
                else
                {
                    speech = "Road Star Could not locate policy information for policy number " + id + " , please try with a different policy number.";
                }

            }
            catch
            {
                speech = "Road Star is currently experiencing voice issue, please try back shortly.";
            }


            var response = new AlexaResponse(speech, false, AddSSML(speech), card);

            speech = "You can find Alternative Fuel policy contacts in each state by saying, Get me All Contacts for your state";
            //response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            //response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);

            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        private string ErrorMessage()
        {
            return "Road Star is having voice issue, please try back again shortly.";
        }

        public dynamic AllPoliciesByLocationIntentHandler(Request request)
        {

            client = new HttpClient();

            string speech = "";
            string state_type = "";
            string categories = "";
            string state = "";
            string cat = "";
            string category = "";
            string card = "All Policies by Location";


            if (request.SlotsList.Any())
            {

                state = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;
                cat = request.SlotsList.FirstOrDefault(s => s.Key == "categories").Value;


                state_type = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;
                categories = request.SlotsList.FirstOrDefault(s => s.Key == "categories").Value;

                state_type = Helper.StateConversion.ConvertState(state_type);

                category = Helper.StateConversion.ConvertCategory(categories);

            }

            try
            {
                string _contacts = string.Empty;
                int _total = 0;
                PolicyRootObject contacts = GetAllPoliciesByLocation(state_type, category);
                if (contacts != null)
                {
                    foreach (var con in contacts.result)
                    {
                        _contacts = _contacts + " Policy Number <say-as interpret-as=\"digits\">" + con.id + "</say-as>" + " The Policy is " + con.title + " ";
                    }

                    _total = contacts.result.Count();
                    if (_total > 0)
                    {
                        speech = "<prosody rate=\"medium\">" + "There are a total of " + _total + " Policies. You can find detailed information on each policy by saying Give me details on policy number. Here are titles of each policy. " + _contacts + "</prosody>" + "Would you like to Find all Policies? You can say, Find all Policies.";

                        card = "There are a total of " + _total + " Policies";
                    }
                    else
                    {
                        speech = "Unable to find any policies for " + cat + " in " + state + " Please try another Policy Category.";
                    }
                }
                else
                {
                    speech = "Unable to find any policies for " + cat + " in " + state + " Please try another Policy Category";
                }
            }
            catch
            {
                speech = "Unable to find any policies for " + cat + " in " + state + " Please try another Policy Category.";
            }

            var response = new AlexaResponse(speech, false, AddSSML(speech), card);


            speech = "Would you like to Find all Policies? You can say, Find all Policies.";
            response.Response.Reprompt.OutputSpeech.Type = "PlainText";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            //response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        public dynamic ContactBasedPolicyIntentHandler(Request request)
        {
            string speech = "";
            string state_type = "";
            string state = "";
            var card = "Contact based Policy";

            string _contacts = string.Empty;


            if (request.SlotsList.Any())
            {
                state = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;
                state_type = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;

                state_type = Helper.StateConversion.ConvertState(state_type);

            }

            try
            {

                int _total = 0;
                ContactRootObject contacts = GetContactBasedPolicy(state_type);
                if (contacts != null)
                {
                    int counter = 1;

                    foreach (var con in contacts.result)
                    {
                        _contacts = _contacts + " Contact " + counter + " " + con.name + " Phone Number is " + con.telephone;

                        counter = counter + 1;
                    }

                    _total = contacts.result.Count();
                }
                if (_total > 0)
                {


                    speech = "<prosody rate=\"slow\">There are a total of " + _total + " Contacts in " + state + " Here they are " + _contacts + "</prosody>" + "Would you like to Find all Policies? You can say, Find all Policies.";
                    card = "There are a total of " + _total + " Contacts in " + state + " Here they are " + _contacts;
                }
                else
                {
                    speech = "Unable to find any contacts, please try another state.";
                }
            }
            catch
            {
                speech = "Unable to find any contacts, please try another state.";
            }



            var response = new AlexaResponse(speech, false, AddSSML(speech), card);

            speech = "Would you like to Find all Policies? You can say, Find all Policies.";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        #region FuelBasedIntentHandler

        public dynamic FuelBasedIntentHandler(Request request)
        {
            var card = "";
            string speech = "Fuel Types";
            try
            {
                string _categories = string.Empty;

                speech = "The Fuel Types are Electric, Hybrid, Ethanol, Hydrogen,LPG,CNG and LNG." + "Do you want to find policies for any state? You can find policies for each category and state by saying, Find all Tax policies in Virginia";
                card = "The Fuel Types are Electric, Hybrid, Ethanol, Hydrogen,LPG,CNG and LNG";
            }
            catch
            {
                speech = ErrorMessage();
            }

            var response = new AlexaResponse(speech, false, AddSSML(speech), card);

            speech = "You can find policies for each category and state by saying, Find all Tax policies in Virginia";
            // response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        #endregion


        #endregion

        #region CategoryBasedPolicyIntentHandler
        public dynamic CategoryBasedPolicyIntentHandler(Request request)
        {
            string speech = "";
            var card = "Policy Categories";

            try
            {
                string _categories = string.Empty;

                speech = "<prosody rate=\"medium\">The Policy Categories are Tax, Rebate, Registration, Biodiesel, Ethanol, Alternative Fuel, After Market, Loans,Exemptions and Other</prosody>" + " Do you want to find all Fuel Types? You can say, Find all Fuel Types and find all Fuel Types";
                card = "The Policy Categories are Tax, Rebate, Registration, Biodiesel, Ethanol, Alternative Fuel, After Market, Loans,Exemptions and Other";
            }
            catch
            {
                speech = speech = ErrorMessage();
            }

            var response = new AlexaResponse(speech, false, AddSSML(speech), card);

            speech = "Do you want to find all Fuel Types? You can say, Find all Fuel Types and find all Fuel Types";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region NearbyStationByZipIntentHandler

        public AlexaResponse NearbyStationByZipIntentHandler(Request request)
        {

            string fuelType = string.Empty;
            string type = string.Empty;
            string zip = string.Empty;

            string _list = string.Empty;

            if (request.SlotsList.Any())
            {

                type = request.SlotsList.FirstOrDefault(s => s.Key == "fuel_type").Value;
                zip = request.SlotsList.FirstOrDefault(s => s.Key == "zipcode").Value;

                fuelType = Helper.StateConversion.ConvertFuelType(fuelType);

            }

            var allStations = GetNearbyStationsAsync(zip, fuelType, "");
            foreach (var station in allStations.fuel_stations)
            {
                _list = _list + "<say-as interpret-as=\"address\">" + station.street_address + "</say-as>" + " in " + station.city + " ";
            }
            if (allStations.fuel_stations.Count == 0)
            {
                _list = "Could not find any  " + type + " stations near " + zip;
            }
            var speech = "<prosody rate=\"slow\">" + _list + "</prosody>";
            var response = new AlexaResponse(speech, false, AddSSML(speech));

            speech = "Would you like to see " + type + " stations near you? You can say, Find " + type + " station near me.";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);

            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
            }
        }
        #endregion


        #region NearestStationIntentHandler

        public AlexaResponse NeareststationIntentHandler(Request request)
        {

            string fuelType = string.Empty;
            string stateType = string.Empty;
            string state = string.Empty;
            string type = string.Empty;
            string _list = string.Empty;

            if (request.SlotsList.Any())
            {

                type = request.SlotsList.FirstOrDefault(s => s.Key == "fuel_type").Value;
                state = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;

                fuelType = Helper.StateConversion.ConvertFuelType(type);
                stateType = Helper.StateConversion.ConvertState(state);
            }

            var allStations = GetNearbyStationbyState(stateType, fuelType);


            foreach (var station in allStations.fuel_stations)
            {
                _list = _list + "<say-as interpret-as=\"address\">" + station.street_address + "</say-as>" + " in " + station.city + "<break time=\"1s\"/> ";
            }

            if (allStations.fuel_stations.Count == 0)
            {
                _list = "Could not find any top ten " + type + " stations in " + state;
            }

            var speech = "<prosody rate=\"slow\">Here is a sample of ten " + type + " stations in " + state + " " + _list + "</prosody>";

            var response = new AlexaResponse(speech, false, AddSSML(speech));

            //Reprompt speech
            speech = "Would you like to see " + type + " stations near you? You can say, Find " + type + " station near me.";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);

            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
            }
        }

        #endregion

        #region AllStationCountIntentHandler

        public dynamic AllStationCountIntentHandler(Request request)
        {
            string fuelType = string.Empty;
            string Type = string.Empty;
            var card = "All Stations count";

            if (request.SlotsList.Any())
            {

                fuelType = request.SlotsList.FirstOrDefault(s => s.Key == "fuel_type").Value;

                Type = Helper.StateConversion.ConvertFuelType(fuelType);
            }



            var speech = "There is a total of <say-as interpret-as=\"cardinal\">" + " " + GetAllStationsCountByFuelType(Type) + "</say-as>" + " " + fuelType + " alternative fuel stations in the US." + " Would you like to see ELectric stations near you? You can say, Find Electric station near me.";
            card = "There is a total of " + " " + GetAllStationsCountByFuelType(Type) + " " + fuelType + " alternative fuel stations in the US";
            var response = new AlexaResponse(speech, false, AddSSML(speech), card);
            //Reprompt 
            speech = "Would you like to see ELectric stations near you? You can say, Find Electric station near me.";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        #endregion



        #region NearestStationCountIntentHandler

        public dynamic NearestStationCountIntentHandler(Request request)
        {
            string fuelType = string.Empty;
            string Type = string.Empty;
            string state_type = string.Empty;
            string state = string.Empty;
            string speech = string.Empty;
            var card = "Nearest Station Count";

            if (request.SlotsList.Any())
            {

                fuelType = request.SlotsList.FirstOrDefault(s => s.Key == "fuel_type").Value;
                state = request.SlotsList.FirstOrDefault(s => s.Key == "state_type").Value;
                state_type = Helper.StateConversion.ConvertState(state);
                Type = Helper.StateConversion.ConvertFuelType(fuelType);
            }
            var totalCount = GetAllNearbyStationsCountByFuelType(state_type, Type, "");

            if (totalCount > 0)
            {
                speech = "There is a total of  <say-as interpret-as=\"cardinal\">" + totalCount + "</say-as>" + " " + fuelType + " stations in " + state + " Would you like to see how many " + fuelType + " stations are in the US? " + " You can say, How many " + fuelType + " stations are in the US";
                card = "There is a total of  " + totalCount + " " + fuelType + " stations in " + state;
            }
            else
            {
                speech = "There are no " + fuelType + " stations in " + state + " Would you like to try another state?";
            }

            var response = new AlexaResponse(speech, false, AddSSML(speech), card);



            speech = "Would you like to see how many " + fuelType + " stations are in the US? " + " You can say, How many " + fuelType + " stations are in the US";

            response.Response.Reprompt.OutputSpeech.Type = "PlainText";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        #endregion


        #region NearbyStationNearMeIntentHandler


        public dynamic NearbyStationNearMeIntentHandler(Request request)
        {
            string zip = string.Empty;
            string type = string.Empty;
            string fuel_type = string.Empty;
            string address = string.Empty;
            PostalAddress location = new PostalAddress();
            string speech = string.Empty;
            var card = "Nearby Station near me";
            AlexaResponse response = new AlexaResponse();
            if (request.SlotsList.Any())
            {

                fuel_type = request.SlotsList.FirstOrDefault(s => s.Key == "fuel_type").Value;

                type = Helper.StateConversion.ConvertFuelType(fuel_type);
            }


            if (request.deviceId != "0" && request.consentToken != "0")
            {
                location = GetAddress(request.deviceId, request.consentToken);

                if (location != null)
                {

                    var stations = GetNearbyStationsAsync(location.postalCode, type, location.city);

                    foreach (var station in stations.fuel_stations)
                    {
                        address = station.street_address + " in " + station.city + " Would you like to Find all Fuel Types? You can say find all Fuel Types.";
                        break;
                    }

                    if (stations.fuel_stations.Count > 0)
                    {
                        speech = "Your current zipcode is " + location.postalCode + " We have found a " + fuel_type + " station at " + "<prosody rate=\"medium\">" + "<say-as interpret-as=\"address\">" + address + "</say-as>" + "</prosody>";
                        card = speech = "Your current zipcode is " + location.postalCode + " We have found a " + fuel_type + " station at " + address;
                        response = new AlexaResponse(speech, false, AddSSML(speech), card);
                    }
                    else
                    {
                        speech = "We are unable to find any " + fuel_type + " Fuel Stations near You. Would you like to Find all Fuel Types, You can say find all Fuel Types?";
                        response = new AlexaResponse(speech, false, AddSSML(speech), card);
                    }
                }
                else
                {
                    speech = "Please grant device location settings permissions to the Road Star skill and try again.";
                    response = new AlexaResponse(speech, true, AddSSML(speech), card);
                }

            }
            else
            {
                speech = "Please grant device location settings permissions to the Road Star skill and try again.";
                response = new AlexaResponse(speech, true, AddSSML(speech), card);
            }





            speech = "Would you like to Find all Fuel Types, You can say find all Fuel Types";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);

            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region HelloIntent
        private dynamic HelloIntentHandler(Request request)
        {


            var speech = "Hi and Welcome to Road Star, your guide to Alternative Transportation information and policy. Please issue your voice commands or say Help to get a list.";
            var response = new AlexaResponse(speech, false, AddSSML(speech));
            response.Response.OutputSpeech.Ssml = AddSSML(speech);
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        #endregion

        #region SessionEndRequest
        private AlexaResponse SessionEndedRequestHandler(Request request)
        {
            return null;
        }

        #endregion


        #region Help or Cancel Intent

        private AlexaResponse HelpIntent(Request request)
        {
            var speech = "<prosody rate=\"slow\">You can say, Find All Categories or Find All Fuel Types or Find Nearest Electric station</prosody>";
            var response = new AlexaResponse(speech, false, AddSSML(speech));

            response.Response.Reprompt.OutputSpeech.Text = "<prosody rate=\"slow\">You can say, Find All Categories or Find All Fuel Types or Find Nearest Electric station</prosody>";
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
            }
        }


        private AlexaResponse CancelOrStopIntentHandler(Request request)
        {
            var speech = "Operation Cancelled. Thank You for visiting Road Star. Please Come back soon";
            var response = new AlexaResponse(speech, true, AddSSML(speech));
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
            }
        }

        private AlexaResponse StopIntentHandler(Request request)
        {
            var speech = "Thank You for visiting Road Star. Please Come back soon";
            var response = new AlexaResponse(speech, true, AddSSML(speech));
            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
            }


        }

        #endregion

        #region LaunchRequest

        private dynamic LaunchRequestHandler(Request request)
        {

            string speech = string.Empty;

            AlexaResponse response = new AlexaResponse();

            speech = "Hi and Welcome to Road Star, your guide to Alternative Transportation information and policy. Please issue your voice commands or say Help to get a list.";

            response = new AlexaResponse(speech);
            response.Response.OutputSpeech.Type = "SSML";
            response.Response.OutputSpeech.Ssml = AddSSML(speech);
            response.Response.OutputSpeech.Text = "";
            response.Session.MemberId = request.MemberId;
            response.Response.Card.Title = "Alternative Transportation Skill";
            response.Response.Card.Content = speech;
            response.Response.Reprompt.OutputSpeech.Text = "Please issue your voice commands or say Help to get a list.";
            response.Response.ShouldEndSession = false;

            Speechlet testspeech = new Speechlet();
            var re = Request;
            var isGood = testspeech.GetResponse(re).Result;

            if (isGood)
            {
                return response;
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

        private string AddSSML(string text)
        {

            string _text = string.Empty;

            _text = "<speak>" + text + "</speak>";

            return _text;

        }
    }

    #endregion
}