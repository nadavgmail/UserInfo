using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using UserInfo;
using UserInfo.Models;

namespace UserInfo.Controllers
{
    public class usersController : Controller
    {
        private UserInfoEntities db = new UserInfoEntities();

        // GET: users
        public ActionResult Index()
        {
            return View(db.users.ToList().OrderBy(u=>u.Name));
        }

       



        // GET: users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Name,ID,IP,Phone")] users users)
        public ActionResult Create(UserData usersdata)
        {
            
            users Users = null;
            if (ModelState.IsValid)
            {
                if(db.users.Where(u=>u.ID == usersdata.ID).Count() > 0)
                {
                    ModelState.AddModelError("ID", "user is already exsit");
                    return View();
                }
                usersdata.Phone = "+972-" + usersdata.Phone.Remove(0, 1);
                Users = new users() { ID = usersdata.ID, IP = usersdata.IP, Name = usersdata.Name, Phone = usersdata.Phone };
                db.users.Add(Users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Users);
        }

        // GET: users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            users users = db.users.Find(id);
            
            if (users == null)
            {
                //probebly someone delete the user RedirectT to the list
                return RedirectToAction("index");
            }
            string shortPhone = "0" + users.Phone.Split('-')[1];
            return View(new UserData() {ID = users.ID, Name = users.Name, IP= users.IP, Phone = shortPhone });
        }
        //Because the site is on https and the api is http the only way i could get data 
        //was using webclient , in order not to get mixed content
        [HttpGet]
        public ActionResult IpInfo(string ipAdress)
        {
            string url = WebConfigurationManager.AppSettings["IpWebApi"] + ipAdress;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            String ipStringInfo = String.Empty;
            var ObjStringInfo = new Dictionary<string, string>() { };
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                ipStringInfo = reader.ReadToEnd();
                ObjStringInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(ipStringInfo);
                reader.Close();
                dataStream.Close();
            }
            if (ObjStringInfo.ContainsKey("status") && ObjStringInfo["status"] == "success")
            {
                if (ObjStringInfo.ContainsKey("status"))
                {
                    ObjStringInfo.Remove("status");
                }
                return View(ObjStringInfo);
            }
            return View("IpInfoMissing");
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //PreviousId
        //public ActionResult Edit([Bind(Include = "Name,ID,IP,Phone")] UserData usersdata)
        public ActionResult Edit(UserData usersdata,string PreviousId)
        {
            users Users = null;

            if(PreviousId != usersdata.ID && db.users.Where(u=>u.ID == usersdata.ID).Count() > 0)
            {
                ModelState.AddModelError("ID", "User Id Already Exists");
                return View(usersdata);
            }
            if (ModelState.IsValid)
            {
                //widht postman we can ovveride the duplicate id logic using try catch
                try
                {
                    usersdata.Phone = "+972-" + usersdata.Phone.Remove(0, 1);
                    Users = new users() { ID = usersdata.ID, IP = usersdata.IP, Name = usersdata.Name, Phone = usersdata.Phone };
                    db.Entry(Users).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "something went wrong try again");
                }
                
            }
            return View(usersdata);
        }

      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult DeleteConfirmedAajax(string id)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (string.IsNullOrEmpty(id))
            {
                result.Data = new { status = "fail", deletedUser = "null" };
                return result;
            }
            var  user = db.users.Find(id);
            if (user == null)
            {
                result.Data = new { status = "fail", deletedUser = id };
                return result;
            }
            db.users.Remove(user);
            db.SaveChanges();

            result.Data = new { status = "ok", deletedUser = id };
            return result;

        }
    }
}
