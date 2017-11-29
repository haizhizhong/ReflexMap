using System;
using Esri.ArcGISRuntime.Geometry;

namespace ReflexMap
{
    public class EventDetails
    {
        public int Id;
        public int TypeId;
        public string EventType;
        public MapPoint Point;

        public DateTime Date; 
        public string Description;

        public override string ToString()
        {
            return $"{Id}. {Date.ToShortDateString()}";
        }
    }
}
