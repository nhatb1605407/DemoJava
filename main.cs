using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EmployeeManager {
    public partial class main : Form {
        public main() {
            InitializeComponent();
            displayData("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chucvu.tenchucvu  FROM tt_nhanvien, chucvu WHERE tt_nhanvien.idcv = chucvu.idcv ORDER BY tt_nhanvien.idnv ASC;");
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e) {
            Application.Exit();
        }

        private void displayData(String query) {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Data");
                dgDisplay.DataSource = ds;
                dgDisplay.DataMember = "Data";
                dgDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgDisplay.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgDisplay.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgDisplay.Columns[0].HeaderText = "ID Nhân viên";
                dgDisplay.Columns[1].HeaderText = "Họ tên";
                dgDisplay.Columns[2].HeaderText = "Chức vụ";
                foreach (DataGridViewColumn col in dgDisplay.Columns) {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                dgDisplay.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
        }

        private void btn_Home_Search_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 1 || tabControl1.SelectedIndex == 0) {
                MessageBox.Show(txt_Keyword.Text);
                displayData("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chucvu.tenchucvu  FROM tt_nhanvien, chucvu WHERE tt_nhanvien.idcv = chucvu.idcv and tt_nhanvien.idnv='"+txt_Keyword.Text+"';");
            } else {
                if (tabControl1.SelectedIndex == 2) {
                    loadSalaryList("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chamcong.songaydilam, chamcong.sogiotangca, chamcong.tienthuong, chamcong.tienphat, luong.tongluong, luong.thang, luong.nam FROM tt_nhanvien,chamcong,luong WHERE tt_nhanvien.idnv = chamcong.idnv AND tt_nhanvien.idnv = luong.idnv AND chamcong.thang=luong.thang AND chamcong.nam = luong.nam AND tt_nhanvien.idnv = '" + txt_Keyword.Text + "';");
                }
            }
        }
        private void load_inf(String idnv) {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT idnv, hoten, gioitinh, ngaysinh, diachi, sdt, mail, ngaybatdaulam FROM tt_nhanvien WHERE idnv ='" + idnv + "' ORDER BY idnv ASC;", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    lbl_Home_ID.Text = rd["idnv"].ToString();
                    txt_Home_Name.Text = rd["hoten"].ToString();
                    txt_Home_Birthday.Text = rd["ngaysinh"].ToString();
                    txt_Home_Adress.Text = rd["diachi"].ToString();
                    txt_Home_No_Phone.Text = rd["sdt"].ToString();
                    txt_Home_Email.Text = rd["mail"].ToString();
                    txt_Home_Start_Day.Text = rd["ngaybatdaulam"].ToString();
                    cbo_Home_Sex.SelectedIndex = Convert.ToInt32(rd["gioitinh"].ToString());
                }
                rd.Close();
                cmd = new MySqlCommand("SELECT chucvu.tenchucvu FROM chucvu, tt_nhanvien WHERE tt_nhanvien.idcv = chucvu.idcv and tt_nhanvien.idnv = '" + idnv + "';", conn);
                rd = cmd.ExecuteReader();
                String chucvu = "";
                while (rd.Read()) {
                    chucvu = rd["tenchucvu"].ToString();
                }
                rd.Close();

                cmd = new MySqlCommand("SELECT tenchucvu FROM chucvu;", conn);
                rd = cmd.ExecuteReader();
                cbo_Home_Position.Items.Clear();
                while (rd.Read()) {
                    cbo_Home_Position.Items.Add(rd["tenchucvu"].ToString());
                }
                cbo_Home_Position.SelectedItem = chucvu;
                rd.Close();

                cmd = new MySqlCommand("SELECT DISTINCT thang FROM luong ORDER BY thang ASC;", conn);
                rd = cmd.ExecuteReader();
                cbo_Home_Month.Items.Clear();
                while (rd.Read()) {
                    cbo_Home_Month.Items.Add(rd["thang"].ToString());
                }
                rd.Close();

                cmd = new MySqlCommand("SELECT DISTINCT nam FROM luong ORDER BY nam ASC;", conn);
                rd = cmd.ExecuteReader();
                cbo_Home_Year.Items.Clear();
                while (rd.Read()) {
                    cbo_Home_Year.Items.Add(rd["nam"].ToString());

                }
                cbo_Home_Month.SelectedIndex = cbo_Home_Month.Items.Count - 1;
                cbo_Home_Year.SelectedIndex = 0;
                load_salary(lbl_Home_ID.Text, cbo_Home_Month.SelectedItem.ToString(), cbo_Home_Year.SelectedItem.ToString());
                conn.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Load Infomation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void load_salary(String id, string month, string year) {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT tongluong FROM luong WHERE idnv ='" + id + "' and thang =" + month + " and nam=" + year + ";", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                String tongLuong = null;
                while (rd.Read()) {
                    tongLuong = rd["tongluong"].ToString();
                }
                if (tongLuong == null) {
                    txt_Home_Salary.Text = "0";
                } else {
                    txt_Home_Salary.Text = tongLuong;
                }
                rd.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Load Salary Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadTimeKeeping(String idnv) {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            lbl_Time_ID.Text = idnv;
            try {
                String query = "SELECT songaydilam, sogiotangca, tienthuong, tienphat FROM chamcong WHERE idnv = '" + idnv + " AND thang =" + cbo_Time_Month.SelectedItem.ToString() + " AND nam = " + cbo_Time_Year.SelectedItem.ToString() + ";";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    txt_Timekeeping_Days_Worked.Text = rd["songaydilam"].ToString();
                    txt_Timekeeping_Overtime.Text = rd["sogiolamthem"].ToString();
                    txt_Timekeeping_Bonus.Text = rd["tienthuong"].ToString();
                    txt_Timekeeping_Fine.Text = rd["tienphat"].ToString();
                }
                rd.Close();

                query = "SELECT hoten FROM tt_nhanvien WHERE idnv = '" + idnv + ";";
                cmd = new MySqlCommand(query, conn);
                rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    txt_Timekeeping_Name.Text = rd["hoten"].ToString();
                }


            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Load Salary Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void calc_Salary() {
            Int16 workeddays, overtime, bonus, fine;
            if (txt_Timekeeping_Days_Worked.Text.Equals("") == true) {
                workeddays = 0;
            } else {
                workeddays = Convert.ToInt16(txt_Timekeeping_Days_Worked.Text);
            }

            if (txt_Timekeeping_Overtime.Text.Equals("") == true) {
                overtime = 0;
            } else {
                overtime = Convert.ToInt16(txt_Timekeeping_Overtime.Text);
            }

            if (txt_Timekeeping_Bonus.Text.Equals("") == true) {
                bonus = 0;
            } else {
                bonus = Convert.ToInt16(txt_Timekeeping_Bonus.Text);
            }

            if (txt_Timekeeping_Fine.Text.Equals("") == true) {
                fine = 0;
            } else {
                fine = Convert.ToInt16(txt_Timekeeping_Fine.Text);
            }

            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            Int32 wage = 0;
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT mucluong.mucluong FROM tt_nhanvien, mucluong WHERE tt_nhanvien.idnv ='" + lbl_Time_ID.Text + "' AND tt_nhanvien.idcv = mucluong.idcv ", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    wage = Convert.ToInt32(rd["mucluong.mucluong"].ToString());
                }
                rd.Close();
                conn.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Load Wage Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            long totalsalary = workeddays * wage + overtime / 24 * wage * 2 + bonus - fine;
            lblSalary.Text = totalsalary.ToString();
        }

        private void dgDisplay_CellClick(object sender, DataGridViewCellEventArgs e) {
            load_inf(dgDisplay.CurrentRow.Cells["idnv"].Value.ToString());
        }

        private void cbo_Home_Month_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbo_Home_Year.SelectedIndex != -1) {
                load_salary(lbl_Home_ID.Text.ToString(), cbo_Home_Month.SelectedItem.ToString(), cbo_Home_Year.SelectedItem.ToString());
            } else {
                txt_Home_Salary.Text = null;
            }
        }

        private void cbo_Home_Year_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbo_Home_Month.SelectedIndex != -1) {
                load_salary(lbl_Home_ID.Text.ToString(), cbo_Home_Month.SelectedItem.ToString(), cbo_Home_Year.SelectedItem.ToString());
            } else {
                txt_Home_Salary.Text = null;
            }
        }

        private void btn_Home_Refesh_Click(object sender, EventArgs e) {
            displayData("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chucvu.tenchucvu  FROM tt_nhanvien, chucvu WHERE tt_nhanvien.idcv = chucvu.idcv ORDER BY tt_nhanvien.idnv ASC;");
        }


        private Boolean update(String id, String name, Int32 sex, String address, String mail, String phone, String Start_Day, String cv) {
            try {
                String query = "UPDATE tt_nhanvien SET hoten = '" + name + "', gioitinh = " + sex + ", ngaysinh = '" + txt_Home_Birthday.Text + "', diachi = '" + address + "', mail = '" + mail + "', sdt = '" + phone + "', ngaybatdaulam = '" + Start_Day + "' WHERE idnv = '" + id + "';";
                MySqlConnection conn = new MySqlConnection();
                conn = conDB.getConnection();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "SELECT idcv FROM chucvu WHERE tenchucvu = '" + cv + "' ORDER BY idcv ASC;";
                cmd = new MySqlCommand(query, conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                String idcv = null;
                while (rd.Read()) {
                    idcv = rd["idcv"].ToString();
                }
                rd.Close();

                query = "UPDATE tt_nhanvien SET idcv = '" + idcv + "' WHERE idnv = '" + id + "';";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                return true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Update Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Cập nhận thông tinh nhân viên " + lbl_Home_ID.Text + "\nHành động này không thể hoàn tác!", "Cập nhật thông tinh nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                if (update(lbl_Home_ID.Text, txt_Home_Name.Text, cbo_Home_Sex.SelectedIndex, txt_Home_Adress.Text, txt_Home_Email.Text, txt_Home_No_Phone.Text, txt_Home_Start_Day.Text, cbo_Home_Position.SelectedItem.ToString())) {
                    MessageBox.Show("Đã cập nhật thông tinh nhân viên thành công!", "Update successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txt_Home_Keyword_MouseClick(object sender, MouseEventArgs e) {
            txt_Keyword.Text = null;
        }

        private void btn_Home_Delete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Xóa thông tin nhân viên " + lbl_Home_ID.Text + "\nHành động này không thể hoàn tác!", "Xóa thông tin nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                if (delete()) {
                    MessageBox.Show("Đã xóa thông tin nhân viên " + lbl_Home_ID.Text + " khỏi cơ sở dữ liệu. ", "Delete Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private Boolean delete() {
            try {
                MySqlConnection conn = new MySqlConnection();
                conn = conDB.getConnection();
                String query = "INSERT INTO nv_nghiviec(idnv, hoten, ngaybatdaulam, ngaynghiviec) VALUES('" + lbl_Home_ID.Text + "', '" + txt_Home_Name.Text + "', '" + txt_Home_Start_Day.Text + "', '" + DateTime.Today.ToString() + "');";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();


                query = "SELECT idcv FROM tt_nhanvien WHERE idnv = '" + lbl_Home_ID.Text + "' ORDER BY idcv ASC;";
                cmd = new MySqlCommand(query, conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                String idcv = null;
                while (rd.Read()) {
                    idcv = rd["idcv"].ToString();
                }
                rd.Close();

                query = "UPDATE nv_nghiviec SET idchucvu = '" + idcv + "' WHERE idnv ='" + lbl_Home_ID.Text + "';";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "DELETE FROM tt_nhanvien WHERE idnv = '" + lbl_Home_ID.Text + "';";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "DELETE FROM chamcong WHERE idnv = '" + lbl_Home_ID.Text + "';";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "DELETE FROM luong WHERE idnv = '" + lbl_Home_ID.Text + "';";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Delete Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void loadSalaryList(String query) {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Data");
                dgDisplay.DataSource = ds;
                dgDisplay.DataMember = "Data";
                dgDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgDisplay.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                for (int index = 2; index < 9; index++) {
                    dgDisplay.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                dgDisplay.Columns[0].HeaderText = "ID Nhân viên";
                dgDisplay.Columns[1].HeaderText = "Họ tên";
                dgDisplay.Columns[2].HeaderText = "Số ngày làm";
                dgDisplay.Columns[3].HeaderText = "Giờ làm thêm";
                dgDisplay.Columns[4].HeaderText = "Thưởng";
                dgDisplay.Columns[5].HeaderText = "Phạt";
                dgDisplay.Columns[6].HeaderText = "Tổng lương";
                dgDisplay.Columns[7].HeaderText = "Tháng";
                dgDisplay.Columns[8].HeaderText = "Năm";
                foreach (DataGridViewColumn col in dgDisplay.Columns) {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                dgDisplay.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
        }

        private string makeNewID() {
            String tmp = (getLastID() + 1).ToString();
            if (tmp.Length < 3) {
                 for (int index = tmp.Length; index < 3; index++) {
                    tmp = "0" + tmp;
                }
            }
            return "NV" + tmp;
        }
        private int getLastID() {
            int tmp = 0;
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT idnv FROM tt_nhanvien ORDER BY idnv ASC;", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    if (tmp < cutNoID(rd["idnv"].ToString())) {
                        tmp = cutNoID(rd["idnv"].ToString());
                    }
                }
                rd.Close();

                cmd = new MySqlCommand("SELECT DISTINCT idnv FROM nv_nghiviec ORDER BY idnv ASC;", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    if (tmp < cutNoID(rd["idnv"].ToString())) {
                        tmp = cutNoID(rd["idnv"].ToString());
                    }
                }
                rd.Close();
                conn.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tmp;
        }
        private int cutNoID(String id) {
            String tmp;
            try {
                tmp = id.Substring(3);
                return Convert.ToInt32(tmp);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Convert Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private void btn_Log_Out_Click(object sender, EventArgs e) {
           
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e) {
            
         
        }
        private void loadCboCV() {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT tenchucvu FROM chucvu;", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                cbo_Edit_Position.Items.Clear();
                while (rd.Read()) {
                    cbo_Edit_Position.Items.Add(rd["tenchucvu"].ToString());
                }
                rd.Close();
                cbo_Edit_Position.SelectedIndex = 0;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Load Combobox Items Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Edit_Add_Click(object sender, EventArgs e) {
           if (addEmploy()) {
                MessageBox.Show("Đã thêm thành công nhân viên " + txt_Add_ID.Text + " vào cơ sở dữ liệu.", "Added Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear_selection();
            }
        }

        private Boolean addEmploy() {
            MySqlConnection conn = new MySqlConnection();
            conn = conDB.getConnection();
            try {
                String query = "INSERT INTO tt_nhanvien(idnv) VALUES('" + txt_Add_ID.Text + "')";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Here!");
                query = "INSERT INTO luong(idnv, thang, nam) VALUES('" + txt_Add_ID.Text + "',"+cbo_Start_Month.SelectedItem.ToString()+", "+cbo_Start_Year.SelectedItem.ToString()+")";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                
                query = "INSERT INTO chamcong(idnv) VALUES('" + txt_Add_ID.Text + "')";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
                String date = txt_Add_Day.Text + "-" + cbo_Start_Month.SelectedItem.ToString() + "-" + cbo_Start_Year.SelectedItem.ToString();
                update(txt_Add_ID.Text, txt_Edit_Name.Text, cbo_Edit_Sex.SelectedIndex, txt_Edit_Adress.Text, txt_Edit_Email.Text, txt_Edit_No_Phone.Text, date, cbo_Edit_Position.SelectedItem.ToString());
                return true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Adding Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }
        private void clear_selection() {
            txt_Add_ID.Text = makeNewID();
            txt_Edit_Adress.Text = null;
            txt_Edit_Email.Text = null;
            txt_Add_Day.Text = null;
            txt_Edit_Name.Text = null;
            txt_Edit_No_Phone.Text = null;
            cbo_Edit_Sex.SelectedIndex = 0;
            cbo_Edit_Position.SelectedIndex = 0;
            cbo_Edit_Position.Text = null;
            cbo_Edit_Sex.Text = null;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0 || tabControl1.SelectedIndex == 1) {
                dgDisplay.DataSource = null;
                displayData("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chucvu.tenchucvu  FROM tt_nhanvien, chucvu WHERE tt_nhanvien.idcv = chucvu.idcv ORDER BY tt_nhanvien.idnv ASC;");
                txt_Add_ID.Text = makeNewID();
                loadCboCV();
            } else {

                if (tabControl1.SelectedIndex == 2) {
                    dgDisplay.DataSource = null;
                    loadSalaryList("SELECT tt_nhanvien.idnv, tt_nhanvien.hoten, chamcong.songaydilam, chamcong.sogiotangca, chamcong.tienthuong, chamcong.tienphat, luong.tongluong, luong.thang, luong.nam FROM tt_nhanvien,chamcong,luong WHERE tt_nhanvien.idnv = chamcong.idnv AND tt_nhanvien.idnv = luong.idnv AND chamcong.thang=luong.thang AND chamcong.nam = luong.nam");
                }
            }
        }

        private void main_Load(object sender, EventArgs e)
        {

        }
    }
    
}
