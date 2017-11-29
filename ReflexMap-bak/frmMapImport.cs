using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using HMConnection;
using WS_Popups;

namespace ReflexMap
{
    internal partial class frmMapImport : DevExpress.XtraEditors.XtraForm
    {
        HMCon _hmCon;
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        List<IMapLayer> _layers;
        frmPopup _pop;

        public frmMapImport()
        {
            InitializeComponent();
        }

        public void Init(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, List<DataRow> rowList, List<IMapLayer> layers)
        {
            _hmCon = con;
            _hmDevMgr = hmDevMgr;
            _layers = layers;
            _pop = new frmPopup(_hmDevMgr);

            foreach (var layer in _layers)
            {
                var page = tabControl.TabPages.Add(layer.LayerName);
                ucMapImport import = new ucMapImport();
                import.Init(_hmCon, _hmDevMgr, rowList, layer);
                import.Parent = page;
                import.Dock = DockStyle.Fill;
                tabControl.TabPages.Add(page);
            }
        }

        private void frmMapImport_Load(object sender, System.EventArgs e)
        {
            _hmDevMgr.FormInit(this);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}