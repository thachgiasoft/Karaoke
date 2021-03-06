using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BKIT.Model;

namespace Karaoke.MDIForms
{
    public partial class frmChart : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dtSelectedProduct = new DataTable();
        private DataTable dtselectedRoom = new DataTable();
        private string[] listofRoomName = null;
        private int[] listofRoomID = null;
        private DataTable dtChartData = new DataTable();
        private string[] listofProductName = null;
        private int[] listofProductID = null;
        public frmChart()
        {
            InitializeComponent();
            myInit();
        }
        private void myInit()
        {
            DataColumn dr1 = new DataColumn("IDSanPham", Type.GetType("System.String"));
            DataColumn dr2 = new DataColumn("TenSanPham", Type.GetType("System.String"));
            DataColumn dr3 = new DataColumn("DVT", Type.GetType("System.String"));
            DataColumn dr4 = new DataColumn("Delete", Type.GetType("System.String"));
            dtSelectedProduct.Columns.Add(dr1);
            dtSelectedProduct.Columns.Add(dr2);
            dtSelectedProduct.Columns.Add(dr3);
            dtSelectedProduct.Columns.Add(dr4);
            DataColumn dr5 = new DataColumn("IDPhong", Type.GetType("System.String"));
            DataColumn dr6 = new DataColumn("TenPhong", Type.GetType("System.String"));
            DataColumn dr7 = new DataColumn("Ghichu", Type.GetType("System.String"));
            DataColumn dr8 = new DataColumn("Delete", Type.GetType("System.String"));
            dtselectedRoom.Columns.Add(dr5);
            dtselectedRoom.Columns.Add(dr6);
            dtselectedRoom.Columns.Add(dr7);
            dtselectedRoom.Columns.Add(dr8);
        }
        private void frmChart_Load(object sender, EventArgs e)
        {

            DataSet ds = new DataAccess().getAllSanPham();
            gridControlSanPham.DataSource = ds.Tables[0];
            DataSet ds1 = new DataAccess().getAllPhong();
            gridRoom.DataSource = ds1.Tables[0];
        }
        #region SanPhamBan
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gridViewSanPham.FocusedRowHandle >= 0)
            {
                bool isAdded = false;
                string iCurrentProductID = Convert.ToString(gridViewSanPham.GetRowCellValue(gridViewSanPham.FocusedRowHandle, "IDSanPham"));
                string currentProductName = Convert.ToString(gridViewSanPham.GetRowCellValue(gridViewSanPham.FocusedRowHandle, "TenSanPham"));
                if (dtSelectedProduct != null && dtSelectedProduct.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelectedProduct.Rows.Count; i++)
                    {
                        if (iCurrentProductID == Convert.ToString(dtSelectedProduct.Rows[i]["IDSanPham"]) ||
                            currentProductName == Convert.ToString(dtSelectedProduct.Rows[i]["TenSanPham"]))
                        {
                            isAdded = true;
                            break;
                        }
                    }
                }
                if (!isAdded)
                {
                    string tenSanPham = Convert.ToString(gridViewSanPham.GetRowCellValue(gridViewSanPham.FocusedRowHandle, "TenSanPham"));
                    string DVT = Convert.ToString(gridViewSanPham.GetRowCellValue(gridViewSanPham.FocusedRowHandle, "DVT"));
                    DataRow dr = dtSelectedProduct.NewRow();
                    dr["IDSanPham"] = iCurrentProductID.ToString();
                    dr["TenSanPham"] = tenSanPham;
                    dr["DVT"] = DVT;
                    dr["Delete"] = "1";
                    dtSelectedProduct.Rows.Add(dr);
                    gridSelectedProduct.DataSource = dtSelectedProduct;
                }
            }
        }

        private void gridViewSanPham_DoubleClick(object sender, EventArgs e)
        {
            btnAdd_Click(null, null);
        }

        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {
            lblFromByDate.Enabled = !rdMonth.Checked;
            dtFromByDate.Enabled = !rdMonth.Checked;
            lblToByDate.Enabled = !rdMonth.Checked;
            dtToByDate.Enabled = !rdMonth.Checked;
            lblMonthByMonth.Enabled = rdMonth.Checked;
            cbMonthByMonth.Enabled = rdMonth.Checked;
            lblYearByMonth.Enabled = rdMonth.Checked;
            cbYearByMonth.Enabled = rdMonth.Checked;
            lblToMonthByMonth.Enabled = rdMonth.Checked;
            cbToMonthByMonth.Enabled = rdMonth.Checked;
            lblToYearByMonth.Enabled = rdMonth.Checked;
            cbToYearByMonth.Enabled = rdMonth.Checked;
        }

        private void rdDate_CheckedChanged(object sender, EventArgs e)
        {
            lblFromByDate.Enabled = rdDate.Checked;
            dtFromByDate.Enabled = rdDate.Checked;
            lblToByDate.Enabled = rdDate.Checked;
            dtToByDate.Enabled = rdDate.Checked;
            lblMonthByMonth.Enabled = !rdDate.Checked;
            cbMonthByMonth.Enabled = !rdDate.Checked;
            lblYearByMonth.Enabled = !rdDate.Checked;
            cbYearByMonth.Enabled = !rdDate.Checked;
            lblToMonthByMonth.Enabled = !rdDate.Checked;
            cbToMonthByMonth.Enabled = !rdDate.Checked;
            lblToYearByMonth.Enabled = !rdDate.Checked;
            cbToYearByMonth.Enabled = !rdDate.Checked;
        }

        private void gridViewBillProduct_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteCTHD)
                {
                    dtSelectedProduct.Rows.RemoveAt(e.RowHandle);
                    gridSelectedProduct.DataSource = dtSelectedProduct;
                }

            }
        }

        private void btnViewChart_Click(object sender, EventArgs e)
        {
            if (rdDate.Checked)
            {//By Date
                if (dtSelectedProduct == null || dtSelectedProduct.Rows.Count <= 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần khảo sát. Vui lòng chọn một vài sản phẩm cần " +
                        "vẽ biều đồ ở khung loại sản phẩm!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime dtFrom = dtFromByDate.Value;
                DateTime dtTo = dtToByDate.Value;
                if (DateTime.Compare(dtFrom, dtTo) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    TimeSpan ts = dtTo - dtFrom;
                    string stridProduct = "";
                    int i = 0, j = 0;
                    DateTime dtCurrent = dtFrom;
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    if (listofProductName != null)
                        listofProductName = null;
                    if (listofProductID != null)
                        listofProductID = null;
                    listofProductName = new string[dtSelectedProduct.Rows.Count];
                    listofProductID = new int[dtSelectedProduct.Rows.Count];
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    for (i = 0; i < dtSelectedProduct.Rows.Count; i++)
                    {
                        DataColumn dc = new DataColumn(Convert.ToString(dtSelectedProduct.Rows[i]["TenSanPham"]),
                            Type.GetType("System.String"));
                        dtChartData.Columns.Add(dc);
                        listofProductName[i] = Convert.ToString(dtSelectedProduct.Rows[i]["TenSanPham"]);
                        listofProductID[i] = Convert.ToInt32(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        if(i == 0){
                            stridProduct += " and (";
                            stridProduct += " SanPham.IDSanPham = " + Convert.ToString(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        }else{
                            stridProduct += " or " + " SanPham.IDSanPham = " + Convert.ToString(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        }
                        if(i == dtSelectedProduct.Rows.Count -1){
                            stridProduct += ")";
                        }

                    }
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= ts.Days; j++)
                    {
                        dtCurrent = dtFrom.AddDays(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year + 
                            " and Month(Ngayxuat) = " + dtCurrent.Month +
                                " and Day(Ngayxuat) = " + dtCurrent.Day + " ";
                        strQuery = "Select SanPham.IDSanPham, SanPham.TenSanPham, SanPham.DVT, " +
                            "Hoadonxuat.Ngayxuat, Sum(soluong) as TongSoluong " +
                            "From SanPham, ChitietHDXuat, Hoadonxuat " +
                            "Where SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                            "ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat " +
                            stridProduct + strWheredate +
                            "Group by SanPham.IDSanPham, SanPham.TenSanPham, " +
                            "SanPham.DVT, Hoadonxuat.Ngayxuat " +
                            "ORDER BY Hoadonxuat.Ngayxuat";
                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/dd/yyyy");
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                string colName = Convert.ToString(dsTemp.Tables[0].Rows[i]["TenSanPham"]);
                                dr[colName] = Convert.ToString(dsTemp.Tables[0].Rows[i]["TongSoluong"]);
                            }
                            dtChartData.Rows.Add(dr);
                        }
                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByDate", listofProductName);
                        ch.ShowDialog();
                    }
                }
            }
            else if (rdMonth.Checked)
            {//By Month
                if (dtSelectedProduct == null || dtSelectedProduct.Rows.Count <= 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần khảo sát. Vui lòng chọn một vài sản phẩm cần " +
                        "vẽ biều đồ ở khung loại sản phẩm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(cbMonthByMonth.Text == "" || cbToMonthByMonth.Text == "" ||
                    cbYearByMonth.Text == "" || cbToYearByMonth.Text == ""){
                    MessageBox.Show("Bạn chưa thông tin tháng năm. Vui lòng chọn tháng bắt đầu và tháng kết thúc ",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime dtFrom = dtFromByDate.Value;
                DateTime dtTo = dtToByDate.Value;
                int startMonth = Convert.ToInt32(cbMonthByMonth.Text);
                int endMondth = Convert.ToInt32(cbToMonthByMonth.Text);
                int startYear = Convert.ToInt32(cbYearByMonth.Text);
                int endYear = Convert.ToInt32(cbToYearByMonth.Text);
                DateTime start = new DateTime(startYear, startMonth, 1);
                DateTime end = new DateTime(endYear, endMondth, 1);
                if (DateTime.Compare(start, end) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    string stridProduct = "";
                    int i = 0, j = 0;
                    DateTime dtCurrent = start;
                    int diff = (endYear - startYear)*12 + (endMondth - startMonth);
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    if (listofProductName != null)
                        listofProductName = null;
                    if (listofProductID != null)
                        listofProductID = null;
                    listofProductName = new string[dtSelectedProduct.Rows.Count];
                    listofProductID = new int[dtSelectedProduct.Rows.Count];
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    for (i = 0; i < dtSelectedProduct.Rows.Count; i++)
                    {
                        DataColumn dc = new DataColumn(Convert.ToString(dtSelectedProduct.Rows[i]["TenSanPham"]),
                            Type.GetType("System.String"));
                        dtChartData.Columns.Add(dc);
                        listofProductName[i] = Convert.ToString(dtSelectedProduct.Rows[i]["TenSanPham"]);
                        listofProductID[i] = Convert.ToInt32(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        if (i == 0)
                        {
                            stridProduct += " and (";
                            stridProduct += " SanPham.IDSanPham = " + Convert.ToString(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        }
                        else
                        {
                            stridProduct += " or " + " SanPham.IDSanPham = " + Convert.ToString(dtSelectedProduct.Rows[i]["IDSanPham"]);
                        }
                        if (i == dtSelectedProduct.Rows.Count - 1)
                        {
                            stridProduct += ")";
                        }

                    }
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= diff; j++)
                    {
                        dtCurrent = start.AddMonths(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year +
                            " and Month(Ngayxuat) = " + dtCurrent.Month + " ";
                        strQuery = "Select SanPham.IDSanPham, SanPham.TenSanPham, SanPham.DVT, " +
                            "Hoadonxuat.Ngayxuat, Sum(soluong) as TongSoluong " +
                            "From SanPham, ChitietHDXuat, Hoadonxuat " +
                            "Where SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                            "ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat " +
                            stridProduct + strWheredate +
                            "Group by SanPham.IDSanPham, SanPham.TenSanPham, " +
                            "SanPham.DVT, Hoadonxuat.Ngayxuat " +
                            "ORDER BY Hoadonxuat.Ngayxuat";
                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/yyyy");
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                string colName = Convert.ToString(dsTemp.Tables[0].Rows[i]["TenSanPham"]);
                                dr[colName] = Convert.ToString(dsTemp.Tables[0].Rows[i]["TongSoluong"]);
                            }
                            dtChartData.Rows.Add(dr);
                        }
                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByMonth", listofProductName);
                        ch.ShowDialog();
                    }
                }
            }
        }
        #endregion
        #region Phong
        private void rdRoomDate_CheckedChanged(object sender, EventArgs e)
        {
            lblRoomByDateFrom.Enabled = rdRoomDate.Checked;
            dtRoomByDateFrom.Enabled = rdRoomDate.Checked;
            lblRoomByDateTo.Enabled = rdRoomDate.Checked;
            dtRoomByDateTo.Enabled = rdRoomDate.Checked;
            lblRoomByMonthFromMonth.Enabled = !rdRoomDate.Checked;
            cbRoomByMonthFromMonth.Enabled = !rdRoomDate.Checked;
            lblRoomByMonthFromYear.Enabled = !rdRoomDate.Checked;
            cbRoomByMonthFromYear.Enabled = !rdRoomDate.Checked;
            lblRoomByMonthToMonth.Enabled = !rdRoomDate.Checked;
            cbRoomByMonthToMonth.Enabled = !rdRoomDate.Checked;
            lblRoomByMonthToYear.Enabled = !rdRoomDate.Checked;
            cbRoomByMonthToYear.Enabled = !rdRoomDate.Checked;
        }

        private void rdRoomMonth_CheckedChanged(object sender, EventArgs e)
        {
            lblRoomByDateFrom.Enabled = !rdRoomMonth.Checked;
            dtRoomByDateFrom.Enabled = !rdRoomMonth.Checked;
            lblRoomByDateTo.Enabled = !rdRoomMonth.Checked;
            dtRoomByDateTo.Enabled = !rdRoomMonth.Checked;
            lblRoomByMonthFromMonth.Enabled = rdRoomMonth.Checked;
            cbRoomByMonthFromMonth.Enabled = rdRoomMonth.Checked;
            lblRoomByMonthFromYear.Enabled = rdRoomMonth.Checked;
            cbRoomByMonthFromYear.Enabled = rdRoomMonth.Checked;
            lblRoomByMonthToMonth.Enabled = rdRoomMonth.Checked;
            cbRoomByMonthToMonth.Enabled = rdRoomMonth.Checked;
            lblRoomByMonthToYear.Enabled = rdRoomMonth.Checked;
            cbRoomByMonthToYear.Enabled = rdRoomMonth.Checked;
        }
        

        private void btnRoomAdd_Click(object sender, EventArgs e)
        {
            if (gridViewRoom.FocusedRowHandle >= 0)
            {
                bool isAdded = false;
                string iCurrentProductID = Convert.ToString(gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "IDPhong"));
                string currentProductName = Convert.ToString(gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "TenPhong"));
                if (dtselectedRoom != null && dtselectedRoom.Rows.Count > 0)
                {
                    for (int i = 0; i < dtselectedRoom.Rows.Count; i++)
                    {
                        if (iCurrentProductID == Convert.ToString(dtselectedRoom.Rows[i]["IDPhong"]) ||
                            currentProductName == Convert.ToString(dtselectedRoom.Rows[i]["TenPhong"]))
                        {
                            isAdded = true;
                            break;
                        }
                    }
                }
                if (!isAdded)
                {
                    string tenPhong = "";
                    if (gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "TenPhong") is DBNull)
                        tenPhong = "";
                    else
                        tenPhong = Convert.ToString(gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "TenPhong"));
                    string ghiChu = "";
                    if (gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "Ghichu") is DBNull)
                        ghiChu = "";
                    else
                        ghiChu = Convert.ToString(gridViewRoom.GetRowCellValue(gridViewRoom.FocusedRowHandle, "Ghichu"));
                    DataRow dr = dtselectedRoom.NewRow();
                    dr["IDPhong"] = iCurrentProductID.ToString();
                    dr["TenPhong"] = tenPhong;
                    dr["Ghichu"] = ghiChu;
                    dr["Delete"] = "1";
                    dtselectedRoom.Rows.Add(dr);
                    gridRoomSelected.DataSource = dtselectedRoom;
                }
            }
        }

        private void gridViewRoom_DoubleClick(object sender, EventArgs e)
        {
            btnRoomAdd_Click(null, null);
        }

        private void gridViewRoomSelected_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colDeleteSelectedRoom)
                {
                    dtselectedRoom.Rows.RemoveAt(e.RowHandle);
                    gridRoomSelected.DataSource = dtselectedRoom;
                }

            }
        }
        
        private void cbRoomViewChart_Click(object sender, EventArgs e)
        {
            if (rdRoomDate.Checked)
            {//By Date
                if (dtselectedRoom == null || dtselectedRoom.Rows.Count <= 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần khảo sát. Vui lòng chọn một vài sản phẩm cần " +
                        "vẽ biều đồ ở khung loại sản phẩm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime dtFrom = dtRoomByDateFrom.Value;
                DateTime dtTo = dtRoomByDateTo.Value;
                if (DateTime.Compare(dtFrom, dtTo) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    TimeSpan ts = dtTo - dtFrom;
                    string stridRoom = "";
                    int i = 0, j = 0;
                    DateTime dtCurrent = dtFrom;
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    if (listofRoomName != null)
                        listofRoomName = null;
                    if (listofRoomID != null)
                        listofRoomID = null;
                    listofRoomName = new string[dtselectedRoom.Rows.Count];
                    listofRoomID = new int[dtselectedRoom.Rows.Count];
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    for (i = 0; i < dtselectedRoom.Rows.Count; i++)
                    {
                        DataColumn dc = new DataColumn(Convert.ToString(dtselectedRoom.Rows[i]["TenPhong"]),
                            Type.GetType("System.String"));
                        dtChartData.Columns.Add(dc);
                        listofRoomName[i] = Convert.ToString(dtselectedRoom.Rows[i]["TenPhong"]);
                        listofRoomID[i] = Convert.ToInt32(dtselectedRoom.Rows[i]["IDPhong"]);
                        if (i == 0)
                        {
                            stridRoom += " and (";
                            stridRoom += " Phong.IDPhong = " + Convert.ToString(dtselectedRoom.Rows[i]["IDPhong"]);
                        }
                        else
                        {
                            stridRoom += " or " + " Phong.IDPhong = " + Convert.ToString(dtselectedRoom.Rows[i]["IDPhong"]);
                        }
                        if (i == dtselectedRoom.Rows.Count - 1)
                        {
                            stridRoom += ")";
                        }

                    }
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= ts.Days; j++)
                    {
                        dtCurrent = dtFrom.AddDays(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year +
                            " and Month(Ngayxuat) = " + dtCurrent.Month +
                                " and Day(Ngayxuat) = " + dtCurrent.Day + " ";

                        strQuery =
                            "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong, Ngayxuat, " +
                            "GioBD, GioKT, TenPhong " +
                            " From  Hoadonxuat, Phong" +
                            " Where Hoadonxuat.IDPhong = Phong.IDPhong " + stridRoom + strWheredate;
                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/dd/yyyy");
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                string colName = Convert.ToString(dsTemp.Tables[0].Rows[i]["TenPhong"]);

                                DateTime dtBD = Convert.ToDateTime(dsTemp.Tables[0].Rows[i]["GioBD"].ToString());
                                DateTime dtKT = Convert.ToDateTime(dsTemp.Tables[0].Rows[i]["GioKT"].ToString());
                                TimeSpan dif = dtKT - dtBD;
                                Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;

                                if (!(dr[colName] is DBNull))
                                    soluongGio += Convert.ToDecimal(dr[colName]);
                                dr[colName] = soluongGio.ToString("###,###,###,##0.##");
                            }
                            dtChartData.Rows.Add(dr);
                        }
                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByDate", listofRoomName);
                        ch.ShowDialog();
                    }
                }
            }
            else if (rdRoomMonth.Checked)
            {//By Month
                if (dtselectedRoom == null || dtselectedRoom.Rows.Count <= 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần khảo sát. Vui lòng chọn một vài sản phẩm cần " +
                        "vẽ biều đồ ở khung loại sản phẩm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbRoomByMonthFromMonth.Text == "" || cbRoomByMonthToMonth.Text == "" ||
                    cbRoomByMonthFromYear.Text == "" || cbRoomByMonthToYear.Text == "")
                {
                    MessageBox.Show("Bạn chưa thông tin tháng năm. Vui lòng chọn tháng bắt đầu và tháng kết thúc ",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int startMonth = Convert.ToInt32(cbRoomByMonthFromMonth.Text);
                int endMondth = Convert.ToInt32(cbRoomByMonthToMonth.Text);
                int startYear = Convert.ToInt32(cbRoomByMonthFromYear.Text);
                int endYear = Convert.ToInt32(cbRoomByMonthToYear.Text);
                DateTime start = new DateTime(startYear, startMonth, 1);
                DateTime end = new DateTime(endYear, endMondth, 1);
                if (DateTime.Compare(start, end) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    DateTime dtCurrent = start;
                    int diff = (endYear - startYear) * 12 + (endMondth - startMonth);

                    string stridRoom = "";
                    int i = 0, j = 0;
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    if (listofRoomName != null)
                        listofRoomName = null;
                    if (listofRoomID != null)
                        listofRoomID = null;
                    listofRoomName = new string[dtselectedRoom.Rows.Count];
                    listofRoomID = new int[dtselectedRoom.Rows.Count];
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    for (i = 0; i < dtselectedRoom.Rows.Count; i++)
                    {
                        DataColumn dc = new DataColumn(Convert.ToString(dtselectedRoom.Rows[i]["TenPhong"]),
                            Type.GetType("System.String"));
                        dtChartData.Columns.Add(dc);
                        listofRoomName[i] = Convert.ToString(dtselectedRoom.Rows[i]["TenPhong"]);
                        listofRoomID[i] = Convert.ToInt32(dtselectedRoom.Rows[i]["IDPhong"]);
                        if (i == 0)
                        {
                            stridRoom += " and (";
                            stridRoom += " Phong.IDPhong = " + Convert.ToString(dtselectedRoom.Rows[i]["IDPhong"]);
                        }
                        else
                        {
                            stridRoom += " or " + " Phong.IDPhong = " + Convert.ToString(dtselectedRoom.Rows[i]["IDPhong"]);
                        }
                        if (i == dtselectedRoom.Rows.Count - 1)
                        {
                            stridRoom += ")";
                        }

                    }
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= diff; j++)
                    {
                        dtCurrent = start.AddMonths(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year +
                            " and Month(Ngayxuat) = " + dtCurrent.Month + " ";

                        strQuery =
                            "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong, Ngayxuat, " +
                            "GioBD, GioKT, TenPhong " +
                            " From  Hoadonxuat, Phong" +
                            " Where Hoadonxuat.IDPhong = Phong.IDPhong " + stridRoom + strWheredate;
                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/yyyy");
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                string colName = Convert.ToString(dsTemp.Tables[0].Rows[i]["TenPhong"]);

                                DateTime dtBD = Convert.ToDateTime(dsTemp.Tables[0].Rows[i]["GioBD"].ToString());
                                DateTime dtKT = Convert.ToDateTime(dsTemp.Tables[0].Rows[i]["GioKT"].ToString());
                                TimeSpan dif = dtKT - dtBD;
                                Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;

                                if (!(dr[colName] is DBNull))
                                    soluongGio += Convert.ToDecimal(dr[colName]);
                                dr[colName] = soluongGio.ToString("###,###,###,##0.##");
                            }
                            dtChartData.Rows.Add(dr);
                        }
                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByMonth", listofRoomName);
                        ch.ShowDialog();
                    }
                }
            }
        }
        #endregion
        #region Income
        private void rdInComeMonth_CheckedChanged(object sender, EventArgs e)
        {
            lblInComeByDateFromDate.Enabled = !rdInComeMonth.Checked;
            dtInComeByDateFromDate.Enabled = !rdInComeMonth.Checked;
            lblInComeByDateToDate.Enabled = !rdInComeMonth.Checked;
            dtInComeByDateToDate.Enabled = !rdInComeMonth.Checked;
            lblInComeByMonthFromMonth.Enabled = rdInComeMonth.Checked;
            cbInComeByMonthFromMonth.Enabled = rdInComeMonth.Checked;
            lblInComeByMonthFromYear.Enabled = rdInComeMonth.Checked;
            cbInComeByMonthFromYear.Enabled = rdInComeMonth.Checked;
            lblInComeByMonthToMonth.Enabled = rdInComeMonth.Checked;
            cbInComeByMonthToMonth.Enabled = rdInComeMonth.Checked;
            lblInComeByMonthToYear.Enabled = rdInComeMonth.Checked;
            cbInComeByMonthToYear.Enabled = rdInComeMonth.Checked;
        }

        private void rdInComeDate_CheckedChanged(object sender, EventArgs e)
        {
            lblInComeByDateFromDate.Enabled = rdInComeDate.Checked;
            dtInComeByDateFromDate.Enabled = rdInComeDate.Checked;
            lblInComeByDateToDate.Enabled = rdInComeDate.Checked;
            dtInComeByDateToDate.Enabled = rdInComeDate.Checked;
            lblInComeByMonthFromMonth.Enabled = !rdInComeDate.Checked;
            cbInComeByMonthFromMonth.Enabled = !rdInComeDate.Checked;
            lblInComeByMonthFromYear.Enabled = !rdInComeDate.Checked;
            cbInComeByMonthFromYear.Enabled = !rdInComeDate.Checked;
            lblInComeByMonthToMonth.Enabled = !rdInComeDate.Checked;
            cbInComeByMonthToMonth.Enabled = !rdInComeDate.Checked;
            lblInComeByMonthToYear.Enabled = !rdInComeDate.Checked;
            cbInComeByMonthToYear.Enabled = !rdInComeDate.Checked;
        }

        private void btnInComeViewChart_Click(object sender, EventArgs e)
        {
            if (rdInComeDate.Checked)
            {//By Date
                DateTime dtFrom = dtInComeByDateFromDate.Value;
                DateTime dtTo = dtInComeByDateToDate.Value;
                if (DateTime.Compare(dtFrom, dtTo) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    TimeSpan ts = dtTo - dtFrom;
                    //string stridProduct = "";
                    int i = 0, j = 0;
                    DateTime dtCurrent = dtFrom;
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    //if (listofProductName != null)
                    //    listofProductName = null;
                    //if (listofProductID != null)
                    //    listofProductID = null;
                    //listofProductName = new string[dtSelectedProduct.Rows.Count];
                    //listofProductID = new int[dtSelectedProduct.Rows.Count];
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    DataColumn dcTT = new DataColumn("ThanhTien", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcTT);
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= ts.Days; j++)
                    {
                        dtCurrent = dtFrom.AddDays(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year +
                            " and Month(Ngayxuat) = " + dtCurrent.Month +
                                " and Day(Ngayxuat) = " + dtCurrent.Day + " ";

                        DataSet dsTP_PT = null;
                        if (chkInComePhuThu_TienPhong.Checked)
                        {
                            string strQueryPhong_PT =
                                "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong, Ngayxuat As NgayBan, " +
                                "Phuthu, GioBD, GioKT, GiaLoaiPhong.Gia, TenPhong As TenSanPham" +
                                " From  Hoadonxuat, Phong, GiaLoaiPhong" +
                                " Where Hoadonxuat.IDPhong = Phong.IDPhong and " +
                                " Hoadonxuat.IDGiaLoaiPhong = GiaLoaiPhong.IDGiaLoaiPhong " + strWheredate;
                            dsTP_PT = (DataSet)(da.getDataByQuery(strQueryPhong_PT));
                        }
                        string subQuerySP = "Select Hoadonxuat.IDHoadonXuat, Ngayxuat As NgayBan, SanPham.IDSanPham, TenSanPham, Soluong, " +
                            "DVT, Hoadonxuat.Giam, Max(NgayXuatSP) as NgayXuatSP1 " +
                            "From Hoadonxuat, ChitietHDXuat,  SanPham, NhomSP, GiaXuatSP " +
                            "Where ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat and " +
                            "NhomSP.IDNhomSP = SanPham.IDNhomSP and " +
                            "SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                            "SanPham.IDSanPham = GiaXuatSP.IDSanPham " + strWheredate +
                            " and (Year(NgayXuatSP) < Year(Ngayxuat) or " +
                            "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) < Month(Ngayxuat)) or " +
                            "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) = Month(Ngayxuat) and Day(NgayXuatSP) <= Day(Ngayxuat)))" +
                            " GROUP BY Hoadonxuat.IDHoadonXuat, Ngayxuat, SanPham.IDSanPham, TenSanPham, Soluong, " +
                            "DVT, Hoadonxuat.Giam ";
                        strQuery = "Select T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                            "T.DVT, T.Giam, T.NgayXuatSP1, Max(Gia) as Gia " +
                            "From GiaXuatSP, (" + subQuerySP + ") as T " +
                            "Where T.IDSanPham = GiaXuatSP.IDSanPham and " +
                            "T.NgayXuatSP1 = GiaXuatSP.NgayXuatSP " +
                            "GROUP BY T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                            "T.DVT, T.Giam, T.NgayXuatSP1";
                        //dsSP = (DataSet)(da.getDataByQuery(strQuery));
                        //strQuery = "Select SanPham.IDSanPham, SanPham.TenSanPham, SanPham.DVT, " +
                        //    "Hoadonxuat.Ngayxuat, Sum(soluong) as TongSoluong " +
                        //    "From SanPham, ChitietHDXuat, Hoadonxuat " +
                        //    "Where SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                        //    "ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat " +
                        //    strWheredate +
                        //    "Group by SanPham.IDSanPham, SanPham.TenSanPham, " +
                        //    "SanPham.DVT, Hoadonxuat.Ngayxuat " +
                        //    "ORDER BY Hoadonxuat.Ngayxuat";
                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/dd/yyyy");
                        /////////////////////////////
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                Decimal soluong = Convert.ToDecimal(dsTemp.Tables[0].Rows[i]["Soluong"].ToString());
                                Decimal gia = Convert.ToDecimal(dsTemp.Tables[0].Rows[i]["Gia"].ToString());
                                Decimal tt = gia * soluong;
                                if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                    tt += Convert.ToDecimal(dr["ThanhTien"]);
                                dr["ThanhTien"] = (tt).ToString("###,###,###,###.##");
                            }
                        }
                        if (dsTP_PT != null)
                        {
                            for (i = 0; i < dsTP_PT.Tables[0].Rows.Count; i++)
                            {
                                Decimal gia = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[i]["Gia"].ToString());
                                DateTime dtBD = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[i]["GioBD"].ToString());
                                DateTime dtKT = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[i]["GioKT"].ToString());
                                TimeSpan dif = dtKT - dtBD;
                                Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;
                                Decimal tt = soluongGio * gia;
                                if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                    tt += Convert.ToDecimal(dr["ThanhTien"]);
                                dr["ThanhTien"] = (tt).ToString("###,###,###,###.##");
                                if (dsTP_PT.Tables[0].Rows[i]["PhuThu"].ToString() != "")
                                {
                                    Decimal phuThu = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[i]["PhuThu"].ToString());
                                    if (phuThu != 0)
                                    {
                                        if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                            phuThu += Convert.ToDecimal(dr["ThanhTien"]);
                                        dr["ThanhTien"] = (phuThu).ToString("###,###,###,###.##");
                                    }
                                }
                            }
                        }
                        dtChartData.Rows.Add(dr);
                        ////////////////////////////
                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByDate");
                        ch.ShowDialog();
                    }
                }
            }
            else if (rdInComeMonth.Checked)
            {//By Month
                if (cbInComeByMonthFromMonth.Text == "" || cbInComeByMonthToMonth.Text == "" ||
                    cbInComeByMonthFromYear.Text == "" || cbInComeByMonthToYear.Text == "")
                {
                    MessageBox.Show("Bạn chưa thông tin tháng năm. Vui lòng chọn tháng bắt đầu và tháng kết thúc ",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int startMonth = Convert.ToInt32(cbInComeByMonthFromMonth.Text);
                int endMondth = Convert.ToInt32(cbInComeByMonthToMonth.Text);
                int startYear = Convert.ToInt32(cbInComeByMonthFromYear.Text);
                int endYear = Convert.ToInt32(cbInComeByMonthToYear.Text);
                DateTime start = new DateTime(startYear, startMonth, 1);
                DateTime end = new DateTime(endYear, endMondth, 1);
                if (DateTime.Compare(start, end) > 0)
                {
                    MessageBox.Show("Ngày bắt đầu lớn hơn ngày kết thúc. Vui lòng chọn lại thông tin ngày tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    //string stridProduct = "";
                    int i = 0, j = 0;
                    DateTime dtCurrent = start;
                    int diff = (endYear - startYear) * 12 + (endMondth - startMonth);
                    if (dtChartData != null && dtChartData.Rows.Count > 0)
                        dtChartData.Rows.Clear();
                    if (dtChartData != null && dtChartData.Columns.Count > 0)
                        dtChartData.Columns.Clear();
                    DataColumn dcdt = new DataColumn("Ngayxuat", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcdt);
                    DataColumn dcTT = new DataColumn("ThanhTien", Type.GetType("System.String"));
                    dtChartData.Columns.Add(dcTT);
                    string strWheredate = "";
                    string strQuery = "";
                    for (j = 0; j <= diff; j++)
                    {
                        dtCurrent = start.AddMonths(j);
                        //Get the number of product sold in that day.
                        strWheredate = "And Year(Ngayxuat) = " + dtCurrent.Year +
                            " and Month(Ngayxuat) = " + dtCurrent.Month + " ";
                        DataSet dsTP_PT = null;
                        if (chkInComePhuThu_TienPhong.Checked)
                        {
                            string strQueryPhong_PT =
                                "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong, Ngayxuat As NgayBan, " +
                                "Phuthu, GioBD, GioKT, GiaLoaiPhong.Gia, TenPhong As TenSanPham" +
                                " From  Hoadonxuat, Phong, GiaLoaiPhong" +
                                " Where Hoadonxuat.IDPhong = Phong.IDPhong and " +
                                " Hoadonxuat.IDGiaLoaiPhong = GiaLoaiPhong.IDGiaLoaiPhong " + strWheredate;
                            dsTP_PT = (DataSet)(da.getDataByQuery(strQueryPhong_PT));
                        }
                        string subQuerySP = "Select Hoadonxuat.IDHoadonXuat, Ngayxuat As NgayBan, SanPham.IDSanPham, TenSanPham, Soluong, " +
                            "DVT, Hoadonxuat.Giam, Max(NgayXuatSP) as NgayXuatSP1 " +
                            "From Hoadonxuat, ChitietHDXuat,  SanPham, NhomSP, GiaXuatSP " +
                            "Where ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat and " +
                            "NhomSP.IDNhomSP = SanPham.IDNhomSP and " +
                            "SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                            "SanPham.IDSanPham = GiaXuatSP.IDSanPham " + strWheredate +
                            " and (Year(NgayXuatSP) < Year(Ngayxuat) or " +
                            "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) < Month(Ngayxuat)) or " +
                            "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) = Month(Ngayxuat) and Day(NgayXuatSP) <= Day(Ngayxuat)))" +
                            " GROUP BY Hoadonxuat.IDHoadonXuat, Ngayxuat, SanPham.IDSanPham, TenSanPham, Soluong, " +
                            "DVT, Hoadonxuat.Giam ";
                        strQuery = "Select T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                            "T.DVT, T.Giam, T.NgayXuatSP1, Max(Gia) as Gia " +
                            "From GiaXuatSP, (" + subQuerySP + ") as T " +
                            "Where T.IDSanPham = GiaXuatSP.IDSanPham and " +
                            "T.NgayXuatSP1 = GiaXuatSP.NgayXuatSP " +
                            "GROUP BY T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                            "T.DVT, T.Giam, T.NgayXuatSP1";

                        DataSet dsTemp = da.getDataByQuery(strQuery);
                        DataRow dr = dtChartData.NewRow();
                        dr["Ngayxuat"] = dtCurrent.ToString("MM/yyyy");

                        /////////////////////////////
                        if (dsTemp != null)
                        {
                            for (i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                Decimal soluong = Convert.ToDecimal(dsTemp.Tables[0].Rows[i]["Soluong"].ToString());
                                Decimal gia = Convert.ToDecimal(dsTemp.Tables[0].Rows[i]["Gia"].ToString());
                                Decimal tt = gia * soluong;
                                if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                    tt += Convert.ToDecimal(dr["ThanhTien"]);
                                dr["ThanhTien"] = (tt).ToString("###,###,###,###.##");
                            }
                        }
                        if (dsTP_PT != null)
                        {
                            for (i = 0; i < dsTP_PT.Tables[0].Rows.Count; i++)
                            {
                                Decimal gia = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[i]["Gia"].ToString());
                                DateTime dtBD = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[i]["GioBD"].ToString());
                                DateTime dtKT = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[i]["GioKT"].ToString());
                                TimeSpan dif = dtKT - dtBD;
                                Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;
                                Decimal tt = soluongGio * gia;
                                if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                    tt += Convert.ToDecimal(dr["ThanhTien"]);
                                dr["ThanhTien"] = (tt).ToString("###,###,###,###.##");
                                if (dsTP_PT.Tables[0].Rows[i]["PhuThu"].ToString() != "")
                                {
                                    Decimal phuThu = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[i]["PhuThu"].ToString());
                                    if (phuThu != 0)
                                    {
                                        if (!(dr["ThanhTien"] is DBNull || dr["ThanhTien"] == ""))
                                            phuThu += Convert.ToDecimal(dr["ThanhTien"]);
                                        dr["ThanhTien"] = (phuThu).ToString("###,###,###,###.##");
                                    }
                                }
                            }
                        }
                        dtChartData.Rows.Add(dr);
                        ////////////////////////////

                    }
                    if (dtChartData == null || dtChartData.Rows.Count == 0)
                    {
                        MessageBox.Show("Dữ liệu rỗng! Xin vui lòng chọn lại dữ liệu", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        frmChartView ch = new frmChartView(dtChartData, "ByMonth");
                        ch.ShowDialog();
                    }
                }
            }
        }
        #endregion
    }
}