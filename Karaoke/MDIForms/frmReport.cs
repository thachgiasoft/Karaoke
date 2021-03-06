using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Karaoke.DataSets;
using BKIT.Model;
namespace Karaoke.MDIForms
{
    public partial class frmReport : DevExpress.XtraEditors.XtraForm
    {
        private dataSetNewDetailReport detailReport = null;
        private DataSetDetailReportDSBH detailReportDSBH = null;
        //private DataSetTienVon TienVonReport = null;
        private DataSetTonKho TonKhoReport = null;
        private DataSetTKHD TKHDReport = null;
        //private 
        public frmReport()
        {
            InitializeComponent();
        }
        private void frmReport_Load(object sender, EventArgs e)
        {
            DataAccess da = new DataAccess();
            DataSet dsNhomSP = da.getAllNhomSP();
            //cbTKTenNhomSP.Items.Add("Tất cả");
            if (dsNhomSP != null && dsNhomSP.Tables[0] != null)
            {
                for (int k = 0; k < dsNhomSP.Tables[0].Rows.Count; k++)
                {
                    cbTKTenNhomSP.Items.Add(dsNhomSP.Tables[0].Rows[k]["TenNhomSP"].ToString());
                    cbTenNhomSP.Items.Add(dsNhomSP.Tables[0].Rows[k]["TenNhomSP"].ToString());
                    cbDSBHProduct.Items.Add(dsNhomSP.Tables[0].Rows[k]["TenNhomSP"].ToString());
                }
            }
            DataSet dsNhanvien = da.getAllNhanvien();
            cbDSHBEmployee.Items.Add("Tất cả");
            cbTKHDEmployee.Items.Add("Tất cả");
            if (dsNhanvien != null && dsNhanvien.Tables[0] != null)
            {
                for (int k = 0; k < dsNhanvien.Tables[0].Rows.Count; k++)
                {
                    cbDSHBEmployee.Items.Add(dsNhanvien.Tables[0].Rows[k]["Ten"].ToString());
                    cbTKHDEmployee.Items.Add(dsNhanvien.Tables[0].Rows[k]["Ten"].ToString());
                }
            }
            DataSet dsPhong = da.getAllPhong();
            cbDSBHRoom.Items.Add("Tất cả");
            cbDSBHRoom.Items.Add("Bán lẻ");
            cbTKHDRoom.Items.Add("Tất cả");
            cbTKHDRoom.Items.Add("Bán lẻ");
            if (dsPhong != null && dsPhong.Tables[0] != null)
            {
                for (int k = 0; k < dsPhong.Tables[0].Rows.Count; k++)
                {
                    cbDSBHRoom.Items.Add(dsPhong.Tables[0].Rows[k]["TenPhong"].ToString());
                    cbTKHDRoom.Items.Add(dsPhong.Tables[0].Rows[k]["TenPhong"].ToString());
                }
            }
            DateTime temp = DateTime.Now;
            string month = temp.Month.ToString();
            string year = temp.Year.ToString();
            for (int i = 0; i < 12; i++)
            {
                if (month == cbMonthByMonth.Items[i].ToString())
                {
                    cbMonthByMonth.SelectedIndex = i;
                }

                if (month == cbDSBHMonthByMonth.Items[i].ToString())
                {
                    cbDSBHMonthByMonth.SelectedIndex = i;
                }
                if (month == cbTKHDByMonthMonth.Items[i].ToString())
                {
                    cbTKHDByMonthMonth.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 30; i++)
            {
                if (year == cbYearByMonth.Items[i].ToString())
                {
                    cbYearByMonth.SelectedIndex = i;
                }

                if (year == cbDSBHYearByMonth.Items[i].ToString())
                {
                    cbDSBHYearByMonth.SelectedIndex = i;
                }
                if (year == cbTKHDByMonthYear.Items[i].ToString())
                {
                    cbTKHDByMonthYear.SelectedIndex = i;
                }
            }
        }
        #region Doanh Thu
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (detailReport != null)
            {
                frmViewReport frmView = new frmViewReport(detailReport);
                frmView.Show();
            }
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            //Get Data From DB
            string strWhere = "";
            string strQuerySP = "";
            string strQueryPhong_PT = "";
            DateTime startDate = dtFromByCustom.Value;
            DateTime endDate = dtToByCustom.Value;
            bool isPhuThu = chbAll.Checked;
            string strName = cbTenNhomSP.Text;
            DataAccess da = new DataAccess();
            DataSetDetailReport dsDetailReport = new DataSetDetailReport();
            DataSet dsTP_PT = null;
            DataSet dsSP = null;
            if ((strName != "") && (strName != "Tất cả"))
            {
                strWhere += " and TenNhomSP = '" + strName + "' ";
            }
            if (rdDate.Checked)
            {
                DateTime dtDate = dtDateByDate.Value;
                strWhere += " AND Year(Ngayxuat) = " + dtDate.Year + " and Month(Ngayxuat) = " + 
                    dtDate.Month + " and Day(Ngayxuat) = " + dtDate.Day;
            }
            else if (rdMonth.Checked)
            {
                string month = "1";
                if (cbMonthByMonth.Text != "")
                    month = cbMonthByMonth.Text;
                else
                    month = DateTime.Now.Month.ToString();
                string year = "2010";
                if (cbYearByMonth.Text != "")
                    year = cbYearByMonth.Text;
                else
                    year = DateTime.Now.Month.ToString();
                strWhere += " AND Year(Ngayxuat) = " + year + " and Month(Ngayxuat) = " + month + " ";
            }
            else if (rdCustom.Checked)
            {
                if (startDate != null && endDate != null)
                {
                    strWhere += " AND ( Year(Ngayxuat) > " + startDate.Year +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) > " + startDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) = " + startDate.Month + " and Day(Ngayxuat) >= " + startDate.Day + "))" +
                    " AND ( Year(Ngayxuat) < " + endDate.Year +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) < " + endDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) = " + endDate.Month + " and Day(Ngayxuat) <= " + endDate.Day + ")) ";
                }
            }
            if (isPhuThu)
            {
                strQueryPhong_PT =
                    "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong,Hoadonxuat.IDPhong as IDPhong,Hoadonxuat.Giam as Giam, Hoadonxuat.Ngayxuat as Ngayxuat, Hoadonxuat.Thue as Thue," +
                    "Phuthu, GioBD, GioKT, GiaLoaiPhong.Gia, TenPhong As TenSanPham" +
                    " From  Hoadonxuat, Phong, GiaLoaiPhong" +
                    " Where Hoadonxuat.IDPhong = Phong.IDPhong and " +
                    " Hoadonxuat.IDGiaLoaiPhong = GiaLoaiPhong.IDGiaLoaiPhong " + strWhere;
                dsTP_PT = (DataSet)(da.getDataByQuery(strQueryPhong_PT));
            }
            string subQuerySP = "Select Hoadonxuat.IDHoadonXuat, Ngayxuat As NgayBan, SanPham.IDSanPham, TenSanPham, Soluong, " +
                "DVT, ChitietHDXuat.Giam as Giam, Max(NgayXuatSP) as NgayXuatSP1 " +
                "From Hoadonxuat, ChitietHDXuat,  SanPham, NhomSP, GiaXuatSP " +
                "Where ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat and " +
                "NhomSP.IDNhomSP = SanPham.IDNhomSP and " +
                "SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                "SanPham.IDSanPham = GiaXuatSP.IDSanPham " + strWhere +
                " and (Year(NgayXuatSP) < Year(Ngayxuat) or " +
                "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) < Month(Ngayxuat)) or " +
                "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) = Month(Ngayxuat) and Day(NgayXuatSP) <= Day(Ngayxuat)))" +
                " GROUP BY Hoadonxuat.IDHoadonXuat, Ngayxuat, SanPham.IDSanPham, TenSanPham, Soluong, " +
                "DVT, Hoadonxuat.Giam ";
            strQuerySP = "Select T.IDHoadonXuat , T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                "T.DVT, T.Giam, T.NgayXuatSP1, Max(Gia) as Gia " +
                "From GiaXuatSP, (" + subQuerySP + ") as T " +
                "Where T.IDSanPham = GiaXuatSP.IDSanPham and " +
                "T.NgayXuatSP1 = GiaXuatSP.NgayXuatSP " +
                "GROUP BY T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                "T.DVT, T.Giam, T.NgayXuatSP1";
            string sqlCommand = "SELECT Hoadonxuat.Ngayxuat AS Ngayxuat,Hoadonxuat.Thue as Thue, ChitietHDXuat.IDChitietHDXuat AS IDChitietHDXuat, ChitietHDXuat.IDHoadonXuat AS IDHoadonXuat, ChitietHDXuat.IDSanpham AS IDSanpham, SanPham.TenSanPham AS TenSanPham, SanPham.DVT AS DVT, GiaXuatSP.IDGiaXuatSP AS IDGiaXuatSP, GiaXuatSP.Gia AS Gia,ChitietHDXuat.Giam AS Giam, ChitietHDXuat.Soluong AS Soluong, ([Gia]*(100 - [Giam])/100)*[Soluong] AS Thanhtien, SanPham.TonKho AS TonKho, ChitietHDXuat.Bep AS Bep, ChitietHDXuat.Kho AS Kho, ChitietHDXuat.Ghichu AS Ghichu,1 as [Delete] " +
                                "FROM Hoadonxuat INNER JOIN (SanPham INNER JOIN (GiaXuatSP INNER JOIN ChitietHDXuat ON GiaXuatSP.IDGiaXuatSP = ChitietHDXuat.IDGiaxuat) ON (SanPham.IDSanPham = ChitietHDXuat.IDSanpham) AND (SanPham.IDSanPham = GiaXuatSP.IDSanPham)) ON Hoadonxuat.IDHoadonXuat = ChitietHDXuat.IDHoadonXuat " +
                                "Where 1=1 "+strWhere + ";";
            dsSP = (DataSet)(da.getDataByQuery(sqlCommand));
            Decimal tongtien = 0;
            Decimal giam = 0;
            Decimal thue = 0;
            int i = 0, j = 0;
            if (dsSP != null)
            {
                detailReport = new dataSetNewDetailReport();
                for (i = 0; i < dsSP.Tables[0].Rows.Count; i++)
                {
                    DataRow dr1 = detailReport.Tables[0].NewRow();
                    Decimal soluong = Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Soluong"].ToString());
                    Decimal gia = Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Gia"].ToString());
                    if (dsSP.Tables[0].Rows[i]["Giam"].ToString() != "")
                    {
                        giam += (soluong * (gia/100)*(Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Giam"].ToString())));
                    }
                    dr1["ID"] = i.ToString();
                    dr1["STT"] = (i + 1).ToString();
                    int index = dsSP.Tables[0].Rows[i]["Ngayxuat"].ToString().IndexOf(' ');
                    dr1["NgayBan"] = dsSP.Tables[0].Rows[i]["Ngayxuat"].ToString().Substring(0, index);
                    dr1["TenSanPham"] = dsSP.Tables[0].Rows[i]["TenSanPham"].ToString();
                    dr1["DVT"] = dsSP.Tables[0].Rows[i]["DVT"].ToString();
                    dr1["Giam"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Giam"]).ToString("##0")+"%";
                    dr1["Thue"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thue"]).ToString("##0") + "%";
                    //dr1["DonGia"] = dsSP.Tables[0].Rows[i]["Gia"].ToString();
                    dr1["DonGia"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Gia"]).ToString("###,###,###,###");
                    dr1["Soluong"] = Convert.ToInt32(soluong).ToString("###,###,###,###");
                    dr1["ThanhTien"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]).ToString("###,###,###,###");
                    if (dsSP.Tables[0].Rows[i]["Thue"].ToString() != "")
                    {
                        thue += ((Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"])/100) * (Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Thue"])));
                        tongtien = tongtien + Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]) + ((Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]) / 100) * (Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Thue"])));
                    }
                    else
                        tongtien += Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]);
                    detailReport.Tables[0].Rows.Add(dr1);
                }
            }
            if (dsTP_PT != null)
            {
                for (j = 0; j < dsTP_PT.Tables[0].Rows.Count; j++)
                {
                    if (Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["IDPhong"]) >= 0)
                    {
                        int thanhtien = 0;
                        DataRow dr1 = detailReport.Tables[0].NewRow();
                        Decimal gia = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Gia"].ToString());
                        dr1["ID"] = i.ToString();
                        dr1["STT"] = (i + 1).ToString();
                        int index = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().IndexOf(' ');
                        dr1["NgayBan"] = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().Substring(0, index);
                        dr1["TenSanPham"] = ("Tiền phòng " + dsTP_PT.Tables[0].Rows[j]["TenSanPham"].ToString()).ToString();
                        DateTime dtBD = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[j]["GioBD"].ToString());
                        DateTime dtKT = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[j]["GioKT"].ToString());
                        TimeSpan dif = dtKT - dtBD;
                        Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;
                        dr1["DVT"] = ("Gio").ToString();
                        dr1["Giam"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Giam"]).ToString("##0") + "%";
                        dr1["Thue"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Thue"]).ToString("##0") + "%";
                        dr1["DonGia"] = Convert.ToInt32(gia).ToString("###,###,###,###");
                        dr1["Soluong"] = soluongGio.ToString("###,###,###,##0.##");
                        thanhtien = (int)((gia / 100) * (100 - Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Giam"])));
                        thanhtien = (int)(thanhtien * soluongGio);
                        dr1["ThanhTien"] = thanhtien.ToString("###,###,###,##0.##");
                        if (dsTP_PT.Tables[0].Rows[j]["Thue"].ToString() != "")
                        {
                            thue += ((thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"])));
                            tongtien = tongtien + thanhtien + (thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"]));
                        }
                        else
                            tongtien += thanhtien;

                        giam += (int)(gia * soluongGio - thanhtien);
                        detailReport.Tables[0].Rows.Add(dr1);
                        i++;
                        if (dsTP_PT.Tables[0].Rows[j]["PhuThu"].ToString() != "")
                        {
                            Decimal phuThu = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["PhuThu"].ToString());
                            if (phuThu != 0)
                            {
                                DataRow dr2 = detailReport.Tables[0].NewRow();
                                dr2["ID"] = i.ToString();
                                dr2["STT"] = (i + 1).ToString();
                                int index1 = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().IndexOf(' ');
                                dr2["NgayBan"] = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().Substring(0, index1);
                                dr2["TenSanPham"] = ("Phụ thu phòng " + dsTP_PT.Tables[0].Rows[j]["TenSanPham"].ToString()).ToString();
                                dr2["ThanhTien"] = (phuThu).ToString("###,###,###,###.##");
                                dr2["Thue"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Thue"]).ToString("##0") + "%";
                                dr2["Giam"] = "0" + "%";
                                if (dsTP_PT.Tables[0].Rows[i]["Thue"].ToString() != "")
                                {
                                    thue += ((phuThu / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"])));
                                    tongtien = tongtien + phuThu + (thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"]));
                                }
                                else
                                    tongtien += phuThu;
                                detailReport.Tables[0].Rows.Add(dr2);
                                i++;
                            }
                        }
                    }
                }
            }
            DataRow dr = detailReport.Tables[1].NewRow();
            dr["StartDate"] = startDate.ToString("dd/MM/yyyy");
            dr["EndDate"] = endDate.ToString("dd/MM/yyyy");
            dr["TCThanhTien"] = tongtien.ToString("###,###,###,###");
            detailReport.Tables[1].Rows.Add(dr);
            txt_TC.Text = tongtien.ToString("###,###,###,###") + " VND";
            txt_Giam.Text = giam.ToString("###,###,###,###") + " VND";
            txt_Thue.Text = thue.ToString("###,###,###,###") + " VND";
            gridSP.DataSource = detailReport.Tables[0];
            btnViewReport.Enabled = true;
        }
        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = !rdMonth.Checked;
            lblMonthByMonth.Enabled = rdMonth.Checked;
            cbMonthByMonth.Enabled = rdMonth.Checked;
            lblYearByMonth.Enabled = rdMonth.Checked;
            cbYearByMonth.Enabled = rdMonth.Checked;
            lblFromByCustom.Enabled = !rdMonth.Checked;
            dtFromByCustom.Enabled = !rdMonth.Checked;
            lblToByCustom.Enabled = !rdMonth.Checked;
            dtToByCustom.Enabled = !rdMonth.Checked;
            btnViewReport.Enabled = false;
        }
        private void rdDate_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = rdDate.Checked;
            lblMonthByMonth.Enabled = !rdDate.Checked;
            cbMonthByMonth.Enabled = !rdDate.Checked;
            lblYearByMonth.Enabled = !rdDate.Checked;
            cbYearByMonth.Enabled = !rdDate.Checked;
            lblFromByCustom.Enabled = !rdDate.Checked;
            dtFromByCustom.Enabled = !rdDate.Checked;
            lblToByCustom.Enabled = !rdDate.Checked;
            dtToByCustom.Enabled = !rdDate.Checked;
            btnViewReport.Enabled = false;
        }
        private void rdCustom_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = !rdCustom.Checked;
            lblMonthByMonth.Enabled = !rdCustom.Checked;
            cbMonthByMonth.Enabled = !rdCustom.Checked;
            lblYearByMonth.Enabled = !rdCustom.Checked;
            cbYearByMonth.Enabled = !rdCustom.Checked;
            lblFromByCustom.Enabled = rdCustom.Checked;
            dtFromByCustom.Enabled = rdCustom.Checked;
            lblToByCustom.Enabled = rdCustom.Checked;
            dtToByCustom.Enabled = rdCustom.Checked;
            btnViewReport.Enabled = false;
        }
        private void dtDateByDate_ValueChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void cbMonthByMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void cbYearByMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void cbTenNhomSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void dtFromByCustom_ValueChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void dtToByCustom_ValueChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }

        private void chbAll_CheckedChanged(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
        }
        #endregion
        #region Doanh so ban hang
        private void rdDSBHByDate_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = rdDSBHByDate.Checked;
            lblDSBHMonthByMonth.Enabled = !rdDSBHByDate.Checked;
            cbDSBHMonthByMonth.Enabled = !rdDSBHByDate.Checked;
            lblDSBHYearByMonth.Enabled = !rdDSBHByDate.Checked;
            cbDSBHYearByMonth.Enabled = !rdDSBHByDate.Checked;
            lblDSBHFromByCustom.Enabled = !rdDSBHByDate.Checked;
            dtDSBHFromByCustom.Enabled = !rdDSBHByDate.Checked;
            lblDSBHToByCustom.Enabled = !rdDSBHByDate.Checked;
            dtDSBHToByCustom.Enabled = !rdDSBHByDate.Checked;
            btnDSBHViewReport.Enabled = false;
        }

        private void rdDSBHByCustom_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = !rdDSBHByCustom.Checked;
            lblDSBHMonthByMonth.Enabled = !rdDSBHByCustom.Checked;
            cbDSBHMonthByMonth.Enabled = !rdDSBHByCustom.Checked;
            lblDSBHYearByMonth.Enabled = !rdDSBHByCustom.Checked;
            cbDSBHYearByMonth.Enabled = !rdDSBHByCustom.Checked;
            lblDSBHFromByCustom.Enabled = rdDSBHByCustom.Checked;
            dtDSBHFromByCustom.Enabled = rdDSBHByCustom.Checked;
            lblDSBHToByCustom.Enabled = rdDSBHByCustom.Checked;
            dtDSBHToByCustom.Enabled = rdDSBHByCustom.Checked;
            btnDSBHViewReport.Enabled = false;
        }

        private void rdDSBHByMonth_CheckedChanged(object sender, EventArgs e)
        {
            dtDateByDate.Enabled = !rdDSBHByMonth.Checked;
            lblDSBHMonthByMonth.Enabled = rdDSBHByMonth.Checked;
            cbDSBHMonthByMonth.Enabled = rdDSBHByMonth.Checked;
            lblDSBHYearByMonth.Enabled = rdDSBHByMonth.Checked;
            cbDSBHYearByMonth.Enabled = rdDSBHByMonth.Checked;
            lblDSBHFromByCustom.Enabled = !rdDSBHByMonth.Checked;
            dtDSBHFromByCustom.Enabled = !rdDSBHByMonth.Checked;
            lblDSBHToByCustom.Enabled = !rdDSBHByMonth.Checked;
            dtDSBHToByCustom.Enabled = !rdDSBHByMonth.Checked;
            btnDSBHViewReport.Enabled = false;
        }

        private void btnDSBHReport_Click(object sender, EventArgs e)
        {

            //Get Data From DB
            string strWheredate = "";
            string strWhereProduct = "";
            string strWhereEmployee = "";
            string strWhereRoom = "";
            string strQuerySP = "";
            string strQueryPhong_PT = "";
            DateTime startDate = dtDSBHFromByCustom.Value;
            DateTime endDate = dtDSBHToByCustom.Value;
            bool isPhuThu = chkDSBH_TP_PT.Checked;
            string strNameProduct = cbDSBHProduct.Text;
            DataAccess da = new DataAccess();
            //DataSetDetailReport dsDetailReport = new DataSetDetailReport();
            DataSet dsTP_PT = null;
            DataSet dsSP = null;
            string strEmployeeName = cbDSHBEmployee.Text;
            if ((strNameProduct != "") && (strNameProduct != "Tất cả"))
            {
                strWhereProduct += " and TenNhomSP = '" + strNameProduct + "' ";
            }

            if ((strEmployeeName != "") && (strEmployeeName != "Tất cả"))
            {
                strWhereEmployee += " and Ten = '" + strEmployeeName + "' ";
            }
            string strRoomName = cbDSBHRoom.Text;

            if ((strRoomName != "") && (strRoomName != "Tất cả"))
            {
                strWhereRoom += " and TenPhong = '" + strRoomName + "' ";
            }
            if (rdDSBHByDate.Checked)
            {
                DateTime dtDate = dtDateDSBHByDate.Value;
                strWheredate += " AND Year(Ngayxuat) = " + dtDate.Year + " and Month(Ngayxuat) = " +
                    dtDate.Month + " and Day(Ngayxuat) = " + dtDate.Day;
            }
            else if (rdDSBHByMonth.Checked)
            {
                string month = "1";
                if (cbDSBHMonthByMonth.Text != "")
                    month = cbDSBHMonthByMonth.Text;
                else
                    month = DateTime.Now.Month.ToString();
                string year = "2010";
                if (cbDSBHYearByMonth.Text != "")
                    year = cbDSBHYearByMonth.Text;
                else
                    year = DateTime.Now.Month.ToString();
                strWheredate += " AND Year(Ngayxuat) = " + year + " and Month(Ngayxuat) = " + month + " ";
            }
            else if (rdDSBHByCustom.Checked)
            {
                if (startDate != null && endDate != null)
                {
                    strWheredate += " AND ( Year(Ngayxuat) > " + startDate.Year +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) > " + startDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) = " + startDate.Month + " and Day(Ngayxuat) >= " + startDate.Day + "))" +
                    " AND ( Year(Ngayxuat) < " + endDate.Year +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) < " + endDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) = " + endDate.Month + " and Day(Ngayxuat) <= " + endDate.Day + ")) ";
                }
            }
            if (isPhuThu)
            {
                strQueryPhong_PT =
                    "Select IDHoadonxuat, Hoadonxuat.IDGiaLoaiPhong, Hoadonxuat.IDPhong as IDPhong,Hoadonxuat.Giam as Giam, Ngayxuat As Ngayxuat,Hoadonxuat.Thue as Thue, " +
                    "Phuthu, GioBD, GioKT, GiaLoaiPhong.Gia, TenPhong As TenSanPham" +
                    " From  Hoadonxuat, Phong, GiaLoaiPhong, Nhanvien" +
                    " Where Hoadonxuat.IDPhong = Phong.IDPhong and " +
                    "Hoadonxuat.IDNhanvien = Nhanvien.IDNhanvien and " +
                    " Hoadonxuat.IDGiaLoaiPhong = GiaLoaiPhong.IDGiaLoaiPhong " + 
                    strWhereEmployee + strWhereRoom + strWheredate;
                dsTP_PT = (DataSet)(da.getDataByQuery(strQueryPhong_PT));
            }
            string subQuerySP = "Select Hoadonxuat.IDHoadonXuat, Ngayxuat As NgayBan, SanPham.IDSanPham, " + 
                "TenNhomSP, TenSanPham, Soluong, Ten, " +
                "DVT, Hoadonxuat.Giam, Max(NgayXuatSP) as NgayXuatSP1 " +
                "From Hoadonxuat, ChitietHDXuat,  SanPham, NhomSP, GiaXuatSP, Nhanvien, Phong " +
                "Where ChitietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat and " +
                "Phong.IDPhong = Hoadonxuat.IDPhong and " +
                "Hoadonxuat.IDNhanvien = Nhanvien.IDNhanvien and " +
                "NhomSP.IDNhomSP = SanPham.IDNhomSP and " +
                "SanPham.IDSanPham = ChitietHDXuat.IDSanPham and " +
                "SanPham.IDSanPham = GiaXuatSP.IDSanPham " + strWhereProduct + 
                strWhereEmployee + strWhereRoom + strWheredate + 
                " and (Year(NgayXuatSP) < Year(Ngayxuat) or " +
                "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) < Month(Ngayxuat)) or " +
                "(Year(NgayXuatSP) = Year(Ngayxuat) and Month(NgayXuatSP) = Month(Ngayxuat) and Day(NgayXuatSP) <= Day(Ngayxuat)))" +
                " GROUP BY Hoadonxuat.IDHoadonXuat, Ngayxuat, SanPham.IDSanPham, TenNhomSP, " +
                "TenSanPham, Soluong, Ten, DVT, Hoadonxuat.Giam ";
            strQuerySP = "Select T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                "T.DVT, T.Giam, T.NgayXuatSP1, Max(Gia) as Gia " +
                "From GiaXuatSP, (" + subQuerySP + ") as T " +
                "Where T.IDSanPham = GiaXuatSP.IDSanPham and " +
                "T.NgayXuatSP1 = GiaXuatSP.NgayXuatSP " +
                "GROUP BY T.IDHoadonXuat, T.NgayBan, T.IDSanPham, T.TenSanPham, T.Soluong, " +
                "T.DVT, T.Giam, T.NgayXuatSP1";
            string sqlCommand = "SELECT Hoadonxuat.Ngayxuat AS Ngayxuat, Hoadonxuat.Thue AS Thue, ChitietHDXuat.IDChitietHDXuat AS IDChitietHDXuat, ChitietHDXuat.IDHoadonXuat AS IDHoadonXuat, ChitietHDXuat.IDSanpham AS IDSanpham, SanPham.TenSanPham AS TenSanPham, SanPham.DVT AS DVT, GiaXuatSP.IDGiaXuatSP AS IDGiaXuatSP, GiaXuatSP.Gia AS Gia, ChitietHDXuat.Giam AS Giam, ChitietHDXuat.Soluong AS Soluong, ([Gia]*(100-[Giam])/100)*[Soluong] AS Thanhtien, SanPham.TonKho AS TonKho, ChitietHDXuat.Bep AS Bep, ChitietHDXuat.Kho AS Kho, ChitietHDXuat.Ghichu AS Ghichu, 1 AS [Delete], Phong.TenPhong as TenPhong "+
                                "FROM (Hoadonxuat INNER JOIN (SanPham INNER JOIN (GiaXuatSP INNER JOIN ChitietHDXuat ON GiaXuatSP.IDGiaXuatSP = ChitietHDXuat.IDGiaxuat) ON (SanPham.IDSanPham = GiaXuatSP.IDSanPham) AND (SanPham.IDSanPham = ChitietHDXuat.IDSanpham)) ON Hoadonxuat.IDHoadonXuat = ChitietHDXuat.IDHoadonXuat) INNER JOIN Phong ON Hoadonxuat.IDPhong = Phong.IDPhong "+
                                "Where 1=1 " + strWhereProduct + strWhereEmployee + strWhereRoom + strWheredate + ";";
            dsSP = (DataSet)(da.getDataByQuery(sqlCommand));
            //dsSP = (DataSet)(da.getDataByQuery(strQuerySP));
            Decimal tongtien = 0;
            Decimal giam = 0;
            Decimal thue = 0;
            int i = 0, j = 0;
            if (dsSP != null)
            {
                detailReportDSBH = new DataSetDetailReportDSBH();
                for (i = 0; i < dsSP.Tables[0].Rows.Count; i++)
                {
                    DataRow dr1 = detailReportDSBH.Tables[0].NewRow();
                    Decimal soluong = Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Soluong"].ToString());
                    Decimal gia = Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Gia"].ToString());
                    if (dsSP.Tables[0].Rows[i]["Giam"].ToString() != "")
                    {
                        giam += (soluong * (gia / 100) * (Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Giam"].ToString())));
                    }
                    dr1["ID"] = i.ToString();
                    dr1["STT"] = (i + 1).ToString();
                    int index = dsSP.Tables[0].Rows[i]["Ngayxuat"].ToString().IndexOf(' ');
                    dr1["NgayBan"] = dsSP.Tables[0].Rows[i]["Ngayxuat"].ToString().Substring(0, index);
                    dr1["TenSanPham"] = dsSP.Tables[0].Rows[i]["TenSanPham"].ToString();
                    dr1["DVT"] = dsSP.Tables[0].Rows[i]["DVT"].ToString();
                    dr1["Giam"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Giam"]).ToString("##0") + "%";
                    dr1["Thue"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thue"]).ToString("##0") + "%";
                    //dr1["DonGia"] = dsSP.Tables[0].Rows[i]["Gia"].ToString();
                    dr1["DonGia"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Gia"]).ToString("###,###,###,###");
                    dr1["Soluong"] = Convert.ToInt32(soluong).ToString("###,###,###,###");
                    dr1["ThanhTien"] = Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]).ToString("###,###,###,###");
                    if (dsSP.Tables[0].Rows[i]["Thue"].ToString() != "")
                    {
                        thue += ((Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]) / 100) * (Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Thue"])));
                        tongtien = tongtien + Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]) + ((Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]) / 100) * (Convert.ToDecimal(dsSP.Tables[0].Rows[i]["Thue"])));
                    }
                    else
                        tongtien += Convert.ToInt32(dsSP.Tables[0].Rows[i]["Thanhtien"]);
                    detailReportDSBH.Tables[0].Rows.Add(dr1);
                }
            }
            if (dsTP_PT != null)
            {
                if(detailReportDSBH == null)
                    detailReportDSBH = new DataSetDetailReportDSBH();
                for (j = 0; j < dsTP_PT.Tables[0].Rows.Count; j++)
                {
                    if (Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["IDPhong"]) >= 0)
                    {
                        int thanhtien = 0;
                        DataRow dr1 = detailReportDSBH.Tables[0].NewRow();
                        Decimal gia = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Gia"].ToString());
                        dr1["ID"] = i.ToString();
                        dr1["STT"] = (i + 1).ToString();
                        int index = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().IndexOf(' ');
                        dr1["NgayBan"] = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().Substring(0, index);
                        dr1["TenSanPham"] = ("Tiền phòng " + dsTP_PT.Tables[0].Rows[j]["TenSanPham"].ToString()).ToString();
                        DateTime dtBD = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[j]["GioBD"].ToString());
                        DateTime dtKT = Convert.ToDateTime(dsTP_PT.Tables[0].Rows[j]["GioKT"].ToString());
                        TimeSpan dif = dtKT - dtBD;
                        Decimal soluongGio = dif.Hours + Convert.ToDecimal(dif.Minutes) / 60;
                        dr1["DVT"] = ("Gio").ToString();
                        dr1["Giam"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Giam"]).ToString("##0") + "%";
                        dr1["Thue"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Thue"]).ToString("##0") + "%";
                        dr1["DonGia"] = Convert.ToInt32(gia).ToString("###,###,###,###");
                        dr1["Soluong"] = soluongGio.ToString("###,###,###,##0.##");
                        thanhtien = (int)((gia / 100) * (100 - Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Giam"])));
                        thanhtien = (int)(thanhtien * soluongGio);
                        dr1["ThanhTien"] = thanhtien.ToString("###,###,###,##0.##");
                        if (dsTP_PT.Tables[0].Rows[j]["Thue"].ToString() != "")
                        {
                            thue += ((thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"])));
                            tongtien = tongtien + thanhtien + (thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"]));
                        }
                        else
                            tongtien += thanhtien;

                        giam += (int)(gia * soluongGio - thanhtien);
                        detailReportDSBH.Tables[0].Rows.Add(dr1);
                        i++;
                        if (dsTP_PT.Tables[0].Rows[j]["PhuThu"].ToString() != "")
                        {
                            Decimal phuThu = Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["PhuThu"].ToString());
                            if (phuThu != 0)
                            {
                                DataRow dr2 = detailReport.Tables[0].NewRow();
                                dr2["ID"] = i.ToString();
                                dr2["STT"] = (i + 1).ToString();
                                int index1 = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().IndexOf(' ');
                                dr2["NgayBan"] = dsTP_PT.Tables[0].Rows[j]["Ngayxuat"].ToString().Substring(0, index1);
                                dr2["TenSanPham"] = ("Phụ thu phòng " + dsTP_PT.Tables[0].Rows[j]["TenSanPham"].ToString()).ToString();
                                dr2["ThanhTien"] = (phuThu).ToString("###,###,###,###.##");
                                dr2["Thue"] = Convert.ToInt32(dsTP_PT.Tables[0].Rows[j]["Thue"]).ToString("##0") + "%";
                                dr2["Giam"] = "0" + "%";
                                if (dsTP_PT.Tables[0].Rows[i]["Thue"].ToString() != "")
                                {
                                    thue += ((phuThu / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"])));
                                    tongtien = tongtien + phuThu + (thanhtien / 100) * (Convert.ToDecimal(dsTP_PT.Tables[0].Rows[j]["Thue"]));
                                }
                                else
                                    tongtien += phuThu;
                                detailReportDSBH.Tables[0].Rows.Add(dr2);
                                i++;
                            }
                        }
                    }
                }
            }
            DataRow dr = detailReportDSBH.Tables[1].NewRow();
            if (rdDSBHByDate.Checked)
            {
                dr["StartDate"] = dtDateDSBHByDate.Value.ToString("dd/MM/yyyy");
                dr["EndDate"] = dtDateDSBHByDate.Value.ToString("dd/MM/yyyy");
            }
            else if (rdDSBHByMonth.Checked)
            {
                string month = "1";
                if (cbDSBHMonthByMonth.Text != "")
                    month = cbDSBHMonthByMonth.Text;
                else
                    month = DateTime.Now.Month.ToString();
                string year = "2010";
                if (cbDSBHYearByMonth.Text != "")
                    year = cbDSBHYearByMonth.Text;
                else
                    year = DateTime.Now.Month.ToString();
                strWheredate += " AND Year(Ngayxuat) = " + year + " and Month(Ngayxuat) = " + month + " ";

                dr["StartDate"] = "Tháng " + month + "/" + year;
                dr["EndDate"] = "Tháng " + month + "/" + year;
            }
            else if (rdDSBHByCustom.Checked)
            {
                dr["StartDate"] = startDate.ToString("dd/MM/yyyy");
                dr["EndDate"] = endDate.ToString("dd/MM/yyyy");
            }
            dr["TCThanhTien"] = tongtien.ToString("###,###,###,###.##");
            if (strRoomName == "")
                strRoomName = "Tất cả";
            if (strEmployeeName == "")
                strEmployeeName = "Tất cả";
            if (strNameProduct == "")
                strNameProduct = "Tất cả";
            dr["TenNhanVien"] = strEmployeeName;
            dr["LoaiSanPham"] = strNameProduct;
            dr["TenPhong"] = strRoomName;
            detailReportDSBH.Tables[1].Rows.Add(dr);
            txtDanhThu.Text = tongtien.ToString("###,###,###,##0") + " VND";
            //txt_Giam.Text = giam.ToString("###,###,###,###.##") + " VND";
            gridDSBH.DataSource = detailReportDSBH.Tables[0];
            btnDSBHViewReport.Enabled = true;
        }

        private void btnDSBHViewReport_Click(object sender, EventArgs e)
        {
            if (detailReportDSBH != null)
            {
                frmViewReport frmView = new frmViewReport(detailReportDSBH);
                frmView.Show();
            }
        }
        private void dtDateDSBHByDate_ValueChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void dtDSBHFromByCustom_ValueChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void dtDSBHToByCustom_ValueChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void cbDSBHRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void chkDSBH_TP_PT_CheckedChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void cbDSBHProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void cbDSHBEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void cbDSBHMonthByMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }

        private void cbDSBHYearByMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDSBHViewReport.Enabled = false;
        }
        #endregion
        #region TonKho
        private void button4_Click(object sender, EventArgs e)
        {
            //Get Data From DB
            string strWhere = "";
            string strWheredate = "";
            string strWheredate1 = "";
            string strQuerySP = "";
            DateTime endDate = dtTKEndDate.Value;
            DataAccess da = new DataAccess();
            DataSetTonKho dsTKReport = new DataSetTonKho();
            string strName = cbTKTenNhomSP.Text;
            DataSet dsTKSP = null;
            if ((strName != "") && (strName != "Tất cả"))
            {
                strWhere += " and TenNhomSP = '" + strName + "' ";
            }
            if (endDate != null)
            {//Query endate should be modified
                //strWhere += " AND ( Year(Ngayxuat) < " + endDate.Year +
                //" or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) < " + endDate.Month + ") " +
                //" or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) = " + endDate.Month + " and Day(Ngayxuat) <= " + endDate.Day + ")) ";
                strWheredate += " AND ( Year(Ngay) < " + endDate.Year +
                " or (Year(Ngay) = " + endDate.Year + " and Month(Ngay) < " + endDate.Month + ") " +
                " or (Year(Ngay) = " + endDate.Year + " and Month(Ngay) = " + endDate.Month + " and Day(Ngay) <= " + endDate.Day + ")) ";

                strWheredate1 += " AND ( Year(Ngayxuat) < " + endDate.Year +
                " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) < " + endDate.Month + ") " +
                " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) = " + endDate.Month + " and Day(Ngayxuat) <= " + endDate.Day + ")) ";
            }

            string subQuerySPTonKho = "Select IDSanPham, TenSanPham, DVT, TonKho as Soluong, IDNhomSP " +
                "From SanPham ";
            string subQueryGiaSP = "Select GiaXuatSP.IDSanPham, NgayXuatSP, Gia " +
                "From GiaXuatSP, " + 
                "(Select IDSanPham, Max(NgayXuatSP) as Ngay From GiaXuatSP Group by IDSanPham) as Tbl " +
                "Where GiaXuatSP.IDSanPham = Tbl.IDSanPham and " +
                "Year(GiaXuatSP.NgayXuatSP) = Year(Tbl.Ngay) and " +
                "Month(GiaXuatSP.NgayXuatSP) = Month(Tbl.Ngay) and " +
                "Day(GiaXuatSP.NgayXuatSP) = Day(Tbl.Ngay) and " +
                "Minute(GiaXuatSP.NgayXuatSP) = Minute(Tbl.Ngay) and " +
                "Second(GiaXuatSP.NgayXuatSP) = Second(Tbl.Ngay)";
            string subQueryNhap = "Select IDSanPham, Sum(SoLuong) as Soluong1 From ChiTietHoaDonNhap, HoaDonnhap "+
                "where ChiTietHoaDonNhap.IDHoaDonNhap = HoaDonNhap.IDHoaDonNhap "+ strWheredate +" Group by IDSanPham";
            string subQueryBan = "Select IDSanPham, Sum(SoLuong) as Soluong1 From ChitietHDXuat, Hoadonxuat "+
                " where ChiTietHDXuat.IDHoadonXuat = Hoadonxuat.IDHoadonXuat " + strWheredate1 +" Group by IDSanPham ";
            string subQueryNhap_Ban = "Select N.IDSanPham, N.Soluong1 - B.Soluong1 As Soluong, N.Soluong1 From " +
                "(" + subQueryNhap + ") as N LEFT JOIN (" + subQueryBan + ") as B ON N.IDSanPham = B.IDSanPham";
            string subQuerySPTonKho1 = "Select TK.IDSanPham,TK.IDNhomSP, TK.TenSanPham, TK.DVT, " + 
                "TK.Soluong + NB.Soluong as Soluong, TK.Soluong + NB.Soluong1 as Soluong1, TK.Soluong as Soluong2" +
                " From (" + subQuerySPTonKho + ") as TK LEFT JOIN (" + subQueryNhap_Ban + ") as NB ON " +
                "TK.IDSanPham = NB.IDSanPham";
            strQuerySP = "Select T1.IDSanPham, T1.IDNhomSP, T1.TenSanPham, T1.DVT, T1.Soluong, T1.Soluong1, T1.Soluong2," +
                " T2.Gia, T2.NgayXuatSP as NgayNhap, TenNhomSP " +
                "From (" + subQuerySPTonKho1 + ") as T1 " + ", (" + subQueryGiaSP + ") as T2 " + ", NhomSP " +
                "Where T1.IDSanPham = T2.IDSanPham and  T1.IDNhomSP = NhomSP.IDNhomSP ";
            strQuerySP += strWhere;
            dsTKSP = (DataSet)(da.getDataByQuery(strQuerySP));
            Decimal tongtien = 0;
            int i = 0;
            if (dsTKSP != null)
            {
                TonKhoReport = new DataSetTonKho();
                for (i = 0; i < dsTKSP.Tables[0].Rows.Count; i++)
                {
                    DataRow dr1 = TonKhoReport.Tables[0].NewRow();
                    Decimal soluong = 0;
                    if (dsTKSP.Tables[0].Rows[i]["Soluong"].ToString() != "")
                        soluong = Convert.ToDecimal(dsTKSP.Tables[0].Rows[i]["Soluong"].ToString());
                    else
                    {
                        if (dsTKSP.Tables[0].Rows[i]["Soluong1"].ToString() != "")
                            soluong = Convert.ToDecimal(dsTKSP.Tables[0].Rows[i]["Soluong1"].ToString());
                        else
                            soluong = Convert.ToDecimal(dsTKSP.Tables[0].Rows[i]["Soluong2"].ToString());
                    }
                    Decimal gia = Convert.ToDecimal(dsTKSP.Tables[0].Rows[i]["Gia"].ToString());
                    dr1["ID"] = dsTKSP.Tables[0].Rows[i]["IDSanPham"].ToString();
                    dr1["STT"] = (i + 1).ToString();
                    int index = dsTKSP.Tables[0].Rows[i]["NgayNhap"].ToString().IndexOf(' ');
                    dr1["NgayNhap"] = dsTKSP.Tables[0].Rows[i]["NgayNhap"].ToString().Substring(0, index);
                    dr1["TenSanPham"] = dsTKSP.Tables[0].Rows[i]["TenSanPham"].ToString();
                    dr1["DVT"] = dsTKSP.Tables[0].Rows[i]["DVT"].ToString();
                    dr1["DonGia"] = Convert.ToInt32(gia).ToString("###,###,###,###");
                    dr1["Soluong"] = Convert.ToInt32(soluong).ToString("###,###,###,###");
                    dr1["ThanhTien"] = (gia * soluong).ToString("###,###,###,###.##");
                    tongtien += gia * soluong;
                    TonKhoReport.Tables[0].Rows.Add(dr1);
                }
                DataRow dr = TonKhoReport.Tables[1].NewRow();
                dr["NgayIn"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                dr["TCThanhTien"] = tongtien.ToString("###,###,###,###.##");
                TonKhoReport.Tables[1].Rows.Add(dr);
                txtTK_TC.Text = tongtien.ToString("###,###,###,###.##") + " VND";
                gridTK.DataSource = TonKhoReport.Tables[0];
                button3.Enabled = true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (TonKhoReport != null)
            {
                frmViewReport frmView = new frmViewReport(TonKhoReport);
                frmView.Show();
            }
        }
        private void dtTKEndDate_ValueChanged(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }

        private void cbTKTenNhomSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }
        #endregion
        #region TKHD
        private void rdTKHDByDate_CheckedChanged(object sender, EventArgs e)
        {
            dtTKHDByDate.Enabled = rdTKHDByDate.Checked;
            lblTKHDByMonth.Enabled = !rdTKHDByDate.Checked;
            cbTKHDByMonthMonth.Enabled = !rdTKHDByDate.Checked;
            lblTKHDByMonthYear.Enabled = !rdTKHDByDate.Checked;
            cbTKHDByMonthYear.Enabled = !rdTKHDByDate.Checked;
            lblTKHDByCustomFrom.Enabled = !rdTKHDByDate.Checked;
            dtTKHDByCustomFrom.Enabled = !rdTKHDByDate.Checked;
            lblTKHDByCustomTo.Enabled = !rdTKHDByDate.Checked;
            dtTKHDByCustomTo.Enabled = !rdTKHDByDate.Checked;
            btnTKHDViewReport.Enabled = false;
        }

        private void rdTKHDByCustom_CheckedChanged(object sender, EventArgs e)
        {
            dtTKHDByDate.Enabled = !rdTKHDByCustom.Checked;
            lblTKHDByMonth.Enabled = !rdTKHDByCustom.Checked;
            cbTKHDByMonthMonth.Enabled = !rdTKHDByCustom.Checked;
            lblTKHDByMonthYear.Enabled = !rdTKHDByCustom.Checked;
            cbTKHDByMonthYear.Enabled = !rdTKHDByCustom.Checked;
            lblTKHDByCustomFrom.Enabled = rdTKHDByCustom.Checked;
            dtTKHDByCustomFrom.Enabled = rdTKHDByCustom.Checked;
            lblTKHDByCustomTo.Enabled = rdTKHDByCustom.Checked;
            dtTKHDByCustomTo.Enabled = rdTKHDByCustom.Checked;
            btnTKHDViewReport.Enabled = false;
        }

        private void rdTKHDByMonth_CheckedChanged(object sender, EventArgs e)
        {
            dtTKHDByDate.Enabled = !rdTKHDByMonth.Checked;
            lblTKHDByMonth.Enabled = rdTKHDByMonth.Checked;
            cbTKHDByMonthMonth.Enabled = rdTKHDByMonth.Checked;
            lblTKHDByMonthYear.Enabled = rdTKHDByMonth.Checked;
            cbTKHDByMonthYear.Enabled = rdTKHDByMonth.Checked;
            lblTKHDByCustomFrom.Enabled = !rdTKHDByMonth.Checked;
            dtTKHDByCustomFrom.Enabled = !rdTKHDByMonth.Checked;
            lblTKHDByCustomTo.Enabled = !rdTKHDByMonth.Checked;
            dtTKHDByCustomTo.Enabled = !rdTKHDByMonth.Checked;
            btnTKHDViewReport.Enabled = false;
        }

        private void btnTKHDReport_Click(object sender, EventArgs e)
        {
            //Get Data From DB
            string strWheredate = "";
            string strWhereEmployee = "";
            string strWhereRoom = "";
            string strWhereTax = "";
            DataAccess da = new DataAccess();
            DataSet dsSP = null;
            string strEmployeeName = cbTKHDEmployee.Text;

            if ((strEmployeeName != "") && (strEmployeeName != "Tất cả"))
            {
                strWhereEmployee += " and Ten = '" + strEmployeeName + "' ";
            }
            string strRoomName = cbTKHDRoom.Text;

            if ((strRoomName != "") && (strRoomName != "Tất cả"))
            {
                strWhereRoom += " and TenPhong = '" + strRoomName + "' ";
            }
            if (rdTKHDByDate.Checked)
            {
                DateTime dtDate = dtTKHDByDate.Value;
                strWheredate += " AND Year(Ngayxuat) = " + dtDate.Year + " and Month(Ngayxuat) = " +
                    dtDate.Month + " and Day(Ngayxuat) = " + dtDate.Day;
            }
            else if (rdTKHDByMonth.Checked)
            {
                if (cbTKHDByMonthMonth.Text == "" || cbTKHDByMonthYear.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn thông tin tháng. Xin vui lòng chọn thông tin tháng",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string month = cbTKHDByMonthMonth.Text;
                string year = cbTKHDByMonthYear.Text;
                strWheredate += " AND Year(Ngayxuat) = " + year + " and Month(Ngayxuat) = " + month + " ";
            }
            else if (rdTKHDByCustom.Checked)
            {
                DateTime startDate = dtTKHDByCustomFrom.Value;
                DateTime endDate = dtTKHDByCustomTo.Value;
                if (startDate != null && endDate != null)
                {
                    strWheredate += " AND ( Year(Ngayxuat) > " + startDate.Year +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) > " + startDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + startDate.Year + " and Month(Ngayxuat) = " + startDate.Month + " and Day(Ngayxuat) >= " + startDate.Day + "))" +
                    " AND ( Year(Ngayxuat) < " + endDate.Year +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) < " + endDate.Month + ") " +
                    " or (Year(Ngayxuat) = " + endDate.Year + " and Month(Ngayxuat) = " + endDate.Month + " and Day(Ngayxuat) <= " + endDate.Day + ")) ";
                }
            }
            if (ckVAT.Checked && (!ckNormal.Checked))
            {
                strWhereTax = " AND Hoadonxuat.Thue > 0"; 
            }
            else if ((!ckVAT.Checked) && (ckNormal.Checked))
                strWhereTax = " AND Hoadonxuat.Thue = 0";

            string strQuery = "SELECT Hoadonxuat.IDHoadonXuat, Hoadonxuat.Ngayxuat, Hoadonxuat.Giam as Giam1,Hoadonxuat.Giam&'%' as Giam, Nhanvien.Ten, " +
                "Hoadonxuat.Phuthu, Phong.TenPhong, Hoadonxuat.GioBD, Hoadonxuat.GioKT,Hoadonxuat.Thue as Thue1,Hoadonxuat.Thue&'%' as Thue, " +
                "GiaLoaiPhong.Gia, Hoadonxuat.Ghichu "+
                " FROM ((Hoadonxuat INNER JOIN GiaLoaiPhong ON " +
                "Hoadonxuat.IDGiaLoaiphong = GiaLoaiPhong.IDGiaLoaiPhong) INNER JOIN Phong " +
                "ON Hoadonxuat.IDPhong = Phong.IDPhong) INNER JOIN Nhanvien ON " +
                "Hoadonxuat.IDNhanvien = Nhanvien.IDNhanvien " +
                "WHERE  1 = 1 " + strEmployeeName + strRoomName + strWheredate + strWhereTax;
            string strQuery1 = "SELECT Hoadonxuat.IDHoadonXuat " +
                " FROM ((Hoadonxuat INNER JOIN GiaLoaiPhong ON " +
                "Hoadonxuat.IDGiaLoaiphong = GiaLoaiPhong.IDGiaLoaiPhong) INNER JOIN Phong " +
                "ON Hoadonxuat.IDPhong = Phong.IDPhong) INNER JOIN Nhanvien ON " +
                "Hoadonxuat.IDNhanvien = Nhanvien.IDNhanvien " +
                "WHERE  1 = 1 " + strEmployeeName + strRoomName + strWheredate + strWhereTax;
            string strQuery2 = "SELECT ChitietHDXuat.IDHoadonXuat, ChitietHDXuat.IDChitietHDXuat,ChitietHDXuat.Giam as Giam1,ChitietHDXuat.Giam&'%' as Giam, " +
                "ChitietHDXuat.IDSanpham, SanPham.TenSanPham, SanPham.DVT, ChitietHDXuat.Soluong as Soluong, " +
                " GiaXuatSP.Gia as Gia, ([Gia]*(100 - [Giam1])/100)*[Soluong] AS Thanhtien " +
                "FROM (SanPham INNER JOIN ChitietHDXuat ON SanPham.IDSanPham = ChitietHDXuat.IDSanpham) " +
                "INNER JOIN GiaXuatSP ON SanPham.IDSanPham = GiaXuatSP.IDSanPham " ;

            dsSP = (DataSet)(da.getDataByQuery(strQuery, strQuery1, strQuery2));
            try
            {
                gridControlTKHD.DataSource = dsSP.Tables[0];
            }
            catch
            {
                gridControlTKHD.DataSource = null;
            }
            btnTKHDViewReport.Enabled = true;
            DataTable dt = new DataTable();
            if (dsSP != null)
            {
                TKHDReport = new DataSetTKHD();
                for (int t = 0; t < dsSP.Tables["HoadonXuat"].Rows.Count; t++)
                {
                    DataRow dr = TKHDReport.Tables[0].NewRow();
                    dr["IDHoadonXuat"] = Convert.ToInt32(dsSP.Tables["HoadonXuat"].Rows[t]["IDHoadonXuat"]);
                    int index1 = dsSP.Tables["HoadonXuat"].Rows[t]["Ngayxuat"].ToString().IndexOf(' ');
                    //dr2["NgayBan"] = dsSP.Tables[0].Rows[j]["NgayBan"].ToString().Substring(0, index1);
                    dr["Ngayxuat"] = dsSP.Tables["HoadonXuat"].Rows[t]["Ngayxuat"].ToString().Substring(0, index1);
                    dr["Ten"] = dsSP.Tables["HoadonXuat"].Rows[t]["Ten"];
                    dr["Phuthu"] = dsSP.Tables["HoadonXuat"].Rows[t]["Phuthu"];
                    dr["Thue"] = dsSP.Tables["HoadonXuat"].Rows[t]["Thue1"];
                    dr["TenPhong"] = dsSP.Tables["HoadonXuat"].Rows[t]["TenPhong"];
                    string strBD = dsSP.Tables["HoadonXuat"].Rows[t]["GioBD"].ToString();
                    string strKT = dsSP.Tables["HoadonXuat"].Rows[t]["GioKT"].ToString();
                    index1 = strBD.IndexOf(' ');
                    //dr["GioBD"] = dsSP.Tables["HoadonXuat"].Rows[t]["GioBD"];
                    dr["GioBD"] = strBD.Substring(index1, strBD.Length - index1);
                    index1 = strKT.IndexOf(' ');
                    //dr["GioKT"] = dsSP.Tables["HoadonXuat"].Rows[t]["GioKT"];
                    dr["GioKT"] = strKT.Substring(index1, strKT.Length - index1);
                    dr["Gia"] = Convert.ToInt32(dsSP.Tables["HoadonXuat"].Rows[t]["Gia"]).ToString("###,###,###,###");
                    dr["Ghichu"] = dsSP.Tables["HoadonXuat"].Rows[t]["Ghichu"];
                    dr["Giam"] = dsSP.Tables["HoadonXuat"].Rows[t]["Giam1"];
                    TKHDReport.Tables[0].Rows.Add(dr);
                }
                for (int t = 0; t < dsSP.Tables["ChitietHDXuat"].Rows.Count; t++)
                {
                    DataRow dr = TKHDReport.Tables[1].NewRow();
                    dr["IDHoadonXuat"] = Convert.ToInt32(dsSP.Tables["ChitietHDXuat"].Rows[t]["IDHoadonXuat"]);
                    dr["IDChitietHDXuat"] = dsSP.Tables["ChitietHDXuat"].Rows[t]["IDChitietHDXuat"];
                    dr["IDSanpham"] = dsSP.Tables["ChitietHDXuat"].Rows[t]["IDSanpham"];
                    dr["TenSanPham"] = dsSP.Tables["ChitietHDXuat"].Rows[t]["TenSanPham"];
                    dr["DVT"] = dsSP.Tables["ChitietHDXuat"].Rows[t]["DVT"];
                    dr["Soluong"] = Convert.ToInt32(dsSP.Tables["ChitietHDXuat"].Rows[t]["Soluong"]).ToString("###,###,###,###");
                    dr["Gia"] = Convert.ToInt32(dsSP.Tables["ChitietHDXuat"].Rows[t]["Gia"]).ToString("###,###,###,###");
                    dr["Giam"] = Convert.ToInt32(dsSP.Tables["ChitietHDXuat"].Rows[t]["Giam1"]).ToString("###,###,###,###");
                    dr["Thanhtien"] = Convert.ToInt32(dsSP.Tables["ChitietHDXuat"].Rows[t]["Thanhtien"]).ToString("###,###,###,###");
                    TKHDReport.Tables[1].Rows.Add(dr);
                }
                DataRow dr1 = TKHDReport.Tables[2].NewRow();
                if (rdTKHDByDate.Checked)
                {
                    DateTime dtDate = dtDateDSBHByDate.Value;
                    dr1["FromDate"] = dtDate.ToString("dd/MM/yyyy");
                    dr1["ToDate"] = dtDate.ToString("dd/MM/yyyy");
                }
                else if (rdTKHDByMonth.Checked)
                {
                    string month = cbTKHDByMonthMonth.Text;
                    string year = cbTKHDByMonthYear.Text;
                    dr1["FromDate"] = "Tháng " + month + "//" + year;
                    dr1["ToDate"] = "Tháng " + month + "//" + year;
                }
                else if (rdTKHDByCustom.Checked)
                {
                    DateTime startDate = dtTKHDByCustomFrom.Value;
                    DateTime endDate = dtTKHDByCustomTo.Value;
                    dr1["FromDate"] = startDate.ToString("dd/MM/yyyy");
                    dr1["ToDate"] = endDate.ToString("dd/MM/yyyy");
                }
                if (strEmployeeName == "")
                    dr1["TenNV"] = "Tất cả";
                else
                    dr1["TenNV"] = strEmployeeName;
                if (strRoomName == "")
                    dr1["TenPhong"] = "Tất cả";
                else
                    dr1["TenPhong"] = strRoomName;
                dr1["PrintDate"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
                TKHDReport.Tables[2].Rows.Add(dr1);
            }
        }

        private void btnTKHDViewReport_Click(object sender, EventArgs e)
        {
            if (TKHDReport != null)
            {
                frmViewReport frmView = new frmViewReport(TKHDReport);
                frmView.Show();
            }
            //btnTKHDViewReport.Enabled = false;
        }

        private void dtTKHDByCustomFrom_ValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }

        private void dtTKHDByCustomTo_ValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }

        private void cbTKHDRoom_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }

        private void cbTKHDEmployee_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }

        private void cbTKHDByMonthMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }

        private void cbTKHDByMonthYear_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTKHDViewReport.Enabled = false;
        }
        #endregion




    }
}