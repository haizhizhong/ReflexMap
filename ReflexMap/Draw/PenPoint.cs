using System.Collections.Generic;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ReflexMap.Draw
{
    public class PenPoint : PenBase
    {
        public MapPoint _newPoint;

        public override void ClearHilight()
        {
            ChangeGraphicSymbol(CURR_GEO, MapPointLayer.GetSymbol(GeoStatus.Normal));
        }

        public override void Init(IGeoInfo geo, bool isCur, List<Envelope> shapeList, List<MapPoint> pointList)
        {
            GraphicsLayer.Graphics.Clear();

            MapPoint point = geo.ConvertTo<PointGeoInfo>()?.Point;
            if (point != null)
            {
                AddGraphic(point, CURR_GEO, MapPointLayer.GetSymbol( isCur ? GeoStatus.Hilight : GeoStatus.Reference));
                pointList.Add(point);
            }
        }

        public override void StartDraw()
        {
            base.StartDraw();
            _newPoint = null;
        }

        public override bool FinishDraw(object input, ref Geometry output, ref string errMsg)
        {
            if (_newPoint == null)
            {
                errMsg = "No point has been set.";
                return false;
            }

            DeleteGraphic(DRAFT);
            DeleteGraphic(CURR_GEO);

            AddGraphic(_newPoint, CURR_GEO, MapPointLayer.GetSymbol(GeoStatus.Hilight));

            output = _newPoint;
            return true;
        }

        public override void MapViewTapped(MapView mapView, MapViewInputEventArgs e, bool drawing)
        {
            if (!drawing)
                return;

            DeleteGraphic(DRAFT);
            _newPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

            AddGraphic(_newPoint, DRAFT, MapPointLayer.GetSymbol(GeoStatus.Hilight));
        }

        public override void Delete(object o)
        {
            DeleteGraphic(CURR_GEO);
        }
    }
}
