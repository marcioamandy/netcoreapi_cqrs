using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace Globo.PIC.Infra.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryStringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string AddQueryString(
            string uri,
            Dictionary<string, object> queryString)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (queryString == null || queryString.Count() == 0 || queryString.Values.Any(k => k == null))
                throw new ArgumentNullException(nameof(queryString));

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurence.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);

            foreach (var parameter in queryString)
            {
                if (parameter.Value is IEnumerable<int>)
                {
                    foreach(var item in ((IEnumerable<int>)parameter.Value))
                    {
                        sb.Append(hasQuery ? '&' : '?');
                        sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                        sb.Append('=');
                        sb.Append(UrlEncoder.Default.Encode(item.ToString()));
                        hasQuery = true;
                    }
                }
                else if (parameter.Value is IEnumerable<long>)
                {
                    foreach (var item in ((IEnumerable<long>)parameter.Value))
                    {
                        sb.Append(hasQuery ? '&' : '?');
                        sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                        sb.Append('=');
                        sb.Append(UrlEncoder.Default.Encode(item.ToString()));
                        hasQuery = true;
                    }
                }
                else if (parameter.Value is IEnumerable<string>)
                {
                    foreach (var item in ((IEnumerable<string>)parameter.Value))
                    {
                        sb.Append(hasQuery ? '&' : '?');
                        sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                        sb.Append('=');
                        sb.Append(UrlEncoder.Default.Encode(item));
                        hasQuery = true;
                    }
                }
                else if (parameter.Value is Dictionary<string, long>)
                {
                    foreach (var item in ((Dictionary<string, long>)parameter.Value))
                    {
                        sb.Append(hasQuery ? '&' : '?');
                        sb.Append(string.Format("{0}[{1}]", UrlEncoder.Default.Encode(parameter.Key), item.Key));
                        sb.Append('=');
                        sb.Append(UrlEncoder.Default.Encode(item.Value.ToString()));
                        hasQuery = true;
                    }
                }
                else if(parameter.Value is DateTime)
                {
                    sb.Append(hasQuery ? '&' : '?');
                    sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                    sb.Append('=');
                    sb.Append(UrlEncoder.Default.Encode(((DateTime)parameter.Value).ToString("yyyy-MM-dd")));
                }
                else if (parameter.Value is string || parameter.Value is int)
                {
                    sb.Append(hasQuery ? '&' : '?');
                    sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                    sb.Append('=');
                    sb.Append(UrlEncoder.Default.Encode(parameter.Value.ToString()));
                }
                else
                    throw new ArgumentNullException(nameof(queryString));

                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}
