namespace ReflexMap.Draw
{
    partial class ucDrawPoint
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.gcAttr = new ReflexGridControl.GridControl();
            this.gvAttr = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttr)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(60, 398);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 26);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(137, 467);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Location = new System.Drawing.Point(59, 467);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(72, 23);
            this.btnFinish.TabIndex = 17;
            this.btnFinish.Text = "Save";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(60, 434);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 23);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "Reset Point";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gcAttr
            // 
            this.gcAttr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcAttr.DataAdapter = null;
            this.gcAttr.EmbeddedNavigator.Enabled = false;
            this.gcAttr.Enabled = false;
            this.gcAttr.Flavour = ReflexGridControl.GridControl.eFlavour.TR;
            this.gcAttr.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcAttr.HMConnection = null;
            this.gcAttr.KBII_ID = -1;
            this.gcAttr.Location = new System.Drawing.Point(7, 7);
            this.gcAttr.MainView = this.gvAttr;
            this.gcAttr.Name = "gcAttr";
            this.gcAttr.Query_Name = null;
            this.gcAttr.Security_ID = 0;
            this.gcAttr.Size = new System.Drawing.Size(256, 318);
            this.gcAttr.TabIndex = 19;
            this.gcAttr.TUC_HMDevXManager = null;
            this.gcAttr.UseDisabledStatePainter = false;
            this.gcAttr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAttr});
            // 
            // gvAttr
            // 
            this.gvAttr.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gvAttr.GridControl = this.gcAttr;
            this.gvAttr.Name = "gvAttr";
            this.gvAttr.OptionsBehavior.Editable = false;
            this.gvAttr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvAttr.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvAttr.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvAttr.OptionsView.ShowColumnHeaders = false;
            this.gvAttr.OptionsView.ShowGroupPanel = false;
            this.gvAttr.OptionsView.ShowIndicator = false;
            // 
            // ucDrawPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcAttr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnDelete);
            this.Name = "ucDrawPoint";
            this.Size = new System.Drawing.Size(270, 516);
            ((System.ComponentModel.ISupportInitialize)(this.gcAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnFinish;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private ReflexGridControl.GridControl gcAttr;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAttr;
    }
}
