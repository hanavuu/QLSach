using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSach.Models
{
    public class Giohang
    {
        // tao doi tuong data chua du lieu tu model dbBansach da tao
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        public int iMasach { get; set; }
        public string sTensach { get; set; }
        public string sHinhminhhoa { get; set; }

        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        //Khoi tao gio hang duoc truyen vao So luong ac dinh la 1
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sHinhminhhoa = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoluong = 1;
        }
    }
}