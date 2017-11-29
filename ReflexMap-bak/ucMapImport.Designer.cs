namespace ReflexMap
{
    partial class ucMapImport
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
            this.gc = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTableName = new System.Windows.Forms.Label();
            this.labelFieldName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFeatureName = new System.Windows.Forms.Label();
            this.labelFeature = new System.Windows.Forms.Label();
            this.btnAction = new DevExpress.XtraEditors.SimpleButton();
            this.btnFile = new DevExpress.XtraEditors.SimpleButton();
            this.txtFile = new DevExpress.XtraEditors.TextEdit();
            this.btnIgnoreAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gc
            // 
            this.gc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gc.Location = new System.Drawing.Point(18, 67);
            this.gc.MainView = this.gv;
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(509, 290);
            this.gc.TabIndex = 10;
            this.gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.GridControl = this.gc;
            this.gv.Name = "gv";
            this.gv.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gv_CustomRowCellEditForEditing);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Table:";
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTableName.Location = new System.Drawing.Point(52, 14);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(70, 13);
            this.labelTableName.TabIndex = 13;
            this.labelTableName.Text = "TableName";
            // 
            // labelFieldName
            // 
            this.labelFieldName.AutoSize = true;
            this.labelFieldName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFieldName.Location = new System.Drawing.Point(239, 14);
            this.labelFieldName.Name = "labelFieldName";
            this.labelFieldName.Size = new System.Drawing.Size(65, 13);
            this.labelFieldName.TabIndex = 15;
            this.labelFieldName.Text = "FieldName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Field:";
            // 
            // labelFeatureName
            // 
            this.labelFeatureName.AutoSize = true;
            this.labelFeatureName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFeatureName.Location = new System.Drawing.Point(448, 14);
            this.labelFeatureName.Name = "labelFeatureName";
            this.labelFeatureName.Size = new System.Drawing.Size(79, 13);
            this.labelFeatureName.TabIndex = 17;
            this.labelFeatureName.Text = "FietureName";
            // 
            // labelFeature
            // 
            this.labelFeature.AutoSize = true;
            this.labelFeature.Location = new System.Drawing.Point(403, 14);
            this.labelFeature.Name = "labelFeature";
            this.labelFeature.Size = new System.Drawing.Size(49, 13);
            this.labelFeature.TabIndex = 16;
            this.labelFeature.Text = "Feature:";
            // 
            // btnAction
            // 
            this.btnAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAction.Location = new System.Drawing.Point(337, 363);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(86, 23);
            this.btnAction.TabIndex = 5;
            this.btnAction.Text = "Process Actions";
            this.btnAction.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.Location = new System.Drawing.Point(460, 39);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(67, 20);
            this.btnFile.TabIndex = 2;
            this.btnFile.Text = "Browse";
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(20, 39);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(434, 20);
            this.txtFile.TabIndex = 1;
            // 
            // btnIgnoreAll
            // 
            this.btnIgnoreAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIgnoreAll.Location = new System.Drawing.Point(21, 363);
            this.btnIgnoreAll.Name = "btnIgnoreAll";
            this.btnIgnoreAll.Size = new System.Drawing.Size(75, 23);
            this.btnIgnoreAll.TabIndex = 3;
            this.btnIgnoreAll.Text = "Ignore All";
            this.btnIgnoreAll.Click += new System.EventHandler(this.btnIgnoreAll_Click);
            // 
            // btnImportAll
            // 
            this.btnImportAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportAll.Location = new System.Drawing.Point(102, 363);
            this.btnImportAll.Name = "btnImportAll";
            this.btnImportAll.Size = new System.Drawing.Size(75, 23);
            this.btnImportAll.TabIndex = 4;
            this.btnImportAll.Text = "Import All";
            this.btnImportAll.Click += new System.EventHandler(this.btnImportAll_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteAll.Location = new System.Drawing.Point(182, 363);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAll.TabIndex = 18;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(441, 363);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(86, 23);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ucMapImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.btnImportAll);
            this.Controls.Add(this.btnIgnoreAll);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.labelFeatureName);
            this.Controls.Add(this.labelFeature);
            this.Controls.Add(this.labelFieldName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTableName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gc);
            this.Name = "ucMapImport";
            this.Size = new System.Drawing.Size(545, 401);
            this.Load += new System.EventHandler(this.ucMapImport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gc;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.Label labelFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFeatureName;
        private System.Windows.Forms.Label labelFeature;
        private DevExpress.XtraEditors.SimpleButton btnAction;
        private DevExpress.XtraEditors.SimpleButton btnFile;
        private DevExpress.XtraEditors.TextEdit txtFile;
        private DevExpress.XtraEditors.SimpleButton btnIgnoreAll;
        private DevExpress.XtraEditors.SimpleButton btnImportAll;
        private DevExpress.XtraEditors.SimpleButton btnDeleteAll;
        private DevExpress.XtraEditors.SimpleButton btnExport;
    }
}
