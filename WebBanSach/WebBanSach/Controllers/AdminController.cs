using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebBanSach.Controllers
{
    public class AdminController : Controller
    {

        //private object ad;
        dbQLBansachDataContext db = new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            //return View(db.SACHes.ToList());
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNum, pageSize));
        }

        //<get---Login--->
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //   <psst--login-->
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loil"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập lại mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);

                if (ad != null)
                {  //view Bag. Thong bao Chuc mung dang nhap thanh cong 
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        //<get---Themmoisach--->
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View();
        }
        //<post---Themmoisach--->
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileupload)
        {
            //dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var filename = Path.GetFileName(fileupload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), filename);
                    //kiem tra ton tai hay chua
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //luu hinh vao duog dan
                        fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = filename;
                    // luu vao csdl
                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Sach");
        }

        //<--chitietsach-->
        //Hiển thị sản phẩm
        public ActionResult Chitietsach(int id)
        {
            //lay doi tuong sach theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        //<Get---Xoa Sach--->
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //lay doi tuong sach can xoa theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        //<Post---Xoa Sach--->
        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            //lay doi tuong sach can xoa theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }
        //<Get---Sua Sach--->
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            //lay doi tuong sach can sua theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //dua du lieu vao dropdownlist
            //lay du lieu tu table chu de, sap xep tang dan theo ten chu de, chon lay gia tri ma cd,hien thi ten chu de 
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileupload)
        {
            //dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //kt duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var filename = Path.GetFileName(fileupload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), filename);
                    //kiem tra ton tai hay chua
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //luu hinh vao duog dan
                        fileupload.SaveAs(path);
                    }
                    SACH s = db.SACHes.Where(x => x.Masach == sach.Masach).Single<SACH>();
                    s.Tensach = sach.Tensach;
                    s.Donvitinh = sach.Donvitinh;
                    s.Dongia = sach.Dongia;
                    s.Mota=sach.Mota;
                    s.Hinhminhhoa = filename;
                    s.Soluongban = sach.Soluongban;
                    s.MaCD = sach.MaCD;
                    sach.MaNXB = sach.MaNXB;

                    sach.Hinhminhhoa = filename;
                    // luu vao csdl
                    UpdateModel(sach);
                    
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Sach");
        }
    }
}