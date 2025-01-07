using Đồ_án_web_quản_lí_phòng_trọ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();

        public ActionResult DanhSachThongKe()
        {
            return View();
        }
        public ActionResult FilterAndCalculateByThang(int thang)
        {
            var filteredPhieuThuTien = database.PhieuThuTiens
                .Where(p => p.Thang == thang && p.TrangThai == "Đã thanh toán")
                .ToList();

            var tongDoanhThu = filteredPhieuThuTien.Sum(p => p.TongTien);

            ViewBag.Thang = thang;
            ViewBag.TongDoanhThu = tongDoanhThu;

            return View("FilteredAndCalculatedPhieuThuTien", filteredPhieuThuTien);
        }
        private int TinhTongDoanhThuCaNam()
        {
            var tongDoanhThuCaNam = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Đã thanh toán")
                .Sum(p => p.TongTien);

            return tongDoanhThuCaNam;
        }

        public ActionResult ChuaThanhToan()
        {
            var chuaThanhToan = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Chưa thanh toán")
                .ToList();

            return View("ChuaThanhToan", chuaThanhToan);
        }
      
    }
}