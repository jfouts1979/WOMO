using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WineOnMyOwn.Models
{
    public class SampleModel
    {
        public string Name { get; set; }
        public int Zoom { get; set; }
        public List<Location> Locations { get; set; }
        public LatLng LatLng { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public object LatLng { get; set; }
        public string Image { get; set; }
    }

    public class LatLng
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    };






}