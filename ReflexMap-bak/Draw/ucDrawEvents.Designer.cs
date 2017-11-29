namespace ReflexMap.Draw
{
    partial class ucDrawEvents
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.gcAttr = new ReflexGridControl.GridControl();
            this.gvAttr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dtpDate = new DevExpress.XtraEditors.DateEdit();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.cboEvents = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblEvents = new DevExpress.XtraEditors.LabelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.cboType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkProp = new DevExpress.XtraEditors.CheckEdit();
            this.chkMarker = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEvents.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkProp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarker.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.btnStart.Text = "Create New Event";
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
            this.gcAttr.Size = new System.Drawing.Size(256, 269);
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
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Location = new System.Drawing.Point(24, 382);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(23, 13);
            this.labelControl3.TabIndex = 26;
            this.labelControl3.Text = "Date";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(23, 406);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(53, 13);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "Description";
            // 
            // dtpDate
            // 
            this.dtpDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpDate.EditValue = null;
            this.dtpDate.Location = new System.Drawing.Point(83, 378);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDate.Size = new System.Drawing.Size(100, 20);
            this.dtpDate.TabIndex = 24;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(82, 403);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(165, 20);
            this.txtDescription.TabIndex = 23;
            // 
            // cboEvents
            // 
            this.cboEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEvents.Location = new System.Drawing.Point(83, 352);
            this.cboEvents.Name = "cboEvents";
            this.cboEvents.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboEvents.Size = new System.Drawing.Size(103, 20);
            this.cboEvents.TabIndex = 22;
            this.cboEvents.SelectedIndexChanged += new System.EventHandler(this.cboEvents_SelectedIndexChanged);
            // 
            // lblEvents
            // 
            this.lblEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEvents.Location = new System.Drawing.Point(24, 356);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(33, 13);
            this.lblEvents.TabIndex = 20;
            this.lblEvents.Text = "Events";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(192, 351);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(66, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboType.Location = new System.Drawing.Point(83, 325);
            this.cboType.Name = "cboType";
            this.cboType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboType.Size = new System.Drawing.Size(103, 20);
            this.cboType.TabIndex = 28;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(24, 329);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(55, 13);
            this.labelControl1.TabIndex = 27;
            this.labelControl1.Text = "Event Type";
            // 
            // chkProp
            // 
            this.chkProp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkProp.EditValue = true;
            this.chkProp.Location = new System.Drawing.Point(20, 294);
            this.chkProp.Name = "chkProp";
            this.chkProp.Properties.Caption = "Proportional";
            this.chkProp.Properties.RadioGroupIndex = 1;
            this.chkProp.Size = new System.Drawing.Size(91, 19);
            this.chkProp.TabIndex = 29;
            this.chkProp.Click += chkProp_Click;
            // 
            // chkMarker
            // 
            this.chkMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMarker.Location = new System.Drawing.Point(129, 294);
            this.chkMarker.Name = "chkMarker";
            this.chkMarker.Properties.Caption = "Marker";
            this.chkMarker.Properties.RadioGroupIndex = 2;
            this.chkMarker.Size = new System.Drawing.Size(75, 19);
            this.chkMarker.TabIndex = 30;
            this.chkMarker.TabStop = false;
            this.chkMarker.Click += chkMarker_Click;
            // 
            // ucDrawEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkMarker);
            this.Controls.Add(this.chkProp);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.cboEvents);
            this.Controls.Add(this.lblEvents);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.gcAttr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnStart);
            this.Name = "ucDrawEvents";
            this.Size = new System.Drawing.Size(270, 516);
            ((System.ComponentModel.ISupportInitialize)(this.gcAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEvents.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkProp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarker.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnFinish;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private ReflexGridControl.GridControl gcAttr;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAttr;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dtpDate;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private DevExpress.XtraEditors.ComboBoxEdit cboEvents;
        private DevExpress.XtraEditors.LabelControl lblEvents;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.ComboBoxEdit cboType;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkProp;
        private DevExpress.XtraEditors.CheckEdit chkMarker;
    }
}
