using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BKIT.Model;
using BKIT.Entities;
using DevExpress.XtraEditors.Controls;
using System.Globalization;

namespace Karaoke.MDIForms
{
    public partial class frmEmployeeManager : DevExpress.XtraEditors.XtraForm
    {
        // Local variables
        private CustomListBoxItem currentItem;
        private int currentIndex;
        private int highlightIndex = -1;
        private Nhanvien oldSelectedNhanvien = new Nhanvien();
        private bool enableItemCheckEvent = true;

        public static QuyenTruycap QuyenMacDinh_Quanly =
            new QuyenTruycap(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        public static QuyenTruycap QuyenMacDinh_Nguoidung =
            new QuyenTruycap(0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1);
        public static QuyenTruycap QuyenMacDinh_Khach =
            new QuyenTruycap(1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0);
        
        public static string[] QuyentruycapList  = 
            {"Vận hành", "Hoá đơn nhập", "Cài đặt", "Quản lý nhân viên",
             "Hoá đơn xuất", "Quản lý sản phẩm", "Quản lý phòng", "Báo cáo",
             "Khách hàng", "Khuyến mãi", "Tồn kho"};
        public enum Quyen
        {
            Vanhanh = 0,
            Hoadonnhap,
            Caidat,
            Nhanvien,
            Hoadonxuat,
            Sanpham,
            Phong,
            Baocao,
            Khachhang,
            Khuyenmai,
            Tonkho
        }

        public CheckedListBoxItem[] QuyenTruyCapItems;// = new CheckedListBoxItem[0];

        public static QuyenTruycap getDefaultPermissionByGroupname(string UserGroupName)
        {
            switch (UserGroupName)
            {
                case "Quản lý":
                    return QuyenMacDinh_Quanly;
                case "Người dùng":
                    return QuyenMacDinh_Nguoidung;
                case "Khách":
                default:
                    return QuyenMacDinh_Khach;
            }
        }

        public frmEmployeeManager()
        {
            InitializeComponent();
        }

        private void frmEmployeeManager_Load(object sender, EventArgs e)
        {
            lblStatusThongTinTaiKhoan.Text = "";
            lblStatusUpdatePermission.Text = "";
            LoadCurrentUser();
            LoadOtherUser("");

            //Add UserGroupName to Combox
            DataSet ds = new DataAccess().getAllLoaiNhanvien();
            string UserGroupName;

            int intRowsCount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < intRowsCount; i++)
            {
                UserGroupName = Convert.ToString(ds.Tables[0].Rows[i]["TenLoaiNV"]);
                cboUserGroupName.Properties.Items.Add(UserGroupName);
            }

            chkListBoxPermission_Init();

            customListBoxCurrentUser.SelectedIndex = 0;
            customListBoxOtherUser.SelectedIndex = -1;
            currentItem = (CustomListBoxItem)customListBoxCurrentUser.SelectedItem;
            currentIndex = customListBoxCurrentUser.SelectedIndex;   
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region OtherUser List
        private void LoadOtherUser(string added_username)
        {
            if (Program.userLevel != Level.Admin)
            {
                TabControlOtherAccount.Enabled = false;
                TabControlOtherAccount.Visible = false;
            }
            else
            {
                TabControlOtherAccount.Enabled = true;
                TabControlOtherAccount.Visible = true;

                DataAccess da = new DataAccess();
                DataSet ds = da.getAllNhanvien();
                int intRowsCount = ds.Tables[0].Rows.Count;
                highlightIndex = 0;
                for (int i = 0; i < intRowsCount; i++)
                {
                    if (!string.Equals(ds.Tables[0].Rows[i]["Username"].ToString(), Program.username))
                    {
                        customListBoxOtherUser.Items.Add(new CustomListBoxItem(
                            Convert.ToString(ds.Tables[0].Rows[i]["Ten"]),
                            Convert.ToString(ds.Tables[0].Rows[i]["Username"]),
                            Convert.ToString(ds.Tables[0].Rows[i]["Loai"])));
                        if (ds.Tables[0].Rows[i]["Username"].ToString() == added_username)
                        {
                            highlightIndex = i;
                        }
                    }
                }
                highlightIndex--;
            }
        }

        private void customListBoxOtherUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customListBoxOtherUser.SelectedIndex >= 0)
            {
                currentItem = (CustomListBoxItem)customListBoxOtherUser.SelectedItem;
                currentIndex = customListBoxOtherUser.SelectedIndex;
                customListBoxCurrentUser.SelectedIndex = -1;
                Update_Info();
            }
        }

        #endregion

        #region CurrrentUser List
        private void LoadCurrentUser()
        {
            DataAccess da = new DataAccess();
            Nhanvien nhvien = da.getNhanvienByUsername_Password(Program.username, Program.password);
            customListBoxCurrentUser.Items.Add(new BKIT.Entities.CustomListBoxItem(nhvien.Ten, nhvien.Username, nhvien.Loai));
        }

        private void customListBoxCurrentUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customListBoxCurrentUser.SelectedIndex >= 0)
            {
                currentItem = (CustomListBoxItem)customListBoxCurrentUser.SelectedItem;
                currentIndex = customListBoxCurrentUser.SelectedIndex;
                customListBoxOtherUser.SelectedIndex = -1;
                Update_Info();
            }
        }
        #endregion

        private void Update_Info()
        {
            if (TabControlManagement.SelectedTabPage == TabPagePersonalInfo)
            {
                TabPagePersonalInfo_Update(currentItem.Username);
            }
            else if (TabControlManagement.SelectedTabPage == TabPagePassword)
            {
                TabPagePassword_Update();
            }
            else
            {
                TabPageAccessPermission_Update();
            }
        }

        private void TabPagePersonalInfo_Update(string p)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime datetime = new DateTime();
            DataAccess da = new DataAccess();
            Nhanvien nhanvien = da.getNhanvienbyUserName(p);
            oldSelectedNhanvien.Ten = txtName.Text = nhanvien.Ten;
            oldSelectedNhanvien.Gioitinh = cboSex.Text = nhanvien.Gioitinh;
            oldSelectedNhanvien.Chucvu = txtPosition.Text = nhanvien.Chucvu;
            oldSelectedNhanvien.Diachi = txtAddress.Text = nhanvien.Diachi;
            oldSelectedNhanvien.SoDT = txtPhone.Text = nhanvien.SoDT;
            cboBirthDay.Text = nhanvien.Ngaysinh.Day.ToString();
            cboBirthMonth.Text = nhanvien.Ngaysinh.ToString("MM");
            txtBirthYear.Text = nhanvien.Ngaysinh.Year.ToString();
            //string s = cboBirthDay.Text + "/" + cboBirthMonth.Text + "/" + txtBirthYear.Text;
            string s = cboBirthDay.Text +cboBirthMonth.Text + txtBirthYear.Text;
            oldSelectedNhanvien.Loai = cboUserGroupName.Text = nhanvien.Loai;
            DateTime.TryParseExact(s, "ddMMyyyy", enUS, DateTimeStyles.None,out datetime);
            oldSelectedNhanvien.Ngaysinh = datetime;
            //oldSelectedNhanvien.Ngaysinh = Convert.ToDateTime(s);
            

            // Load Avatar
            //Image image = CustomListBox.ImageFromStr(employee.Avatar);
            //pictureAvatar.Image = image;
        }

        private void TabPagePassword_Update()
        {
            if (string.Equals(currentItem.Username, Program.username))
            {
                txtPassword.Enabled = true;
                txtNewPassword.Enabled = true;
                txtNewPasswordConfirm.Enabled = true;
                txtPasswordHint.Enabled = true;
            }
            else
            {
                txtPassword.Enabled = false;
                txtNewPassword.Enabled = false;
                txtNewPasswordConfirm.Enabled = false;
                txtPasswordHint.Enabled = false;
            }
            txtPassword.Text = "";
            txtNewPassword.Text = "";
            txtNewPasswordConfirm.Text = "";
            txtPasswordHint.Text = "";
        }

        private void TabPageAccessPermission_Update()
        {
            DataAccess da = new DataAccess();
            Nhanvien nhanvien = da.getNhanvienbyUserName(currentItem.Username);

            if (nhanvien.Loai != "Quản lý" && Program.userLevel == Level.Admin)
            {
                chkListBoxPermission.Enabled = true;
                simpleButtonDefaut.Enabled = true;
            }
            else
            {
                chkListBoxPermission.Enabled = false;
                simpleButtonDefaut.Enabled = false;
            }

            QuyenTruycap quyentruycap = da.getQuyenTruycapByID(nhanvien.IDQuyenTruycap); // item in list box
            if (quyentruycap == null)
            {
                quyentruycap = getDefaultPermissionByGroupname(nhanvien.Loai);
            }
            chkListBoxPermission_Update(quyentruycap); 
        }

        private void chkListBoxPermission_Init()
        {
            QuyenTruyCapItems = new CheckedListBoxItem[QuyentruycapList.Length];
            for (int i = 0; i < QuyentruycapList.Length; i++)
            {
                QuyenTruyCapItems[i] = new CheckedListBoxItem(QuyentruycapList[i]);
            }
            chkListBoxPermission.Items.AddRange(QuyenTruyCapItems);
        }

        private void chkListBoxPermission_Update(QuyenTruycap quyentruycap)
        {
            enableItemCheckEvent = false;
            if (quyentruycap.Vanhanh == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Vanhanh), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Vanhanh), CheckState.Unchecked);

            if (quyentruycap.HoadonNhap == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Hoadonnhap), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Hoadonnhap), CheckState.Unchecked);

            if (quyentruycap.Setting == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Caidat), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Caidat), CheckState.Unchecked);

            if (quyentruycap.Nhanvien == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Nhanvien), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Nhanvien), CheckState.Unchecked);

            if (quyentruycap.HoadonxuatSP == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Hoadonxuat), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Hoadonxuat), CheckState.Unchecked);

            if (quyentruycap.Sanpham == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Sanpham), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Sanpham), CheckState.Unchecked);

            if (quyentruycap.Phong == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Phong), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Phong), CheckState.Unchecked);

            if (quyentruycap.Report == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Baocao), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Baocao), CheckState.Unchecked);

            if (quyentruycap.Khachhang == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Khachhang), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Khachhang), CheckState.Unchecked);

            if (quyentruycap.Khuyenmai == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Khuyenmai), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Khuyenmai), CheckState.Unchecked);

            if (quyentruycap.Tonkho == 1)
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Tonkho), CheckState.Checked);
            else
                chkListBoxPermission.SetItemCheckState(Convert.ToInt32(Quyen.Tonkho), CheckState.Unchecked);

            enableItemCheckEvent = true;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUser adduser = new frmAddUser();
            if (adduser.ShowDialog() == DialogResult.OK)
            {
                customListBoxOtherUser.Items.Clear();
                LoadOtherUser(frmAddUser.AddedUser.Username);
                customListBoxOtherUser.SelectedIndex = highlightIndex;
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string username = currentItem.Username;    // = lstUserAccount1.SelectedItem.ToString();

            if (string.Equals(username, Program.username))
            {
                MessageBox.Show("Lỗi. Tài khoản " + Program.username + " đang được đăng nhập",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataAccess da = new DataAccess();
            Nhanvien employee = da.getNhanvienbyUserName(username);
            DialogResult dresult = MessageBox.Show("Tài khoản: " + employee.Username + " sẽ bị xóa?",
                "Cảnh báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dresult == DialogResult.Yes)
            {
                if (da.deleteNhanvien(employee))
                {
                    MessageBox.Show("Đã xóa tài khoản: " + employee.Username,
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    currentIndex = customListBoxOtherUser.SelectedIndex;
                    customListBoxOtherUser.Items.RemoveAt(currentIndex);
                    if (customListBoxOtherUser.Items.Count > 0)
                    {
                        if (currentIndex < customListBoxOtherUser.Items.Count)
                            customListBoxOtherUser.SelectedIndex = currentIndex;
                        else
                            customListBoxOtherUser.SelectedIndex = currentIndex - 1;
                    }
                }
            }
        }

        #region Update User Info
        private void txtName_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            string s = txtName.Text;
            if (s == "")
            {
                lblStatusThongTinTaiKhoan.Text = "Thông tin Họ và tên không hợp lệ";
                txtName.Text = emp.Ten;
                txtName.SelectAll();
                return;
            }
            lblStatusThongTinTaiKhoan.Text = "";
            emp.Ten = s;
            if (da.updateNhanvien(emp))
            {
                if (string.Equals(Program.username, currentItem.Username))
                {
                    customListBoxCurrentUser.Items.Clear();
                    LoadCurrentUser();
                }
                else
                {
                    LoadOtherUser("");
                }
            }
        }

        private void cboSex_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            emp.Gioitinh = cboSex.Text;

            da.updateNhanvien(emp);
        }

        private void txtPosition_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            emp.Chucvu = txtPosition.Text;
            da.updateNhanvien(emp);
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            emp.Diachi = txtAddress.Text;
            da.updateNhanvien(emp);
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            string s = txtPhone.Text;
            try
            {
                double tmp;
                if (!double.TryParse(s, out tmp) && !string.IsNullOrEmpty(s))
                {
                    lblStatusThongTinTaiKhoan.Text = "Số điện thoại không hợp lệ!!!";
                    txtPhone.Text = emp.SoDT;
                    txtPhone.SelectAll();
                    return;
                }
            }
            catch
            {
                lblStatusThongTinTaiKhoan.Text = "Số điện thoại không hợp lệ!!!";
                txtPhone.Text = emp.SoDT;
                return;
            }
            lblStatusThongTinTaiKhoan.Text = "";
            emp.SoDT = s;
            
            da.updateNhanvien(emp);
        }


        #endregion

        private void cboBirthDay_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            //string s = cboBirthDay.Text + "/" + cboBirthMonth.Text + "/" + txtBirthYear.Text;
            int bYear = Convert.ToInt16(txtBirthYear.Text);
            int bMonth = cboBirthMonth.SelectedIndex + 1;
            int bDay = cboBirthDay.SelectedIndex + 1;
            try
            {
                //emp.Ngaysinh = Convert.ToDateTime(s);
                emp.Ngaysinh = new DateTime(bYear, bMonth, bDay);
            }
            catch
            {
                lblStatusThongTinTaiKhoan.Text = "Thông tin Ngày sinh không hợp lệ";
                cboBirthDay.Focus();
                return;
            }
            lblStatusThongTinTaiKhoan.Text = "";
            da.updateNhanvien(emp);
        }

        private void cboBirthMonth_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            //string s = cboBirthDay.Text + "/" + cboBirthMonth.Text + "/" + txtBirthYear.Text;
            int bYear = Convert.ToInt16(txtBirthYear.Text);
            int bMonth = cboBirthMonth.SelectedIndex + 1;
            int bDay = cboBirthDay.SelectedIndex + 1;
            try
            {
                //emp.Ngaysinh = Convert.ToDateTime(s);
                emp.Ngaysinh = new DateTime(bYear, bMonth, bDay);
            }
            catch
            {
                lblStatusThongTinTaiKhoan.Text = "Thông tin Ngày sinh không hợp lệ";
                cboBirthMonth.Focus();
                return;
            }
            lblStatusThongTinTaiKhoan.Text = "";
            da.updateNhanvien(emp);
        }

        private void txtBirthYear_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
            //string s = cboBirthDay.Text + "/" + cboBirthMonth.Text + "/" + txtBirthYear.Text;
            int bYear = Convert.ToInt16(txtBirthYear.Text);
            int bMonth = cboBirthMonth.SelectedIndex + 1;
            int bDay = cboBirthDay.SelectedIndex + 1;
            try
            {
                //emp.Ngaysinh = Convert.ToDateTime(s);
                emp.Ngaysinh = new DateTime(bYear, bMonth, bDay);
            }
            catch
            {
                lblStatusThongTinTaiKhoan.Text = "Thông tin Ngày sinh không hợp lệ";
                txtBirthYear.Focus();
                return;
            }
            lblStatusThongTinTaiKhoan.Text = "";
            da.updateNhanvien(emp);
        }

        private void cboUserGroupName_Leave(object sender, EventArgs e)
        {
            string username = currentItem.Username;
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(username);
                        
            // Admin can not modify "Admin" GroupName
            if ((emp.Loai == "Quản lý") || (username == Program.username))
            {
                cboUserGroupName.Text = emp.Loai;
                return;
            }
            else
            {
                lblStatusThongTinTaiKhoan.Text = "";
                string s = cboUserGroupName.Text;
                if (!string.Equals(s, oldSelectedNhanvien.Loai))
                {
                    emp.Loai = s;
                    da.updateNhanvien(emp);
                    QuyenTruycap quyentruycap = getDefaultPermissionByGroupname(cboUserGroupName.Text);
                    quyentruycap.IDQuyentruycap = emp.IDQuyenTruycap;
                    quyentruycap.TenLoaiNV = s;
                    quyentruycap.Ngaythietlap = DateTime.Now.Date;
                    da.updateQuyenTruycap(quyentruycap);
                    
                    int selectedIndex = currentIndex;
                    customListBoxOtherUser.Items.Clear();
                    LoadOtherUser("");
                    customListBoxOtherUser.SelectedIndex = selectedIndex;
                }
            } 
        }

        private void chkListBoxPermission_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (enableItemCheckEvent == false)
                return;
            DataAccess da = new DataAccess();
            Nhanvien employee = da.getNhanvienbyUserName(currentItem.Username);
            QuyenTruycap permission = da.getQuyenTruycapByID(employee.IDQuyenTruycap);

            permission.Vanhanh = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Vanhanh)].CheckState);

            permission.HoadonNhap = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Hoadonnhap)].CheckState);

            permission.Setting = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Caidat)].CheckState);

            permission.Nhanvien = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Nhanvien)].CheckState);

            permission.HoadonxuatSP = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Hoadonxuat)].CheckState);

            permission.Sanpham = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Sanpham)].CheckState);

            permission.Phong = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Phong)].CheckState);

            permission.Report = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Baocao)].CheckState);

            permission.Khachhang = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Khachhang)].CheckState);

            permission.Khuyenmai = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Khuyenmai)].CheckState);

            permission.Tonkho = Convert.ToInt32(
                chkListBoxPermission.Items[Convert.ToInt32(Quyen.Tonkho)].CheckState);

            if (lblStatusUpdatePermission.ForeColor == System.Drawing.Color.Black)
            {
                lblStatusUpdatePermission.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lblStatusUpdatePermission.ForeColor = System.Drawing.Color.Black;
            }

            permission.IDQuyentruycap = employee.IDQuyenTruycap;
            permission.TenLoaiNV = employee.Loai;
            permission.Ngaythietlap = DateTime.Now.Date;
            da.updateQuyenTruycap(permission);
        }

        private void TabControlManagement_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            Update_Info();
        }

        private void simpleButtonDefaut_Click(object sender, EventArgs e)
        {
            DataAccess da = new DataAccess();
            Nhanvien emp = da.getNhanvienbyUserName(currentItem.Username);
            QuyenTruycap quyentruycap = getDefaultPermissionByGroupname(emp.Loai);
            quyentruycap.IDQuyentruycap = emp.IDQuyenTruycap;
            quyentruycap.TenLoaiNV = emp.Loai;
            quyentruycap.Ngaythietlap = DateTime.Now.Date;
            da.updateQuyenTruycap(quyentruycap);
            Update_Info();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
            txtNewPassword.Text = "";
            txtNewPasswordConfirm.Text = "";
            txtPasswordHint.Text = "";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string currentPassword;
            string newPassword;
            string newPasswordConfirm;

            if (string.Equals(currentItem.Username, Program.username))
            {
                // Check current password
                currentPassword = txtPassword.Text;
                if (currentPassword == "")
                {
                    //lblMessPassword.Text = "Nhập mật khẩu hiện tại";
                    MessageBox.Show("Nhập mật khẩu hiện tại",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Focus();
                    return;
                }
                else
                {
                    DataAccess da = new DataAccess();
                    Nhanvien employee = da.getNhanvienbyUserName(currentItem.Username);
                    if (!string.Equals(currentPassword, employee.Password))
                    {
                        //lblMessPassword.Text = "Mật khẩu không đúng!";
                        MessageBox.Show("Mật khẩu không đúng!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        txtPassword.SelectAll();
                        txtNewPassword.Text = "";
                        txtNewPasswordConfirm.Text = "";
                        return;
                    }
                }
            }

            // Check new password
            newPassword = txtNewPassword.Text;
            newPasswordConfirm = txtNewPasswordConfirm.Text;
            if (newPassword == "")
            {
                //lblMessPassword.Text = "Nhập mật khẩu hiện tại";
                MessageBox.Show("Vui lòng nhập Mật khẩu mới",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewPassword.Focus();
                return;
            }

            if (newPasswordConfirm == "")
            {
                //lblMessPassword.Text = "Nhập mật khẩu hiện tại";
                MessageBox.Show("Vui lòng nhập Xác nhận lại Mật khẩu mới",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewPasswordConfirm.Focus();
                return;
            }

            if (String.Equals(newPassword, newPasswordConfirm))
            {
                DataAccess da = new DataAccess();
                Nhanvien employee = da.getNhanvienbyUserName(currentItem.Username);
                employee.Password = newPassword;
                //employee.PasswordHint = txtPasswordHint.Text;
                if (da.updateNhanvien(employee))
                {
                    MessageBox.Show("Thay đổi mật khẩu thành công",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtPassword.Text = "";
                txtNewPassword.Text = "";
                txtNewPasswordConfirm.Text = "";
                txtPasswordHint.Text = "";
            }
            else
            {
                MessageBox.Show("Mật khẩu mới và Mật khẩu xác nhận phải giống nhau!",
                       "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //lblMessPasswordRetype.Text = "Mật khẩu mới và Mật khẩu xác nhận phải giống nhau!";
                txtNewPasswordConfirm.Text = "";
                txtNewPassword.Focus();
                txtNewPassword.SelectAll();
            }
        }
    }
}