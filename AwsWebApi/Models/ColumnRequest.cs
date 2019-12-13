using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class ColumnRequest
    {
        /// <summary>
        /// Specific search information for the column
        /// </summary>
        [JsonProperty("search")]
        public SearchRequest Search { get; set; }

        /// <summary>
        /// Column's data source, as defined by columns.data.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Column's name, as defined by columns.name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is search-able (true) or not (false). This is controlled by columns.searchable.
        /// </summary>
        [JsonProperty("searchable")]
        public bool? Searchable { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false). This is controlled by columns.orderable.
        /// </summary>
        [JsonProperty("orderable")]
        public bool? Orderable { get; set; }

    }
}