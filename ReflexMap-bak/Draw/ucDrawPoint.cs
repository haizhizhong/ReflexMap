using System;
using Esri.ArcGISRuntime.Geometry;
using HMConnection;

namespace ReflexMap.Draw
{
    internal partial class ucDrawPoint : ucDrawBase
    {
        MapPointLayer _layer;
        PointGeoInfo _pointInfo;
        int _layerIndex;

        public ucDrawPoint(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map, MapPointLayer layer, int layerIndex) 
            : base(con, hmDevMgr, map)
        {
            InitializeComponent();

            _layer = layer;
            _layerIndex = layerIndex;
            gcAttr.DataSource = _attrTable;

            InDrawingStatus(false);
        }

        public override int CurrLayerIndex { get { return _layerIndex; } }

        public override void SetCurrent(IGeoInfo info)
        {
            base.SetCurrent(info);

            _pointInfo = info.ConvertTo<PointGeoInfo>();
            InDrawingStatus(false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _map.StartDraw();
            InDrawingStatus(true);
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                var newPoint = _map.FinishDraw() as MapPoint;
                if (newPoint != null)
                {
                    if (_pointInfo.PointId > 0)
                    {
                        string sql = $"update Geo_Point set Latitude={newPoint.Y}, Longitude={newPoint.X}, TimeStamp='{DateTime.Now.ToShortDateString()}' where Id = {_pointInfo.PointId}";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }
                    else
                    {
                        string sql = $"insert into Geo_Point( Latitude, Longitude, TimeStamp) " +
                        $"values({newPoint.Y}, {newPoint.X}, '{DateTime.Now.ToShortDateString()}'); SELECT SCOPE_IDENTITY()";
                        _pointInfo.PointId = Convert.ToInt32(_hmCon.SQLExecutor.ExecuteScalar(sql, _hmCon.TRConnection));

                        sql = $"insert into Geo_Link(LinkTableName, LinkCode, Feature, LinkType, LinkId)" +
                            $" values('{_layer.LinkTable}','{_pointInfo.KeyCode}', '{_layer.LayerName}', {(int)GeoType.Point}, {_pointInfo.PointId})";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }

                    _pointInfo.Point = newPoint;
                    InDrawingStatus(false);
                }
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }

        public override void InDrawingStatus(bool on)
        {
            btnDelete.Enabled = !on && (_pointInfo?.Point != null);
            btnStart.Enabled = !on && (_pointInfo != null);

            btnFinish.Enabled = on;
            btnCancel.Enabled = on;

            base.InDrawingStatus(on);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _map.CancelDraw();
            InDrawingStatus(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _map.Delete(null);

            try
            {
                string sql = $"delete Geo_Point where ID='{_pointInfo.PointId}' ";
                _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);

                sql = $"delete Geo_Link where LinkTableName='{_layer.LinkTable}' and Feature='{_layer.LayerName}' and LinkCode='{_pointInfo.KeyCode}'";
                _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);

                _pointInfo.PointId = 0;
                _pointInfo.Point = null;
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }
    }
}
