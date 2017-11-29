using System.Collections.Generic;
using Esri.ArcGISRuntime.Geometry;

namespace ReflexMap
{
    public enum GeoType
    {
        Point = 1,
        Shape = 2,
        Polyline = 3,
        Event = 4
    }

    public interface IGeoInfo
    {
        string KeyCode { get; set; }
        Dictionary<string, object> AttrList { get; set; }

        T ConvertTo<T>() where T : GeoInfo;
    }

    public class GeoInfo : IGeoInfo
    {
        public string KeyCode { get; set; }
        public Dictionary<string, object> AttrList { get; set; }

        public GeoInfo()
        {
            AttrList = new Dictionary<string, object>();
        }

        public T ConvertTo<T>() where T : GeoInfo
        {
            return this as T;
        }
    }


    public class PointGeoInfo : GeoInfo
    {
        public int PointId { get; set; }
        public MapPoint Point { get; set; }

    }

    public class ShapeGeoInfo : GeoInfo
    {
        public int ShapeId { get; set; }
        public Dictionary<int, Polygon> Polygons { get; set; }

        public ShapeGeoInfo() : base()
        {
            Polygons = new Dictionary<int, Polygon>();
        }
    }

    public class LineGeoInfo : GeoInfo
    {
        public int PolylineId { get; set; }
        public Dictionary<int, Polyline> Polylines { get; set; }

        public LineGeoInfo() : base()
        {
            Polylines = new Dictionary<int, Polyline>();
        }
    }

    public class EventGeoInfo : GeoInfo
    {
        public List<EventDetails> EventList { get; set; }

        public EventGeoInfo() : base()
        {
            EventList = new List<EventDetails>();
        }
    }

    public class AttrInfo
    {
        public string DisplayName { get; set; }
        public string LinkField { get; set; }
        public Dictionary<string, string> LinkDictionary { get; set; }

        public AttrInfo(string displayName, string linkField, Dictionary<string, string> linkDictionary = null)
        {
            DisplayName = displayName;
            LinkField = linkField;
            LinkDictionary = linkDictionary;
        }

        static public Dictionary<string, object> GenAttr(string key, object value)
        {
            return new Dictionary<string, object>() { { key, value } };
        }
    }
}