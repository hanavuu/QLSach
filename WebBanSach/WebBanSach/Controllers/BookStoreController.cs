using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;
using PagedList;
using PagedList.Mvc;
namespace WebBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        // GET: BookStore
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        //public ActionResult Index()
        //{
        //    var sachmoi = Laysachmoi(6);
        //    return View(sachmoi);
        //}
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);

        }
        public ActionResult NhaXuatBan()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);

        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return View(sach.Single());
        }
    }
}