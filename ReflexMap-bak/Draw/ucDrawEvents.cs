using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using Esri.ArcGISRuntime.Geometry;
using HMConnection;

namespace ReflexMap.Draw
{
    internal partial class ucDrawEvents : ucDrawBase
    {
        Dictionary<MapEventLayer, int> _layerList;
        EventGeoInfo _eventInfo;
        MapPoint _currentPoint;
        bool _drawing = false;

        public event EventHandler EventTypeChanged;

        public delegate void MarkerChangedHandler(MapEventLayer.EventMarkerType markerType);

        public event MarkerChangedHandler MarkerChanged;

        public ucDrawEvents(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map, MapEventLayer layer, int layerIndex) 
            : base(con, hmDevMgr, map)
        {
            InitializeComponent();

            _layerList = new Dictionary<MapEventLayer, int>() { { layer, layerIndex } };
            cboType.Properties.Items.AddRange(_layerList.Keys);
            cboType.SelectedIndex = 0;

            gcAttr.DataSource = _attrTable;

            InDrawingStatus(false);
        }

        public void AddEventLayer(MapEventLayer layer, int layerIndex)
        {
            _layerList.Add(layer, layerIndex);

            cboType.Properties.Items.Clear();
            cboType.Properties.Items.AddRange(_layerList.Keys);
        }

        public override int CurrLayerIndex { get { return _layerList[cboType.SelectedItem as MapEventLayer]; } }

        public override void SetCurrent(IGeoInfo info)
        {
            base.SetCurrent(info);
            _eventInfo = info.ConvertTo<EventGeoInfo>();

            cboEvents.Properties.Items.Clear();

            InDrawingStatus(false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _map.StartDraw();

            txtDescription.Text = "";
            dtpDate.DateTime = DateTime.Now;

            InDrawingStatus(true);
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                MapEventLayer layer = cboType.SelectedItem as MapEventLayer;

                EventDetails item = new EventDetails();
                item.Point = _currentPoint;
                item.Description = txtDescription.Text;
                item.Date = dtpDate.DateTime;
                item.TypeId = layer.TypeId;
                item.EventType = layer.EventTypeName;

                string sql = $"insert into Geo_Event(LinkCode, Latitude, Longitude, EventTypeId, Description, EventDate)" +
                    $" values('{_eventInfo.KeyCode}', {item.Point.Y}, {item.Point.X}, {item.TypeId}, '{item.Description}', '{item.Date}'); SELECT SCOPE_IDENTITY()";
                item.Id = Convert.ToInt32(_hmCon.SQLExecutor.ExecuteScalar(sql, _hmCon.TRConnection));

                _eventInfo.EventList.Add(item);
                cboEvents.Properties.Items.Add(item);
                cboEvents.SelectedIndex = cboEvents.Properties.Items.Count - 1;
                InDrawingStatus(false);

                _map.FinishDraw(item);
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }

        public override void InDrawingStatus(bool on)
        {
            _drawing = on;

            chkProp.Enabled = !on;
            chkMarker.Enabled = !on;
            cboType.Enabled = !on && (cboType.Properties.Items.Count > 0);
            cboEvents.Enabled = !on && (cboEvents.Properties.Items.Count > 0);
            btnDelete.Enabled = !on && (cboEvents.Properties.Items.Count > 0);

            btnStart.Enabled = !on && (_eventInfo != null);

            btnFinish.Enabled = on;
            btnCancel.Enabled = on;
            txtDescription.Enabled = on;
            dtpDate.Enabled = on;

            base.InDrawingStatus(on);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _map.CancelDraw();
            InDrawingStatus(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                EventDetails item = cboEvents.SelectedItem as EventDetails;
                _map.Delete(item);

                _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Event where Id={item.Id}", _hmCon.TRConnection);
                _eventInfo.EventList.Remove(item);

                cboEvents.Properties.Items.Remove(item);
                if (cboEvents.Properties.Items.Count == 0)
                {
                    cboEvents.Enabled = false;
                    btnDelete.Enabled = false;

                    txtDescription.Text = "";
                    dtpDate.Text = "";
                }
                else
                {
                    cboEvents.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Sql exception: " + ex.Message);
            }
        }

        public void SetPoint(MapPoint p)
        {
            _currentPoint = p;

            cboEvents.Properties.Items.Clear();
            var list = _eventInfo.EventList.Where(e => e.Point.X == p.X && e.Point.Y == p.Y).ToList();
            if (list.Any())
            {
                cboEvents.Properties.Items.AddRange(list);
                if (!_drawing)
                {
                    cboEvents.SelectedIndex = 0;
                }
            }

            InDrawingStatus(_drawing);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventTypeChanged?.Invoke(sender, e);
        }


        private void cboEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = cboEvents.SelectedItem as EventDetails;
            txtDescription.Text = data.Description;
            dtpDate.DateTime = data.Date;
        }

        private void chkProp_Click(object sender, System.EventArgs e)
        {
            chkMarker.Checked = false;
            MarkerChanged?.Invoke( MapEventLayer.EventMarkerType.Proportional);
        }

        private void chkMarker_Click(object sender, System.EventArgs e)
        {
            chkProp.Checked = false;
            MarkerChanged?.Invoke(MapEventLayer.EventMarkerType.Marker);
        }
    }
}
