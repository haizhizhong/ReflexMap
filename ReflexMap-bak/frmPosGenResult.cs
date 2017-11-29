using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ReflexMap
{
    public partial class frmPosGenResult : DevExpress.XtraEditors.XtraForm
    {
        static class Status
        {
            public const string Key = "#";
        }

        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        DataTable _table;

        public frmPosGenResult()
        {
            InitializeComponent();
        }

        public void Init(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, string msg, DataTable table)
        {
            _hmDevMgr = hmDevMgr;
            lableResult.Text = msg;

            _table = table;

            gc.DataSource = _table;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog frm = new OpenFileDialog())
            {
                frm.DefaultExt = "txt";
                frm.Filter = "text files (*.txt)|*.txt";
                frm.InitialDirectory = " C:\\";
                frm.RestoreDirectory = true;
                frm.Title = "Save Results As";
                frm.CheckFileExists = false;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string path = frm.FileName;

                    using (TextWriter writer = new StreamWriter(path))
                    {
                        foreach (DataRow row in _table.Rows)
                        {
                            writer.WriteLine($"{row[PosGenColumnNames.Key]}\t{row[PosGenColumnNames.Status]}\t{row[PosGenColumnNames.Address]}");
                        }

                        writer.Close();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPosGenResult_Load(object sender, EventArgs e)
        {
            _hmDevMgr.FormInit(this);
        }
    }
}