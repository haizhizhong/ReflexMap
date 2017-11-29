using System.Collections.Generic;
using System.Linq;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ReflexMap.Draw
{
    public class PenLine : PenBase
    {
        Dictionary<int, Polyline> _polyList;
        List<MapPoint> _drawPointList;

        public delegate void ItemTappedHandler(int i);
        public event ItemTappedHandler ItemTapped;

        public override void ClearHilight()
        {
            ChangeGraphicSymbol(CURR_GEO, MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Normal));
        }

        public override void Init(IGeoInfo geo, bool isCur, List<Envelope> shapeList, List<MapPoint> pointList)
        {
            GraphicsLayer.Graphics.Clear();

            _polyList = geo.ConvertTo<LineGeoInfo>().Polylines ?? new Dictionary<int, Polyline>();
            foreach (var poly in _polyList)
            {
                AddGraphic(poly.Value, CURR_GEO, $"{poly.Key}", MapLineLayer.GetSymbol(GeoMarkerType.Line, isCur ? GeoStatus.Normal : GeoStatus.Reference));
                shapeList.Add(poly.Value.Extent);
            }
        }

        public override void StartDraw()
        {
            base.StartDraw();
            _drawPointList = new List<MapPoint>();
        }

        public override bool FinishDraw(object input, ref Geometry output, ref string errMsg)
        {
            if (_drawPointList.Count <= 1)
            {
                errMsg = "A line need 2 or more points.";
                return false;
            }

            DeleteGraphic(DRAFT);

            Polyline polyline = new Polyline(_drawPointList.ToArray(), SpatialReferences.Wgs84);
            int polyId = (_polyList.Count > 0) ? _polyList.Keys.Max() + 1 : 1;
            _polyList.Add(polyId, polyline);

            AddGraphic(polyline, CURR_GEO, $"{polyId}", MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Normal));

            output = new Multipoint(_drawPointList);
            return true;
        }

        public override async void MapViewTapped(MapView mapView, MapViewInputEventArgs e, bool drawing)
        {
            if (!drawing)
            {
                var graphic = await GraphicsLayer.HitTestAsync(mapView, e.Position);
                if (graphic != null)
                {
                    OnItemTapped(int.Parse($"{graphic.Attributes[CURR_GEO]}"));
                }
                return;
            }

            MapPoint newPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

            if (_drawPointList.Count == 0)
            {
                AddGraphic(newPoint, DRAFT, MapLineLayer.GetSymbol(GeoMarkerType.Point, GeoStatus.Hilight));
            }
            else
            {
                Polyline line = new Polyline(new MapPoint[] { _drawPointList.Last(), newPoint });
                AddGraphic(line, DRAFT, MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Hilight));
            }

            _drawPointList.Add(newPoint);
        }

        public override void Delete(object o)
        {
            string selected = o as string;
            _polyList.Remove(int.Parse(selected));
            DeleteGraphic(CURR_GEO, selected);
        }

        public override void Select(object o)
        {
            ClearHilight();
            ChangeGraphicSymbol(CURR_GEO, o as string, MapLineLayer.GetSymbol(GeoMarkerType.Line, GeoStatus.Hilight));
        }

        protected void OnItemTapped(int i)
        {
            ItemTapped?.Invoke(i);
        }
    }
}
