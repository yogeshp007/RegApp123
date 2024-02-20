using Newtonsoft.Json;
using RegApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegApp.Controllers
{
    public class MyUserController : Controller
    {
        private DBEntities db = new DBEntities();

        public ActionResult Index(int id=0)
        {
           
            ViewBag.states = new SelectList(db.tblStates.ToList(), "Id", "sname");
            ViewBag.edus = new List<string> { "SSC", "HSC", "Diploma", "Degree", "Masters", "Phd" };
            if (id > 0)
                return View(db.tblUsers.Find(id));
            else
            {
                return View(new tblUser());
            }
        }

        [HttpPost]
        public ActionResult Index(tblUser user,HttpPostedFileBase ufile,FormCollection fc)
        {
            if (user.Id>0)
            {
                tblUser euser = db.tblUsers.Find(user.Id);
            
                if (ufile!=null)
                {
                    euser.uphoto = "/UserImages/" + ufile.FileName;
                    ufile.SaveAs(Server.MapPath(euser.uphoto));
                }
                else
                {
                    euser.uphoto = fc["imgval"].ToString();
                }
                
                euser.ueducation = fc["edus"].ToString();
                euser.uname = user.uname;
                euser.ugender = user.ugender;
                euser.uemail = user.uemail;
                euser.stateid = user.stateid;
                euser.cityid = user.cityid;

                //save changes

                db.SaveChanges();
                return RedirectToAction("ViewAllUsers");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    user.uphoto = "/UserImages/" + ufile.FileName;
                    ufile.SaveAs(Server.MapPath(user.uphoto));
                    user.ueducation = fc["edus"].ToString();
                    user.isActive = true;
                    user.regDate = DateTime.Now;

                    db.tblUsers.Add(user);
                    db.SaveChanges();

                    ViewBag.msg = "user record saved";
                    ModelState.Clear();
                    ViewBag.states = new SelectList(db.tblStates.ToList(), "Id", "sname");
                    ViewBag.edus = new List<string> { "SSC", "HSC", "Diploma", "Degree", "Masters", "Phd" };
                    return RedirectToAction("ViewAllUsers");
                }
                ViewBag.states = new SelectList(db.tblStates.ToList(), "Id", "sname");
                ViewBag.edus = new List<string> { "SSC", "HSC", "Diploma", "Degree", "Masters", "Phd" };
                return View(new tblUser());
               
            }
          
        }

        public JsonResult GetCityByState(int id)
        {
            var data = db.tblCities.Include("stateid").Select(e => new { e.Id, e.cname, e.stateid }).Where(e => e.stateid == id).ToList();
            //var data = db.tblCities.Where(a => a.stateid == id).ToList();
            //var list = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});
            //// var data = db.tblCities.Where(a => a.stateid == id).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAllUsers()
        {
            //var data = from u in db.tblUsers
            //           join c in db.tblCities
            //           on u.cityid equals c.Id
            //           join s in db.tblStates 
            //           on u.stateid equals s.Id
            //           select new {s.sname,c.cname,u.Id,u.uname,u.uphoto};

            //List<tblUser> users = new List<tblUser>();
            //foreach (var item in data)
            //{
            //    tblUser user = new tblUser();
            //    user.Id = item.u.Id;
            //    user.uname = item.u.uname;
            //    user.uphoto = item.u.uphoto;
            //    tblState ts = new tblState();
            //    ts.sname = item.s.sname;
            //    user.tblState = ts;
            //    tblCity tc = new tblCity();
            //    tc.cname = item.c.cname;
            //    user.tblCity = tc;

            //    users.Add(user);
            //}

         var li = db.Database.SqlQuery<UserList>("GetUsersList");


            return View(li);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            var tu = db.tblUsers.SingleOrDefault(a => a.Id == id);
            tu.isActive = false;
            db.SaveChanges();
            return RedirectToAction("ViewAllUsers");
        }
    }
}