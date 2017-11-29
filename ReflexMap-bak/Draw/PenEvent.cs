using System.Collections.Generic;
using System.Linq;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using static ReflexMap.MapEventLayer;

namespace ReflexMap.Draw
{
    public class PenEvent : PenBase
    {
        Dictionary<MapPoint, List<EventDetails>> _eventList;

        MapEventLayer _layerInfo;

        public delegate void PointTappedHandler(MapPoint p);
        public event PointTappedHandler PointTapped;

        public PenEvent(MapEventLayer layerInfo)
        {
            _layerInfo = layerInfo;
            _layerInfo.MarkerType = EventMarkerType.Proportional;
        }

        public override void ClearHilight()
        {
            if (_layerInfo.MarkerType == EventMarkerType.Proportional)
                GraphicsLayer.Graphics.ToList().ForEach(g => (g.Symbol as SimpleMarkerSymbol).Color = DefaultSettings.GetColor(GeoStatus.Normal));
            else
                GraphicsLayer.Graphics.ToList().ForEach(g => g.Symbol = _layerInfo.GetSymbol(GeoStatus.Normal));
        }

        public override void Init(IGeoInfo geo, bool isCur, List<Envelope> shapeList, List<MapPoint> pointList)
        {
            GraphicsLayer.Graphics.Clear();

            _eventList = new Dictionary<MapPoint, List<EventDetails>>();
            foreach (var e in (geo.ConvertTo<EventGeoInfo>()).EventList)
            {
                var key = _eventList.Keys.SingleOrDefault(p => p.X == e.Point.X && p.Y == e.Point.Y);
                if (key == null)
                {
                    key = e.Point;
                    _eventList.Add(key, new List<EventDetails>());
                }
                _eventList[key].Add(e);
            }

            foreach (var e in _eventList)
            {
                AddGraphic(e.Key, CURR_GEO, e.Value.Count, isCur ? GeoStatus.Normal : GeoStatus.Reference);
                pointList.Add(e.Key);
            }
        }

        public override bool FinishDraw(object input, ref Geometry output, ref string errMsg)
        {
            DeleteGraphic(DRAFT);

            EventDetails eve = input as EventDetails;

            var key = _eventList.Keys.SingleOrDefault(x => eve.Point.X == x.X && eve.Point.Y == x.Y);
            if (key == null)
            {
                key = eve.Point;
                _eventList.Add(key, new List<EventDetails>());
                _eventList[key].Add(eve);
                AddGraphic(key, CURR_GEO, _eventList[key].Count, GeoStatus.Hilight);
            }
            else
            {
                _eventList[key].Add(eve);
                var g = GraphicsLayer.Graphics.First(x => (x.Geometry as MapPoint).X == key.X && (x.Geometry as MapPoint).Y == key.Y);
                if (_layerInfo.MarkerType == MapEventLayer.EventMarkerType.Proportional)
                {
                    g.Symbol = _layerInfo.GetProportionalSymbol(_eventList[key].Count, GeoStatus.Hilight);
                }
            }

            return true;
        }

        public override async void MapViewTapped(MapView mapView, MapViewInputEventArgs e, bool drawing)
        {
            if (drawing)
            {
                DeleteGraphic(DRAFT);
            }

            ClearHilight();

            var graphic = await GraphicsLayer.HitTestAsync(mapView, e.Position);
            if (graphic != null)
            {
                if (_layerInfo.MarkerType == EventMarkerType.Proportional)
                {
                    (graphic.Symbol as SimpleMarkerSymbol).Color = DefaultSettings.GetColor(GeoStatus.Hilight);
                }
                else
                {
                    graphic.Symbol = _layerInfo.GetSymbol(GeoStatus.Hilight);
                }

                OnPointTapped(graphic.Geometry as MapPoint);
            }
            else
            {
                if (drawing)
                {
                    AddGraphic(e.Location, DRAFT, 1, GeoStatus.Hilight);
                }

                OnPointTapped((MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84));
            }
        }

        public override void Delete(object o)
        {
            EventDetails eve = o as EventDetails;

            var key = _eventList.Keys.Single(x => eve.Point.X == x.X && eve.Point.Y == x.Y);

            if (_eventList[key].Count > 1)
            {
                var item = _eventList[key].Single(x => x.Id == eve.Id);
                _eventList[key].Remove(item);

                if (_layerInfo.MarkerType == MapEventLayer.EventMarkerType.Proportional)
                {
                    var g = GraphicsLayer.Graphics.Single(x => (x.Geometry as MapPoint).X == key.X && (x.Geometry as MapPoint).Y == key.Y);
                    g.Symbol = _layerInfo.GetProportionalSymbol(_eventList[key].Count, GeoStatus.Hilight);
                }
            }
            else
            {
                _eventList.Remove(key);

                var g = GraphicsLayer.Graphics.Single(x => (x.Geometry as MapPoint).X == key.X && (x.Geometry as MapPoint).Y == key.Y);
                GraphicsLayer.Graphics.Remove(g);
            }
        }

        internal void ChangeMarker(MapEventLayer.EventMarkerType markerType, bool isCur)
        {
            if (_layerInfo.MarkerType == markerType)
                return;

            _layerInfo.MarkerType = markerType;
            GraphicsLayer.Graphics.Clear();
            foreach (var e in _eventList)
            {
                AddGraphic(e.Key, CURR_GEO, e.Value.Count, isCur ? GeoStatus.Normal : GeoStatus.Reference);
            }
        }

        void AddGraphic(MapPoint point, string attr, int prop, GeoStatus status)
        {
            if (_layerInfo.MarkerType == MapEventLayer.EventMarkerType.Marker)
            {
                AddGraphic(point, attr, _layerInfo.GetSymbol(status));
            }
            else
            {
                AddGraphic(point, attr, _layerInfo.GetProportionalSymbol(prop, status));
            }
        }

        protected void OnPointTapped(MapPoint p)
        {
            PointTapped?.Invoke(p);
        }
    }
}
