namespace ReflexMap
{
    partial class ucMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMap));
            this.linkShowMap = new ReflexEditors.RHyperLinkEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.linkGen = new ReflexEditors.RHyperLinkEdit();
            this.checkDoAll = new DevExpress.XtraEditors.CheckEdit();
            this.linkImport = new ReflexEditors.RHyperLinkEdit();
            this.linkDraw = new ReflexEditors.RHyperLinkEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.linkShowMap.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkGen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkDoAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkImport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkDraw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // linkShowMap
            // 
            this.linkShowMap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.linkShowMap.EditValue = "View";
            this.linkShowMap.Location = new System.Drawing.Point(12, 12);
            this.linkShowMap.Margin = new System.Windows.Forms.Padding(0);
            this.linkShowMap.Name = "linkShowMap";
            this.linkShowMap.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.linkShowMap.Properties.Appearance.Options.UseBackColor = true;
            this.linkShowMap.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.linkShowMap.Properties.Image = ((System.Drawing.Image)(resources.GetObject("linkShowMap.Properties.Image")));
            this.linkShowMap.RESG_ImageType = ReflexImgSrc.Image.ImageType.Map;
            this.linkShowMap.Size = new System.Drawing.Size(139, 29);
            this.linkShowMap.StyleController = this.layoutControl1;
            this.linkShowMap.TabIndex = 1;
            this.linkShowMap.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.linkShowMap_OpenLink);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.linkGen);
            this.layoutControl1.Controls.Add(this.checkDoAll);
            this.layoutControl1.Controls.Add(this.linkImport);
            this.layoutControl1.Controls.Add(this.linkDraw);
            this.layoutControl1.Controls.Add(this.linkShowMap);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(306, 145);
            this.layoutControl1.TabIndex = 5;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // linkGen
            // 
            this.linkGen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.linkGen.EditValue = "Generate";
            this.linkGen.Location = new System.Drawing.Point(12, 79);
            this.linkGen.Margin = new System.Windows.Forms.Padding(0);
            this.linkGen.Name = "linkGen";
            this.linkGen.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.linkGen.Properties.Appearance.Options.UseBackColor = true;
            this.linkGen.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.linkGen.Properties.Image = ((System.Drawing.Image)(resources.GetObject("linkPos.Properties.Image")));
            this.linkGen.RESG_ImageType = ReflexImgSrc.Image.ImageType.Play;
            this.linkGen.Size = new System.Drawing.Size(282, 29);
            this.linkGen.StyleController = this.layoutControl1;
            this.linkGen.TabIndex = 5;
            this.linkGen.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.linkGen_OpenLink);
            // 
            // checkDoAll
            // 
            this.checkDoAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkDoAll.EditValue = true;
            this.checkDoAll.Location = new System.Drawing.Point(155, 12);
            this.checkDoAll.Name = "checkDoAll";
            this.checkDoAll.Properties.Caption = "Do All";
            this.checkDoAll.Size = new System.Drawing.Size(139, 19);
            this.checkDoAll.StyleController = this.layoutControl1;
            this.checkDoAll.TabIndex = 4;
            // 
            // linkImport
            // 
            this.linkImport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.linkImport.EditValue = "Import/Export";
            this.linkImport.Location = new System.Drawing.Point(155, 45);
            this.linkImport.Margin = new System.Windows.Forms.Padding(0);
            this.linkImport.Name = "linkImport";
            this.linkImport.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.linkImport.Properties.Appearance.Options.UseBackColor = true;
            this.linkImport.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.linkImport.Properties.Image = ((System.Drawing.Image)(resources.GetObject("linkImport.Properties.Image")));
            this.linkImport.RESG_ImageType = ReflexImgSrc.Image.ImageType.InsertRows;
            this.linkImport.Size = new System.Drawing.Size(139, 30);
            this.linkImport.StyleController = this.layoutControl1;
            this.linkImport.TabIndex = 2;
            this.linkImport.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.linkImport_OpenLink);
            // 
            // linkDraw
            // 
            this.linkDraw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.linkDraw.EditValue = "Draw ";
            this.linkDraw.Location = new System.Drawing.Point(12, 45);
            this.linkDraw.Margin = new System.Windows.Forms.Padding(0);
            this.linkDraw.Name = "linkDraw";
            this.linkDraw.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.linkDraw.Properties.Appearance.Options.UseBackColor = true;
            this.linkDraw.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.linkDraw.Properties.Image = ((System.Drawing.Image)(resources.GetObject("linkDraw.Properties.Image")));
            this.linkDraw.RESG_ImageType = ReflexImgSrc.Image.ImageType.GeoPointMap;
            this.linkDraw.Size = new System.Drawing.Size(139, 30);
            this.linkDraw.StyleController = this.layoutControl1;
            this.linkDraw.TabIndex = 3;
            this.linkDraw.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.linkDraw_OpenLink);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(306, 145);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.linkShowMap;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(143, 33);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 100);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(286, 25);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.checkDoAll;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.layoutControlItem2.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.layoutControlItem2.Location = new System.Drawing.Point(143, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(143, 33);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.linkDraw;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 33);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(143, 34);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.linkImport;
            this.layoutControlItem4.Location = new System.Drawing.Point(143, 33);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(143, 34);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.linkGen;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 67);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(286, 33);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // ucMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucMap";
            this.Size = new System.Drawing.Size(306, 145);
            ((System.ComponentModel.ISupportInitialize)(this.linkShowMap.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linkGen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkDoAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkImport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkDraw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReflexEditors.RHyperLinkEdit linkShowMap;
        private ReflexEditors.RHyperLinkEdit linkDraw;
        private DevExpress.XtraEditors.CheckEdit checkDoAll;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private ReflexEditors.RHyperLinkEdit linkImport;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private ReflexEditors.RHyperLinkEdit linkGen;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}
