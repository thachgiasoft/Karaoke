using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BKIT.Entities;
using BKIT.Model;

namespace Karaoke.MDIForms
{
    public partial class frmRoom : DevExpress.XtraEditors.XtraForm
    {
        public frmRoom()
        {
            InitializeComponent();
        }
        private void getAllLoaiPhong()
        {
            DataSet ds = new DataAccess().getAllLoaiPhong();
            gridControlLoaiPhong.DataSource = ds.Tables[0];
        }
        private void AddItemForComboboxGiaLoaiPhong()
        {
            int i, rowcount;
            DataSet ds = new DataAccess().getAllKhunggio();
            repositoryItemComboBox5.Items.Clear();
            rowcount = Convert.ToInt32(ds.Tables[0].Rows.Count);
            for (i = 0; i < rowcount; i++)
                repositoryItemComboBox5.Items.Add(Convert.ToString(ds.Tables[0].Rows[i]["IDKhunggio"]));
            
        }
        private void AddItemForComboboxLoaiphongSPBandau()
        {
            int i, rowcount;
            DataSet ds = new DataAccess().getAllSanPham();
            repositoryItemComboBox6.Items.Clear();
            rowcount = Convert.ToInt32(ds.Tables[0].Rows.Count);
            for (i = 0; i < rowcount; i++)
                repositoryItemComboBox6.Items.Add(Convert.ToString(ds.Tables[0].Rows[i]["TenSanPham"]));

        }
        private void Initialgrid()
        {

            
            DataSet ds2 = new DataAccess().getPhongByIDLoaiPhong(0);
            DataSet ds1 = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(0);
            DataSet ds3 = new DataAccess().getAllKhunggio();
            gridControlPhong.DataSource = ds2.Tables[0];
            gridControlGiaLoaiPhong.DataSource = ds1.Tables[0];
            gridControlKhunggio.DataSource = ds3.Tables[0];
            getSPBandau(0);
            AddItemForComboboxGiaLoaiPhong();
            AddItemForComboboxLoaiphongSPBandau();
        }
        private void frmRoom_Load(object sender, EventArgs e)
        {
            Initialgrid();
            getAllLoaiPhong();
            
        }


        #region LoaiPhong

        private void gridViewLoaiPhong_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteLoaiPhong)
                {
                    //do nothing
                }
                else
                {
                    //update here
                    LoaiPhong objLoaiPhong = new LoaiPhong();
                    objLoaiPhong.IDLoaiPhong = Convert.ToInt32(gridViewLoaiPhong.GetRowCellValue(e.RowHandle, "IDLoaiPhong"));
                    objLoaiPhong.TenLoaiPhong = Convert.ToString(gridViewLoaiPhong.GetRowCellValue(e.RowHandle, "TenLoaiPhong"));
                    if (objLoaiPhong.TenLoaiPhong == "")
                    {
                        getAllLoaiPhong();
                        MessageBox.Show(this, "Không có Tên Loại Phòng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objLoaiPhong.Ghichu = Convert.ToString(gridViewLoaiPhong.GetRowCellValue(e.RowHandle, "Ghichu"));


                    if (new DataAccess().updateLoaiPhong(objLoaiPhong) == true)
                    {

                        getAllLoaiPhong();
                        ((frmMain)(this.MdiParent)).setStatus("Cập nhật dữ liệu Loai phòng thành công");
                    }
                    else
                    {
                        getAllLoaiPhong();
                        MessageBox.Show(this, "Cập nhật dữ liệu Loai phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void gridViewLoaiPhong_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteLoaiPhong)
                {
                    LoaiPhong objLoaiPhong = new LoaiPhong();
                    objLoaiPhong.IDLoaiPhong = Convert.ToInt32(gridViewLoaiPhong.GetRowCellValue(e.RowHandle, "IDLoaiPhong"));

                    if (Convert.ToBoolean(gridViewLoaiPhong.GetRowCellValue(e.RowHandle, colDeleteLoaiPhong)) == true)
                    {
                        //warnning
                        if (MessageBox.Show(this, "Bạn có muốn xóa Loại phòng này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (new DataAccess().deleteLoaiPhong(objLoaiPhong) == true)
                            {
                                ((frmMain)(this.MdiParent)).setStatus("Xóa Loại phòng thành công");
                                gridViewLoaiPhong.DeleteRow(e.RowHandle);
                            }
                            else
                            {
                                MessageBox.Show(this, "Xóa Loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((frmMain)(this.MdiParent)).setStatus("");
                            }
                        }
                        else
                        {
                            //set the image to uncheck
                            gridViewLoaiPhong.SetRowCellValue(e.RowHandle, colDeleteLoaiPhong, true);
                        }
                    }
                }
            }
        }

        private void gridViewLoaiPhong_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataRowView aRowView = (DataRowView)(e.Row);
            DataRow aRow = aRowView.Row;
            if (aRow.RowState == DataRowState.Added)
            {
                //insert command here
                LoaiPhong objLoaiPhong = new LoaiPhong();
                objLoaiPhong.TenLoaiPhong = Convert.ToString(aRow["TenLoaiPhong"]);
                if (objLoaiPhong.TenLoaiPhong == "")
                {
                    getAllLoaiPhong();
                    MessageBox.Show(this, "Không có Tên Loại Phòng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objLoaiPhong.Ghichu = Convert.ToString(aRow["Ghichu"]);


                if (new DataAccess().insertLoaiPhong(objLoaiPhong) >= 0)
                {
                    getAllLoaiPhong();
                    ((frmMain)(this.MdiParent)).setStatus("Thêm mới Loại phòng thành công");

                }
                else
                {
                    getAllLoaiPhong();
                    MessageBox.Show(this, "Thêm mới Loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
        private void gridViewLoaiPhong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && e.PrevFocusedRowHandle >= 0)
            {
                //list all phong by ID
                int ID = Convert.ToInt32(gridViewLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "IDLoaiPhong"));
                string TenLoaiPhong = Convert.ToString(gridViewLoaiPhong.GetRowCellValue(e.FocusedRowHandle, "TenLoaiPhong")).ToUpper();
                curIDLoaiPhong = ID; //update current ID for SanPham processing
                
                try
                {
                    if (TenLoaiPhong == "ALL")
                    {
                        gridControlPhong.DataSource = new DataAccess().getAllPhong().Tables[0];
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getAllGiaLoaiPhong().Tables[0];
                    }
                    else
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    }
                    getSPBandau(curIDLoaiPhong);
                }
                catch// (Exception ex)
                {

                }
                gridViewPhong.NewItemRowText = "Click vào đây để thêm mới";
            }
            else
            {
                //get all products
                //gridControlPhong.DataSource = new DataAccess().getAllPhong().Tables[0];
                //update status
                gridViewPhong.NewItemRowText = "Click vào 1 nhóm sản phẩm";
            }
        }
        #endregion


        #region Phong
        private int curIDLoaiPhong = 0;
        private void gridViewPhong_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
                if (e.RowHandle >= 0)
            {
                if (e.Column == colDeletePhong)
                {
                    //do nothing
                }
                else
                {
                    //update here
                    Phong objPhong = new Phong();
                    objPhong.IDPhong = Convert.ToInt32(gridViewPhong.GetRowCellValue(e.RowHandle, "IDPhong"));
                    objPhong.TenPhong = Convert.ToString(gridViewPhong.GetRowCellValue(e.RowHandle, "TenPhong"));
                    if (objPhong.TenPhong == "")
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Không có tên phòng!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        Int32.Parse(Convert.ToString(gridViewPhong.GetRowCellValue(e.RowHandle, "Congtac")));
                        if (Convert.ToString(gridViewPhong.GetRowCellValue(e.RowHandle, "Congtac")) == "")
                        {
                            gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                            MessageBox.Show(this, "Không nhập công tắc cho phòng!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        objPhong.Congtac = Convert.ToInt32(gridViewPhong.GetRowCellValue(e.RowHandle, "Congtac"));
                        if (new DataAccess().IsCongtacExistedExceptIDPhong(objPhong.Congtac,objPhong.IDPhong))
                        {
                            gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                            MessageBox.Show(this, "Công tắc này đã có, yêu cầu nhập số khác!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (new DataAccess().IsTenPhongExistedExceptIDPhong(objPhong.TenPhong,objPhong.IDPhong))
                        {
                            gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                            MessageBox.Show(this, "Tên phòng này đã có, yêu cầu nhập tên khác!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Giá trị công tắc không hợp lệ (phải nhập số)", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objPhong.IDLoaiPhong = curIDLoaiPhong;
                    objPhong.Ghichu = Convert.ToString(gridViewPhong.GetRowCellValue(e.RowHandle, "Ghichu"));


                    if (new DataAccess().updatePhong(objPhong) == true)
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        ((frmMain)(this.MdiParent)).setStatus("Cập nhật dữ liệu Loai phòng thành công");
                    }
                    else
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Cập nhật dữ liệu Loai phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void gridViewPhong_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeletePhong)
                {
                    Phong objPhong = new Phong();
                    objPhong.IDPhong = Convert.ToInt32(gridViewPhong.GetRowCellValue(e.RowHandle, "IDPhong"));

                    //objPhong.IDLoaiPhong = curIDLoaiPhong;
                    if (Convert.ToBoolean(gridViewPhong.GetRowCellValue(e.RowHandle, colDeletePhong)) == true)
                    {
                        //warnning
                        if (MessageBox.Show(this, "Bạn có muốn xóa Loại phòng này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (new DataAccess().deletePhong(objPhong) == true)
                            {
                                ((frmMain)(this.MdiParent)).setStatus("Xóa Loại phòng thành công");
                                gridViewPhong.DeleteRow(e.RowHandle);
                            }
                            else
                            {
                                MessageBox.Show(this, "Xóa Loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((frmMain)(this.MdiParent)).setStatus("");
                            }
                        }
                        else
                        {
                            //set the image to uncheck
                            gridViewPhong.SetRowCellValue(e.RowHandle, colDeletePhong, true);
                        }
                    }
                }
            }
        }

        private void gridViewPhong_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataRowView aRowView = (DataRowView)(e.Row);
            DataRow aRow = aRowView.Row;
            if (aRow.RowState == DataRowState.Added)
            {
                //insert command here
                Phong objPhong = new Phong();
                objPhong.TenPhong = Convert.ToString(aRow["TenPhong"]);
                if (objPhong.TenPhong == "")
                {
                    gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    MessageBox.Show(this, "Không có tên phòng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Int32.Parse(Convert.ToString(aRow["Congtac"]));
                    if (Convert.ToString(aRow["Congtac"]) == "")
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Không nhập công tắc cho phòng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objPhong.Congtac = Convert.ToInt32(aRow["Congtac"]);
                    if (new DataAccess().IsCongtacExisted(objPhong.Congtac))
                    {
                        gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Công tắc này đã có, yêu cầu nhập số khác!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch
                {
                    gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    MessageBox.Show(this, "Công tắc không phải là số", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objPhong.IDLoaiPhong = curIDLoaiPhong;
                objPhong.Ghichu = Convert.ToString(aRow["Ghichu"]);

                if (new DataAccess().insertPhong(objPhong) >= 0)
                {
                    gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    ((frmMain)(this.MdiParent)).setStatus("Thêm mới Nhóm sản phẩm thành công");
                }
                else
                {
                    gridControlPhong.DataSource = new DataAccess().getPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    MessageBox.Show(this, "Thêm mới Nhóm sản phẩm không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion



        #region GiaLoaiPhong
        private void gridViewGiaLoaiPhong_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteGiaLoaiPhong)
                {
                    //do nothing
                }
                else
                {
                    //update here
                    GiaLoaiPhong objGiaLoaiPhong = new GiaLoaiPhong();
                    objGiaLoaiPhong.IDGiaLoaiPhong = Convert.ToInt32(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "IDGiaLoaiPhong"));
                    objGiaLoaiPhong.Gia = Convert.ToDecimal(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "Gia"));
                    if (Convert.ToString(objGiaLoaiPhong.Gia) == "")
                    {
                        MessageBox.Show(this, "Giá loại phòng không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        return;
                    }
                    else if(objGiaLoaiPhong.Gia < 0)
                    {
                        MessageBox.Show(this, "Giá loại phòng không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        return;
                    }
                    objGiaLoaiPhong.Ngay = Convert.ToDateTime(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "Ngay"));
                   // objGiaLoaiPhong.IDKhunggio = new DataAccess().getIDKhunggiofromTenKhunggio(Convert.ToString(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "IDKhunggio")));
                    objGiaLoaiPhong.IDKhunggio = Convert.ToInt32(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "IDKhunggio"));
                    objGiaLoaiPhong.IDLoaiPhong = curIDLoaiPhong;
                     if (new DataAccess().updateGiaLoaiPhong(objGiaLoaiPhong) == true)
                    {
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        ((frmMain)(this.MdiParent)).setStatus("Cập nhật dữ liệu Loai phòng thành công");
                    }
                    else
                    {
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        MessageBox.Show(this, "Cập nhật dữ liệu Loai phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }


        private void gridViewGiaLoaiPhong_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteGiaLoaiPhong)
                {
                    GiaLoaiPhong objGiaLoaiPhong = new GiaLoaiPhong();
                    objGiaLoaiPhong.IDGiaLoaiPhong = Convert.ToInt32(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, "IDGiaLoaiPhong"));

                    if (Convert.ToBoolean(gridViewGiaLoaiPhong.GetRowCellValue(e.RowHandle, colDeleteGiaLoaiPhong)) == true)
                    {
                        //warnning
                        if (MessageBox.Show(this, "Bạn có muốn xóa Giá loại phòng này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (new DataAccess().deleteGiaLoaiPhong(objGiaLoaiPhong) == true)
                            {
                                ((frmMain)(this.MdiParent)).setStatus("Xóa Loại phòng thành công");
                                gridViewGiaLoaiPhong.DeleteRow(e.RowHandle);
                            }
                            else
                            {
                                MessageBox.Show(this, "Xóa Giá loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((frmMain)(this.MdiParent)).setStatus("");
                            }
                        }
                        else
                        {
                            //set the image to uncheck
                            gridViewGiaLoaiPhong.SetRowCellValue(e.RowHandle, colDeleteGiaLoaiPhong, true);
                        }
                    }
                }
            }
        }

        private void gridViewGiaLoaiPhong_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataRowView aRowView = (DataRowView)(e.Row);
            DataRow aRow = aRowView.Row;
            if (aRow.RowState == DataRowState.Added)
            {
                //insert command here
                GiaLoaiPhong objGiaLoaiPhong = new GiaLoaiPhong();
                try
                {
                    
                    objGiaLoaiPhong.Gia = Convert.ToDecimal(aRow["Gia"]);
                    if (objGiaLoaiPhong.Gia < 0)
                    {
                        MessageBox.Show(this, "Giá loại phòng không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        return;
                    }

                    // check exist khung gio
                    if (new DataAccess().IsGiaLoaiPhongKhungGioExisted(Convert.ToInt16(aRow["IDKhunggio"]),curIDLoaiPhong))
                    {
                        MessageBox.Show(this, "Khung giờ đã có. Hãy chỉnh sửa thông tin của khung giờ muốn sửa đổi ở bên dưới.", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(this, "Giá loại phòng không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    return;
                }
                

                try
                {
                    objGiaLoaiPhong.IDLoaiPhong = curIDLoaiPhong;
                    //objGiaLoaiPhong.IDKhunggio = new DataAccess().getIDKhunggiofromTenKhunggio(Convert.ToString(aRow["IDKhunggio"]));
                    objGiaLoaiPhong.IDKhunggio = Convert.ToInt32(aRow["IDKhunggio"]);
                    objGiaLoaiPhong.Ngay = Convert.ToDateTime(aRow["Ngay"]);
                }
                catch
                {
                    gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    return;
                }
                
                if (new DataAccess().insertGiaLoaiPhong(objGiaLoaiPhong) >= 0)
                {
                    gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];
                    ((frmMain)(this.MdiParent)).setStatus("Thêm mới Giá loại phòng thành công");
                }
                else
                {
                    gridControlGiaLoaiPhong.DataSource = new DataAccess().getGiaLoaiPhongByIDLoaiPhong(curIDLoaiPhong).Tables[0];    
                    MessageBox.Show(this, "Thêm mới Giá loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
        #endregion
        #region Khunggio

        private void gridViewKhunggio_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteKhunggio)
                {
                    //do nothing
                }
                else
                {
                    //update here
                    Khunggio objKhunggio = new Khunggio();
                    objKhunggio.IDKhunggio = Convert.ToInt32(gridViewKhunggio.GetRowCellValue(e.RowHandle, "IDKhunggio"));
                    objKhunggio.Ten = Convert.ToString(gridViewKhunggio.GetRowCellValue(e.RowHandle, "Ten"));
                    if (objKhunggio.Ten == "")
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Không có tên khung giờ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objKhunggio.GioBD = Convert.ToString(gridViewKhunggio.GetRowCellValue(e.RowHandle, "GioBD"));
                    if (objKhunggio.GioBD == "")
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Không có giờ bắt đầu", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    objKhunggio.GioKT = Convert.ToString(gridViewKhunggio.GetRowCellValue(e.RowHandle, "GioKT"));
                    if (objKhunggio.GioKT == "")
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Không có giờ kết thúc", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objKhunggio.Ghichu = Convert.ToString(gridViewKhunggio.GetRowCellValue(e.RowHandle, "Ghichu"));
                    if (new DataAccess().updateKhunggio(objKhunggio) == true)
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        ((frmMain)(this.MdiParent)).setStatus("Cập nhật dữ liệu Khung giờ thành công");
                    }
                    else
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Cập nhật dữ liệu Khung giờ không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void gridViewKhunggio_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteKhunggio)
                {
                    Khunggio objKhunggio = new Khunggio();
                    objKhunggio.IDKhunggio = Convert.ToInt32(gridViewKhunggio.GetRowCellValue(e.RowHandle, "IDKhunggio"));

                    if (Convert.ToBoolean(gridViewKhunggio.GetRowCellValue(e.RowHandle, colDeleteKhunggio)) == true)
                    {
                        //warnning
                        if (MessageBox.Show(this, "Bạn có muốn xóa Khung giờ này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (new DataAccess().deleteKhunggio(objKhunggio) == true)
                            {
                                ((frmMain)(this.MdiParent)).setStatus("Xóa Khung giờ thành công");
                                gridViewKhunggio.DeleteRow(e.RowHandle);
                                AddItemForComboboxGiaLoaiPhong();
                            }
                            else
                            {
                                MessageBox.Show(this, "Xóa Khung giờ không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((frmMain)(this.MdiParent)).setStatus("");
                            }
                        }
                        else
                        {
                            //set the image to uncheck
                            gridViewKhunggio.SetRowCellValue(e.RowHandle, colDeleteKhunggio, true);
                        }
                    }
                }
            }
        }

        private void gridViewKhunggio_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {

            int i, rowcount;
            DataRowView aRowView = (DataRowView)(e.Row);
            DataRow aRow = aRowView.Row;
            if (aRow.RowState == DataRowState.Added)
            {
                //insert command here
                Khunggio objKhunggio = new Khunggio();
                //objKhunggio.IDKhunggio = Convert.ToInt32(aRow["IDKhunggio"]);
                objKhunggio.Ten = Convert.ToString(aRow["Ten"]);
                if (objKhunggio.Ten == "")
                {
                    gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                    MessageBox.Show(this, "Không có tên khung giờ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataSet ds = new DataAccess().getAllKhunggio();
                rowcount = Convert.ToInt32(ds.Tables[0].Rows.Count);
                for (i = 0; i < rowcount; i++)
                {
                    if (objKhunggio.Ten == Convert.ToString(ds.Tables[0].Rows[i]["Ten"]))
                    {
                        MessageBox.Show(this, "Khung giờ trùng tên", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        return;
                    }
                }
                try
                {
                    objKhunggio.GioBD = Convert.ToString(aRow["GioBD"]);
                    if (objKhunggio.GioBD == "")
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Không có giờ bắt đầu", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objKhunggio.GioKT = Convert.ToString(aRow["GioKT"]);
                    if (objKhunggio.GioKT == "")
                    {
                        gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                        MessageBox.Show(this, "Không có giờ kết thúc", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    objKhunggio.Ghichu = Convert.ToString(aRow["Ghichu"]);
                }
                catch
                {
                    gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                    return;
                }

                if (new DataAccess().insertKhunggio(objKhunggio) >= 0)
                {
                    gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                    ((frmMain)(this.MdiParent)).setStatus("Thêm mới Khung giờ thành công");
                    AddItemForComboboxGiaLoaiPhong();
                }
                else
                {
                    gridControlKhunggio.DataSource = new DataAccess().getAllKhunggio().Tables[0];
                    MessageBox.Show(this, "Thêm mới Khung giờ không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
        #endregion

        #region LoaiphongSPBandau
        private void getSPBandau(int IDLoaiPhong)
        {
            DataSet ds = new DataAccess().getAllSPBandauIDLoaiPhong(IDLoaiPhong);
            gridLoaiphongSPBandau.DataSource = ds.Tables[0];
        }
        private void gridViewLPSPBD_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteSPBD)
                {
                    LoaiphongSPBandau objLoaiPhong = new LoaiphongSPBandau();
                    objLoaiPhong.IDLPSP = Convert.ToInt32(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "IDLPSP"));

                    if (Convert.ToBoolean(gridViewLPSPBD.GetRowCellValue(e.RowHandle, colDeleteSPBD)) == true)
                    {
                        //warnning
                        if (MessageBox.Show(this, "Bạn có muốn xóa Sản phẩm ban đầu này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (new DataAccess().deleteLoaiphongSPBandau(objLoaiPhong) == true)
                            {
                                ((frmMain)(this.MdiParent)).setStatus("Xóa Sản phẩm ban đầu thành công");
                                gridViewLPSPBD.DeleteRow(e.RowHandle);
                            }
                            else
                            {
                                MessageBox.Show(this, "Xóa Sản phẩm ban đầu không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((frmMain)(this.MdiParent)).setStatus("");
                            }
                        }
                        else
                        {
                            //set the image to uncheck
                            gridViewLPSPBD.SetRowCellValue(e.RowHandle, colDeleteSPBD, true);
                        }
                    }
                }
            }
        }
        private void gridViewLPSPBD_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteSPBD)
                {
                    //do nothing
                }
                else
                {
                    //update here
                    LoaiphongSPBandau objLoaiPhong = new LoaiphongSPBandau();
                    objLoaiPhong.IDLPSP = Convert.ToInt32(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "IDLPSP"));
                    objLoaiPhong.IDLoaiPhong = Convert.ToInt32(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "IDLoaiPhong"));
                    objLoaiPhong.Soluong = Convert.ToInt32(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "Soluong"));
                    objLoaiPhong.Ghichu = Convert.ToString(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "Ghichu"));
                    DataSet dsSP = new DataAccess().getSanPhamByTenSP(Convert.ToString(gridViewLPSPBD.GetRowCellValue(e.RowHandle, "TenSanPham")));
                    try{
                        objLoaiPhong.IDSanPham = Convert.ToInt32(dsSP.Tables[0].Rows[0]["IDSanPham"]);
                    }
                    catch{
                        objLoaiPhong.IDSanPham = -1;
                    }

                    if ((objLoaiPhong.IDSanPham == -1)||(objLoaiPhong.Soluong <=0))
                    {
                        getSPBandau(curIDLoaiPhong);
                        MessageBox.Show(this, "Thông tin sản phẩm không hợp lệ (không có sản phẩm cùng tên hoặc số lượng <= 0)", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    if (new DataAccess().updateLoaiphongSPBandau(objLoaiPhong) == true)
                    {

                        getSPBandau(curIDLoaiPhong);
                        ((frmMain)(this.MdiParent)).setStatus("Cập nhật dữ liệu Loai phòng thành công");
                    }
                    else
                    {
                        getSPBandau(curIDLoaiPhong);
                        MessageBox.Show(this, "Cập nhật dữ liệu Loai phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void gridViewLPSPBD_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataRowView aRowView = (DataRowView)(e.Row);
            DataRow aRow = aRowView.Row;
            if (aRow.RowState == DataRowState.Added)
            {
                //insert command here
                LoaiphongSPBandau objLoaiPhong = new LoaiphongSPBandau();
                objLoaiPhong.IDLoaiPhong = curIDLoaiPhong;
                objLoaiPhong.Soluong = Convert.ToInt32(aRow["Soluong"]);
                objLoaiPhong.Ghichu = Convert.ToString(aRow["Ghichu"]);
                DataSet dsSP = new DataAccess().getSanPhamByTenSP(Convert.ToString(aRow["TenSanPham"]));
                    try{
                        objLoaiPhong.IDSanPham = Convert.ToInt32(dsSP.Tables[0].Rows[0]["IDSanPham"]);
                    }
                    catch{
                        objLoaiPhong.IDSanPham = -1;
                    }
                    if ((objLoaiPhong.IDSanPham == -1) || (objLoaiPhong.Soluong <= 0))
                    {
                        getSPBandau(curIDLoaiPhong);
                        MessageBox.Show(this, "Thông tin sản phẩm không hợp lệ (không có sản phẩm cùng tên hoặc số lượng <= 0)", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                if (new DataAccess().insertLoaiphongSPBandau(objLoaiPhong) >= 0)
                {
                    getSPBandau(curIDLoaiPhong);
                    ((frmMain)(this.MdiParent)).setStatus("Thêm mới Loại phòng thành công");

                }
                else
                {
                    getSPBandau(curIDLoaiPhong);
                    MessageBox.Show(this, "Thêm mới Loại phòng không thành công", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
        #endregion LoaiphongSPBandau

     
        

        

        


    }
}