using System;
using System.Linq;
using Esri.ArcGISRuntime.Geometry;
using HMConnection;

namespace ReflexMap.Draw
{
    internal partial class ucDrawShape : ucDrawBase
    {
        MapShapeLayer _layer;
        ShapeGeoInfo _shapeInfo;
        int _layerIndex;

        public ucDrawShape(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map, MapShapeLayer layer, int layerIndex)
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

            _shapeInfo = info.ConvertTo<ShapeGeoInfo>();

            cboPolygon.Properties.Items.Clear();
            cboPolygon.Properties.Items.AddRange(_shapeInfo.Polygons.Keys.ToArray());

            if (cboPolygon.Properties.Items.Count > 0)
            {
                cboPolygon.SelectedIndex = 0;
                _map.Select($"{cboPolygon.SelectedItem}");
            }

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
                Multipoint pointList = _map.FinishDraw() as Multipoint;
                if (pointList != null)
                {
                    if (_shapeInfo.ShapeId == 0)
                    {
                        object temp = _hmCon.SQLExecutor.ExecuteScalar("select max(ShapeId) from Geo_Shape", _hmCon.TRConnection);
                        _shapeInfo.ShapeId = (DBNull.Value.Equals(temp)) ? 1 : (int)temp + 1;

                        string sql = $"insert into Geo_Link(LinkTableName, LinkCode, Feature, LinkType, LinkId)" +
                            $" values('{_layer.LinkTable}','{_shapeInfo.KeyCode}', '{_layer.LayerName}', {(int)GeoType.Shape}, {_shapeInfo.ShapeId})";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }

                    int polygonId = _shapeInfo.Polygons.Keys.Max(); // already added
                    foreach (MapPoint point in pointList.Points)
                    {
                        string sql = $"insert into Geo_Shape(ShapeId, Latitude, Longitude, PolygonId) values({_shapeInfo.ShapeId}, {point.Y}, {point.X}, {polygonId})";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }

                    cboPolygon.Properties.Items.Add(polygonId);
                    cboPolygon.SelectedIndex = cboPolygon.Properties.Items.Count - 1;
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
            cboPolygon.Enabled = !on && (cboPolygon.Properties.Items.Count > 0);
            btnDelete.Enabled = !on && (cboPolygon.Properties.Items.Count > 0);

            btnStart.Enabled = !on && (_shapeInfo != null);

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
            _map.Delete($"{cboPolygon.SelectedItem}");

            try
            {
                int polygonId = int.Parse($"{cboPolygon.SelectedItem}");

                _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Shape where ShapeId={_shapeInfo.ShapeId} and PolygonId={polygonId}", _hmCon.TRConnection);

                cboPolygon.Properties.Items.Remove(cboPolygon.SelectedItem);
                if (cboPolygon.Properties.Items.Count == 0)
                {
                    _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Link where LinkTableName='{_layer.LinkTable}' and Feature='{_layer.LayerName}' and LinkCode='{_shapeInfo.KeyCode}'", _hmCon.TRConnection);
                    _shapeInfo.ShapeId = 0;
                    cboPolygon.Text = "";
                    cboPolygon.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    cboPolygon.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }

        private void cboPolygon_SelectedIndexChanged(object sender, EventArgs e)
        {
            _map.Select($"{cboPolygon.SelectedItem}");
        }

        public void SetItem(int i)
        {
            int index = cboPolygon.Properties.Items.IndexOf(i);
            cboPolygon.SelectedIndex = index;
        }

    }
}
