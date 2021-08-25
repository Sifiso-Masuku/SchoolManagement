﻿using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using SchoolManagement.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers.CartController
{
    public class OrdersController : Controller
    {
        private Order_Service order_Service;
        private ApplicationDbContext db = new ApplicationDbContext();

        public OrdersController()
        {
            this.order_Service = new Order_Service();
        }

        //Customer orders
        public ActionResult Customer_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.Status = "All";
                return View(order_Service.GetOrders());
            }
            else
            {
                ViewBag.Status = id;
                return View(order_Service.GetOrders(id));
            }
        }
        public ActionResult New_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.Status = "All";
                return View(order_Service.GetOrders());
            }
            else
            {
                ViewBag.Status = id;
                return View(order_Service.GetOrders(id));
            }
        }
        public ActionResult Order_Details(string id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (order_Service.GetOrder(id) != null)
                return View(order_Service.GetOrderDetail(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Order_Tracking(string id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (order_Service.GetOrder(id) != null)
            {
                ViewBag.Order = order_Service.GetOrder(id);
                return View(order_Service.GetOrderTrackingReport(id));
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult Mark_As_Packed(string id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (order_Service.GetOrder(id) != null)
            {
                order_Service.MarkOrderAsPacked(id);
                return RedirectToAction("Order_Details", new { id = id });
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult schedule_OrderDelivery(string id, DateTime date)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (order_Service.GetOrder(id) != null)
            {
                order_Service.schedule_OrderDelivery(id, date);
                return RedirectToAction("Order_Details", new { id = id });
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        //account orders
        public ActionResult Order_History()
        {
            var userName = User.Identity.GetUserName();
            var gaudian = db.Guardians.Where(x => x.Email == userName);
            return View(order_Service.GetOrders().Where(x => x.Email == User.Identity.Name));
        }
    }
}