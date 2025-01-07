using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class PhieuThuTienController : Controller
    {
        // GET: PhieuThuTien
        QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();
        public ActionResult Index()
        {
            return View(database.PhieuThuTiens.ToList());
        }
        public ActionResult TongDoanhThu()
        {
            return View(database.PhieuThuTiens.ToList());

        }

        // Thêm phiếu

        [HttpGet]
        public SelectList GetMaQuanLyList()
        {
            var maQuanLyList = database.QuanLies.ToList();
            return new SelectList(maQuanLyList, "MaQuanLy");
        }

        // Lấy danh sách mã khách hàng từ cơ sở dữ liệu
        public SelectList GetMaKhachHangList()
        {
            var maKhachHangList = database.KhachHangs.ToList();
            return new SelectList(maKhachHangList, "MaKhachHang");
        }

        // Lấy danh sách mã phiếu thuê/trả phòng từ cơ sở dữ liệu
        public SelectList GetMaPhieuTTPList()
        {
            var maPhieuTTPList = database.PhieuThueTraPhongs.ToList();
            return new SelectList(maPhieuTTPList, "MaPhieuTTP");
        }
        //GIá thuê
        [HttpGet]
        public ActionResult GetGiaPhong(int maPhieuTTP)
        {
            // Retrieve GiaPhong based on the selected MaPhieuTTP from the database
            // Replace "database" with the correct DbContext instance for your application
            var giaPhong = database.PhieuThueTraPhongs
                                   .Where(p => p.MaPhieuTTP == maPhieuTTP)
                                   .Select(p => p.PhongTro.GiaThue)
                                   .FirstOrDefault();

            return Content(giaPhong.ToString());
        }
        public ActionResult Create()
        {
            using (var db = new QuanLyNhaTroEntities3())
            {

                var giaPhongData = database.PhieuThueTraPhongs.ToDictionary(p => p.MaPhieuTTP, p => p.PhongTro.GiaThue);
                ViewBag.GiaPhongData = giaPhongData;

                var maQuanLyList = db.QuanLies.Select(p => p.MaQuanLy).ToList();
                ViewBag.MaQuanLyList = new SelectList(maQuanLyList);


                var maKhachHangList = db.KhachHangs.Select(p => p.MaKhachHang).ToList();
                ViewBag.MaKhachHangList = new SelectList(maKhachHangList);


                var maPhieuTTPList = db.PhieuThueTraPhongs.Select(p => p.MaPhieuTTP).ToList();
                ViewBag.MaPhieuTTPList = new SelectList(maPhieuTTPList);

                return View();

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhieuThuTien phieuthutien)
        {
            try
            {
                // Get the PhieuThueTraPhongId from the submitted form data
                int maPhieuTTP = phieuthutien.MaPhieuTTP;

                // Retrieve PhongTro based on PhieuThueTraPhongId (maPhieuTTP)
                PhongTro phongTro = database.PhongTroes.FirstOrDefault(p => p.MaPhong == maPhieuTTP);

                if (phongTro != null)
                {
                    // Calculate the total amount and set it to the model
                    int giaPhong = phongTro.GiaThue;
                    int tienDien = phieuthutien.TienDien;
                    int tienNuoc = phieuthutien.TienNuoc;
                    int tienMang = phieuthutien.TienMang;

                    int total = giaPhong + tienDien + tienNuoc + tienMang;
                    phieuthutien.TongTien = total;

                    database.PhieuThuTiens.Add(phieuthutien);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("Không tìm thấy thông tin về giá phòng cho mã phiếu thuê trả phòng đã chọn!");
                }
            }
            catch
            {
                return Content("Dữ liệu này đang được sử dụng trong bảng khác. Tạo dữ liệu thất bại!!!");
            }
        }


        // Xem chi tiết phiếu thu tiền Details

        public ActionResult Details(int id)
        {
            return View(database.PhieuThuTiens.Where(s => s.MaPhieuThuTien == id).FirstOrDefault());
        }

        //Edit
        public ActionResult Edit(int? id)
        {
            return View(database.PhieuThuTiens.Where(s => s.MaPhieuThuTien == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, PhieuThuTien phieuthutien)
        {
            database.Entry(phieuthutien).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }


        //Delete
        public ActionResult Delete(int id)
        {
            return View(database.PhieuThuTiens.Where(s => s.MaPhieuThuTien == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, PhieuThuTien phieuthutien)
        {
            try
            {
                phieuthutien = database.PhieuThuTiens.Where(s => s.MaPhieuThuTien == id).FirstOrDefault();
                database.PhieuThuTiens.Remove(phieuthutien);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }


        //Tính tổng tiền


    }
}