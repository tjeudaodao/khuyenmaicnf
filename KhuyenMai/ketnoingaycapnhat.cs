using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace KhuyenMai
{
    class ketnoingaycapnhat
    {
        #region khoitao
        private ketnoingaycapnhat()
        {
            string connstring = @"Data source=ngaycapnhat.db;version=3;new=false";
            connection = new SQLiteConnection(connstring);
        }

        private SQLiteConnection connection = null;

        private static ketnoingaycapnhat _instance = null;
        public static ketnoingaycapnhat Khoitao()
        {
            if (_instance == null)
                _instance = new ketnoingaycapnhat();
            return _instance;
        }
        public void Open()
        {
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {

                    return;
                }

            }
        }

        public void Close()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        #endregion

        public string layngayData()
        {
            string sql = "SELECT ngaydata FROM ngaycapnhat";
            string h = null;

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader dtr = cmd.ExecuteReader();
            while (dtr.Read())
            {
                h = dtr[0].ToString();
            }
            Close();
            return h;
        }
        public string layngaycapnhat()
        {
            string sql = "SELECT ngaykhuyenmai FROM ngaycapnhat";
            string h = null;

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader dtr = cmd.ExecuteReader();
            while (dtr.Read())
            {
                h = dtr[0].ToString();
            }
            Close();
            return h;
        }
        public string tongmaKM()
        {
            string sql = "SELECT tongmakhuyenmai FROM ngaycapnhat";
            string h = null;

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader dtr = cmd.ExecuteReader();
            while (dtr.Read())
            {
                h = dtr[0].ToString();
            }
            Close();
            return h;
        }
        public void capnhatngayKM(string ngaycanup)
        {
            string sql = "update ngaycapnhat set ngaykhuyenmai='" + ngaycanup + "'";

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            Close();
        }
        public void capnhatngayData(string ngaycanup)
        {
            string sql = "update ngaycapnhat set ngaydata='" + ngaycanup + "'";

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            Close();
        }
        public void capnhattongmaKM(string tongma)
        {
            string sql = "update ngaycapnhat set tongmakhuyenmai='" + tongma + "'";

            Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            Close();
        }
    }
}
