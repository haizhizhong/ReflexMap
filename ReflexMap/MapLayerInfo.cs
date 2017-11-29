using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using HMConnection;

namespace ReflexMap
{
    public interface IMapLayer
    {
        string LayerName { get; set; }
        string LinkTable { get; set; }
        string LinkColumn { get; set; }
        List<AttrInfo> Attributes { get; set; }

        T ConvertTo<T>() where T : MapLayer;
    };

    public class MapLayer : IMapLayer
    {
        public virtual string LayerName { get; set; }
        public string LinkTable { get; set; }
        public string LinkColumn { get; set; }
        public string TimeStamp { get; set; }
        public List<AttrInfo> Attributes { get; set; }

        public T ConvertTo<T>() where T : MapLayer
        {
            return this as T;
        }

        public MapLayer()
        {
            Attributes = new List<AttrInfo>();
        }
    }

    public class MapPointLayer : MapLayer
    {
        private string _layerName = "Point";
        public override string LayerName { get { return _layerName; } set { _layerName = value; } }

        static public MarkerSymbol GetSymbol(GeoStatus status)
        {
            var symbol = new PictureMarkerSymbol();
            if (status == GeoStatus.Normal)
                symbol.SetSourceAsync(new System.Uri("http://static.arcgis.com/images/Symbols/Basic/GreenShinyPin.png"));
            else if (status == GeoStatus.Hilight)
                symbol.SetSourceAsync(new System.Uri("http://static.arcgis.com/images/Symbols/Basic/GoldShinyPin.png"));
            if (status == GeoStatus.Reference)
                symbol.SetSourceAsync(new System.Uri("http://static.arcgis.com/images/Symbols/Basic/BlueShinyPin.png"));

            return symbol;
        }
    }

    public class MapShapeLayer : MapLayer
    {
        private string _layerName = "Shape";
        public override string LayerName { get { return _layerName; } set { _layerName = value; } }

        const SimpleMarkerStyle DefaultMarkerStyle = SimpleMarkerStyle.X;
        const SimpleLineStyle DefaultLineStyle = SimpleLineStyle.Solid;
        const SimpleFillStyle DefaultFillStyle = SimpleFillStyle.BackwardDiagonal;
        const int DefaultLineWidth = 2;
        const int DefaultMarkerSize = 12;
        static public Symbol GetSymbol(GeoMarkerType type, GeoStatus status)
        {
            if (type == GeoMarkerType.Point)
            {
                return new SimpleMarkerSymbol() { Style = DefaultMarkerStyle, Color = DefaultSettings.GetColor(status), Size = DefaultMarkerSize };
            }
            else if (type == GeoMarkerType.Line)
            {
                return new SimpleLineSymbol() { Style = DefaultLineStyle, Color = DefaultSettings.GetColor(status), Width = DefaultLineWidth };
            }
            else if (type == GeoMarkerType.Fill)
            {
                return new SimpleFillSymbol()
                {
                    Style = DefaultFillStyle,
                    Color = status == GeoStatus.Hilight ? Colors.Gold : DefaultSettings.GetColor(status),
                    Outline = new SimpleLineSymbol() { Style = SimpleLineStyle.Solid, Color = DefaultSettings.GetColor(status), Width = DefaultLineWidth }
                };
            }

            return null;
        }
    }

    public class MapLineLayer : MapLayer
    {
        private string _layerName = "Line";
        public override string LayerName { get { return _layerName; } set { _layerName = value; } }

        const SimpleMarkerStyle DefaultMarkerStyle = SimpleMarkerStyle.X;
        const SimpleLineStyle DefaultLineStyle = SimpleLineStyle.Solid;
        const int DefaultLineWidth = 4;
        const int DefaultMarkerSize = 12;
        static public Symbol GetSymbol( GeoMarkerType type, GeoStatus status)
        {
            if (type == GeoMarkerType.Point)
            {
                return new SimpleMarkerSymbol() { Style = DefaultMarkerStyle, Color = DefaultSettings.GetColor(status), Size = DefaultMarkerSize };
            }
            else if (type == GeoMarkerType.Line)
            {
                return new SimpleLineSymbol() { Style = DefaultLineStyle, Color = DefaultSettings.GetColor(status), Width = DefaultLineWidth };
            }

            return null;
        }
    }

    public class MapEventLayer : MapLayer
    {
        public enum EventMarkerType
        {
            Proportional,
            Marker
        }
        private string _layerName = null;
        public override string LayerName { get { return _layerName??EventTypeName; } set { _layerName = value; } }
        public int TypeId { get; set; }
        public string EventTypeName { get; set; }

        public EventMarkerType MarkerType { get; set; }

        public Dictionary<GeoStatus, PictureMarkerSymbol> PictureMarkerSymbols { get; set; }

        private SimpleMarkerSymbol _markerSymbol;
        public MarkerSymbol MarkerSymbol
        {
            get { return _markerSymbol ; }
            set { _markerSymbol = (SimpleMarkerSymbol)value; }
        }

        const int MinSize = 12;
        const int DefaultSize = 8;
        const int StepSize = 4;
        const int MaxNumber = 8;
        const int PictureMarkerSize = 40;

        const SimpleMarkerStyle DefaultStyle = SimpleMarkerStyle.Circle;

        public MarkerSymbol GetProportionalSymbol(int count, GeoStatus status)
        {
            return new SimpleMarkerSymbol() { Style = SimpleMarkerStyle.Circle, Color = DefaultSettings.GetColor(status), Size = MinSize + StepSize * Math.Min(count, MaxNumber) };
        }

        public MarkerSymbol GetSymbol(GeoStatus status)
        {
            if (PictureMarkerSymbols != null)
                return PictureMarkerSymbols[status];

            return new SimpleMarkerSymbol() { Style = DefaultStyle, Color = DefaultSettings.GetColor(status), Size = DefaultSize };
        }

        public MapEventLayer() : base()
        {
            MarkerType = EventMarkerType.Proportional;
        }

        public static List<MapEventLayer> GetEventLayers(HMCon hmConn, string tableName, string columnName)
        {
            List<MapEventLayer> list = new List<MapEventLayer>();

            string sql = $"select * from Geo_EventType where LinkTableName='{tableName}'";
            var table = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            foreach( DataRow row in table.Rows)
            {
                MapEventLayer layer = new MapEventLayer();
                layer.LinkTable = tableName;
                layer.LinkColumn = columnName;
                layer.TypeId = (int)row["Id"];
                layer.EventTypeName = (string)row["TypeName"];

                if (!string.IsNullOrEmpty((string)row["MarkerUri"]))
                {
                    layer.PictureMarkerSymbols = new Dictionary<GeoStatus, PictureMarkerSymbol>
                    {
                        { GeoStatus.Normal, new PictureMarkerSymbol()},
                        { GeoStatus.Hilight, new PictureMarkerSymbol()},
                        { GeoStatus.Reference, new PictureMarkerSymbol()}
                    };

                    layer.PictureMarkerSymbols[GeoStatus.Normal].SetSourceAsync(new System.Uri($"{row["MarkerUri"]}"));
                    layer.PictureMarkerSymbols[GeoStatus.Hilight].SetSourceAsync(new System.Uri($"{row["HilightMarkerUri"]}"));
                    layer.PictureMarkerSymbols[GeoStatus.Reference].SetSourceAsync(new System.Uri($"{row["ReferenceMarkerUri"]}"));

                    foreach (PictureMarkerSymbol symbol in layer.PictureMarkerSymbols.Values)
                    {
                        symbol.Height = PictureMarkerSize;
                        symbol.Width = PictureMarkerSize;
                    }
                }

                list.Add(layer);
            }

            return list;
        }

        public override string ToString()
        {
            return EventTypeName;
        }
    }

    public enum GeoMarkerType
    {
        Point,
        Line,
        Fill,
    }

    public enum GeoStatus
    {
        Normal,
        Hilight,
        Reference,
    }

    public static class DefaultSettings 
    {
        static Dictionary<GeoStatus, Color> colorList = new Dictionary<GeoStatus, Color>()
        {
            { GeoStatus.Normal, Colors.Green},
            { GeoStatus.Hilight, Colors.Red},
            { GeoStatus.Reference, Colors.Black},
        };

        static public Color GetColor(GeoStatus status)
        {
            return colorList[status];
        }

        static MapPoint CenterPoint = new MapPoint(-113.493588, 53.540766, SpatialReferences.Wgs84);

        static public double MarginForPoint = 0.05;
        static public double MarginForShape = 0.0005;

        static public double ExpandFactor = 2.0;

        static public Envelope GetRange(double margin)
        {
            return new Envelope(CenterPoint.X - margin, CenterPoint.Y - margin, CenterPoint.X + margin, CenterPoint.Y + margin, SpatialReferences.Wgs84);
        }
    }
}
