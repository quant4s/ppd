using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using CsvHelper;

namespace PpdCrawler
{
    public partial class Form1 : Form
    {
        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        HttpClient client;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // 登录拍拍贷
            String loginUrl = "https://ac.ppdai.com/User/Login";
            String body = "UserName=13305278179&Password=1qaz2wsx";
            String loginHtml = client.Post(loginUrl, body);
            Console.WriteLine(loginHtml);

            // 爬借款首页
            //String loanUrl = "http://invest.ppdai.com/loan/listnew";
            //client.Get(loanUrl);

            // 爬借款人 种子
            String userUrl = "http://www.ppdai.com/user/davidliu66";
            String userHtml = client.Get(userUrl, Encoding.UTF8);
            writeFile(userHtml, "d:\\ppdai\\user\\davidliu66.html");
            extractUserHtml(userUrl, userHtml);

            String url = "";
            int i = 0;
            // 开始爬取url 列表
            while (queue.Count != 0)
            {
                url = queue.Dequeue();
                if (!url.StartsWith("http://invest.ppdai.com/MiddleUser/"))
                {
                    String resp = "";
                    try
                    {
                        resp = client.Get(url, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "发生异常");
                    }
                    if (resp != null)
                    {
                        string name = "";
                        log.Debug(url);

                        if (url.StartsWith("http://www.ppdai.com/user/"))
                        {
                            extractUserHtml(url, resp);
                            name = url.Substring(url.LastIndexOf("/"));
                            if (name.Contains("="))
                            {
                                name = name.Substring(name.LastIndexOf("="));
                            }
                            // 保存到user
                            name = "d:\\ppdai\\user\\" + name + ".html";
                        }
                        else
                        {
                            extractLoanHtml(url, resp);
                            // 保存到loan
                            name = "d:\\ppdai\\loan\\" + url.Substring(url.LastIndexOf("/")) + ".html";
                        }

                        Application.DoEvents();
                        txtLogInfo.Text = String.Format("{2}: 合计有{0}序列，{1}在待爬取队列 \r\n", totalUrls.Count, queue.Count, i++);
                        // 写文件		content	"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>彩虹计划投资明细</title>\r\n    <meta name=\"renderer\" content=\"webkit\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"description\" content=\"互联网金融P2P网络优质借贷平台。提供小额贷款,短期贷款,个人贷款,自定利率,借期灵活。您还可以成为借出人理财投资,低门槛，获得较之银行高年收益率回报，更有一键投标，自动投标等快速投资工具\" />\r\n    <meta name=\"keywords\" content=\"网络贷款,民间借贷,小额贷款,无抵押贷款,信用贷款,网络借贷,借贷平台,拍拍贷,人人贷,投资理财,个人理财,p2p贷款,贷款,互联网金融,投融资\" />\r\n    <link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.ppdaicdn.com/2014/css/basic.css?091901\" />\r\n    <link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.ppdaicdn.com/2014/css/layout.css?091901\" />\r\n    <link href=\"http://www.ppdaicdn.com/css/min/validation-min.css\" rel=\"stylesheet\" />\r\n    <link rel=\"shortcut icon\" href=\"http://www.ppdaicdn.com/favicon.ico\" type=\"image/x-icon\" />\r\n    \r\n    <link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.ppdaicdn.com/invest/2014/css/activity/rainbowdetail.css\" />\r\n\r\n</head>\r\n<body>\r\n    <div class=\"top PPD_header_nav\">\r\n        <div class=\"top_inner w1188center clearfix PPD_login_status\">\r\n        </div>\r\n    </div>\r\n\r\n\r\n <!--头部开始-->\r\n    <div class=\"mainNav\">\r\n        <div class=\"mainNav_inner clearfix w1000center\">\r\n            <h1 class=\"logo\">\r\n                <a href=\"http://www.ppdai.com/\"><img src=\"https://ac.ppdaicdn.com/img/logo.png\" alt=\"\" /></a>\r\n            </h1>\r\n            <ul id=\"tabIcon\">\r\n                <li class=\"hasSubMenu\">\r\n                    <a href=\"http://invest.ppdai.com/loan/listnew\"  category=\"Lend\" id=\"top-menu-lend\">我要投资</a>\r\n                    <div class=\"subMenu\">\r\n                        <a href=\"http://invest.ppdai.com/loan/listnew\">散标列表</a>\r\n                        <a href=\"http://www.ppdai.com/debtdeal/AlldebtList/DebtList\" >债权交易</a>\r\n                        <a href=\"http://www.ppdai.com/product/plan/rainbow\">彩虹计划</a>\r\n                        <a href=\"http://rise.invest.ppdai.com/\">月月涨</a>\r\n                        <a href=\"http://product.invest.ppdai.com/\" >拍活宝</a>\r\n                        <a href=\"http://www.ppdai.com/help/nuDeftFinancierPlan\">新手专区</a>\r\n                      <a href=\"http://www.ppdai.com/landinglender.html\">新手产品</a> \r\n                        \r\n                    </div>\r\n                </li>\r\n                <li class=\"hasSubMenu\">\r\n                    <a href=\"http://loan.ppdai.com/borrow\"  category=\"Borrow\">我要借款</a>\r\n                    <div class=\"subMenu\">\r\n                        <a href=\"http://loan.ppdai.com/borrow\">我要借款</a>\r\n                        <a href=\"http://loan.ppdai.com/account/repaymentlist\">我要还款</a>\r\n                        <a href=\"http://www.ppdai.com/borrow/interestcalculate\">利息计算器</a>\r\n                    </div>\r\n                </li>\r\n              \r\n               <li class=\"hasSubMenu\">\r\n                    <a href=\"http://www.ppdai.com/account\"  category=\"Account\">我的账户</a>\r\n                    <div class=\"subMenu\">\r\n                        <a href=\"http://loan.ppdai.com/account/borrow\">借款账户</a>\r\n                        <a href=\"http://www.ppdai.com/account/lend\">投资账户</a>\r\n                    </div>\r\n                </li>\r\n\r\n               <li class=\"hasSubMenu\">\r\n                    <a href=\"http://www.ppdai.com/landinginformat.html\"  id=\"top-menu-about\">信息披露</a>\r\n                    <div class=\"subMenu\">\r\n                        <a href=\"http://www.ppdai.com/help/aboutus\">关于我们</a>\r\n                        <a href=\"http://www.ppdai.com/help/howworks\">工作原理</a>\r\n                      \t<a href=\"http://ppdai.zhiye.com/\" target=\"_blank\">招贤纳士</a>\r\n                       <a href=\"http://www.ppdai.com/landinginformat.html\" target=\"_blank\">信息披露</a>\r\n                    </div>\r\n                </li>\r\n               <li class=\"hasSubMenu\">\r\n                    <a  id=\"top-menu-about\" href=\"http://group.ppdai.com/forum.php\" target=\"_blank\">论坛/帮助</a>\r\n                    <div class=\"subMenu\">\r\n                      <a href=\"http://group.ppdai.com/\">论坛</a>\r\n                        <a href=\"http://help.ppdai.com/\">帮助中心</a>\r\n                        \r\n                    </div>\r\n                </li>\r\n            </ul>\r\n        </div>\r\n    </div>\r\n\r\n    <!--头部结束-->\r\n\r\n    \r\n<div class=\"main\" style=\"margin-top:20px;\">\r\n    <!--面包屑开始-->\r\n    \r\n<div>\r\n    <ul class=\"breadcrumb\" style=\"border: none !important;\">\r\n        <li><a href=\"http://www.ppdai.com\">首页</a> <span class=\"divider\">&gt;</span></li>\r\n                        <li><a href=\"http://www.ppdai.com/lend\">我要投资</a> <span class=\"divider\">&gt;</span></li>\r\n                    <li class=\"active\">借款列表详情</li>\r\n    </ul>\r\n</div>\r\n<script>\r\n    var breadcrumbCategory = \"Lend\";\r\n</script>\r\n\r\n    <!--面包屑结束-->\r\n    <!--/*start*/-->\r\n    <div class=\"rain_con\">\r\n        <div class=\"ranbowdcon\">\r\n            <div class=\"ranbowtit\">月月涨投资明细</div>\r\n            <div class=\"ranbacc\">\r\n                <span class=\"ranbaccou\">账号名：<em>ppd_sys_ddup18mu0003</em></span>\r\n                <span class=\"ranbaccp\">期数：<em>第6期</em></span>\r\n            </div>\r\n            <div class=\"rbInvestlist\">\r\n                <dl>\r\n                        <dt class=\"rbinvpbr rbinvurrent\">当前投资人列表</dt>\r\n                         <a href=\"/middleuser/index?user=ppd_sys_ddup18mu0003&typeid=1\"><dt>当前投资标列</dt></a>\r\n                </dl>\r\n                <div class=\"rbinlistconde\">\r\n                    <div class=\"rbinalistcon\">\r\n                        <div class=\"rbn890\">\r\n                            <ul>\r\n                                <li class=\"rbinalusern\">用户名 </li>\r\n                                <li class=\"rbinalusemon\">投资金额</li>\r\n                            </ul>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">hippy</li>\r\n                                <li class=\"rbinalusemond\">&#165;100,000.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">souledge</li>\r\n                                <li class=\"rbinalusemond\">&#165;0.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">cmore</li>\r\n                                <li class=\"rbinalusemond\">&#165;0.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">shalove</li>\r\n                                <li class=\"rbinalusemond\">&#165;1,700.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">ckelzhao</li>\r\n                                <li class=\"rbinalusemond\">&#165;0.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">yedeyun</li>\r\n                                <li class=\"rbinalusemond\">&#165;500.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">andylee018</li>\r\n                                <li class=\"rbinalusemond\">&#165;0.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">jsjhyyb123</li>\r\n                                <li class=\"rbinalusemond\">&#165;1,600.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">zxy228</li>\r\n                                <li class=\"rbinalusemond\">&#165;70,300.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">qiaoqiao0628</li>\r\n                                <li class=\"rbinalusemond\">&#165;500.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">8991542</li>\r\n                                <li class=\"rbinalusemond\">&#165;50,000.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">toyel</li>\r\n                                <li class=\"rbinalusemond\">&#165;800.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">adonisxzh</li>\r\n                                <li class=\"rbinalusemond\">&#165;20,000.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">benlye</li>\r\n                                <li class=\"rbinalusemond\">&#165;500.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">552416884</li>\r\n                                <li class=\"rbinalusemond\">&#165;300.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">66114997</li>\r\n                                <li class=\"rbinalusemond\">&#165;1,500.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">eric922</li>\r\n                                <li class=\"rbinalusemond\">&#165;100.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">superginger</li>\r\n                                <li class=\"rbinalusemond\">&#165;2,000.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">yanhui3</li>\r\n                                <li class=\"rbinalusemond\">&#165;1,300.00</li>\r\n                            </ol>\r\n                            <ol>\r\n                                <li class=\"rbinalusernd\">szyr</li>\r\n                                <li class=\"rbinalusemond\">&#165;0.00</li>\r\n                            </ol>\r\n                        </div>\r\n                        <div style=\"clear:both\"></div>\r\n                        <div class='pager'><span class='pagerstatus'>共156页</span><a class='lastPage' href='javascript:void(0)'>首页</a><a class='prepage' href='javascript:void(0)'><</a><a href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=1' class='currentpage'>1</a><a href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=2'>2</a><a href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=3'>3</a><a href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=4'>4</a><a href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=5'>5</a><a class='nextpage' href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=2'>></a><a class='' href='/MiddleUser/index?user=ppd_sys_ddup18mu0003&pageindex=156'>尾页</a></div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--end-->\r\n    </div>\r\n    <!--/*end*-->\r\n</div>\r\n <!--底部-->\r\n    <div class=\"footer\">\r\n        <div class=\"footer_footerBottom\">\r\n            <ul class=\"footer_footerBottomNav clearfix\">\r\n                <li><span class=\"webindex\"></span><a href=\"http://www.ppdai.com/\">网站首页</a>|</li>\r\n                <li><span class=\"aboutus\"></span><a href=\"http://www.ppdai.com/help/aboutus\">关于我们</a>|</li>\r\n                <li><span class=\"mapsite\"></span><a href=\"http://www.ppdai.com/home/sitemap\">网站地图</a>|</li>\r\n                <li><span class=\"webservice\"></span><a href=\"http://help.ppdai.com/\">帮助中心</a>|</li>\r\n                <li class=\"nomr\"><span class=\"onlneserve\"></span><a href=\"http://v1.live800.com/live800/chatClient/chatbox.jsp?companyID=507669&configID=41819&jid=1392904562\" target=\"_blank\">在线咨询</a></li>\r\n            </ul>\r\n            <p>Copyright Reserved 2007-2017©拍拍贷（www.ppdai.com）&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;沪ICP备05063398号-4&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;上海拍拍贷金融信息服务有限公司</p>\r\n        </div>\r\n    </div>\r\n    <!--底部结束-->\r\n\r\n    <script src=\"http://www.ppdaicdn.com/js/jquery.js\" type=\"text/javascript\" charset=\"utf-8\"></script>\r\n    <script src=\"http://www.ppdaicdn.com/2014/js/init.js\" type=\"text/javascript\" charset=\"utf-8\"></script>\r\n    <script src=\"http://www.ppdaicdn.com/js/min/servicestack-min.js\"></script>\r\n    <script src=\"http://www.ppdaicdn.com/js/jquery.cookie.js\"></script>\r\n    <script src=\"http://www.ppdaicdn.com/2014/js/globalfn.js\" type=\"text/javascript\"></script>\r\n    <script src=\"http://www.ppdaicdn.com/js/newRefer.js?v=0807\"></script>\r\n    \r\n    <script type=\"text/javascript\">\r\n        document.write(\"<script type='text/javascript' src='http://status.ppdai.com/status?v=2014&tmp=\" + Math.random() + \"'>\" + \"<\\/script>\");\r\n    </script>\r\n\r\n    <script>\r\n        $(\".my-f-l-list li a.on\").closest(\".my-f-l-list\").prev(\".my-f-l-nav\").addClass(\"my-f-l-nav-sd\");\r\n        try {\r\n            $(\"#tabIcon a[category='\" + breadcrumbCategory + \"']\").addClass(\"tabon\");\r\n        } catch (e) {\r\n\r\n        }\r\n    </script>\r\n    \r\n    <script type='text/javascript'>\r\n        var _hmt = _hmt || [];\r\n        (function () {\r\n            var hm = document.createElement(\"script\");\r\n            hm.src = \"//hm.baidu.com/hm.js?f87746aec9be6bea7b822885a351b00f\";\r\n            var s = document.getElementsByTagName(\"script\")[0];\r\n            s.parentNode.insertBefore(hm, s);\r\n        })();\r\n    </script>\r\n    <script type='text/javascript'>\r\n        //GrowingIO接口 begin\r\n        var _vds = _vds || [];\r\n        window._vds = _vds;\r\n        (function () {\r\n            _vds.push(['setAccountId', 'b9598a05ad0393b9']);\r\n            var isAuthenticated = \"True\".toLowerCase();\r\n            if (isAuthenticated == \"true\") {\r\n                _vds.push(['setCS1', 'user_name', 'pdu4500755606']);\r\n            }\r\n            (function () {\r\n                var vds = document.createElement('script');\r\n                vds.type = 'text/javascript';\r\n                vds.async = true;\r\n                vds.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'dn-growing.qbox.me/vds.js';\r\n                var s = document.getElementsByTagName('script')[0];\r\n                s.parentNode.insertBefore(vds, s);\r\n            })();\r\n        })();\r\n        //GrowingIO接口 end\r\n    </script>\r\n</body>\r\n</html>\r\n"	string

                        writeFile(resp, name);
                    }
                }
            }// end while

        }

        private void writeFile(String content, String path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex, "写文件错误");
            }

        }


        HashSet<String> totalUrls = new HashSet<string>();        // 用于保存所有检测到的URL序列
        Queue<String> queue = new Queue<string>();       // 未爬取的队列

        /// <summary>
        /// 分析用户html找出关注的借款url， 投资url
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private void extractUserHtml(String url, String html)
        {

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            // 借款列表
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class=\"borrowlist_tit\"]/a");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    String loanUrl = "http://www.ppdai.com" + node.Attributes["href"].Value;
                    if (!totalUrls.Contains(loanUrl))
                    {
                        Console.WriteLine("借款链接" + loanUrl);
                        totalUrls.Add(loanUrl);
                        queue.Enqueue(loanUrl);
                    }
                }
            }

            // 投标列表
            HtmlAgilityPack.HtmlNode node2 = doc.DocumentNode.SelectSingleNode("//div[@id=\"div2\"]");
            if (node2 != null)
            {
                var node2s = node2.SelectNodes("//tr/td[1]/a");
                if (node2s != null)
                {
                    foreach (HtmlNode node in node2s)
                    {
                        String loanUrl = "http://www.ppdai.com" + node.Attributes["href"].Value;
                        if (!totalUrls.Contains(loanUrl))
                        {
                            Console.WriteLine("投标链接" + loanUrl);
                            totalUrls.Add(loanUrl);
                            queue.Enqueue(loanUrl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 分析借款Html，找出关注的用户URL
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private void extractLoanHtml(String url, String html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            // 关注的用户
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class=\"w179\"]/a[1]");
            addUrls(nodes);

            // 其他借款
            HtmlAgilityPack.HtmlNodeCollection nodes1 = doc.DocumentNode.SelectNodes("//table[@class=\"lendDetailTab_tabContent_table1 normal\"]/tr/td/a");
            if (nodes1 != null)
            {
                foreach (HtmlNode node in nodes1)
                {
                    String loanUrl = "http://www.ppdai.com" + node.Attributes["href"].Value;
                    if (!totalUrls.Contains(loanUrl))
                    {
                        Console.WriteLine("其他借款链接" + loanUrl);
                        totalUrls.Add(loanUrl);
                        queue.Enqueue(loanUrl);
                    }
                }
            }
        }


        private void addUrls(HtmlNodeCollection nodes)
        {
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    String userUrl = node.Attributes["href"].Value;
                    if (!totalUrls.Contains(userUrl))
                    {
                        Console.WriteLine(userUrl);
                        totalUrls.Add(userUrl);
                        queue.Enqueue(userUrl);
                    }
                }
            }
        }

        /// <summary>
        /// 分析最新借贷列表，找出借款URL
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private List<String> extractNewListHtml(String html)
        {
            List<String> x = new List<String>();

            // 用正则找出所有的 link

            // 用正则

            return x;
        }

        private void btnLoan_Click(object sender, EventArgs e)
        {
            client.Timeout = 6000;
            String loginUrl = "https://ac.ppdai.com/User/Login";
            String body = "UserName=13305278179&Password=1qaz2wsx";
            String loginHtml = client.Post(loginUrl, body);
            // 赔标,按照利率排序
            String safeBidUrl = "http://invest.ppdai.com/loan/listnew?LoanCategoryId=8&CreditCodes=&ListTypes=2%2C&Rates=&Months=&AuthInfo=&BorrowCount=&didibid=&SortType=0&MinAmount=0&MaxAmount=0";
            //safeBidUrl = "http://invest.ppdai.com/loan/listnew?LoanCategoryId=8&CreditCodes=&ListTypes=2%2C&Rates=&Months=&AuthInfo=&BorrowCount=&didibid=&SortType=3&MinAmount=0&MaxAmount=0";
            safeBidUrl = "http://invest.ppdai.com/loan/listnew?LoanCategoryId=4&CreditCodes=&ListTypes=&Rates=3%2C&Months=&AuthInfo=&BorrowCount=&didibid=&SortType=3&MinAmount=0&MaxAmount=0";
            //safeBidUrl = "http://www.ppdai.com/";
            while (true)
            {
                monitor(safeBidUrl);
                Application.DoEvents();
            }
        }

        private void monitor(String url)
        {
//            client.Referer = "";
            String resp = client.Get(url, Encoding.UTF8);
            if (resp != null)
            {
                var loans = parseLoanList(resp);
                foreach (var loan in loans)
                {
                    Console.WriteLine(loan.Title);
                }
            }
        }

        /// <summary>
        /// 分析借款列表， 选出符合投资标的借款
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private List<Loan> parseLoanList(String html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var olNodes = doc.DocumentNode.SelectNodes("//div[@class=\"outerBorrowList\"]/div/ol");


            // 借款金额、投资额度、认证信息、
            // 非第一次借款、有学历认证、
            var loans = new List<Loan>();
            foreach (var olNode in olNodes)
            {
                var loan = new Loan() { LoanCount = 1 };
                var title = olNode.SelectSingleNode("//li/div[2]/a").InnerText;
                var start = title.IndexOf("第");
                var end = title.IndexOf("次");
                if (start != -1 && end != -1 && end > start)
                {
                    loan.LoanCount = int.Parse(title.Substring(start + 1, end - start - 1));
                }
                loan.Title = title;
                loan.Record = olNode.SelectSingleNode("//li/div[3]/i[@class=\"record\"]") != null;
                loan.Phone = olNode.SelectSingleNode("//li/div[3]/i[@class=\"phone\"]") != null;
                loan.Hukou = olNode.SelectSingleNode("//li/div[3]/i[@class=\"hukou\"]") != null;
                if (loan.LoanCount > 1 && loan.Record)
                    loans.Add(loan);
            }
            return loans;
        }

        /// <summary>
        /// 投资
        /// </summary>
        private void invest()
        {

        }

        private void btnStoreToDb_Click(object sender, EventArgs e)
        {
            saveLoanInfo("D:\\ppdai\\loan\\17406033.html");
        }


        private void saveLoanInfo(String file)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(file);

            // 借款金额
            var moneyNodes = doc.DocumentNode.SelectSingleNode("//div[@class=\"newLendDetailMoneyLeft\"]");
            var lendmoney = moneyNodes.SelectSingleNode("//dl[1]/dd/text()");
            var lendrate = moneyNodes.SelectSingleNode("//dl[2]/dd/text()");
            var lendspan = moneyNodes.SelectSingleNode("//dl[3]/dd/text()");
            Console.WriteLine("借款金额 {0}, {1}, {2}", lendmoney.InnerText, lendrate.InnerText, lendspan.InnerText);

            //*[@id="leftTime"]class="newLendDetailRefundLeft"
            var refundNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"newLendDetailRefundLeft\"]");
            var endDate = moneyNodes.SelectSingleNode("//div[1]/dd/text()");

            // 详细模块
            var detailnode = doc.DocumentNode.SelectSingleNode("//div[@class=\"lendDetailTab_tabContent w1000center\"]");

            //借款人信息
            var detailnodes1 = detailnode.SelectNodes("//div[1]/div/p/span");
            if (detailnodes1 != null)
            {
                var xb = detailnodes1[0].InnerText;
                var nn = detailnodes1[1].InnerText;
                var sj = detailnodes1[2].InnerText;
                var whcd = detailnodes1[3].InnerText;
                var byyx = detailnodes1[4].InnerText;
                var xxxs = detailnodes1[5].InnerText;
                Console.WriteLine("{0},{1},{2},{3},{4},{5}", xb, nn, sj, whcd, byyx, xxxs);
            }

            // 认证信息
            var detailnodes2 = detailnode.SelectNodes("//ul[@class=\"record-info\"]/li/text()");
            if (detailnodes2 != null)
            {
                foreach (var node in detailnodes2)
                {
                    Console.WriteLine(node.InnerText);
                }
            }
            // 统计信息
            // 借款记录 1
            var detailnodes3 = detailnode.SelectNodes("//div[3]/div/div/p/span");
            if (detailnodes3 != null)
            {
                var num = detailnodes3[1].InnerText;
                var sj = detailnodes3[2].InnerText;
                Console.WriteLine("{0}, {1}", num, sj);
            }

            // 借款记录 2 历史记录
            var detailnodes32 = detailnode.SelectNodes("//div[3]/div/p/span");
            if (detailnodes32 != null)
            {
                Console.WriteLine("{0}, 借款历史几率{1}, {2}, 成功还款次数{3}, {4}", detailnodes32[0].InnerText, detailnodes32[1].InnerText, detailnodes32[2].InnerText, detailnodes32[3].InnerText, detailnodes32[4].InnerText);
            }

            // 还款相关
            var huankuannode41 = detailnode.SelectNodes("//div[3]/div/p/span");
            Console.WriteLine("{0}", huankuannode41[0].InnerText);

            var huankuannode4 = detailnode.SelectNodes("//div[4]/div/div/div[@class=\"flex\"]/p/span");
            if (huankuannode4 != null)
            {
                Console.WriteLine("{0}, {1}, {2}", huankuannode4[0].InnerText, huankuannode4[1].InnerText, huankuannode4[2].InnerText);
            }

        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            String ppdai_detail_file = "C:\\Users\\joe\\Documents\\拍拍贷资金流水\\ppdai_detail.csv";
            String month_file = "C:\\Users\\joe\\Documents\\拍拍贷资金流水\\ppdai_record_2017-02.csv";
            // 读取文件，缓存已有的记录
            // read_ppdai_detail(ppdai_detail_file);

            // 读取文件  C:\Users\joe\Document\ppdai_record_2017-03-01.csv
            read_month_record(month_file);

            // 创建文件 C:\Users\joe\Document\ppdai_detail.csv"
            write_ppdai_detail(ppdai_detail_file);
        }
        private void read_month_record(String filename)
        {
            using (CsvReader reader = new CsvReader(new StreamReader(filename)))
            {
                reader.Configuration.HasHeaderRecord = true;
                reader.Configuration.Delimiter = "\t";
                while(reader.Read())
                {
                    if(reader.GetField("类型") == "投标成功")
                    {
                        String tzbh = getInvestID(reader.GetField(5));
                        if (!investCollection.ContainsKey(tzbh))
                        {
                            //var i = new InvestInfo()
                            //{
                            //    TZBH = tzbh
                            //};
                            var i = parse(tzbh);
                            investCollection.Add(tzbh, i);
                            Console.WriteLine("增加投标" + tzbh);
                        }
                        InvestInfo ii = investCollection[tzbh];
                        // ii.TZBH = tzbh;
                        ii.TZSJ = reader.GetField("日期");
                        ii.TZJE += int.Parse(reader.GetField(2));

                    }
                }
            }

            using (CsvReader reader = new CsvReader(new StreamReader(filename)))
            {
                reader.Configuration.HasHeaderRecord = true;
                reader.Configuration.Delimiter = "\t";
                while (reader.Read())
                {
                    if (reader.GetField("类型") == "收到还款")
                    {
                        String tzbh = getInvestID(reader.GetField(5));

                        InvestInfo ii = investCollection[tzbh];
                        var t1 = double.Parse(reader.GetField("存入"));
                        ii.YHJE += t1;


                        Console.WriteLine("处理还款" + tzbh + "   " + ii.YHJE);
                    }

                }

            }
        }

        private InvestInfo parse(string tzbh)
        {
            String url = String.Format("http://invest.ppdai.com/loan/info?id={0}", tzbh);
            String html = client.Get(url, Encoding.UTF8);
            // 开始分析html
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // 借款金额
            var moneyNodes = doc.DocumentNode.SelectSingleNode("//div[@class=\"newLendDetailMoneyLeft\"]");
            var lendmoney = moneyNodes.SelectSingleNode("//dl[1]/dd/text()");
            var lendrate = moneyNodes.SelectSingleNode("//dl[2]/dd/text()");
            var lendspan = moneyNodes.SelectSingleNode("//dl[3]/dd/text()");
            Console.WriteLine("借款金额 {0}, {1}, {2}", lendmoney.InnerText, lendrate.InnerText, lendspan.InnerText);

            var ii = new InvestInfo()
            {
                TZBH = tzbh,
                JKR = "",
                JKJE = Double.Parse(lendmoney.InnerText),
                TZJE = 0,
                JKLL = double.Parse(lendrate.InnerText),
                TZSJ = "未填写",
                JKQX = int.Parse(lendspan.InnerText),
                YSBX = 0,
                YHJE = 0,
                SFYQ = "N",

            };
            return ii;
        }

        private string getInvestID(string v)
        {
            return v.Substring(v.IndexOf("借款ID：") + 5);
        }

        private const String F_TZBH = "投资编号";
        private const String F_JKR = "借款人";
        private const string F_JKJE = "借款金额";
        private const string F_TZJE = "投资金额";
        private const string F_JKLL = "利率";
        private const string F_TZSJ = "投资时间";
        private const string F_JKQX = "借款期限";
        private const string F_YSBX = "应收本息";
        private const string F_YHJE = "已还金额";
        private const string F_SFYQ = "是否逾期";
        private Dictionary<String, InvestInfo> investCollection = new Dictionary<string, InvestInfo>();
        private void read_ppdai_detail(String filename)
        {
            using (CsvReader reader = new CsvReader(new StreamReader(filename)))
            {
                reader.Configuration.HasHeaderRecord = true;
                reader.Configuration.Encoding = Encoding.UTF8;
                while (reader.Read())
                {
                    InvestInfo ii = new InvestInfo()
                    {
                        TZBH = reader.GetField(F_TZBH),
                        JKR = reader.GetField(F_JKR),
                        JKJE = Double.Parse(reader.GetField(F_JKJE)),
                        TZJE = Double.Parse(reader.GetField(F_TZJE)),
                        JKLL = Double.Parse(reader.GetField(F_JKLL)),
                        TZSJ = reader.GetField(F_TZSJ),
                        JKQX = int.Parse(reader.GetField(F_JKQX)),
                        YSBX = Double.Parse(reader.GetField(F_YSBX)),
                        YHJE = Double.Parse(reader.GetField(F_YHJE)),
                        SFYQ = reader.GetField(F_SFYQ),
                    };
                    // ii = reader.GetRecord<InvestInfo>();
                    if (investCollection.ContainsKey(ii.TZBH))
                    {
                        Console.WriteLine("发现重复编号");
                    }
                    else
                    {
                        investCollection.Add(ii.TZBH, ii);
                        Console.WriteLine("读取编号" +  ii.TZBH);
                    }
                }
            }

        }

 
        private void write_ppdai_detail(String filename)
        {
            
            using (TextWriter tw = new StreamWriter(filename, false, Encoding.UTF8))
            using (CsvWriter writer = new CsvWriter(tw))
            {
                writer.WriteField(F_TZBH);
                writer.WriteField(F_JKR);
                writer.WriteField(F_JKJE);
                writer.WriteField(F_TZJE);
                writer.WriteField(F_JKLL);
                writer.WriteField(F_TZSJ);
                writer.WriteField(F_JKQX);
                writer.WriteField(F_YSBX);
                writer.WriteField(F_YHJE);
                writer.WriteField(F_SFYQ);
                writer.NextRecord();

                foreach(var ii in investCollection.Values)
                {
                    writer.WriteField(ii.TZBH);
                    writer.WriteField(ii.JKR);
                    writer.WriteField(ii.JKJE);
                    writer.WriteField(ii.TZJE);
                    writer.WriteField(ii.JKLL);
                    writer.WriteField(ii.TZSJ);
                    writer.WriteField(ii.JKQX);
                    writer.WriteField(ii.YSBX);
                    writer.WriteField(ii.YHJE);
                    writer.WriteField(ii.SFYQ);
                    writer.NextRecord();
                }
            }
        }

    }


    public class InvestInfo {
        public String TZBH { get; set; }
        public  String JKR { get; set; }
        public double JKJE { get; set; }
        public double TZJE { get; set; }
        public double JKLL { get; set; }
        public String TZSJ { get; set; }
        public int JKQX { get; set; }
        public double YSBX { get; set; }
        public double YHJE { get; set; }
        public String SFYQ { get; set; }

    }

    public class Loan
    {
        public String Title { get; internal set; }
        public int LoanCount { get; set; }
        public int Lender { get; set; }

        public bool Record { get; set; }
        public bool Hukou { get; set; }
        public bool Phone { get; set; }
    }
}
