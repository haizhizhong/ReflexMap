using System.Collections.Generic;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ReflexMap.Draw
{
    /// <summary>
    /// Interaction logic for WpfMapDrawPoint.xaml
    /// </summary>
    public class WpfMapDrawBase : WpfMapBase
    {
        protected bool _drawing;

        Dictionary<int, PenBase> _penList;
        int _curLayer;

        public event PenEvent.PointTappedHandler PointTapped;

        public event PenShape.ItemTappedHandler ItemTapped;

        public WpfMapDrawBase(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr) : base(hmDevMgr)
        {
            _drawing = false;
            mapView.MapViewTapped += MapView_MapViewTapped;

            _penList = new Dictionary<int, PenBase>();
        }

        public void RegisterLayer(int layer, IMapLayer info) 
        {
            PenBase pen = PenBase.CreateInstance(info);
            if (pen is PenShape)
            {
                pen.ConvertTo<PenShape>().ItemTapped += WpfMapDrawBase_ItemTapped;
            }
            else if (pen is PenLine)
            {
                pen.ConvertTo<PenLine>().ItemTapped += WpfMapDrawBase_ItemTapped;
            }

            else if (pen is PenEvent)
            {
                pen.ConvertTo<PenEvent>().PointTapped += WpfMapDrawBase_PointTapped;
            }

            pen.GraphicsLayer = new GraphicsLayer();
            mapView.Map.Layers.Add(pen.GraphicsLayer);

            _penList.Add(layer, pen);
        }

        private void WpfMapDrawBase_PointTapped(MapPoint p)
        {
            PointTapped?.Invoke(p);
        }

        private void WpfMapDrawBase_ItemTapped(int i)
        {
            ItemTapped?.Invoke(i);
        }

        internal void SetCurrentLayer(int curr)
        {
            _penList[curr].ClearHilight();
            _curLayer = curr;
        }

        internal void DrawAll(List<IGeoInfo> allGeo)
        {
            List<Envelope> shapeList = new List<Envelope>();
            List<MapPoint> pointList = new List<MapPoint>();

            for (int i = 0; i < allGeo.Count; i++)
            {
                _penList[i].Init(allGeo[i], _curLayer == i, shapeList, pointList);
            }

            Envelope viewArea = null;
            shapeList.ForEach(g => viewArea = (viewArea == null ? g : viewArea.Union(g)));
            double margin = (viewArea == null) ? DefaultSettings.MarginForPoint : DefaultSettings.MarginForShape;
            pointList.ForEach(p =>
            {
                var g = new Envelope(p.X - margin, p.Y - margin, p.X + margin, p.Y + margin, SpatialReferences.Wgs84);
                viewArea = (viewArea == null ? g : viewArea.Union(g));
            });

            viewArea = viewArea ?? DefaultSettings.GetRange(margin);
            mapView.SetViewAsync(new Viewpoint(viewArea.Expand(2)));
        }

        internal void Delete(object o)
        {
            _penList[_curLayer].Delete(o);
        }

        internal void StartDraw()
        {
            _penList[_curLayer].StartDraw();
            _drawing = true;
        }

        internal Geometry FinishDraw(object o = null)
        {
            Geometry geo = null;
            string msg = "";
            if (!_penList[_curLayer].FinishDraw(o, ref geo, ref  msg))
            {
                _pop.ShowPopup(msg);
                return null;
            }

            _drawing = false;
            return geo;
        }

        internal virtual void CancelDraw()
        {
            _penList[_curLayer].Cancel();
            _drawing = false;
        }

        internal virtual void MapView_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            _penList[_curLayer].MapViewTapped(mapView, e, _drawing);
        }

        internal void Select(string str)
        {
            _penList[_curLayer].Select(str);
        }

        public void EventMarkerChanged(MapEventLayer.EventMarkerType markerType)
        {
            for (int i = 0; i < _penList.Count; i++)
            {
                if (_penList[i] is PenEvent)
                {
                    _penList[i].ConvertTo<PenEvent>().ChangeMarker(markerType, i == _curLayer);
                }
            }
        }
    }
}
