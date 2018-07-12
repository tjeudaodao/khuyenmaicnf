﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tulpep.NotificationWindow;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.IO;

namespace KhuyenMai
{
    public partial class Form1 : Form
    {
        bool chay = true;
        DataTable dt = new DataTable();
        Thread capnhat;
        //Thread loadBang;

        public Form1()
        {
            InitializeComponent();
            capnhat = new Thread(hamcapnhat);
            capnhat.IsBackground = true;
            capnhat.Start();

            //loadBang = new Thread(hamloadBang);
            //loadBang.IsBackground = true;
            //loadBang.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            amthanh.amluong(chay);
            var con = ketnoikhuyenmai.Khoitao();
            datag1.DataSource = con.bangKhuyenmai();
            dt.Columns.Add("Mã hàng");
            dt.Columns.Add("Giá chốt");
            dt.Columns.Add("Giá giảm");
            dt.AcceptChanges();
        }
        //void hamloadBang()
        //{
        //    Thread.Sleep(200);
        //    var con = ketnoikhuyenmai.Khoitao();
        //    datag1.Invoke(new MethodInvoker(delegate ()
        //   {
        //       datag1.DataSource = con.bangKhuyenmai();
        //       dt.Columns.Add("Mã hàng");
        //       dt.Columns.Add("Giá chốt");
        //       dt.Columns.Add("Giá giảm");
        //       dt.AcceptChanges();
        //   }));
            
        //}
        void hamcapnhat()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    var con = ketnoi.Khoitao();
                    var consqlite = ketnoingaycapnhat.Khoitao();

                    string ngay = con.layngaycapnhat();
                    string ngaydata = con.layngayData();

                    string ngaykm2 = consqlite.layngaycapnhat();
                    string ngaydata2 = consqlite.layngayData();

                    if (ngay != null)
                    {
                        if (ngaykm2 != ngay)
                        {
                            ftp ftpClient = new ftp(@"ftp://27.72.29.28/", "hts", "hoanglaota");
                            if (File.Exists(Application.StartupPath + @"\datakhuyenmai.db"))
                            {
                                File.Delete(Application.StartupPath + @"\datakhuyenmai.db");
                            }
                            ftpClient.download("/app/luutru/datakhuyenmai.db", Application.StartupPath + @"\datakhuyenmai.db");

                            
                            datag1.Invoke(new MethodInvoker(delegate ()
                            {
                                var conkm = ketnoikhuyenmai.Khoitao();
                                datag1.DataSource = conkm.bangKhuyenmai();

                            }));
                            this.Invoke(new Action(delegate ()
                            {
                                NotificationHts("Vừa Cập Nhật bảng khuyến mãi xong\nOk, triển chiêu.");
                            }));
                            lbcapnhat.Invoke(new MethodInvoker(delegate ()
                            {
                                lbcapnhat.Text = ngay;
                            }));
                            consqlite.capnhatngayKM(ngay);
                        }
                        if (ngaydata2 != ngaydata)
                        {
                            ftp ftpClient = new ftp(@"ftp://27.72.29.28/", "hts", "hoanglaota");
                            if (File.Exists(Application.StartupPath + @"\databarcode.db"))
                            {
                                File.Delete(Application.StartupPath + @"\databarcode.db");
                            }
                            ftpClient.download("/app/luutru/databarcode.db", Application.StartupPath + @"\databarcode.db");
                            
                            this.Invoke(new Action(delegate()
                            {
                                NotificationHts("Vừa Cập Nhật bảng barcode xong\nOk, triển chiêu.");
                            }));
                            consqlite.capnhatngayData(ngay);
                        }
                    }
                    Thread.Sleep(1800000);

                }
            }
            catch (Exception)
            {

                return;
            }
           
            
        }
        void NotificationHts(string noidung)
        {
            PopupNotifier pop = new PopupNotifier();
            pop.TitleText = "Thông báo";
            pop.ContentText = "\" " + noidung + " \"";
            pop.Image = Properties.Resources.totoro1;
            pop.IsRightToLeft = false;
            pop.TitleColor = System.Drawing.Color.Navy;
            pop.TitleFont = new System.Drawing.Font("Comic Sans MS", 11, System.Drawing.FontStyle.Underline);
            pop.BodyColor = System.Drawing.Color.DimGray;
            pop.Size = new System.Drawing.Size(380, 130);
            pop.ImageSize = new System.Drawing.Size(100, 100);
            pop.ImagePadding = new Padding(15);
            pop.ContentColor = System.Drawing.Color.White;
            pop.ContentFont = new System.Drawing.Font("Comic Sans MS", 13, System.Drawing.FontStyle.Bold);
            pop.Delay = 1000;
            pop.BorderColor = System.Drawing.Color.DimGray;
            pop.HeaderHeight = 1;
            pop.Popup();
        }
        void doiMau_Phatam()
        {
            string mau = @"(^9,)|(^[123456789]9,)|(^1[0123456789]9,)|(^2[0123456789]9,)";
            
            string mauPhantram30 = @"30.0%$";
            string mauPhantram40 = @"40.0%$";
            string mauPhantram50 = @"50.0%$";
            //sua mau gia ca
            if (Regex.IsMatch(lbgiachot.Text,mau) && lbphantram.Text != "-")
            {
                lbgiachot.ForeColor = Color.Violet;
                amthanh.phatDonggia();
            }
            else
            {
                lbgiachot.ForeColor = Color.DimGray;
            }
           // sua mau phan tram
            if (Regex.IsMatch(lbphantram.Text,mauPhantram30))
            {
                lbphantram.ForeColor = Color.SpringGreen;
                amthanh.phat30();
            }
            else if (Regex.IsMatch(lbphantram.Text, mauPhantram40))
            {
                lbphantram.ForeColor = Color.DarkOrange;
                amthanh.phat40();
            }
            else if (Regex.IsMatch(lbphantram.Text, mauPhantram50))
            {
                lbphantram.ForeColor = Color.Tomato;
                amthanh.phat50();
            }
            else
            {
                lbphantram.ForeColor = Color.DimGray;
            }
        }
        void chenBang()
        {

            if (dt.Rows.Count > 12)
            {
                dt.Clear();
            }
            dt.Rows.Add(lbmatong.Text, lbgiachot.Text, lbphantram.Text);
            datag1.DataSource = dt;
            datag1.FirstDisplayedScrollingRowIndex =datag1.RowCount - 1;
            datag1.Rows[datag1.RowCount - 2].Selected = true;
        }

        private void txtbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtbarcode.Text))
                {
                    var con = ketnoisqlite.Khoitao();
                    var conkm = ketnoikhuyenmai.Khoitao();

                    hamtao ham = new hamtao();
                    string matong = con.laymasp(txtbarcode.Text);
                    if (matong == null)
                    {
                        NotificationHts("Lỗi\nMã này chưa có trong danh mục Khuyến Mãi");
                        txtbarcode.Clear();
                        txtbarcode.Focus();
                        return;
                    }
                    lbmatong.Text = matong;
                    string[] laygiatri = conkm.laythongtinkhuyenmai(matong);
                    string[] laythongtinMota= null;
                    try
                    {
                        var conmysql = ketnoi.Khoitao();
                        laythongtinMota = conmysql.laythongtinMota(matong);
                    }
                    catch (Exception)
                    {

                        laythongtinMota = null;
                    }
                    string[] ketqua = new string[2];
                    if (laygiatri[0] != null || laygiatri[1] != null)
                    {
                        ketqua = ham.tinhToan(laygiatri[0], laygiatri[1]);
                        lbgiachot.Text = ketqua[0];
                        lbphantram.Text = ketqua[1];
                        if (laythongtinMota[0] !=null || laythongtinMota[1] != null)
                        {
                            lbmota.Text = "\" " + laythongtinMota[0] + " - " + laythongtinMota[1] + "\"";
                        }
                        
                        doiMau_Phatam(); // chay ham doi mau
                        chenBang(); // them ma moi vao bang hien thi
                    }
                    else
                    {
                        lbgiachot.Text = "Nguyên giá";
                        lbphantram.Text = "-";
                        lbmota.Text = "^-^";
                        chenBang();
                        lbgiachot.ForeColor = Color.DimGray;
                        lbphantram.ForeColor = Color.DimGray;
                    }
                    txtbarcode.Clear();
                    txtbarcode.Focus();
                }
            }
            
        }

        private void datag1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var con = ketnoisqlite.Khoitao();
                var conkm = ketnoikhuyenmai.Khoitao();
                hamtao ham = new hamtao();

                DataGridViewRow roww = datag1.Rows[e.RowIndex];
                string matong = roww.Cells[0].Value.ToString();
                lbmatong.Text = matong;
                string[] laygiatri = conkm.laythongtinkhuyenmai(matong);

                string[] laythongtinMota = null;
                try
                {
                    var conmysql = ketnoi.Khoitao();
                    laythongtinMota = conmysql.laythongtinMota(matong);
                }
                catch (Exception)
                {

                    laythongtinMota = null;
                }
                string[] ketqua = new string[2];
                if (laygiatri != null)
                {
                    ketqua = ham.tinhToan(laygiatri[0], laygiatri[1]);
                    lbgiachot.Text = ketqua[0];
                    lbphantram.Text = ketqua[1];
                    if (laythongtinMota[0] != null || laythongtinMota[1] != null)
                    {
                        lbmota.Text = "\" " + laythongtinMota[0] + " - " + laythongtinMota[1] + "\"";
                    }
                    doiMau_Phatam(); // chay ham doi mau
                    
                }
                else
                {
                    lbgiachot.Text = "-";
                    lbphantram.Text = "-";
                    lbmota.Text = "";
                    
                }
               
            }
            catch (Exception)
            {

                return;
            }
        }

        private void txtmatong_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var con = ketnoikhuyenmai.Khoitao();
                datag1.DataSource = con.bangKhuyenmai(txtmatong.Text);
            }
            catch (Exception)
            {

                NotificationHts("Có lỗi rồi");
            }
            
        }

        private void pbxoa_Click(object sender, EventArgs e)
        {
            txtmatong.Clear();
            txtmatong.Focus();

        }

        private void pbamthanh_Click(object sender, EventArgs e)
        {
            chay = !chay;
            amthanh.amluong(chay);
            if (chay)
            {
                pbamthanh.Image = Properties.Resources.speaker;
            }
            else
            {
                pbamthanh.Image = Properties.Resources.mute;
            }
        }
    }
}
