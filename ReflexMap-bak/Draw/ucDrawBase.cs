using System.Data;
using DevExpress.XtraEditors;
using HMConnection;
using WS_Popups;

namespace ReflexMap.Draw
{
    public abstract class ucDrawBase : XtraUserControl
    {
        protected HMCon _hmCon;
        protected TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        protected frmPopup _pop;

        protected WpfMapDrawBase _map;

        protected DataTable _attrTable;

        public delegate void DrawingStatusHandler(bool on);

        public event DrawingStatusHandler DrawingStatusChanged;

        public ucDrawBase(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, WpfMapDrawBase map)
        {
            _hmCon = con;
            _hmDevMgr = hmDevMgr;
            _pop = new frmPopup(hmDevMgr);
            _map = map;

            _attrTable = new DataTable();
            _attrTable.Columns.Add(new DataColumn("Attribute"));
            _attrTable.Columns.Add(new DataColumn("Value"));
        }

        public virtual int CurrLayerIndex { get; }

        public virtual void SetCurrent(IGeoInfo geoInfo)
        {
            _attrTable.Clear();
            var row = _attrTable.NewRow();
            row["Attribute"] = "#";
            row["Value"] = geoInfo.KeyCode;
            _attrTable.Rows.Add(row);

            foreach (var attr in geoInfo.AttrList)
            {
                row = _attrTable.NewRow();
                row["Attribute"] = attr.Key;
                row["Value"] = attr.Value;
                _attrTable.Rows.Add(row);
            }
        }

        public virtual void InDrawingStatus(bool on)
        {
            DrawingStatusChanged?.Invoke(on);
        }

        public T ConvertTo<T>() where T : ucDrawBase
        {
            return this as T;
        }
    }
}
