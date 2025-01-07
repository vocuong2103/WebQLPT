using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class DangNhapController : Controller
    {
        QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();
        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }
        //Xử lí hàm đăng nhập
        [HttpPost]
        public ActionResult LoginAcount(TaiKhoan _taikhoan)
        {
            var check = database.TaiKhoans.Where(s => s.UserName == _taikhoan.UserName && s.MatKhau == _taikhoan.MatKhau).FirstOrDefault();

            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
                return View("DangNhap");
            }
            else
            {
                if (check.Role == "Quan ly")
                {
                    Session["UserName"] = check.UserName;
                    return RedirectToAction("Index2", "PhongTro");
                }
                else if (check.Role == "Khach hang")
                {
                    Session["UserName"] = check.UserName;
                    return RedirectToAction("Index", "PhongTro");
                }
                else
                {
                    ViewBag.ErrorInfo = "Bạn không có quyền truy cập vào trang này";
                    return View("DangNhap");
                }

            }
        }
        //Đăng ký
        public ActionResult DangKy()
        {
            return View(new DangKyViewModel { TaiKhoan = new TaiKhoan(), KhachHang = new KhachHang() });
        }

        [HttpPost]
        public ActionResult RegisterUser(DangKyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản có tồn tại trong database chưa
                var existingAccount = database.TaiKhoans.FirstOrDefault(s => s.UserName == viewModel.TaiKhoan.UserName);
                if (existingAccount == null)
                {
                    // Nếu tài khoản chưa tồn tại, thêm tài khoản mới
                    viewModel.TaiKhoan.Role = "Khach hang"; // Gán role mặc định là "Khach hang"

                    // Lấy mã tài khoản tự động tăng bằng cách lấy mã tài khoản lớn nhất trong database, sau đó tăng lên 1
                    int maxMaTaiKhoan = database.TaiKhoans.Max(t => t.MaTaiKhoan);
                    viewModel.TaiKhoan.MaTaiKhoan = maxMaTaiKhoan + 1;

                    // Thêm thông tin khách hàng vào bảng KhachHang
                    viewModel.KhachHang.MaTaiKhoan = viewModel.TaiKhoan.MaTaiKhoan; // Gán mã tài khoản mới cho khách hàng
                    database.KhachHangs.Add(viewModel.KhachHang); // Thêm khách hàng vào database

                    database.Configuration.ValidateOnSaveEnabled = false; // Tạm thời tắt validate để không bị lỗi khi thêm dữ liệu
                    database.TaiKhoans.Add(viewModel.TaiKhoan); // Thêm tài khoản vào database
                    database.SaveChanges(); // Lưu thay đổi vào database

                    // Đăng ký thành công, chuyển hướng về trang đăng nhập
                    return RedirectToAction("DangNhap", "DangNhap");
                }
                else
                {
                    // Tài khoản đã tồn tại, hiển thị thông báo lỗi
                    ViewBag.ErrorRegister = "Tài khoản đã đăng nhập";
                    return View(viewModel);
                }
            }

            // Model không hợp lệ, hiển thị lại view đăng ký với thông báo lỗi
            return View(viewModel);
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }

}
