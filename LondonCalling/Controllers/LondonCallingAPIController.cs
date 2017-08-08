using LondonCalling.Data.APIModels;
using LondonCalling.Data.APIModels.Search;
using LondonCalling.Data.Models;
using LondonCalling.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace LondonCalling.Controllers
{
    public class LondonCallingAPIController : ApiController
    {
        public LondonCallingAPIController()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        }
        static HttpClient client = new HttpClient();
        private static string host = "https://api.tfl.gov.uk/";
        private static string applicationId = "142e9a71";
        private static string applicationKey = "1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string amazonHost = @"https://api.amazonalexa.com";
        private static string amazonAddress = @"/v1/devices/{0}/settings/address/countryAndPostalCode";
        private static string amazonFullAddress = @"/v1/devices/{0}/settings/address";
        private static string googleAPI = @"AIzaSyCypgNnsEhrdlb_DJRL_Ynybe4xetytGz4";
        private static string googleAddress = @"https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}";
        private  string amazonAppId = "amzn1.ask.skill.8f446a08-9800-4da5-8423-eca420517c2d";

        #region TFL Calls

        private static string tubeDisruptions = @"/Line/Mode/Tube/Disruption?" + allCred;
        private static string allBikePoints = @"/bikepoint?" + allCred;
        private static string allBikePointsByName = @"/BikePoint/Search?query={0}&" + allCred;
        private static string allBikePointsById = @"/BikePoint/{0}?" + allCred;
        private static string allCred = "app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string allStopsByName = @"/StopPoint/Search/{0}?app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string stationArrival = @"/StopPoint/{0}/Arrivals?app_id={1}&app_key={2}";
        private static string nearestStation = @"/Stoppoint?lat={0}&lon={1}&stoptypes={2}&app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string allModes = @"/Line/Mode/{0}?app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string stopTypes = @"https://api.tfl.gov.uk/StopPoint/meta/stoptypes";

        #endregion

        #region Disruptions

        public static List<DisruptionRootObject> TubeDisruptions()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(tubeDisruptions);


            List<DisruptionRootObject> allStations = null;
            var response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DisruptionRootObject>>(result);
            }



            return allStations;


        }
        #endregion


        #region GetAddressCoordinates

        static GoogleRootObject GetGoogleAddress(string address)
        {
            GoogleRootObject fullAddress = null;

            var formatAddress = string.Format(googleAddress, address, googleAPI);

            client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(formatAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                fullAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleRootObject>(result);
            }


            return fullAddress;

        }

        #endregion


        #region GetAllTubePoints

        static  StationRootObject GetAllTubePoints(string lat, string lon, string type)
        {

            client = new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(nearestStation, lat, lon,type);

            StationRootObject allStations = new StationRootObject();
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<StationRootObject>(result);
            }



            return allStations;
        }

        #endregion

        #region GetAllBikePoints

        static async Task<List<BikeRootObject>> GetAllBikePoints()
        {

            client = new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePoints);

            List<BikeRootObject> allStations = new List<BikeRootObject>();
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BikeRootObject>>(result);
            }



            return allStations;
        }

        #endregion

        #region GetAllBikePointsByName
        static BikeRootObject GetAllBikeStopsByName(string name)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePointsByName, name);


            BikeRootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<BikeRootObject>(result);
            }



            return allStations;
        }
        #endregion

        #region GetAllBikePointsById

        static BikeRootObject GetAllBikeStopsById(string id)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePointsById, id);


            BikeRootObject allStations = null;
            var response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<BikeRootObject>(result);
            }



            return allStations;
        }

        #endregion

        #region GetAllByName

        /// <summary>
        /// Returns all stops based on name
        /// </summary>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static SearchRootObject GetAllStopsByName(string name)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allStopsByName, name);


            SearchRootObject allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchRootObject>(result);
            }



            return allStations;
        }



        #endregion

        #region Get Arrival Information

        static List<ArrivalRootObject> GetArrivalInformation(string id)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(stationArrival, id, applicationId, applicationKey);


            List<ArrivalRootObject> allStations = null;
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ArrivalRootObject>>(result);
            }



            return allStations;

        }


        #endregion

        #region London Calling Commands


        [HttpPost, Route("API/LondonCalling/London")]
        public dynamic London(AlexaRequest alexaRequest)
        {

            //Validate application Id
            if (alexaRequest.Session.Application.ApplicationId != amazonAppId)
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
                            text = "London Calling was unable to understand your voice command, please try your voice command again with a different set of parameters"
                        },
                        Card = new
                        {

                            type = "Simple",
                            title = "London Calling Error Message.",
                            content = "London Calling is currently having voice issues."
                        },
                        shouldEndSession = false
                    }

                };

            }

        }
        #endregion

        #region SessionEndRequest
        private AlexaResponse SessionEndedRequestHandler(Request request)
        {
            return null;
        }

        #endregion


        #region LaunchRequest

        private dynamic LaunchRequestHandler(Request request)
        {

            string speech = string.Empty;

            AlexaResponse response = new AlexaResponse();

            speech = "Welcome to London Calling, your voice enabled guide to the Transportation for London. You can say When is the next tube arriving or say How many Bikes near me.";

            response = new AlexaResponse(speech);
            response.Response.OutputSpeech.Type = "SSML";
            response.Response.OutputSpeech.Ssml = AddSSML(speech);
            response.Response.OutputSpeech.Text = "";
            response.Session.MemberId = request.MemberId;
            response.Response.Card.Title = "Transportation for London Skill - Tubes and Bike Stations";
            response.Response.Card.Content = speech;
            response.Response.Reprompt.OutputSpeech.Text = "Please issue your voice commands or say Help to get a list.";
            response.Response.ShouldEndSession = false;

          
                return response;
           

        }

        private string AddSSML(string text)
        {

            string _text = string.Empty;

            _text = "<speak>" + text + "</speak>";

            return _text;

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
             
                case "BikePointsNearMeIntent":
                    response = GetBikePointsNearMe(request);
                    break;
                case "TubeArrivalNearMeIntent":
                    response = GetTubeArrivalNearMe(request);
                    break;
                case "TubeArrivalByName":
                    response = GetTubeArrivalByName(request);
                    break;
                case "TubeDisruptionsIntent":
                    response = GetTubeDisruptions(request);
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



        #region GetAllBikePointsNearMe by lat/lon location
  

        public AlexaResponse GetBikePointsNearMe(Request request)
        {
        double lat = 51.5224013;
        double lon = -0.12850700000001325;
            double distance = 1000000;
            int numberOfBikes = 0;
            PostalAddress location = new PostalAddress();
            string speech = string.Empty;
            string repromptSpeech = string.Empty;
            var card = "Bike Stations near me";


            AlexaResponse response = new AlexaResponse();
        
   
            if (request.deviceId != "0" && request.consentToken != "0")
            {
                location = GetAddress(request.deviceId, request.consentToken);
                if (location != null)
                {
                    //Find the Coordinates
                    //Find the closest station


                    var address = location.addressLine1 + "," + location.addressLine2 + "," + location.city + "," + location.postalCode + "," + location.stateOrRegion + "," + location.countryCode;

                    var addressComponent = GetGoogleAddress(address).results[0].geometry;
                    lat = addressComponent.location.lat;
                    lon = addressComponent.location.lng;

                    var allStations = GetAllBikePoints();



                    foreach (var station in allStations.Result)
                    {


                        var newDistance = Helper.DistanceCalculator.Calculate(lat, lon, station.lat, station.lon);
                        if (newDistance <= distance)
                        {
                            distance = newDistance;
                            foreach (var info in station.additionalProperties)
                            {
                                if (info.key == "NbBikes")
                                {
                                    numberOfBikes = Convert.ToInt16(info.value);
                                    if (Convert.ToInt16(info.value) > 0)
                                    {
                                        speech = "There are " + Convert.ToInt16(info.value) + " bikes remaining at " + station.commonName + " .Would you like to find out when the next tube is arriving? You can say, When is the next tube arriving?";


                                    }
                                    else
                                    {

                                        speech = "There are no bikes remaining in " + station.commonName + " .Would you like to find out when the next tube is arriving? You can say, When is the next tube arriving?";
                                    }
                                }


                            }

                            }


                    }

                    response = new AlexaResponse(speech, false, AddSSML(speech), card);

                }


            }

            else
            {
                speech = "Please grant device location settings permissions to the London Calling skill and try again to get nearest Bike Locations.";
                response = new AlexaResponse(speech, true, AddSSML(speech), card);
            }

            repromptSpeech = "Would you like to find out about the next tube? You can say, When is the next tube arriving?";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = repromptSpeech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(repromptSpeech);
            return response;
        }


        #endregion

        #region GetBikepointsByName Intent Handler


        public AlexaResponse GetBikePointsByName(Request request)
        {

            string name = string.Empty;

            int numberOfDocks = 0;
            int emptyDocks = 0;
            int numberOfBikes = 0;

            string _list = "I Could not find any  " + " bike stations in " + name + " Would you like to try another Bike Station?";

            if (request.SlotsList.Any())
            {

                name = request.SlotsList.FirstOrDefault(s => s.Key == "SlotName").Value;



            }

            var allStations = GetAllBikePoints();

            if (allStations.Result.Count() == 0)
            {
                _list = "I Could not find any bike stations at " + name + " Would you like to try another Bike Station?";

            }
            else
            {

                int stationCounter = 0;
                string bikeList = string.Empty;
                foreach (var station in allStations.Result)
                {
                    if (station.commonName.ToUpper().Contains(name.ToUpper()))
                    {
                        stationCounter = stationCounter + 1;
                        bikeList = bikeList + station.commonName.ToUpper() + " ";
                        foreach (var info in station.additionalProperties)
                        {

                            if (info.key == "NbEmptyDocks")
                            {
                                emptyDocks = Convert.ToInt16(info.value);
                            }
                            if (info.key == "NbDocks")
                            {
                                numberOfDocks = Convert.ToInt16(info.value);
                            }

                            if (info.key == "NbBikes")
                            {
                                numberOfBikes = Convert.ToInt16(info.value);
                                if (Convert.ToInt16(info.value) > 0)
                                {
                                    _list = "There are " + Convert.ToInt16(info.value) + " bikes remaining at " + name + " .Would you like to find out when the next train is arriving? You can say, When is the next train arriving?";


                                }
                                else
                                {

                                    _list = "There are no bikes remaining in " + name + " Would you like to try another Bike Station?";
                                }
                            }
                        }

                        if (numberOfDocks - (emptyDocks + numberOfBikes) != 0)
                        {
                            _list = "There is a malfunction in this Biking Station. Would you like to try another Bike Station?";
                        }
                        break;
                    }
                    numberOfDocks = 0;
                    emptyDocks = 0;
                    numberOfBikes = 0;
                }

                if (stationCounter > 1)
                {
 
                    _list = " We found multiple bike station locations : " + bikeList + " Please narrow down your bike station name. Would you like to try it again with another Bike Station?";
                }
          
   
            }
            var speech = "<prosody rate=\"medium\">" + _list + "</prosody>";
            var response = new AlexaResponse(speech, false, AddSSML(speech));

            speech = "Would you like to find out about the next train? You can say, When is the next train arriving?";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);

                return response;
           
        }
        #endregion

        #region Get Tube arrival times by name of tube station
        public AlexaResponse GetTubeArrivalByName(Request request)
        {
            string _list = string.Empty;
            string name = string.Empty;
            string natID = string.Empty;
            List<TubeModel> tubeModels = new List<TubeModel>();


            if (request.SlotsList.Any())
            {

                name = request.SlotsList.FirstOrDefault(s => s.Key == "TubeName").Value;

            }
            var stops = GetAllStopsByName(name);

            if (stops.matches.Count == 0)
            {
                _list = "Unable to find any tubes arriving at " + name +" Would you like to try another station name?";
            }
            else
            {


                foreach (var stop in stops.matches)
                {
                    if (stop.modes.Contains("tube"))
                    {
                        natID = stop.id;
                    }
                }


                if (natID == string.Empty)
                {
                    _list = "Unable to find any tubes arriving at " + name + " Would you like to try another station name?";
                }
                else
                {
                    var arrivalInfo = GetArrivalInformation(natID);

                    if (arrivalInfo.Count() > 0)
                    {

                        foreach (var tube in arrivalInfo)
                        {
                            var TubeModel = new TubeModel();
                            TubeModel.ArrivalTime = Convert.ToDateTime(tube.expectedArrival).ToString("hh:mm tt");
                            TubeModel.CurrentLocation = tube.currentLocation;
                            TubeModel.Destination = tube.platformName;
                            TubeModel.Line = tube.lineName;
                            TubeModel.Id = tube.id;

                            tubeModels.Add(TubeModel);

                            if (TubeModel.Destination != string.Empty)
                                _list = _list + "There is a Tube arriving at " + TubeModel.ArrivalTime + " on the " + TubeModel.Line + " line, stopping at the " + TubeModel.Destination + " ";

                        }
                    }
                    else
                    {
                        _list = "Unable to find any tubes arriving at " + name + " Would you like to try another station name?";
                    }
                }
            }

            var speech =  _list;
            var response = new AlexaResponse(speech, false, string.Empty);
            speech = "Would you like to find out about the next tube? You can say, When is the next tube arriving?";
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
            return response;
        }
        #endregion

        #region Get  Arrival times of nearest tube

             public AlexaResponse GetTubeArrivalNearMe(Request request)
        {
            AlexaResponse response = new AlexaResponse();
            string _list = string.Empty;
            string name = string.Empty;
            string natID = string.Empty;
            double lat = 51.632376;
            double lon = -0.127756;
            double distance = 1000000;
            string stationType = "TUBE";
            string mode = "tube";
            PostalAddress location = new PostalAddress();
            string speech = string.Empty;
            string repromptSpeech = string.Empty;
            var card = "Tube/Train Stations near me";
            string type = "NaptanMetroStation";



            if (request.deviceId != "0" && request.consentToken != "0")
            {
                location = GetAddress(request.deviceId, request.consentToken);
                if (location != null)
                {
                    //Find the Coordinates
                    //Find the closest station


                    var address = location.addressLine1 + "," + location.addressLine2 + "," + location.city + "," + location.postalCode + "," + location.stateOrRegion + "," + location.countryCode;

                    var addressComponent = GetGoogleAddress(address).results[0].geometry;
                    lat = addressComponent.location.lat;
                    lon = addressComponent.location.lng;

                    var allStations = GetAllTubePoints(lat.ToString(), lon.ToString(), type);



                    foreach (var station in allStations.stopPoints)
                    {


                        var newDistance = Helper.DistanceCalculator.Calculate(lat, lon, station.lat, station.lon);
                        if (newDistance <= distance)
                        {
                            if (station.modes.Contains("tube"))
                            {
                                distance = newDistance;
                                natID = station.stationNaptan;

                            }
                        }

                    }


                }


            }

            else
            {
                speech = "Please grant device location settings permissions to the London Calling skill and try again to get nearest Bike Locations.";
                response = new AlexaResponse(speech, true, AddSSML(speech), card);
            }



            if (natID == string.Empty)
                    {
                        _list = "Unable to find any tubes based on your location. Would you like to find out bike stations near you? You can say, How many bikes near me?";
                    }
                    else
                    {
                        var arrivalInfo = GetArrivalInformation(natID);


                if (arrivalInfo.Count() > 0)
                {
                    foreach (var tube in arrivalInfo)
                    {
                        var TubeModel = new TubeModel();
                        TubeModel.ArrivalTime = Convert.ToDateTime(tube.expectedArrival).ToString("hh:mm tt");
                        TubeModel.CurrentLocation = tube.currentLocation;
                        TubeModel.Destination = tube.destinationName;
                        TubeModel.Line = tube.lineName;
                        TubeModel.Id = tube.id;


                        if (TubeModel.Destination != string.Empty)
                            _list = _list + "There is a Tube arriving at " + TubeModel.ArrivalTime + " on the " + TubeModel.Line + " line, going towards " + TubeModel.Destination + " ";

                    }
                }
                else
                {
                    _list = "Unable to find any tubes based on your location. Would you like to find out bike stations near you? You can say, How many bikes near me?";
                }
                    }
               

             speech = "<prosody rate=\"medium\">" + _list + " Would you like to find out bike stations near you? You can say, How many bikes near me ?"+ "</prosody>";
             response = new AlexaResponse(speech, false, AddSSML(speech));
            return response;
        }
        #endregion

        #region Find all tube disruptions


        public AlexaResponse GetTubeDisruptions(Request request)
        {
         
            var disruptions = TubeDisruptions();
            var _list = "There are currently no disruptions on the  London Tube" + " Would you like to find out about the next tube? You can say, When is the next tube arriving?";


            if (disruptions.Count() > 0)
            {
                _list = string.Empty;
            }
            foreach(var dis in disruptions)
            {
                _list = _list + dis.description;
            }

            var speech = "<prosody rate=\"medium\">" + _list + " Would you like to find out about the next tube? You can say, When is the next tube arriving?" + "</prosody>";
            var response = new AlexaResponse(speech, false, AddSSML(speech));
            return response;

        }

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


        #region HelloIntent
        private dynamic HelloIntentHandler(Request request)
        {


            var speech = "Welcome to London Calling, your voice enabled guide to the Transportation for London. You can say, When is the next tube arriving near me or say Find arrivals at Watford or say How many Bikes near me or say Help to get a list.";
            var response = new AlexaResponse(speech, false, AddSSML(speech));
            response.Response.OutputSpeech.Ssml = AddSSML(speech);
            response.Response.Reprompt.OutputSpeech.Text = speech;
            response.Response.Reprompt.OutputSpeech.Type = "SSML";
           
                return response;
         
            

        }

        #endregion

        #region stop/cancel

        private AlexaResponse CancelOrStopIntentHandler(Request request)
        {
            var speech = "Operation Cancelled. Thank You for visiting London Calling.";
            var response = new AlexaResponse(speech, true, AddSSML(speech));
        

                return response;
          
        }

        private AlexaResponse StopIntentHandler(Request request)
        {
            var speech = "Thank You for visiting London Calling.";
            var response = new AlexaResponse(speech, true, AddSSML(speech));
           
                return response;
           


        }

        #endregion

        #region HelpIntent

        private AlexaResponse HelpIntent(Request request)
        {
            var speech = "<prosody rate=\"medium\">You can say When is the next tube arriving near me or say Find arrivals at Watford or say How many Bikes near me or Get all tube disruptions</prosody>";
            var response = new AlexaResponse(speech, false, AddSSML(speech));

            response.Response.Reprompt.OutputSpeech.Text = "<prosody rate=\"medium\">You can say When is the next tube arriving near me or say Find arrivals at Watford or say How many Bikes near me  or Get all tube disruptions</prosody>";
            response.Response.Reprompt.OutputSpeech.Ssml = AddSSML(speech);
         
                return response;
            
        }

        #endregion
      

    }
}
