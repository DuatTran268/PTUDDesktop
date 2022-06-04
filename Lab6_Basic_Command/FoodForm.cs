using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6_Basic_Command
{
	public partial class FoodForm : Form
	{
		public FoodForm()
		{
			InitializeComponent();
		}

		public void LoadFood(int categoryID)
		{
			// tạo đối tượng kết nối
			string connectionString = "server=MSI\\SQLEXPRESS; database = RestaurantManagementDemo; Integrated Security = true; ";
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			// tạo đối tượng thực thi lệnh
			SqlCommand sqlCommand = sqlConnection.CreateCommand();
			// lệnh truy vấn cho đối tượng command
			sqlCommand.CommandText = "SELECT Name FROM Category where ID = " + categoryID;
			// mở kết nối tới database
			sqlConnection.Open();
			// gán tên nhóm sản phẩm cho tiêu đề
			string catName = sqlCommand.ExecuteScalar().ToString();
			this.Text = "Danh sách các món ăn thuộc nhóm: " + catName;
			sqlCommand.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = " + categoryID;

			// tạo đối tượng DataAdapter
			SqlDataAdapter sqlDA = new SqlDataAdapter(sqlCommand);
			// tạo DataTable chứa dữ liệu
			DataTable sqlDT = new DataTable("Food");
			sqlDA.Fill(sqlDT);

			// hiển thị danh sách món ăn lên form
			dgvFood.DataSource = sqlDT;

			// đóng kết nối và giiar phóng bộ nhớ
			sqlConnection.Close();
			sqlConnection.Dispose();
			sqlDA.Dispose();
		}
	}
}
