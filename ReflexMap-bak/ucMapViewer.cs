using System.Collections.Generic;
using DevExpress.XtraEditors;

namespace ReflexMap
{
    internal partial class ucMapViewer : XtraUserControl
    {
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        WpfMapViewer _map;

        public ucMapViewer(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr)
        {
            InitializeComponent();

            _hmDevMgr = hmDevMgr;
            _map = new WpfMapViewer(_hmDevMgr);
            host.Child = _map;
        }

        public void AddLayer(List<IGeoInfo> list, IMapLayer layer)
        {
            _map.AddLayer(list, layer);
        }

        public void NoMoreLayer()
        {
            _map.NoMoreLayer();
        }

        private void ucMapViewer_Load(object sender, System.EventArgs e)
        {
            _hmDevMgr.FormInit(this);
        }
    }
}
