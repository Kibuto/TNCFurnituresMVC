﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNCFurnitures.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace TNCFurnitures.Controllers
{
    public class AdminController : Controller
    {
        dbQLFurnituresDataContext db = new dbQLFurnituresDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Furnitures(int? page)
        {
            if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(db.NOITHATs.ToList().OrderBy(n => n.MaNT).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(FormCollection collection)
        {
            var email = collection["email"];
            var password = collection["pass"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Error"] = "Email is required!";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Error"] = "Password is required!";
            }
            else
            {
                NGUOIQUANTRI nqt = db.NGUOIQUANTRIs.SingleOrDefault(n => n.TaiKhoanNQT == email && n.MatKhauNQT == password);
                if (nqt != null)
                {
                    ViewBag.Thongbao = "Login successful!";
                    Session["NguoiQuanTri"] = nqt;
                    return RedirectToAction("Furnitures", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Wrong email or password!";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateFurniture()
        {
            if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.MaLoaiNT = new SelectList(db.LOAINOITHATs.ToList().OrderBy(n => n.TenLoaiNT), "MaLoaiNT", "TenLoaiNT");
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateFurniture(NOITHAT nt, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.MaLoaiNT = new SelectList(db.LOAINOITHATs.ToList().OrderBy(n => n.TenLoaiNT), "MaLoaiNT", "TenLoaiNT");
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Choose image, please!!!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Image exist";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    nt.AnhBia = fileName;
                    db.NOITHATs.InsertOnSubmit(nt);
                    db.SubmitChanges();
                }
                return RedirectToAction("Furnitures");
            }
        }
        public ActionResult DetailsFurniture(int id)
        {
            if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            NOITHAT nt = db.NOITHATs.SingleOrDefault(n => n.MaNT == id);
            ViewBag.MaNT = nt.MaNT;
            if (nt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nt);
        }

        [HttpGet]
        public ActionResult DeleteFurniture(int id)
        {
            if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            NOITHAT nt = db.NOITHATs.SingleOrDefault(n => n.MaNT == id);
            ViewBag.MaNT = nt.MaNT;
            if (nt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nt);
        }

        [HttpPost, ActionName("DeleteFurniture")]
        public ActionResult ConfirmFurniture(int id)
        {
            NOITHAT nt = db.NOITHATs.SingleOrDefault(n => n.MaNT == id);
            ViewBag.MaNT = nt.MaNT;
            if (nt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NOITHATs.DeleteOnSubmit(nt);
            db.SubmitChanges();
            return RedirectToAction("Furnitures");
        }
        [HttpGet]
        public ActionResult EditFurniture(int id)
        {
            //if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            NOITHAT nt = db.NOITHATs.SingleOrDefault(n => n.MaNT == id);
            if (nt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //ViewBag.MaLoaiNT = new SelectList(db.LOAINOITHATs.ToList().OrderBy(n => n.TenLoaiNT), "MaLoaiNT", "TenLoaiNT");
            //ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
            return View(nt);
        }
        [HttpPost]
        public ActionResult EditFurniture(NOITHAT nt, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
                    //ViewBag.MaLoaiNT = new SelectList(db.LOAINOITHATs.ToList().OrderBy(n => n.TenLoaiNT), "MaLoaiNT", "TenLoaiNT");
                    //ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
                    var editFurniture = db.NOITHATs.SingleOrDefault(f => f.MaNT == nt.MaNT);
                    if(fileUpload != null)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Image exists";
                        }
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        editFurniture.AnhBia = fileName;
                    }
                    string tes = Convert.ToString(nt.GiaBan);
                    decimal test = decimal.Parse(tes);
                    editFurniture.TenNT = nt.TenNT;
                    editFurniture.SoLuongTon = nt.SoLuongTon;
                    editFurniture.NgayCapNhat = DateTime.Parse(String.Format("{0:dd/MM/yyyy}", DateTime.Now));
                    editFurniture.BestSeller = nt.BestSeller;
                    editFurniture.GiaBan = test;
                    UpdateModel(editFurniture);
                    db.SubmitChanges();
                    return RedirectToAction("Furnitures", "Admin");
                }
                catch (Exception ex)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Customers(int ? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(db.NGUOIDUNGs.ToList().OrderBy(n => n.MaND).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult DeleteCustomer(int id)
        {
            if (Session["NguoiQuanTri"] == null || Session["NguoiQuanTri"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(n => n.MaND == id);
            ViewBag.MaND = nd.MaND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nd);
        }
        [HttpPost, ActionName("DeleteCustomer")]
        public ActionResult ConfirmCustomer(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(n => n.MaND == id);
            ViewBag.MaND = nd.MaND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NGUOIDUNGs.DeleteOnSubmit(nd);
            db.SubmitChanges();
            return RedirectToAction("Customers");
        }

        public ActionResult ShowNamePartial()
        {
            NGUOIQUANTRI nqt = (NGUOIQUANTRI)Session["NguoiQuanTri"];
            return PartialView(nqt);
        }
    }
}