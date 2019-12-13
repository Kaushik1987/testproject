using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class SearchRequest
    {
        /// <summary>
        /// Search value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// true if the search value should be treated as a regular expression for advanced searching, false otherwise. 
        /// </summary>
        [JsonProperty("regex")]
        public bool Regex { get; set; }

        public virtual string Operator { get; set; }

        public SearchRequest()
        {
            Operator = @"contains";
        }

        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"}
        };

        public virtual string ToExpression(JqDataTable request)
        {
            if (Regex)
                throw new NotImplementedException("Regular Expression is not implemented");

            string comparison = operators[Operator];
            List<string> expressions = new List<string>();
            foreach (var searchableColumn in request.Columns.Where(c => c.Searchable.HasValue && c.Searchable.Value))
            {
                if (Operator == "doesnotcontain")
                {
                    expressions.Add(string.Format("!{0}.ToString().{1}(\"{2}\")", searchableColumn.Data, comparison, Value));
                }
                else if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
                {
                    expressions.Add(string.Format("{0}.ToString().{1}(\"{2}\")", searchableColumn.Data, comparison, Value));
                }
                else
                {
                    expressions.Add(string.Format("{0} {1} \"{2}\"", searchableColumn.Data, comparison, Value));
                }
            }
            return string.Join(" or ", expressions);
        }
    }
}