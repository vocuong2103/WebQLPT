using Đồ_án_web_quản_lí_phòng_trọ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đồ_án_web_quản_lí_phòng_trọ.Areas.Admin.Controllers
{
    public class PhongTroAdController : Controller
    {
		// GET: Admin/PhongTro
		QuanLyNhaTroEntities3 database = new QuanLyNhaTroEntities3();
		public ActionResult IndexAd()
		{
			return View(database.PhongTroes.ToList());
		}
		//Tạo mới phòng trọ
		[HttpGet]
		public ActionResult Create()
		{
			PhongTro phongTro = new PhongTro();
			return View(phongTro);
		}
		[HttpPost]
		public ActionResult Create(PhongTro phongtro, HttpPostedFileBase[] UploadImage)
		{
			if (ModelState.IsValid)
			{
				try
				{
					string fileName = Path.GetFileNameWithoutExtension(phongtro.UploadImage.FileName);
					string extension = Path.GetExtension(phongtro.UploadImage.FileName);
					fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
					phongtro.Hinh = "~/images/" + fileName;
					fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
					phongtro.UploadImage.SaveAs(fileName);
					using (QuanLyNhaTroEntities3 db = new QuanLyNhaTroEntities3())
					{
						db.PhongTroes.Add(phongtro);
						db.SaveChanges();

					}
					return RedirectToAction("Index2");
				}
				catch (Exception e)
				{
					ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình thêm phòng trọ.");

				}
			}
			return View(phongtro);

		}
		//Detail
		public ActionResult DetailsAd(int id)
		{
			return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
		}
		//Chỉnh sửa phòng trọ
		public ActionResult Edit(int id)
		{
			return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Edit(int id, PhongTro phongTro)
		{
			database.Entry(phongTro).State = System.Data.Entity.EntityState.Modified;
			database.SaveChanges();
			return RedirectToAction("Index2");
		}
		//Xoá phòng trọ
		public ActionResult Delete(int id)
		{
			return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Delete(int id, PhongTro phongtro)
		{
			try
			{
				phongtro = database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault();
				database.PhongTroes.Remove(phongtro);
				database.SaveChanges();
				return RedirectToAction("Index2");
			}
			catch
			{
				return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
			}
		}

		//Search OPtion
		public ActionResult SearchOption(int? min, int? max, int? minArea, int? maxArea)
		{
			// Get all "PhongTro" entities from the database
			var phongTroList = database.PhongTroes.ToList();

			// Filter the "PhongTro" entities based on the provided price range
			if (min.HasValue && max.HasValue)
			{
				phongTroList = phongTroList.Where(p => p.GiaThue >= min.Value && p.GiaThue <= max.Value).ToList();
			}

			if (minArea.HasValue && maxArea.HasValue)
			{
				phongTroList = phongTroList
					.Where(p => p.DienTich >= minArea.Value && p.DienTich <= maxArea.Value)
					.ToList();
			}

			// Return the filtered list of "PhongTro" entities to the view
			return View(phongTroList.ToList());
		}
	}
}