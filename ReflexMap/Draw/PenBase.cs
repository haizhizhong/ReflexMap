using System.Collections.Generic;
using System.Linq;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

namespace ReflexMap.Draw
{
    public abstract class PenBase
    {
        protected const string DRAFT = "Draft";
        protected const string CURR_GEO = "CurrentGeo";

        public GraphicsLayer GraphicsLayer { get; set; }

        static public PenBase CreateInstance(IMapLayer info)
        {
            if (info is MapPointLayer)
                return new PenPoint();
            else if (info is MapShapeLayer)
                return new PenShape();
            else if(info is MapLineLayer)
                return new PenLine();
            else if(info is MapEventLayer)
                return new PenEvent(info.ConvertTo<MapEventLayer>());

            return null;
        }

        public T ConvertTo<T>() where T : PenBase
        {
            return this as T;
        }

        public abstract void Init(IGeoInfo geo, bool isCur, List<Envelope> shapeList, List<MapPoint> pointList);
        public virtual void StartDraw()
        {
            ClearHilight();
        }
        public abstract bool FinishDraw(object input, ref Geometry output, ref string errMsg);
        public virtual void Cancel()
        {
            DeleteGraphic(DRAFT);
        }
        public virtual void Select(object o)
        {
        }
        public abstract void Delete(object o);
        public abstract void MapViewTapped(MapView mapView, MapViewInputEventArgs e, bool drawing);
        public abstract void ClearHilight();

        protected void AddGraphic(Geometry geo, string attr, Symbol symbol)
        {
            GraphicsLayer.Graphics.Add(new Graphic(geo, AttrInfo.GenAttr(attr, true), symbol));
        }

        protected void AddGraphic(Geometry geo, string attr, string value, Symbol symbol)
        {
            GraphicsLayer.Graphics.Add(new Graphic(geo, AttrInfo.GenAttr(attr, value), symbol));
        }

        protected Graphic FindGraphic(string type)
        {
            return GraphicsLayer.Graphics.ToList().Find(x => x.Attributes.Keys.Contains(type));
        }

        protected Graphic FindGraphic(string type, string value)
        {
            return GraphicsLayer.Graphics.ToList().Find(x => x.Attributes[type].ToString()==value);
        }

        protected List<Graphic> FindGraphicList(string type)
        {
            return GraphicsLayer.Graphics.ToList().Where(x => x.Attributes.Keys.Contains(type)).ToList();
        }

        protected void DeleteGraphic(string type)
        {
            var list = FindGraphicList(type);
            list.ForEach(x => GraphicsLayer.Graphics.Remove(x));
        }

        protected void DeleteGraphic(string type, string value)
        {
            var g = FindGraphic(type, value);
            if (g != null)
            {
                GraphicsLayer.Graphics.Remove(g);
            }
        }

        protected void ChangeGraphicSymbol(string type, Symbol symbol)
        {
            var list = FindGraphicList(type);
            list.ForEach(x => x.Symbol = symbol);
        }

        protected void ChangeGraphicSymbol(string type, string value, Symbol symbol)
        {
            var g = FindGraphic(type, value);
            if (g != null)
            {
                g.Symbol = symbol;
            }
        }
    }
}
