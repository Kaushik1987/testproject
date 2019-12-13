using AwsWebApi.Models;
using FizzWare.NBuilder;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AwsWebApi.Core;
using System.Linq;

namespace AwsWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Redis Sample";
            //IEnumerable<string> allKey;
            //using (var redisClient = new RedisClient("ehrredis.y2ehux.0001.aps1.cache.amazonaws.com", 6379))
            //{
            //    redisClient.Set("kaushik", "TwoMate");
            //    allKey = redisClient.ScanAllKeys();
            //}
            return View();
        }
        [HttpPost]
        public ActionResult Index(SearchParam dataTable)
        {
            #region DataTable
            var search = Request["search[value]"];
            var sortColumn = -1;
            var sortDirection = "asc";
            dataTable.Start = dataTable.Start.HasValue ? dataTable.Start / dataTable.Length : 0;

            // note: we only sort one column at a time
            if (Request["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request["order[0][column]"]);
            }
            if (Request["order[0][dir]"] != null)
            {
                sortDirection = Request["order[0][dir]"];
            }
            #endregion

            var request = GetEmp(dataTable, dataTable.Start.Value,
                dataTable.Length ?? 10,
                out var totalRecords,
                out var recordsFiltered,
                sortColumn,
                sortDirection);

            var result = request.Data?.ToList();

            var response = new
            {
                data = result,
                draw = dataTable.Draw,
                recordsTotal = totalRecords,
                recordsFiltered,
                ViewBag.source
            };
            return Json(response);
        }

        public ActionResult Test()
        {
            try
            {
                using (var redisClient = new RedisClient("ehrredis.y2ehux.0001.aps1.cache.amazonaws.com", 6379))
                {

                    redisClient.Set("employess", Common.EmployeesList);
                    var result = redisClient.Get<IEnumerable<Employee>>("employess");

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(Common.EmployeesList, JsonRequestBehavior.AllowGet);
            }
           

        }

        private GeneralResponse<IEnumerable<Employee>> GetEmp(SearchParam filter, int initialPage, int pageSize, out int totalRecords, out int recordFilterd,
           int sortColumn, string sortDirection)
        {
            var response = new GeneralResponse<IEnumerable<Employee>>();
            totalRecords = 0;
            recordFilterd = 0;

            try
            {
                var data = Enumerable.Empty<Employee>().AsQueryable();
                try
                {
                    using (var redisClient = new RedisClient("ehrredis.y2ehux.0001.aps1.cache.amazonaws.com", 6379))
                    {
                        var rdata = redisClient.Get<IEnumerable<Employee>>("employess");
                        data = rdata.AsQueryable();
                        ViewBag.source = "redis";
                    }
                }
                catch (Exception e)
                {
                    data = Common.EmployeesList.AsQueryable();
                    ViewBag.source = "Database";
                }
                

                totalRecords = data.Count();
                //filter 
                if (!string.IsNullOrWhiteSpace(filter.FirstName))
                {
                    data = data.Where(x =>
                        x.FirstName.ToLower().Contains(filter.FirstName.Trim().ToLower())
                    );

                }
                if (!string.IsNullOrWhiteSpace(filter.LastName))
                {
                    data = data.Where(x =>
                        x.LastName.ToLower().Contains(filter.LastName.Trim().ToLower())
                    );
                }
                if (!string.IsNullOrWhiteSpace(filter.Department))
                {
                    data = data.Where(x =>
                        x.Department.ToLower() == filter.Department.Trim().ToLower()
                    );
                }
                if (filter.FromDob != null && filter.FromDob != default(DateTime))
                {
                    data = data.Where(x => x.DOB >= filter.FromDob);
                }
                if (filter.ToDob != null && filter.ToDob != default(DateTime))
                {
                    filter.ToDob = filter.ToDob.AddHours(23).AddMinutes(59);
                    data = data.Where(x => x.DOB <= filter.ToDob);

                }
                recordFilterd = data.Count();

                //sort 
                var ascending = sortDirection == "asc";
                switch (sortColumn)
                {
                    case 0:
                        data = data.OrderBy(p => p.EmployeeId, ascending);
                        break;
                    case 1:
                        data = data.OrderBy(p => p.LastName, ascending);
                        break;
                    case 2:
                        data = data.OrderBy(p => p.FirstName, ascending);
                        break;
                    case 3:
                        data = data.OrderBy(p => p.DOB, ascending);
                        break;
                    case 4:
                        data = data.OrderBy(p => p.Gender, ascending);
                        break;
                    case 5:
                        data = data.OrderBy(p => p.Street, ascending);
                        break;
                    case 6:
                        data = data.OrderBy(p => p.City, ascending);
                        break;
                    case 7:
                        data = data.OrderBy(p => p.State, ascending);
                        break;
                    case 8:
                        data = data.OrderBy(p => p.Zip, ascending);
                        break;
                    case 9:
                        data = data.OrderBy(p => p.Department, ascending);
                        break;
                    case 10:
                        data = data.OrderBy(p => p.Occupation, ascending);
                        break;
                    case 11:
                        data = data.OrderBy(p => p.Occupation, ascending);
                        break;
                    default:
                        data = data.OrderBy(p => p.Salary, ascending);
                        break;
                }

                data = data
                    .Skip(initialPage * pageSize)
                    .Take(pageSize);

                var result = data.ToList();
                response.Data = result;

            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
            }
            return response;
        }
    }
}
