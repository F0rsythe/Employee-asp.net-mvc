using empldotnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace empldotnet.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(User s, string ReturnUrl)
        {
            ViewBag.mess = "User can't add employee";
            var db = new EmployeeDbcontext();
            if (ModelState.IsValid)
            {

                var query1 = db.Users.Where(a => a.Username == s.Username && a.Password == s.Password).FirstOrDefault();
                var query = db.Roles.Where(a => a.UserId == s.Id && a.Role=="Admin");

                if (query1 != null)
                {
                    FormsAuthentication.SetAuthCookie(s.Username, false);
                    Session["uname"] = s.Username.ToString();
                    if (query != null)
                    {
                        ViewBag.mess = "Welcome Admin";
                        return Redirect("~/Login/Addemp");
                    }
                    else
                    {
                        Employee emp1 = db.Employees.FirstOrDefault(a => a.FirstName == s.Username);
                        emp1.FirstName = (string)TempData["Firstname"];
                        emp1.LastName = (string)TempData["LastName"];
                        //emp1.DOB = (DateTime)TempData["Date"];
                        emp1.Department = (string)TempData["Department"];
                        emp1.Status = (string)TempData["Satus"];
                        s.Username = (string)TempData["Usname"];
                        ViewBag.mess = "User can't add employee";
                        return Redirect("~/Login/Userdata");
                    }
                }
            }
            return View();
        }
        public ActionResult Userdata()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Addemp(Employee pass)
        {
            var dc = new EmployeeDbcontext();
            ViewBag.message = "Different";
            var model = new Employee
            {
                FirstName = pass.FirstName,
                LastName = pass.LastName,
                DOB = pass.DOB,
                Status = pass.Status,
                Department = pass.Department
            };
            if (model.FirstName != null && model.LastName != null && model.Department != null && model.DOB != null && model.Status != null)
            {

                dc.Employees.Add(model);
                dc.SaveChanges();

                if (pass.Status == "Admin")
                {
                    TempData["Firstname"] = pass.FirstName;
                    return RedirectToAction("admn");


                }
                else

                    return RedirectToAction("usr");
            }
            else
                ViewBag.message = "Please fill all fields...";
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Search(string search, string deps)
        {

            var db = new EmployeeDbcontext();
            var que = from f in db.Departments
                      select f.dep;
            var list1 = new List<string>();
            list1.AddRange(que.Distinct());
            ViewBag.deps = new SelectList(list1);

            var sch = from t in db.Employees
                      select t;
            var sch1 = from d in db.Departments
                       select d;
            string g = deps;
            if (!String.IsNullOrEmpty(search))
            {
                if (deps == "FirstName")
                {
                    sch = db.Employees.Where(a => a.FirstName.Equals(search));
                }
                else if (deps == "LastName")
                {
                    sch = db.Employees.Where(a => a.LastName.Equals(search));
                }
                else if (deps == "Date")
                {
                    sch = db.Employees.Where(a => a.DOB.Equals(search));
                }
                else if (deps == "Department")
                {
                    sch = db.Employees.Where(a => a.Department.Equals(search));
                }
            }
            if (!String.IsNullOrEmpty(deps))
            {
                sch1 = db.Departments.Where(x => x.dep == deps);
            }
            return View(sch);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult admn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult admn(User a, string passwordconfirm)
        {
            var db = new EmployeeDbcontext();
            var log = new User
            {
                Username = a.Username,
                Password = a.Password,
            };

            var rl = new Models.Roles
            {
                UserId = log.Id,
                Role = "Admin"
            };
            if (passwordconfirm == a.Password)
            {
                db.Users.Add(log);
                db.Roles.Add(rl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.mess = "Make sure both ppasswords are the same";


            return View();
        }
        
        public ActionResult usr()
        {
            return View();
        }
        [HttpPost]
        public ActionResult usr(User a, string passwordconfirm)
        {
            var db = new EmployeeDbcontext();
            var log = new User
            {
                Username = a.Username,
                Password = a.Password,
            };

            var rl = new Models.Roles
            {
                UserId = log.Id,
                Role = "User"
            };
            if (passwordconfirm == a.Password)
            {
                db.Users.Add(log);
                db.Roles.Add(rl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.mess = "Make sure both ppasswords are the same";


            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult getList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getList(string val)
        {
            var db = new EmployeeDbcontext();
            var sche = from t in db.Employees
                      select t;
            if (val == "User")
            {
                sche = db.Employees.Where(a => a.FirstName.Equals(val));
            }
            else if (val == "Admin")
            {
                sche = db.Employees.Where(a => a.LastName.Equals(val));
            }
            return View(sche);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult changeRole(Employee employee)
        {
            var db = new EmployeeDbcontext();
            Models.Roles df = new Models.Roles();
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                df = db.Roles.FirstOrDefault(x=>x.UserId == employee.Id);
                df.Role = employee.Status;
                db.SaveChanges();
                return RedirectToAction("getList");
            }
            return View(employee);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}