//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Controls;
//using Esri.ArcGISRuntime.Controls;
//using Esri.ArcGISRuntime.Geometry;
//using Esri.ArcGISRuntime.Layers;
//using WS_Popups;

//namespace ReflexMap
//{
//    /// <summary>
//    /// Interaction logic for WpfMapDrawPoint.xaml
//    /// </summary>
//    public partial class WpfMapDrawPoint : UserControl
//    {
//        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
//        frmPopup _pop;

//        private Dictionary<string, string> _baseMaps = new Dictionary<string, string>();

//        const string POINT_TYPE = "PointType";
//        enum PointType
//        {
//            Old = 0,
//            New = 1
//        }

//        GraphicsLayer _graphicsLayer;
//        GraphicsLayer _otherGraphicsLayer;
//        bool _drawing;
//        MapPointLayer _layerInfo;
//        MapPoint _newPoint = null;

//        public WpfMapDrawPoint(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, MapPointLayer layerInfo)
//        {
//            InitializeComponent();
//            _hmDevMgr = hmDevMgr;
//            _pop = new frmPopup(_hmDevMgr);
//            _layerInfo = layerInfo;

//            _baseMaps.Add("Open Street", "OpenStreetMap");
//            _baseMaps.Add("Streets", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer");
//            _baseMaps.Add("Topo", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer");
//            _baseMaps.Add("Imagery", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer");
//            _baseMaps.Add("National Geographic", "http://services.arcgisonline.com/ArcGIS/rest/services/NatGeo_World_Map/MapServer");

//            basemapChooser.ItemsSource = _baseMaps;
//            basemapChooser.DisplayMemberPath = "Key";
//            basemapChooser.SelectedIndex = 0;

//            _otherGraphicsLayer = new GraphicsLayer();
//            mapView.Map.Layers.Add(_otherGraphicsLayer);

//            _graphicsLayer = new GraphicsLayer();
//            mapView.Map.Layers.Add(_graphicsLayer);

//            _drawing = false;
//            mapView.MapViewTapped += MapView_MapViewTapped;
//        }

//        internal void SetOldPoint(MapPoint point, List<IGeoInfo> otherLayerGeoList)
//        {
//            _graphicsLayer.Graphics.Clear();
//            _otherGraphicsLayer.Graphics.Clear();

//            List<Envelope> shapeList = new List<Envelope>();
//            List<MapPoint> pointList = new List<MapPoint>();

//            if (point != null)
//            {
//                AddPoint(point, PointType.Old);
//                pointList.Add(point);
//            }

//            foreach (var other in otherLayerGeoList)
//            {
//                if (other is PointGeoInfo)
//                {
//                    MapPoint otherPoint = (other as PointGeoInfo)?.Point;
//                    if (otherPoint != null)
//                    {
//                        _otherGraphicsLayer.Graphics.Add(new Graphic(otherPoint, MapPointLayer.GetSymbol(GeoStatus.Other)));
//                        pointList.Add(otherPoint);
//                    }
//                }
//                else if (other is ShapeGeoInfo)
//                {
//                    var plist = (other as ShapeGeoInfo).Polygons;
//                    if (plist != null && plist.Count > 0)
//                    {
//                        foreach (var poly in plist)
//                        {
//                            _otherGraphicsLayer.Graphics.Add(new Graphic(poly.Value, MapShapeLayer.GetSymbol(GeoType.Fill, GeoStatus.Other)));
//                            shapeList.Add(poly.Value.Extent);
//                        }
//                    }
//                }
//                else if (other is LineGeoInfo)
//                {
//                    var plist = (other as LineGeoInfo).Polylines;
//                    if (plist != null && plist.Count > 0)
//                    {
//                        foreach (var poly in plist)
//                        {
//                            _otherGraphicsLayer.Graphics.Add(new Graphic(poly.Value, MapLineLayer.GetSymbol(GeoType.Line, GeoStatus.Other)));
//                            shapeList.Add(poly.Value.Extent);
//                        }
//                    }
//                }
//                else if (other is EventGeoInfo)
//                {
//                    var plist = (other as EventGeoInfo).EventList;
//                    if (plist != null && plist.Count > 0)
//                    {
//                        foreach (var e in plist)
//                        {
//                            _otherGraphicsLayer.Graphics.Add(new Graphic(e.Point, MapEventLayer.GetSymbol(GeoStatus.Other)));
//                            pointList.Add(e.Point);
//                        }
//                    }
//                }
//            }

//            Envelope area = null;
//            shapeList.ForEach(g => area = (area == null ? g : area.Union(g)));
//            double stepSize = (area == null) ? 0.05 : 0.0005;
//            pointList.ForEach(p =>
//            {
//                var g = new Envelope(p.X - stepSize, p.Y - stepSize, p.X + stepSize, p.Y + stepSize, SpatialReferences.Wgs84);
//                area = (area == null ? g : area.Union(g));
//            });

//            area = area ?? new Envelope(-113.493588 - stepSize, 53.540766 - stepSize, -113.493588 + stepSize, 53.540766 + stepSize, SpatialReferences.Wgs84);
//            mapView.SetViewAsync(new Viewpoint(area.Expand(2)));
//        }

//        internal void StartDraw()
//        {
//            var g = FindPoint(PointType.Old);
//            if (g != null)
//            {
//                g.Symbol = MapPointLayer.GetSymbol(GeoStatus.Normal);
//            }

//            _newPoint = null;
//            _drawing = true;
//        }

//        internal MapPoint FinishDraw()
//        {
//            if (_newPoint == null)
//            {
//                _pop.ShowPopup("No new point has been set.");
//                return null;
//            }

//            DeletePoint(PointType.Old);
//            _drawing = false;
//            return _newPoint;
//        }

//        internal void CancelDraw()
//        {
//            DeletePoint(PointType.New);

//            var gOld = FindPoint(PointType.Old);
//            if (gOld != null)
//            {
//                gOld.Symbol = MapPointLayer.GetSymbol(GeoStatus.Hilight);
//            }

//            _drawing = false;
//        }

//        private void MapView_MapViewTapped(object sender, MapViewInputEventArgs e)
//        {
//            if (!_drawing)
//                return;

//            DeletePoint(PointType.New);
//            _newPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

//            AddPoint(_newPoint, PointType.New);
//        }

//        void AddPoint(MapPoint point, PointType type)
//        {
//            Dictionary<string, object> attr = new Dictionary<string, object>();
//            attr.Add(POINT_TYPE, type);
//            _graphicsLayer.Graphics.Add(new Graphic(point, attr, MapPointLayer.GetSymbol(GeoStatus.Hilight)));
//        }

//        internal void DeletePoint()
//        {
//            DeletePoint(PointType.Old);
//            DeletePoint(PointType.New);
//        }

//        Graphic FindPoint(PointType type)
//        {
//            return _graphicsLayer.Graphics.ToList().Find(x => (PointType)x.Attributes[POINT_TYPE] == type);
//        }

//        void DeletePoint(PointType type)
//        {
//            var g = FindPoint(type);
//            if (g != null)
//            {
//                _graphicsLayer.Graphics.Remove(g);
//            }
//        }

//        private void OnBasemapChooserSelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (mapView.Map.Layers.Count > 0)
//            {
//                mapView.Map.Layers.RemoveAt(0);
//            }

//            KeyValuePair<string, string> selected = (KeyValuePair<string, string>)e.AddedItems[0];
//            if (selected.Value == "OpenStreetMap")
//            {
//                mapView.Map.Layers.Insert(0, new OpenStreetMapLayer());
//            }
//            else
//            {
//                mapView.Map.Layers.Insert(0, new ArcGISTiledMapServiceLayer()
//                {
//                    ServiceUri = selected.Value
//                });
//            }
//        }
//    }
//}
