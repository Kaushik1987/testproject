using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class SearchParam : JqDataTable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime FromDob { get; set; }
        public DateTime ToDob { get; set; }
        public string Department { get; set; }
    }
}