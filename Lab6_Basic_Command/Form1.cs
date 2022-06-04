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


namespace Lab6_Basic_Command
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			// kết nối tới csdl
			string connectionString = "server=MSI\\SQLEXPRESS; database = RestaurantManagementDemo; Integrated Security = true; ";
			var sqlConnection = new SqlConnection(connectionString);
			var sqlCommand = sqlConnection.CreateCommand();

			// thiết lập lệnh truy vấn cho đối tượng command
			string query = "SELECT ID, Name, Type FROM Category";
			sqlCommand.CommandText = query;
			// mở kết nối tới csdl
			sqlConnection.Open();

			// đọc csdl từ sql
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			// gọi hàm hiển thị lên màn hình
			this.DisplayCategory(sqlDataReader);

			// đóng kết nối
			sqlConnection.Close();
		}

		private void DisplayCategory(SqlDataReader sqlDataReader)
		{
			// xóa tất cả các dòng hiện tại 
			lvCategory.Items.Clear();
			// đọc một dòng dữ liệu
			while (sqlDataReader.Read())
			{
				//tạo một dòng mới trong lisview
				ListViewItem item = new ListViewItem(sqlDataReader["ID"].ToString());

				// thêm dòng mới vào listview
				lvCategory.Items.Add(item);
				// bổ sung các thông tin khác cho ListViewItem
				item.SubItems.Add(sqlDataReader["Name"].ToString());
				item.SubItems.Add(sqlDataReader["Type"].ToString());

			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			// tạo đối tượng kết nối
			string connectionString = "server=MSI\\SQLEXPRESS; database = RestaurantManagementDemo; Integrated Security = true; ";
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			SqlCommand sqlCommand = sqlConnection.CreateCommand();

			// thiết lập lệnh truy vấn cho đối tượng command
			sqlCommand.CommandText = "INSERT INTO Category(Name, [Type])" + "VALUES (N'" + txtName.Text + "'," + txtType.Text + ")";

			// mở kết nối tới cơ sở dữ liệu
			sqlConnection.Open();

			// thực thi lệnh bằng phương thức ExcuteReder
			int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

			// đóng kết nối tới cơ sở dữ liệu
			sqlConnection.Close();

			if (numOfRowsEffected == 1)
			{
				MessageBox.Show("Thêm nhóm món ăn thành công");
				// reload lại dữ liệu
				btnLoad.PerformClick();
				// xóa các ô nhập
				txtName.Text = "";
				txtType.Text = "";
			}
			else
			{
				MessageBox.Show("Đã có lỗi xảy ra");
			}

		}

		private void lvCategory_Click(object sender, EventArgs e)
		{
			// lấy dòng được select trong listview
			ListViewItem item = lvCategory.SelectedItems[0];
			// hiển thị dữ liệu lên textbox controls
			txtID.Text = item.Text;
			txtName.Text = item.SubItems[1].Text;
			txtType.Text = item.SubItems[1].Text == "0" ? "Thức uống" : "Đồ ăn";

			// show hiển thị nút cập nhật, xóa
			btnUpDate.Enabled = true;
			btnDelete.Enabled = true;

		}

		private void btnUpDate_Click(object sender, EventArgs e)
		{
			// tạo đối tượng kết nối
			string connectionString = "server=MSI\\SQLEXPRESS; database = RestaurantManagementDemo; Integrated Security = true; ";
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			SqlCommand sqlCommand = sqlConnection.CreateCommand();

			// thiết lập lệnh truy vấn cho đối tượng Command
			sqlCommand.CommandText = "UPDATE Category SET Name = N'" + txtName.Text + 
										"', [Type] = " + txtType.Text + 
										" WHERE ID = " + txtID.Text;
			// mở kết nối tới csdl
			sqlConnection.Open();
			// thực thi lệnh
			int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

			// đóng kết nối
			sqlConnection.Close();
			if (numOfRowsEffected == 1)	// nếu đúng
			{
				// cập nhật lại dữ liệu 
				ListViewItem item = lvCategory.SelectedItems[0];

				item.SubItems[1].Text = txtName.Text;
				item.SubItems[2].Text = txtType.Text;
				// xóa các ô nhập
				txtID.Text = "";
				txtName.Text = "";
				txtType.Text = "";

				// Disable nút xóa cập nhật
				btnUpDate.Enabled = false;
				btnDelete.Enabled = false;
				// xuất thông báo
				MessageBox.Show("Cập nhật món ăn thành công");
			}
			else
			{
				MessageBox.Show("Xảy ra lỗi cập nhật, vui lòng thử lại");
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			// tạo đối tượng kết nối
			string connectionString = "server=MSI\\SQLEXPRESS; database = RestaurantManagementDemo; Integrated Security = true; ";
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			SqlCommand sqlCommand = sqlConnection.CreateCommand();

			// thiết lập lệnh truy vấn cho đối tượng command
			sqlCommand.CommandText = "DELETE FROM Category " + "WHERE ID = " + txtID.Text;

			// mở kết nối tới cơ sở dữ liệu
			sqlConnection.Open();

			// thực thi lệnh bằng phương thức ExcuteReder
			int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

			// đóng kết nối tới cơ sở dữ liệu
			sqlConnection.Close();

			if (numOfRowsEffected == 1)
			{
				ListViewItem item = lvCategory.SelectedItems[0];
				lvCategory.Items.Remove(item);
				// xóa các ô nhận
				txtID.Text = "";
				txtName.Text = "";
				txtType.Text = "";

				// tắt các nút cập nhật, xóa
				btnUpDate.Enabled = false;
				btnDelete.Enabled = false;

				MessageBox.Show("Xóa nhóm món ăn thành công");
			}
			else
			{
				MessageBox.Show("Đã có lỗi xảy ra ");
			}
		}

		private void tsmDelete_Click(object sender, EventArgs e)
		{
			if (lvCategory.SelectedItems.Count > 0)
			{
				btnDelete.PerformClick();
			}
		}

		private void tsmViewFood_Click(object sender, EventArgs e)
		{
			if (txtType.Text != "")
			{
				FoodForm foodForm = new FoodForm();
				foodForm.Show(this);
				foodForm.LoadFood(Convert.ToInt32(txtID.Text));
			}
		}
	}
}
