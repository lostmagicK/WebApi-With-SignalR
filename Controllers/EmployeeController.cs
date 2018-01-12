using Dependency2.DAL;
using Dependency2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dependency2.Controllers
{
    public class EmployeeController : ApiController
    {
        //DataAccess Empdata = new DataAccess();
        public IEnumerable<Employee> Get()
        {
            return DAL.DataAccess.GetData();
        }
    }
}
