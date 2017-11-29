using System;
using System.Linq;
using Esri.ArcGISRuntime.Geometry;
using HMConnection;

namespace ReflexMap.Draw
{
    internal partial class ucDrawLine : ucDrawBase
    {
        MapLineLayer _layer;
        LineGeoInfo _lineInfo;
        int _layerIndex;

        public ucDrawLine(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map, MapLineLayer layer, int layerIndex)
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

            _lineInfo = info.ConvertTo<LineGeoInfo>();

            cboPolyline.Properties.Items.Clear();
            cboPolyline.Properties.Items.AddRange(_lineInfo.Polylines.Keys.ToArray());

            if (cboPolyline.Properties.Items.Count > 0)
            {
                cboPolyline.SelectedIndex = 0;
                _map.Select($"{cboPolyline.SelectedItem}");
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
                    if (_lineInfo.PolylineId == 0)
                    {
                        object temp = _hmCon.SQLExecutor.ExecuteScalar("select max(PolylineId) from Geo_Polyline", _hmCon.TRConnection);
                        _lineInfo.PolylineId = (DBNull.Value.Equals(temp)) ? 1 : (int)temp + 1;

                        string sql = $"insert into Geo_Link(LinkTableName, LinkCode, Feature, LinkType, LinkId)" +
                            $" values('{_layer.LinkTable}','{_lineInfo.KeyCode}', '{_layer.LayerName}', {(int)GeoType.Polyline}, {_lineInfo.PolylineId})";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }

                    int lineId = _lineInfo.Polylines.Keys.Max(); // already added
                    foreach (MapPoint point in pointList.Points)
                    {
                        string sql = $"insert into Geo_Polyline(PolylineId, Latitude, Longitude, LineId) values({_lineInfo.PolylineId}, {point.Y}, {point.X}, {lineId})";
                        _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                    }

                    cboPolyline.Properties.Items.Add(lineId);
                    cboPolyline.SelectedIndex = cboPolyline.Properties.Items.Count - 1;
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
            cboPolyline.Enabled = !on && (cboPolyline.Properties.Items.Count > 0);
            btnDelete.Enabled = !on && (cboPolyline.Properties.Items.Count > 0);

            btnStart.Enabled = !on && (_lineInfo != null);

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
            _map.Delete($"{cboPolyline.SelectedItem}");

            try
            {
                int lineId = int.Parse($"{cboPolyline.SelectedItem}");

                _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Polyline where PolylineId={_lineInfo.PolylineId} and LineId={lineId}", _hmCon.TRConnection);

                cboPolyline.Properties.Items.Remove(cboPolyline.SelectedItem);
                if (cboPolyline.Properties.Items.Count == 0)
                {
                    _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Link where LinkTableName='{_layer.LinkTable}' and Feature='{_layer.LayerName}' and LinkCode='{_lineInfo.KeyCode}'", _hmCon.TRConnection);
                    _lineInfo.PolylineId = 0;
                    cboPolyline.Text = "";
                    cboPolyline.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    cboPolyline.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }

        private void cboPolyline_SelectedIndexChanged(object sender, EventArgs e)
        {
            _map.Select($"{cboPolyline.SelectedItem}");
        }

        public void SetItem(int i)
        {
            int index = cboPolyline.Properties.Items.IndexOf(i);
            cboPolyline.SelectedIndex = index;
        }
    }
}
