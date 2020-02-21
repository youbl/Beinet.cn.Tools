using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beinet.cn.Tools.Gitlab
{
    public class GitlabHelper
    {
        public string PrefixUrl { get; private set; }
        private string PrivateToken { get; set; }

        public GitlabHelper(string prefixUrl, string privateToken)
        {
            if (string.IsNullOrEmpty(prefixUrl) || string.IsNullOrEmpty(privateToken))
            {
                throw new Exception("前缀Url或私匙不能为空");
            }

            if (prefixUrl[prefixUrl.Length - 1] != '/')
                prefixUrl += '/';
            PrefixUrl = prefixUrl;
            PrivateToken = privateToken;
        }

        /// <summary>
        /// 分页获取项目
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // curl --header "PRIVATE-TOKEN: 12345678" "https://xxx/api/v4/projects/"
        public List<GitProject> GetProjects(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            // per_page每页记录数，默认20，最大100, page显示第几页，默认1
            var url = PrefixUrl + "api/v4/projects?order_by=id&per_page=" + pageSize.ToString() + "&page=" + page.ToString();
            var headers = new Dictionary<string, string>()
            {
                {"PRIVATE-TOKEN", PrivateToken}
            };

            var ret = new List<GitProject>();

            var result = Utility.GetPage(url, headers: headers);

            var jsonObj = JsonConvert.DeserializeObject(result) as JArray;
            if (jsonObj == null)
                return null;

            foreach (var item in jsonObj)
            {
                var id = Convert.ToInt32(item["id"]);
                var name = Convert.ToString(item["name"]);
                var desc = Convert.ToString(item["description"]);
                var giturl = Convert.ToString(item["http_url_to_repo"]);

                var project = new GitProject
                {
                    Id = id,
                    Name = name,
                    Desc = desc,
                    Url = giturl
                };
                ret.Add(project);
            }

            return ret;
        }

        /// <summary>
        /// 一次性返回所有项目
        /// </summary>
        /// <returns></returns>
        public List<GitProject> GetAllProjects()
        {
            var ret = new List<GitProject>();

            List<GitProject> perResult;
            var page = 1;
            var pageSize = 100;
            do
            {
                perResult = GetProjects(page, pageSize);
                page++;
                if(perResult != null && perResult.Count > 0)
                    ret.AddRange(perResult);
            } while (perResult != null && perResult.Count >= pageSize);

            return ret;
        }

        public class GitProject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
            public string Url { get; set; }

        }
    }
}