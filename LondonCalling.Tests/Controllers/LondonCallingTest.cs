using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using LondonCalling.Data.APIModels;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.IO;
using LondonCalling.Data.APIModels.Search;

namespace LondonCalling.Tests.Controllers
{
    [TestClass]
    public class LondonCallingTest
    {
        //NaptanMetroStation,NaptanRailStation
        static HttpClient client = new HttpClient();
        private static string host = "https://api.tfl.gov.uk/";
        private static string applicationId = "142e9a71";
        private static string applicationKey = "1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string amazonHost = @"https://api.amazonalexa.com";
        private static string amazonAddress = @"/v1/devices/{0}/settings/address/countryAndPostalCode";
        private static string amazonFullAddress = @"/v1/devices/{0}/settings/address";
        private static string googleAPI = @"AIzaSyCypgNnsEhrdlb_DJRL_Ynybe4xetytGz4";
        private static string googleAddress = @"https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}";

        #region TFL Calls

        private static string cred = @"&app_id={1}&app_key={2}";

        private static string allBusStops = @"/line/24/stoppoints";
        private static string allBikePoints = @"/BikePoint";
        private static string allBikePointsByName = @"/BikePoint/Search?query={0}&app_id={1}&app_key={2}";
        private static string allBikePointsById = @" /BikePoint/{0}&app_id={1}&app_key={2}";
        private static string allStopsByName = @"/StopPoint/Search/{0}?app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string stationArrival = @"/StopPoint/{0}/Arrivals?app_id={1}&app_key={2}";
        private static string tubeDisruptions = @"/Line/Mode/Tube/Disruption?app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";
        private static string allModes = @"/Line/Mode/{0}";
        private static string nearestStation = @"/Stoppoint?lat={0}&lon={1}&stoptypes={2}&radius=2000&app_id=142e9a71&app_key=1946cdc2d57eeadf7ed2dd3e10b8568a";

        #region initialize

        [TestInitialize]
        public void Initialize()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        }

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

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject< List<DisruptionRootObject>>(result);
            }



            return allStations;


        }
        #endregion

        #endregion

        private static BikeRootObject GetAllBikeStopsByName(string name)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePointsByName, name, applicationId, applicationKey);


            BikeRootObject allStations = null;
           var response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<BikeRootObject>(result);
            }



            return allStations;
        }

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


        async Task<TestRootObject>  GetAllBikeStopsById(string id)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tfl.gov.uk/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePointsById, id, applicationId, applicationKey);


            TestRootObject allStations = null;
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRootObject>(result);
            }



            return allStations;
        }

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

        static List<BikeRootObject> GetAllBikeStops()
        {
            client = new HttpClient();

            var url = "https://api.tfl.gov.uk/BikePoint";

          



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.BaseAddress = new Uri("https://api.tfl.gov.uk/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(allBikePoints);

            HttpResponseMessage response1 = client.GetAsync("bikepoint").Result;

            WebClient webClient = new WebClient();
            Stream responseData = webClient.OpenRead(url);


            List<BikeRootObject> allStations = new List<BikeRootObject>();
            HttpResponseMessage response =  client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                allStations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BikeRootObject>>(result);
            }



            return allStations;
        }

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

        #region GetAllTubePoints

        static StationRootObject GetAllTubePoints(string lat, string lon, string type)
        {

            client = new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string path = string.Format(nearestStation, lat, lon, type);

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

        [TestMethod]
        public void TestBikeStopsByName()
        {
            var stationsByName = GetAllBikeStopsByName("st.%20james");
        }

        [TestMethod]
        public void TestBikeStopsById ()
        {
            var stationsById = GetAllBikeStopsById("BikePoints_160");
        }

        [TestMethod]
        public void TestGetAllBikeStops()
        {
            var allBikeStops = GetAllBikeStops();
        }

        [TestMethod]
        public void FindClosestStation()
        {
            double lat = 51.4998403;
            double lon = -0.1246627;
            double distance = 100;

            var allStations = GetAllBikeStops();

            foreach (var station in allStations)
            {


                var newDistance = Helper.DistanceCalculator.Calculate(lat, lon, station.lat, station.lon);
                if (newDistance <= distance)
                {
                    distance = newDistance;
                }


            }


            Assert.IsTrue(distance != 100);
        }

        [TestMethod]
        public void TestGoogleAddress()
        {
            var newaddress = "Westminster, London SW1A 0AA,UK";
            var address = GetGoogleAddress(newaddress);

        }
        [TestMethod]
        public void TestTubeDisruptions()
        {
            var disruptions = TubeDisruptions();
        }

        [TestMethod]

        public void TestGetAllStopsByName()
        {
            var stops = GetAllStopsByName("amersham");        
            string natID = string.Empty;


            foreach (var stop in stops.matches)
            {
                if (stop.modes.Contains("tube"))
                {
                    natID = stop.id;
                }
            }

            var arrivalInfo = GetArrivalInformation(natID);



        }

        [TestMethod]
        public void TestInvalidTubeName()
        {

            var stops = GetAllStopsByName("invalid");
        }

        [TestMethod]

        public void GetNearestTube()
        {
             double lat = 51.4998403;
            double lon = -0.1246627;
            string type = "NaptanMetroStation";
            string mode = "tube";
            var allStations = GetAllTubePoints(lat.ToString(), lon.ToString(), type);
            double distance = 10000000;
            string natID = string.Empty;

            foreach (var station in allStations.stopPoints)
            {


                var newDistance = Helper.DistanceCalculator.Calculate(lat, lon, station.lat, station.lon);
                if (newDistance <= distance)
                {
                    if (station.modes.Contains(mode))
                    {
                        distance = newDistance;
                        natID = station.stationNaptan;
                    }

                }
            }

            if (natID != string.Empty)
            {
                var arrivalInfo = GetArrivalInformation(natID);
            }



        }

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
    }
}
