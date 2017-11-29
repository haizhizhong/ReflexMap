using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HMConnection;
using WS_Popups;

namespace ReflexMap.Draw
{
    public partial class frmMapDraw : DevExpress.XtraEditors.XtraForm
    {
        HMCon _hmCon;
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        List<IMapLayer> _layers;
        frmPopup _pop;

        Dictionary<int, List<IGeoInfo>> _data;      // layer, geo for all rows

        WpfMapDrawBase _map;

        public frmMapDraw()
        {
            InitializeComponent();
        }

        public void Init(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, List<DataRow> rowList, List<IMapLayer> layers)
        {
            _hmCon = con;
            _hmDevMgr = hmDevMgr;
            _layers = layers;
            _pop = new frmPopup(_hmDevMgr);

            _map = new WpfMapDrawBase(_hmDevMgr);
            host.Child = _map;

            _data = new Dictionary<int, List<IGeoInfo>>();
            DrawTabFactory factory = new DrawTabFactory();
            for (int i = 0; i < layers.Count; i++)
            {
                try
                {
                    _map.RegisterLayer(i, layers[i]);
                    ucDrawBase ucDraw = factory.CreateInstance(con, hmDevMgr, _map, layers[i], i);
                    if (ucDraw != null)
                    {
                        ucDraw.DrawingStatusChanged += SetDrawingMode;
                        if (ucDraw is ucDrawEvents)
                        {
                            ucDraw.ConvertTo<ucDrawEvents>().EventTypeChanged += FrmMapDraw_EventTypeChanged;
                            ucDraw.ConvertTo<ucDrawEvents>().MarkerChanged += _map.EventMarkerChanged;
                            _map.PointTapped += ucDraw.ConvertTo<ucDrawEvents>().SetPoint;
                        }
                        else if (ucDraw is ucDrawShape)
                        {
                            _map.ItemTapped += ucDraw.ConvertTo<ucDrawShape>().SetItem;
                        }
                        else if (ucDraw is ucDrawLine)
                        {
                            _map.ItemTapped += ucDraw.ConvertTo<ucDrawLine>().SetItem;
                        }

                        var page = tabControl.TabPages.Add(layers[i] is MapEventLayer ? "Event" : layers[i].LayerName);
                        ucDraw.Parent = page;
                        ucDraw.Dock = DockStyle.Fill;
                    }

                    _data.Add(i, MapData.GetDataList(con, rowList, layers[i]));
                }
                catch (ConstraintException ex)
                {
                    _pop.ShowPopup($"Error in loading {layers[i].LinkTable}: {ex.Message}");
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("#"));
            foreach (var layer in layers)
            {
                layer.Attributes.ToList().ForEach(attr => {
                    if (dataTable.Columns.IndexOf(attr.LinkField) == -1)
                    {
                        DataColumn col = new DataColumn(attr.LinkField);
                        col.Caption = attr.DisplayName;
                        dataTable.Columns.Add(col);
                    }
                });
            }

            foreach(var srcRow in rowList)
            {
                var row = dataTable.NewRow();

                foreach (var layer in layers)
                {
                    row["#"] = srcRow[layer.LinkColumn];
                    layer.Attributes.ToList().ForEach(attr => row[attr.LinkField] = attr.LinkDictionary?[$"{srcRow[attr.LinkField]}"] ?? srcRow[attr.LinkField]);
                }
                dataTable.Rows.Add(row);
            }

            gc.DataSource = dataTable;

            InDrawingStatus(false);
        }

        private void FrmMapDraw_EventTypeChanged(object sender, EventArgs e)
        {
            DispayCurrentRowNTab();
        }

        private void gv_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DispayCurrentRowNTab();
        }

        public void DispayCurrentRowNTab()
        {
            if (gc.DataSource == null || gv.GetFocusedDataRow() == null)
                return;

            string keyCode = (string)gv.GetFocusedDataRow()["#"];


            ucDrawBase curDrawer = tabControl.SelectedTabPage.Controls[0] as ucDrawBase;
            int layerIndex = curDrawer.CurrLayerIndex;

            _map.SetCurrentLayer(layerIndex);
            List<IGeoInfo> allLayerGeo = new List<IGeoInfo>();
            _data.ToList().ForEach(l => allLayerGeo.Add(l.Value?.Find(x => x.KeyCode == keyCode)));
            _map.DrawAll(allLayerGeo);

            var geoInfo = _data[layerIndex]?.Find(x => x.KeyCode == keyCode);
            curDrawer.SetCurrent(geoInfo);
        }

        public void SetDrawingMode( bool drawing)
        {
            gc.Enabled = !drawing;
            tabControl.TabPages.Where( page => page!= tabControl.SelectedTabPage).ToList().ForEach(page => page.PageEnabled = !drawing);
        }

        private void frmMapDraw_Load(object sender, System.EventArgs e)
        {
            _hmDevMgr.FormInit(this);
        }

        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            DispayCurrentRowNTab();
        }

        internal void InDrawingStatus(bool on)
        {
            ucDrawBase curDrawer = tabControl.SelectedTabPage.Controls[0] as ucDrawBase;
            curDrawer.InDrawingStatus(on);
        }
    }
}