using JunkSite.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JunkSite.db;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace JunkSite.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=Junk;Integrated Security=true;";
       
        public IActionResult Index()
        {
           
            ViewJunkViewModel vm = new ViewJunkViewModel();

            DbManager db = new DbManager(_connectionString);
            vm.JunkList = db.GetAllJunk();
            List<int> list = HttpContext.Session.Get<List<int>>("ListOfIds");
           if(list != null)
            {
                vm.Ids = list;
            }
            return View(vm);
        }
        public IActionResult AddJunk()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitJunk(string name, string details, string phone)
        {
            Junk junk = new Junk
            {
                Name = name,
                Details = details,
                Phone = phone
            };
            DbManager db = new DbManager(_connectionString);
            int newId = db.AddJunk(junk);           
            if (HttpContext.Session.Get<List<int>>("ListOfIds") == null)
            {
                List<int> idList = new List<int>();
                HttpContext.Session.Set("ListOfIds", idList);
            }
            List<int> idListToAddTo= HttpContext.Session.Get<List<int>>("ListOfIds");
            idListToAddTo.Add(newId);
            HttpContext.Session.Set("ListOfIds", idListToAddTo);
            return Redirect($"/home/index");
        }
        public IActionResult DeletePost(int deleteId)
        {
            DbManager db = new DbManager(_connectionString);
            db.Delete(deleteId);
            return Redirect("/home/index");
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
