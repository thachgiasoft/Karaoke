namespace Karaoke.MDIForms
{
    partial class frmDieuChinhTonKho
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gcDieuChinhTonKho = new DevExpress.XtraGrid.GridControl();
            this.gvDieuChinhTonKho = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTenSanPham = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSoluongDC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNgayDieuChinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGhiChu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDieuChinhTK = new DevExpress.XtraEditors.GroupControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtGhiChu = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSoLuongDC = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblChonSP = new DevExpress.XtraEditors.LabelControl();
            this.cboSanPham = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDieuChinhTonKho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDieuChinhTonKho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDieuChinhTK)).BeginInit();
            this.gcDieuChinhTK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGhiChu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoLuongDC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSanPham.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDieuChinhTonKho
            // 
            this.gcDieuChinhTonKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDieuChinhTonKho.EmbeddedNavigator.Name = "";
            this.gcDieuChinhTonKho.FormsUseDefaultLookAndFeel = false;
            this.gcDieuChinhTonKho.Location = new System.Drawing.Point(0, 0);
            this.gcDieuChinhTonKho.MainView = this.gvDieuChinhTonKho;
            this.gcDieuChinhTonKho.Name = "gcDieuChinhTonKho";
            this.gcDieuChinhTonKho.Size = new System.Drawing.Size(827, 472);
            this.gcDieuChinhTonKho.TabIndex = 0;
            this.gcDieuChinhTonKho.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDieuChinhTonKho});
            // 
            // gvDieuChinhTonKho
            // 
            this.gvDieuChinhTonKho.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTenSanPham,
            this.colSoluongDC,
            this.colNgayDieuChinh,
            this.colGhiChu});
            this.gvDieuChinhTonKho.GridControl = this.gcDieuChinhTonKho;
            this.gvDieuChinhTonKho.Name = "gvDieuChinhTonKho";
            // 
            // colTenSanPham
            // 
            this.colTenSanPham.Caption = "Tên sản phẩm";
            this.colTenSanPham.FieldName = "TenSanPham";
            this.colTenSanPham.Name = "colTenSanPham";
            this.colTenSanPham.Visible = true;
            this.colTenSanPham.VisibleIndex = 0;
            // 
            // colSoluongDC
            // 
            this.colSoluongDC.Caption = "Số lượng điều chỉnh";
            this.colSoluongDC.FieldName = "SoLuongDC";
            this.colSoluongDC.Name = "colSoluongDC";
            this.colSoluongDC.Visible = true;
            this.colSoluongDC.VisibleIndex = 1;
            // 
            // colNgayDieuChinh
            // 
            this.colNgayDieuChinh.Caption = "Ngày điều chỉnh";
            this.colNgayDieuChinh.FieldName = "NgayDieuChinh";
            this.colNgayDieuChinh.Name = "colNgayDieuChinh";
            this.colNgayDieuChinh.Visible = true;
            this.colNgayDieuChinh.VisibleIndex = 2;
            // 
            // colGhiChu
            // 
            this.colGhiChu.Caption = "Ghi chú";
            this.colGhiChu.FieldName = "GhiChu";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.Visible = true;
            this.colGhiChu.VisibleIndex = 3;
            // 
            // gcDieuChinhTK
            // 
            this.gcDieuChinhTK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.gcDieuChinhTK.Controls.Add(this.btnSave);
            this.gcDieuChinhTK.Controls.Add(this.txtGhiChu);
            this.gcDieuChinhTK.Controls.Add(this.labelControl2);
            this.gcDieuChinhTK.Controls.Add(this.txtSoLuongDC);
            this.gcDieuChinhTK.Controls.Add(this.labelControl1);
            this.gcDieuChinhTK.Controls.Add(this.lblChonSP);
            this.gcDieuChinhTK.Controls.Add(this.cboSanPham);
            this.gcDieuChinhTK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gcDieuChinhTK.Location = new System.Drawing.Point(0, 325);
            this.gcDieuChinhTK.Name = "gcDieuChinhTK";
            this.gcDieuChinhTK.Size = new System.Drawing.Size(827, 147);
            this.gcDieuChinhTK.TabIndex = 1;
            this.gcDieuChinhTK.Text = "Điều chỉnh";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(120, 104);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 38);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(436, 42);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(362, 86);
            this.txtGhiChu.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(395, 44);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(35, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Ghi chú";
            // 
            // txtSoLuongDC
            // 
            this.txtSoLuongDC.Location = new System.Drawing.Point(120, 74);
            this.txtSoLuongDC.Name = "txtSoLuongDC";
            this.txtSoLuongDC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSoLuongDC.Size = new System.Drawing.Size(172, 20);
            this.txtSoLuongDC.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 77);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(97, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Số lượng điều chỉnh:";
            // 
            // lblChonSP
            // 
            this.lblChonSP.Location = new System.Drawing.Point(5, 44);
            this.lblChonSP.Name = "lblChonSP";
            this.lblChonSP.Size = new System.Drawing.Size(78, 13);
            this.lblChonSP.TabIndex = 1;
            this.lblChonSP.Text = "Chọn sản phẩm:";
            // 
            // cboSanPham
            // 
            this.cboSanPham.Location = new System.Drawing.Point(120, 41);
            this.cboSanPham.Name = "cboSanPham";
            this.cboSanPham.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSanPham.Size = new System.Drawing.Size(172, 20);
            this.cboSanPham.TabIndex = 0;
            // 
            // frmDieuChinhTonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 472);
            this.Controls.Add(this.gcDieuChinhTK);
            this.Controls.Add(this.gcDieuChinhTonKho);
            this.Name = "frmDieuChinhTonKho";
            this.Text = "Điều chỉnh tồn kho";
            this.Load += new System.EventHandler(this.frmDieuChinhTonKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcDieuChinhTonKho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDieuChinhTonKho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDieuChinhTK)).EndInit();
            this.gcDieuChinhTK.ResumeLayout(false);
            this.gcDieuChinhTK.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGhiChu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoLuongDC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSanPham.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcDieuChinhTonKho;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDieuChinhTonKho;
        private DevExpress.XtraEditors.GroupControl gcDieuChinhTK;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblChonSP;
        private DevExpress.XtraEditors.ComboBoxEdit cboSanPham;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit txtGhiChu;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtSoLuongDC;
        private DevExpress.XtraGrid.Columns.GridColumn colTenSanPham;
        private DevExpress.XtraGrid.Columns.GridColumn colSoluongDC;
        private DevExpress.XtraGrid.Columns.GridColumn colNgayDieuChinh;
        private DevExpress.XtraGrid.Columns.GridColumn colGhiChu;
    }
}