using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;



namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class KhachHangController : Controller
    {
		QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(database.KhachHangs.ToList());
        }
        public ActionResult LienHe()
        {
            return View();
        }
        //Them moi khach hàng
        public ActionResult Create()
        {
            return View();
        }
		[HttpPost]
		public ActionResult Create(KhachHang khachhang)
		{
			try
			{
				database.KhachHangs.Add(khachhang);
				database.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				return Content("Lỗi tạo mới khách hàng!!!");
			}
		}
		//Details
		public ActionResult Details(int id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		//Edit
		public ActionResult Edit(int? id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Edit(int id, KhachHang khachhang)
		{
			database.Entry(khachhang).State = EntityState.Modified;
			database.SaveChanges();
			return RedirectToAction("Index");
		}
        //Edit Profile
        public ActionResult EditProfile()
        {
            // Get the current logged-in user's username
            string currentUserName = User.Identity.Name;

            // Find the corresponding KhachHang record based on the username
            KhachHang khachHang = database.KhachHangs.FirstOrDefault(kh => kh.TenKhachHang == currentUserName);

            if (khachHang == null)
            {
                return Content("Khách hàng không tồn tại!");
            }

            return View(khachHang);
        }

        [HttpPost]
        public ActionResult EditProfile(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Update the customer's profile information in the database
                database.Entry(khachHang).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

		public ActionResult HistoryRentRoom()
		{
			// Xác định Username của tài khoản đang đăng nhập
			string loggedInUsername = User.Identity.Name;

			// Kết nối đến cơ sở dữ liệu
			using (var dbContext = new QuanLyNhaTroEntities3())
			{
				// Tìm tài khoản trong database dựa trên Username
				var userAccount = dbContext.TaiKhoans.SingleOrDefault(u => u.UserName == loggedInUsername);

				if (userAccount != null)
				{
					// Truy xuất IDTaiKhoan từ tài khoản
					int idTaiKhoan = userAccount.MaTaiKhoan;

					// Lấy khách hàng tương ứng với tài khoản
					var khachHang = dbContext.KhachHangs.SingleOrDefault(kh => kh.MaTaiKhoan == idTaiKhoan);

					if (khachHang != null)
					{
						// Lấy IDKhachHang của khách hàng
						String tenKhachHang = khachHang.TenKhachHang;

						// Lọc ra các phiếu thuê trả phòng dựa trên IDKhachHang
						var rentalHistory = dbContext.PhieuThueTraPhongs
							.Where(pttp => pttp.TenKhachHang == tenKhachHang)
							.ToList();

						// Trả về view và truyền dữ liệu lịch sử thuê phòng
						return View("HistoryRentRoom", rentalHistory);
					}
				}

				// Xử lý khi không tìm thấy tài khoản hoặc thông tin khách hàng
				return View("NotFound");
			}
		}


		//Delete

		public ActionResult Delete(int id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Delete(int id, KhachHang khachhang)
		{
			try
			{
				khachhang = database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault();
				database.KhachHangs.Remove(khachhang);
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