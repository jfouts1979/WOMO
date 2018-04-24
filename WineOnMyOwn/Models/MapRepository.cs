using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace WineOnMyOwn.Models
{
    public class MapRepository
    {
        //********************************
        //*** Return connection string ***
        //********************************
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        //**************************************************
        //*** Get lat and lng by address using geocoding ***
        //**************************************************

        public static GeocoderLocation Locate(string query)
        {
            //Need to use my own key here...
            WebRequest request = WebRequest
                .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="
                        + HttpUtility.UrlEncode(query));

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement != null && latitudeElement != null)
                    {
                        return new GeocoderLocation
                        {
                            Longitude = Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                            Latitude = Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture)
                        };
                    }
                }
            }

            return null;
        }

        //***************************************************************

        WOMOEntities db = new WOMOEntities();

        // Do heavy duty work in controller or repo - not in view...
        // Look for addresses that are blank, identify them, but return view before
        // updating this information
        public static GeocoderLocation Locate2(TTBWinePermit item)
        {
            //if(item.)

            var fullAddress = item.STREET + "," + item.CITY + "," + item.STATE + "," + item.ZIP;

            return null;
        }

        //*************************************************
        //*** Returns Count of Records in Permits Table ***
        //*************************************************

        public static int countPermitRecords()
        {
            string stmt = "SELECT COUNT(*) FROM dbo.TTBWinePermits";
            int count;

            using (SqlConnection thisConnection = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }

            return count;
        }

        //*************************************************************
        //*** Returns A List of TTB Permits Within Specified Radius ***
        //*************************************************************
        public static List<TTBWinePermit> GetTTBWinePermits()
        {
            using (WOMOEntities db = new WOMOEntities())
            {
                //List<int> permitListInt = new List<int>();
                List<TTBWinePermit> permitRecordRadiusList = new List<TTBWinePermit>();

                //This needs to be updated to grab coordinates from user's phone/device
                var louisvilleLat = 38.328732;
                var louisvilleLng = -85.764771;

                foreach (var permit in db.TTBWinePermits)
                {
                    if ((permit.Lat != null) || (permit.Lng != null))
                    {
                        double lat = (double)permit.Lat;
                        double lng = (double)permit.Lng;

                        var distance = distance2PointsAsCrowFlies(lat, lng, louisvilleLat, louisvilleLng);

                        //Needs to be established as a setting from the user
                        var radius = 50;
                        if (distance < radius)
                        {
                            permitRecordRadiusList.Add(permit); 
                        }
                    }
                }
                return permitRecordRadiusList;
            }
        }
        //**************************************************************
        //**************************************************************
        public static IEnumerable<TTBWinePermit> GetTTBWinePermitById(int winePermitId)
        {
            using (WOMOEntities db = new WOMOEntities())
            {
                var permit = db.TTBWinePermits.FirstOrDefault(p => p.WinePermitId == winePermitId);
                return permit;
            }
            
        }

    //**************************************************************
    // GET:  TTBWinePermits Linear Distance Between 2 Points
    //**************************************************************
    public static double distance2PointsAsCrowFlies(double lat1, double lon1, double lat2, double lon2)
    {
        var p = 0.017453292519943295; // Math.PI / 180
        var a = 0.5 - Math.Cos((lat2 - lat1) * p) / 2 +
                Math.Cos(lat1 * p) * Math.Cos(lat2 * p) *
                (1 - Math.Cos((lon2 - lon1) * p)) / 2;

        //return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km or 3959 for mi
        return 3959 * 2 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 3959 for mi
    }

    //**************************************************************
    // GET:  TTBWinePermits Within Radius Of User's Current Location
    //**************************************************************
    //public IEnumerable<TTBWinePermit> GetRadiusResult(int radius, double currLat, double currLng)
    //{

    //    using (var connection = new SqlConnection(GetConnectionString()))
    //    {
    //        connection.Open();

    //        //Use as reference - ultimately use user's device location


    //        var parameters = new DynamicParameters();
    //        //3959 for miles in formula below
    //        //6371 for km instead
    //        //should have settings screen for user
    //        //to pick km or mi at some point

    //        int maxToShow;

    //        //Set max number of wineries/breweries/distilliers/dispensaries to show
    //        maxToShow = 20;

    //        //what does the 37 represent in the radius formula?

    //        //var stringSQL = @"Select * FROM TTBWinePermits";
    //        //return connection.Query<TTBWinePermit>(stringSQL, commandType: CommandType.Text);

    //    }
    //}

    //**************************************************************
    // GET:  TTBWinePermits Linear Distance Between 2 Points ver. 2
    //**************************************************************
    static class DistanceAlgorithm
    {
        const double PIx = 3.141592653589793;
        const double RADIUS = 3959; //mi

        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public static double DistanceBetweenPlaces(
            double lon1,
            double lat1,
            double lon2,
            double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) *
                       Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIUS;
        }
    }


}
}
