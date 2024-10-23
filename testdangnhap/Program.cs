using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace App
{




    class Program
    {

        static void Main(string[] args)
        {
            Login a = new Login();

            Console.WriteLine("Chon mot tuy chon:");
            Console.WriteLine("1.Dang ky");
            Console.WriteLine("2.Dang nhap");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    a.Dangky();
                    break;
                case "2":
                    a.Dangnhap();
                    break;
                default:
                    Console.WriteLine("Khong hop le");
                    break;
            }
        }
    }



    class Login
    {
        string sqlConnectStr = "Data Source=DESKTOP-EQK69O6;Initial Catalog=chatbottelegram;User ID=sa;Password=123456";
        private string sqlconnectStr;
        string tendangnhap;
        string matkhau;

        string _tendangnhap;
        string _matkhau;

        private int userID;
        public void Dangky()
        {

            Console.WriteLine("Đăng ký \n Tên đăng nhập : ");
            tendangnhap = Console.ReadLine();
            SaveUserName(tendangnhap); // chưa hiểu
            userID = SaveUserName(tendangnhap);





            Console.WriteLine("Nhập mật khẩu:");
            matkhau = Console.ReadLine();
            SaveUserPhone(tendangnhap, matkhau);

        }

        public void Dangnhap()
        {

            bool isValid = false; // Biến để kiểm tra tính hợp lệ của tên đăng nhập

            while (!isValid) // Lặp cho đến khi tên hợp lệ được nhập
            {
                Console.WriteLine("Dang nhap tai khoa \n Ten dang nhap");
                _tendangnhap = Console.ReadLine();
                using SqlConnection connection = new(sqlConnectStr);
                connection.Open();
                string sqlCheck = $"SELECT COUNT(1) FROM Users WHERE Name = @Name";
                using SqlCommand command = new(sqlCheck, connection); // chưa hiểu
                command.Parameters.AddWithValue("@Name", _tendangnhap); // kiểm tra _tendangnhap có bằng @Name không
                int count = (int)command.ExecuteScalar(); // Thực thi truy vấn và lấy số lượng

                if (count > 0)
                {
                    Console.WriteLine("Ten da ton tai");

                    bool isValidPassword = false; // Biến để kiểm tra tính hợp lệ của mật khẩu
                    while (!isValidPassword) // Lặp cho đến khi mật khẩu hợp lệ được nhập
                    {
                        Console.WriteLine("Nhap mat khau");
                        _matkhau = Console.ReadLine();
                        string sqlCheckpass = $"SELECT COUNT(1) FROM Users WHERE Phone = @Phone";
                        using SqlCommand commandpass = new(sqlCheckpass, connection);
                        commandpass.Parameters.AddWithValue("@Phone", _matkhau);
                        int countpass = (int)commandpass.ExecuteScalar();

                        if (countpass > 0)
                        {
                            Console.WriteLine("Dang nhap thanh cong");
                            isValid = true;
                            isValidPassword = true;
                        }
                        else
                        {
                            Console.WriteLine("Mat khau khong dung , nhap lai");

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ten dang nhap khong ton tai , nhap lai");
                }
            }

        }

        public int SaveUserName(string userName)
        {
            using (var con = new SqlConnection(sqlConnectStr))
            {
                int newID;


                var cmd = "INSERT INTO Users (Name) VALUES (@Name);SELECT CAST(scope_identity() AS int)";

                using (var insertCommand = new SqlCommand(cmd, con))
                {

                    insertCommand.Parameters.AddWithValue("@Name", userName);
                    con.Open();
                    newID = (int)insertCommand.ExecuteScalar();
                    return newID;
                }
            }
        }
        public async Task SaveUserPhone(string userNamestring, string userPhone)
        {

            using SqlConnection connection = new(sqlConnectStr);

            // Câu lệnh SQL để cập nhật số điện thoại
            string sqlUpdate = "UPDATE Users set Phone = @Phone WHERE id=@userID";
            await connection.OpenAsync();
            using SqlCommand command = new(sqlUpdate, connection);

            command.Parameters.AddWithValue("@Phone", userPhone);
            command.Parameters.AddWithValue("@userID", userID);

            // Thực hiện cập nhật
            await command.ExecuteNonQueryAsync();
        }
    }

}

