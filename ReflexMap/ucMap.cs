using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HMConnection;
using DevExpress.XtraGrid.Views.Grid;
using WS_Popups;
using System.ComponentModel;
using System;
using System.Data;
using System.Linq;
using Esri.ArcGISRuntime;
using ReflexMap.Draw;
using static WS_Popups.frmPopup;

namespace ReflexMap
{
    public partial class ucMap : DevExpress.XtraEditors.XtraUserControl
    {
        [Browsable(true), Description("Allow import GEO data"), Category("Design")]
        public bool EnableMapImport
        {
            set { linkImport.Enabled = value; }
            get { return linkImport.Enabled; }
        }

        [Browsable(true), Description("Allow draw GEO"), Category("Design")]
        public bool EnableMapDraw
        {
            set { linkDraw.Enabled = value; }
            get { return linkDraw.Enabled; }
        }

        HMCon _hmCon;
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        GridView _gv;
        List<IMapLayer> _layers;
        List<PosGenerator> _posGens;
        frmPopup _pop;

        public ucMap()
        {
            InitializeComponent();
        }

        public void Init(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, GridView gv, List<IMapLayer> layers)
        {
            Init(con, hmDevMgr, gv, layers, null);
        }

        public void Init(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, GridView gv, List<IMapLayer> layers, List<PosGenerator> posGens)
        {
            _hmCon = con;
            _hmDevMgr = hmDevMgr;
            _gv = gv;
            _layers = layers;
            _posGens = posGens;
            linkGen.Enabled = _posGens != null;

            _pop = new frmPopup(_hmDevMgr);

            if (!ArcGISRuntimeEnvironment.IsInitialized)
            {
                try
                {
                    //ArcGISRuntimeEnvironment.InstallPath = Application.StartupPath;
                    ArcGISRuntimeEnvironment.ClientId = "EdzP18VEt80l7prT";
                    ArcGISRuntimeEnvironment.Initialize();
                }
                catch (Exception ex)
                {
                    _pop.ShowPopup($"Unable to initialize the ArcGIS Runtime with the client ID provided: {ex.Message}");
                }
            }

            _hmDevMgr.FormInit(this);
        }

        private List<DataRow> GetDataList()
        {
            List<DataRow> list = new List<DataRow>();
            if (checkDoAll.Checked)
            {
                for (int i = 0; i < _gv.RowCount; i++)
                    list.Add(_gv.GetDataRow(i));
            }
            else
            {
                _gv.GetSelectedRows().ToList().ForEach(i => list.Add(_gv.GetDataRow(i)));
            }

            return list;
        }

        private void linkShowMap_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            var list = GetDataList();

            Form frm = new Form();
            CL_Dialog.PleaseWait.Show("Loading...\r\nPlease Wait", null);

            ucMapViewer map = new ucMapViewer(_hmDevMgr);
            foreach (var layer in _layers)
            {
                List<IGeoInfo> data = null;
                try
                {
                    data = MapData.GetDataList(_hmCon, list, layer);
                }
                catch (Exception ex)
                {
                    _pop.ShowPopup($"Fail in loading {layer.LinkTable}: {ex.Message}");
                }
                map.AddLayer(data, layer);
            }
            map.NoMoreLayer();

            frm.WindowState = FormWindowState.Maximized;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Icon = this.ParentForm.Icon;
            frm.Text = "Map";
            map.Parent = frm;
            map.Dock = DockStyle.Fill;

            CL_Dialog.PleaseWait.Hide();
            frm.Show();
        }

        private void linkImport_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            CL_Dialog.PleaseWait.Show("Loading...\r\nPlease Wait", null);
            var list = GetDataList();

            using (var frm = new frmMapImport())
            {
                frm.Init(_hmCon, _hmDevMgr, list, _layers);
                frm.Size = new Size(800, 800);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Icon = this.ParentForm.Icon;
                frm.Text = "Import map data";

                CL_Dialog.PleaseWait.Hide();
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void linkDraw_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            CL_Dialog.PleaseWait.Show("Loading...\r\nPlease Wait", null);
            var list = GetDataList();
            var frm = new frmMapDraw();
            frm.Init(_hmCon, _hmDevMgr, list, _layers);
            frm.WindowState = FormWindowState.Maximized;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Icon = this.ParentForm.Icon;
            frm.Text = "Draw Geo";

            CL_Dialog.PleaseWait.Hide();
            frm.Show();
        }

        private void linkGen_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add(PosGenColumnNames.Key);
            table.Columns.Add(PosGenColumnNames.Status);
            table.Columns.Add(PosGenColumnNames.Address);
            table.Columns.Add(PosGenColumnNames.Exception);

            var srcList = GetDataList();

            foreach (var gen in _posGens)
            {
                int skipped = 0;
                int generated = 0;
                int failed = 0;
                int Current = 0;
                table.Clear();

                var rowList = gen.GetData(srcList);
                foreach (DataRow row in rowList)
                {
                    Current++;
                    CL_Dialog.PleaseWait.Show($"Processing {gen.LayerName}... {Current} of {rowList.Count}\r\nPlease Wait", null);
                    var logRow = table.NewRow();
                    string keyCode = $"{row[gen.LinkColumn]}";
                    logRow[PosGenColumnNames.Key] = keyCode;

                    try
                    {
                        if (!gen.RecordExist(_hmCon, keyCode))
                        {
                            string fullAddress = gen.GetFullAddress(row);
                            logRow[PosGenColumnNames.Address] = fullAddress;

                            var pos = PosGenerator.GetPositionFromAddress(fullAddress);
                            if (pos != null)
                            {
                                gen.Save(_hmCon, keyCode, pos.Latitude, pos.Longitude);
                                logRow[PosGenColumnNames.Status] = PosGenStatus.Generated;
                                generated++;
                            }
                            else
                            {
                                logRow[PosGenColumnNames.Status] = PosGenStatus.Failed;
                                failed++;
                            }
                            table.Rows.Add(logRow);
                        }
                        else
                        {
                            skipped++;
                        }
                    }
                    catch (Exception ex)
                    {
                        logRow[PosGenColumnNames.Status] = PosGenStatus.Exception;
                        logRow[PosGenColumnNames.Exception] = ex.Message;
                        failed++;
                        table.Rows.Add(logRow);
                    }
                }

                CL_Dialog.PleaseWait.Hide();
                using (var frm = new frmPosGenResult())
                {
                    string msg = $"{gen.LayerName} results: Skipped={skipped}; Generated={generated}; Failed={failed}";
                    frm.Init(_hmDevMgr, msg, table);
                    frm.Size = new Size(800, 800);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.Icon = this.ParentForm.Icon;
                    frm.Text = $"{gen.LayerName} Results";

                    frm.ShowDialog();
                    frm.Dispose();
                }
            }
        }
    }
}
