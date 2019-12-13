using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class OrderRequest
    {
        /// <summary>
        /// Indicates the orientation of the sort "asc" for ascending or "desc" for desc
        /// </summary>
        [JsonProperty("dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Column which contains the number of column which requires this sort.
        /// </summary>
        [JsonProperty("column")]
        public int? Column { get; set; }

        public string ToExpression(JqDataTable request)
        {
            return string.Concat(request.Columns[Column.Value].Data, " ", Dir);
        }
    }
}