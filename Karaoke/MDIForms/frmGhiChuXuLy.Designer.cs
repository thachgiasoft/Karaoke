namespace Karaoke.MDIForms
{
    partial class frmGhiChuXuLy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGhiChuXuLy));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnThemLoaiXuLy = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtTenGoiNho = new DevExpress.XtraEditors.TextEdit();
            this.txtGhiChu = new DevExpress.XtraEditors.MemoEdit();
            this.cboLoaiXuLy = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtTien = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtTenGoiNho.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGhiChu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoaiXuLy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTien.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tên gợi nhớ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(42, 64);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(46, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Loại xử lý";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 146);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(62, 13);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Ghi chú thêm";
            // 
            // btnThemLoaiXuLy
            // 
            this.btnThemLoaiXuLy.Location = new System.Drawing.Point(12, 264);
            this.btnThemLoaiXuLy.Name = "btnThemLoaiXuLy";
            this.btnThemLoaiXuLy.Size = new System.Drawing.Size(119, 56);
            this.btnThemLoaiXuLy.TabIndex = 5;
            this.btnThemLoaiXuLy.Text = "Thêm loại xử lý";
            this.btnThemLoaiXuLy.Visible = false;
            this.btnThemLoaiXuLy.Click += new System.EventHandler(this.btnThemLoaiXuLy_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 264);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 56);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtTenGoiNho
            // 
            this.txtTenGoiNho.Location = new System.Drawing.Point(109, 22);
            this.txtTenGoiNho.Name = "txtTenGoiNho";
            this.txtTenGoiNho.Size = new System.Drawing.Size(201, 20);
            this.txtTenGoiNho.TabIndex = 0;
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(109, 144);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(201, 96);
            this.txtGhiChu.TabIndex = 3;
            // 
            // cboLoaiXuLy
            // 
            this.cboLoaiXuLy.Location = new System.Drawing.Point(109, 61);
            this.cboLoaiXuLy.Name = "cboLoaiXuLy";
            this.cboLoaiXuLy.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLoaiXuLy.Size = new System.Drawing.Size(201, 20);
            this.cboLoaiXuLy.TabIndex = 1;
            // 
            // txtTien
            // 
            this.txtTien.Location = new System.Drawing.Point(109, 100);
            this.txtTien.Name = "txtTien";
            this.txtTien.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTien.Size = new System.Drawing.Size(201, 20);
            this.txtTien.TabIndex = 2;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(22, 103);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(66, 13);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "Tiền liên quan";
            // 
            // frmGhiChuXuLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 332);
            this.Controls.Add(this.txtTien);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.cboLoaiXuLy);
            this.Controls.Add(this.txtGhiChu);
            this.Controls.Add(this.txtTenGoiNho);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnThemLoaiXuLy);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGhiChuXuLy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ghi chú các xử lý phát sinh";
            this.Load += new System.EventHandler(this.frmGhiChuXuLy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtTenGoiNho.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGhiChu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoaiXuLy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTien.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnThemLoaiXuLy;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtTenGoiNho;
        private DevExpress.XtraEditors.MemoEdit txtGhiChu;
        private DevExpress.XtraEditors.ComboBoxEdit cboLoaiXuLy;
        private DevExpress.XtraEditors.TextEdit txtTien;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}