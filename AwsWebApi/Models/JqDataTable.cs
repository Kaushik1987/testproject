using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class JqDataTable
    {
        [JsonProperty("draw")]
        public int? Draw { get; set; }
        [JsonProperty("start")]
        public int? Start { get; set; }
        [JsonProperty("length")]
        public int? Length { get; set; }
        [JsonProperty("columns")]
        public List<ColumnRequest> Columns { get; set; }
        [JsonProperty("order")]
        public List<OrderRequest> Order { get; set; }
    }
}