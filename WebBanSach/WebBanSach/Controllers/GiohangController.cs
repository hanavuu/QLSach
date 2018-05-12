using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class GiohangController : Controller
    {
        // Tao doi tuong chu du lieu tu model dbBansach da tao
        dbQLBansachDataContext data = new dbQLBansachDataContext();

        //Lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                //Neu gio hang ton tai thi khoi tao listGiohang
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Them gio hang
        public ActionResult ThemGioHang(int iMasach, string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach nay ton tai  trong Session["Gio hang"] chua?
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        //tao Partial View de hien thi thong tin gio hang
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //kt sach da co rong sssion gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            //neu ton tai sp
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("Giohang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Bookstore");
            }
            return RedirectToAction("Giohang");
        }
        //Cap nha gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //kt sach da co rong sssion gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            //neu ton tai sp
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        //xoa tat ca gio hang
        public ActionResult XoaTatcaGiohang()
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Bookstore");
        }
        //Hien thi view dathang de cap na các thong tin cho don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"]==null || Session["Taikhoan"].ToString()=="")
            {
            
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Them don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiaohang = DateTime.Parse(ngaygiao);
            ddh.HTGiaohang = false;
            ddh.HTThanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //them chi iet don hang
            foreach(var item in gh)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        
    }
}