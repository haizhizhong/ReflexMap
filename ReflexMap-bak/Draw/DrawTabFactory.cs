using HMConnection;

namespace ReflexMap.Draw
{
    public class DrawTabFactory
    {
        ucDrawEvents _eventDrawer;

        public ucDrawBase CreateInstance(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map, IMapLayer info, int layerIndex)
        {
            if (info is MapPointLayer)
            {
                return new ucDrawPoint(con, hmDevMgr, map, info.ConvertTo<MapPointLayer>(), layerIndex);
            }
            else if (info is MapShapeLayer)
            {
                return new ucDrawShape(con, hmDevMgr, map, info.ConvertTo<MapShapeLayer>(), layerIndex);
            }
            else if (info is MapLineLayer)
            {
                return new ucDrawLine(con, hmDevMgr, map, info.ConvertTo<MapLineLayer>(), layerIndex);
            }
            else if (info is MapEventLayer)
            {
                if (_eventDrawer == null)
                {
                    _eventDrawer = new ucDrawEvents(con, hmDevMgr, map, info.ConvertTo<MapEventLayer>(), layerIndex);
                    return _eventDrawer;
                }
                else
                {
                    _eventDrawer.AddEventLayer(info.ConvertTo<MapEventLayer>(), layerIndex);
                    return null;
                }
            }

            return null;
        }
    }
}
