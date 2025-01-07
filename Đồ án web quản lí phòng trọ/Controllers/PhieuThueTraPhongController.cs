using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class PhieuThueTraPhongController : Controller
    {
        QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();
        // GET: PhieuThueTraPhong
        //Danh sách phiếu 
        public ActionResult Index()
        {
            return View(database.PhieuThueTraPhongs.ToList());
        }

		// Thêm phiếu
		[HttpGet]
		public ActionResult GetThongTinPhong(int maPhong)
		{
			var phong = database.PhongTroes.FirstOrDefault(p => p.MaPhong == maPhong);
			if (phong != null)
			{
				return PartialView("_PhongDetailsPartial", phong);
			}
			else
			{
				// Nếu không tìm thấy phòng với mã phòng tương ứng, trả về một đối tượng JSON trống
				return Json(new { }, JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult Create()
		{
			using (var db = new QuanLyNhaTroEntities3())
			{
				// Truy vấn danh sách mã phòng từ cơ sở dữ liệu
				var danhSachMaPhong = db.PhongTroes.Select(p => p.MaPhong).ToList();
				var danhSachTenKH = db.KhachHangs.Select(p => p.TenKhachHang).ToList();

				// Đưa danh sách mã phòng vào ViewBag để hiển thị trong view
				ViewBag.DanhSachMaPhong = new SelectList(danhSachMaPhong);
				ViewBag.DanhSachTenKH = new SelectList(danhSachTenKH);

				// Khởi tạo phiếu thuê trả phòng mới
				var phieuThueTraPhong = new PhieuThueTraPhong();

				// Gắn thông tin phòng đầu tiên vào model nếu có
				if (danhSachMaPhong.Any())
				{
					var firstMaPhong = danhSachMaPhong.First();
					var phong = db.PhongTroes.FirstOrDefault(p => p.MaPhong == firstMaPhong);
					if (phong != null)
					{
						phieuThueTraPhong.MaPhong = phong.MaPhong;
						phieuThueTraPhong.MaPhong = phong.GiaThue;
						phieuThueTraPhong.MaPhong = phong.DienTich;
						// Gán các thông tin phòng khác tương tự tại đây (nếu có)
					}
				}

				return View(phieuThueTraPhong);
			}

		}

		[HttpPost]
		public ActionResult Create(PhieuThueTraPhong phieuttp)
		{

			try
			{
				database.PhieuThueTraPhongs.Add(phieuttp);
				database.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				return Content("Dữ liệu này đang được sử dụng trong bảng khác. Tạo dữ liệu thất bại!!!");
			}
		}

		// Xem chi tiết phiếu thuê trả phòng Details

		public ActionResult Details(int id)
		{
			return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
		}

		//Chỉnh sửa thông tin phiếu thuê trả phòng

		public ActionResult Edit(int? id)
		{
			return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Edit(int id, PhieuThueTraPhong phieuttp)
		{
			database.Entry(phieuttp).State = EntityState.Modified;
			database.SaveChanges();
			return RedirectToAction("Index");
		}

		//Xoá phiếu thuê trả phòng 

		public ActionResult Delete(int id)
		{
			return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Delete(int id, PhieuThueTraPhong phieuttp)
		{
			try
			{
				phieuttp = database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault();
				database.PhieuThueTraPhongs.Remove(phieuttp);
				database.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
			}
		}
	}
}