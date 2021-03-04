using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JunkSite.db;
namespace JunkSite.web.Models
{
    public class ViewJunkViewModel
    {
        public List<Junk> JunkList {get;set;}
        public List<int> Ids { get; set; }
  
        
       
    }
   
}
