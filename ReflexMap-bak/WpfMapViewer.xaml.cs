using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using WS_Popups;
using System.Linq;
using static ReflexMap.MapEventLayer;
using Esri.ArcGISRuntime.Symbology;

namespace ReflexMap
{
    public class Layer
    {
        public GraphicsLayer GraphicsLayer { get; set; }
        public IMapLayer Info{ get; set; }

        public string LayerName
        {
            get { return Info.LayerName; }
        }

        public bool Visible
        {
            get { return GraphicsLayer.IsVisible; }
            set { GraphicsLayer.IsVisible = value; }
        }
    }

    public partial class WpfMapViewer : UserControl
    {
        private Dictionary<string, string> _baseMaps = new Dictionary<string, string>();

        List<Envelope> _shapeList = new List<Envelope>();
        List<MapPoint> _pointList = new List<MapPoint>();

        Dictionary<int, Dictionary<MapPoint, List<EventDetails>>> EventDataList = new Dictionary<int, Dictionary<MapPoint, List<EventDetails>>>();

        List<Layer> _layerList = new List<Layer>();
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        frmPopup _pop;
        const string KEY_CODE = "_KeyCode_";
        const string OPEN_STREET_MAP = "OpenStreetMap";

        public WpfMapViewer(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr)
        {
            InitializeComponent();
            _hmDevMgr = hmDevMgr;
            _pop = new frmPopup(_hmDevMgr);

            _baseMaps.Add("Open Street", OPEN_STREET_MAP);
            _baseMaps.Add("Streets", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer");
            _baseMaps.Add("Topo", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer");
            _baseMaps.Add("Imagery", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer");
            _baseMaps.Add("National Geographic", "http://services.arcgisonline.com/ArcGIS/rest/services/NatGeo_World_Map/MapServer");

            basemapChooser.ItemsSource = _baseMaps;
            basemapChooser.DisplayMemberPath = "Key";
            basemapChooser.SelectedIndex = 0;

            layerList.ItemsSource = _layerList;
        }

        public void AddLayer(List<IGeoInfo> list, IMapLayer layerInfo)
        {
            if (list == null || list.Count == 0)
                return;

            Layer layer = new Layer();
            layer.GraphicsLayer = new GraphicsLayer();
            layer.Info = layerInfo;

            _layerList.Add(layer);
            mapView.Map.Layers.Add(layer.GraphicsLayer);

            try
            {
                if (layerInfo is MapPointLayer)
                    AddPoinLayer(list, layer);
                else if (layerInfo is MapShapeLayer)
                    AddShapeLayer(list, layer);
                else if (layerInfo is MapLineLayer)
                    AddLineLayer(list, layer);
                else if (layerInfo is MapEventLayer)
                    AddEventLayer(list, layer);
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Unable to create map: " + ex.Message, "Error");
            }
        }

        private void AddPoinLayer(List<IGeoInfo> pointList, Layer layer)
        {
            foreach (PointGeoInfo point in pointList)
            {
                if (point.Point != null)
                {
                    layer.GraphicsLayer.Graphics.Add(new Graphic(point.Point, point.AttrList, MapPointLayer.GetSymbol(GeoStatus.Normal)));
                    _pointList.Add(point.Point);
                }
            }
        }

        private void AddShapeLayer(List<IGeoInfo> shapeList, Layer layer)
        {
            foreach (ShapeGeoInfo shape in shapeList)
            {
                if (shape.Polygons.Values.Count == 0)
                    continue;

                PolygonBuilder builder = new PolygonBuilder(SpatialReferences.Wgs84);
                shape.Polygons.Values.ToList().ForEach(p => builder.AddParts(p.Parts));
                Polygon polygon = builder.ToGeometry();

                Dictionary<string, object> attr = new Dictionary<string, object>(shape.AttrList);
                attr[KEY_CODE] = shape.KeyCode;
                layer.GraphicsLayer.Graphics.Add(new Graphic(polygon, attr, MapShapeLayer.GetSymbol(GeoMarkerType.Fill, GeoStatus.Normal)));
                _shapeList.Add(polygon.Extent);

                layer.GraphicsLayer.Graphics.Add(new Graphic(polygon.Extent.GetCenter(), attr, MapShapeLayer.GetSymbol(GeoMarkerType.Point, GeoStatus.Normal)));
            }
        }

        private void AddLineLayer(List<IGeoInfo> lineList, Layer layer)
        {
            foreach (LineGeoInfo line in lineList)
            {
                if (line.Polylines.Values.Count == 0)
                    continue;

                PolylineBuilder builder = new PolylineBuilder(SpatialReferences.Wgs84);
                line.Polylines.Values.ToList().ForEach(p => builder.AddParts(p.Parts));

                Polyline polyline = builder.ToGeometry();
                layer.GraphicsLayer.Graphics.Add(new Graphic(polyline, line.AttrList, MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Normal)));
                _shapeList.Add(polyline.Extent);

                //_otherGraphic.Graphics.Add(new Graphic(polyline.Extent.GetCenter(), line.AttrList, MapLineLayer.GetSymbol(GeoType.Point, GeoStatus.Normal)));
            }
        }

        private void AddEventLayer(List<IGeoInfo> eventList, Layer layer)
        {
            var dataDict = new Dictionary<MapPoint, List<EventDetails>>();
            foreach (EventGeoInfo geo in eventList)
            {
                foreach (var eve in geo.EventList)
                {
                    var key = dataDict.Keys.SingleOrDefault(p => p.X == eve.Point.X && p.Y == eve.Point.Y);
                    if (key == null)
                    {
                        key = eve.Point;
                        dataDict.Add(key, new List<EventDetails>());
                    }
                    dataDict[key].Add(eve);
                }
            }

            EventDataList.Add(_layerList.Count-1, dataDict);

            foreach (var entry in dataDict)
            {
                var info = layer.Info.ConvertTo<MapEventLayer>();
                var attr = new Dictionary<string, object>();
                attr.Add(info.EventTypeName, entry.Value.Count);
                entry.Value.ForEach(x => attr.Add($"{attr.Count}. {x.Date.ToShortDateString()}", x.Description));
                layer.GraphicsLayer.Graphics.Add(new Graphic(entry.Key, attr, info.GetProportionalSymbol(entry.Value.Count, GeoStatus.Normal)));
            }
        }

        public void NoMoreLayer()
        {
            eventMarker.Visibility = _layerList.Any(x => x.Info is MapEventLayer) ? Visibility.Visible : Visibility.Hidden;

            Envelope viewArea = null;
            _shapeList.ForEach(g => viewArea = viewArea?.Union(g) ?? g);

            double margin = (viewArea == null) ? DefaultSettings.MarginForPoint : DefaultSettings.MarginForShape;
            _pointList.ForEach(p =>
            {
                var g = new Envelope(p.X - margin, p.Y - margin, p.X + margin, p.Y + margin, SpatialReferences.Wgs84);
                viewArea = viewArea?.Union(g) ?? g;
            });

            viewArea = viewArea ?? DefaultSettings.GetRange(margin);
            mapView.SetViewAsync(new Viewpoint(viewArea.Expand(DefaultSettings.ExpandFactor)));

            attrInfo.Visibility = Visibility.Hidden;
            mapView.MapViewTapped += MapView_MapViewTapped;
        }

        private async void MapView_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            try
            {
                _layerList.ForEach(layer => SetAllNormal(layer));

                bool hit = false;
                foreach (var layer in _layerList.Where(x=>x.GraphicsLayer.IsVisible))
                {
                    var graphic = await layer.GraphicsLayer.HitTestAsync(mapView, e.Position);

                    if (graphic != null)
                    {
                        HilightItem(graphic, layer);

                        attrList.ItemsSource = graphic.Attributes.Where(x => x.Key != KEY_CODE);
                        attrInfo.Visibility = Visibility.Visible;
                        hit = true;
                        break;
                    }
                }

                if (!hit)
                {
                    attrInfo.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                attrInfo.Visibility = Visibility.Hidden;
                _pop.ShowPopup("HitTest Error: " + ex.Message, "Hit Testing");
            }
        }

        void SetAllNormal(Layer layer)
        {
            if (layer.Info is MapPointLayer)
            {
                layer.GraphicsLayer.Graphics.ToList().ForEach(g => g.Symbol = MapPointLayer.GetSymbol(GeoStatus.Normal));
            }
            else if (layer.Info is MapShapeLayer)
            {
                layer.GraphicsLayer.Graphics.ToList().ForEach(g =>
                {
                    if (g.Geometry is MapPoint)
                        g.Symbol = MapShapeLayer.GetSymbol(GeoMarkerType.Point, GeoStatus.Normal);
                    else
                        g.Symbol = MapShapeLayer.GetSymbol(GeoMarkerType.Fill, GeoStatus.Normal);
                });
            }
            else if (layer.Info is MapLineLayer)
            {
                layer.GraphicsLayer.Graphics.ToList().ForEach(g => g.Symbol = MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Normal));
            }
            else if (layer.Info is MapEventLayer)
            {
                if ((layer.Info.ConvertTo<MapEventLayer>()).MarkerType == EventMarkerType.Proportional)
                    layer.GraphicsLayer.Graphics.ToList().ForEach(g => (g.Symbol as SimpleMarkerSymbol).Color = DefaultSettings.GetColor(GeoStatus.Normal));
                else
                    layer.GraphicsLayer.Graphics.ToList().ForEach(g => g.Symbol = (layer.Info.ConvertTo<MapEventLayer>().GetSymbol(GeoStatus.Normal)));
            }
        }

        void HilightItem(Graphic graphic, Layer layer)
        {
            if (layer.Info is MapPointLayer)
                graphic.Symbol = MapPointLayer.GetSymbol(GeoStatus.Hilight);
            else if (layer.Info is MapShapeLayer)
            {
                var code = graphic.Attributes[KEY_CODE];
                var list = layer.GraphicsLayer.Graphics.Where(x => x.Attributes[KEY_CODE].ToString() == code.ToString());
                list.ToList().ForEach(g =>
                {
                    if (g.Geometry is MapPoint)
                        g.Symbol = MapShapeLayer.GetSymbol(GeoMarkerType.Point, GeoStatus.Hilight);
                    else
                        g.Symbol = MapShapeLayer.GetSymbol(GeoMarkerType.Fill, GeoStatus.Hilight);
                });
            }
            else if (layer.Info is MapLineLayer)
                graphic.Symbol = MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Hilight);
            else if (layer.Info is MapEventLayer)
            {
                if (layer.Info.ConvertTo<MapEventLayer>().MarkerType == EventMarkerType.Proportional)
                {
                    (graphic.Symbol as SimpleMarkerSymbol).Color = DefaultSettings.GetColor(GeoStatus.Hilight);
                }
                else
                {
                    graphic.Symbol = layer.Info.ConvertTo<MapEventLayer>().GetSymbol(GeoStatus.Hilight);
                }
            }
        }

        private void OnBasemapChooserSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mapView.Map.Layers.Count > 0)
            {
                mapView.Map.Layers.RemoveAt(0);
            }

            KeyValuePair<string, string> selected = (KeyValuePair<string, string>)e.AddedItems[0];
            if (selected.Value == OPEN_STREET_MAP)
            {
                mapView.Map.Layers.Insert(0, new OpenStreetMapLayer());
            }
            else
            {
                mapView.Map.Layers.Insert(0, new ArcGISTiledMapServiceLayer() { ServiceUri = selected.Value });
            }
        }

        private void EventMarkerChecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _layerList.Count; i++)
            {
                if (_layerList[i].Info is MapEventLayer)
                {
                    MapEventLayer info = _layerList[i].Info.ConvertTo<MapEventLayer>();
                    info.MarkerType = EventUseMarker();

                    _layerList[i].GraphicsLayer.Graphics.Clear();
                    foreach (var entry in EventDataList[i])
                    {
                        var attr = new Dictionary<string, object>();
                        attr.Add(info.EventTypeName, entry.Value.Count);
                        entry.Value.ForEach(x => attr.Add($"{attr.Count}. {x.Date.ToShortDateString()}", x.Description));
                        if (info.MarkerType== EventMarkerType.Marker)
                        {
                            _layerList[i].GraphicsLayer.Graphics.Add(new Graphic(entry.Key, attr, info.GetSymbol(GeoStatus.Normal)));
                        }
                        else
                        {
                            _layerList[i].GraphicsLayer.Graphics.Add(new Graphic(entry.Key, attr, info.GetProportionalSymbol(entry.Value.Count, GeoStatus.Normal)));
                        }
                    }
                }
            }
        }

        private EventMarkerType EventUseMarker()
        {
            return (chkMarker != null && chkMarker.IsChecked == true) ? EventMarkerType.Marker : EventMarkerType.Proportional;
        }
    }
}
