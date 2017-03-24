using Common;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PpdInvest
{
    public partial class Form1 : Form
    {
        #region 字段定义
        private const String F_TZBH = "投资编号";
        private const String F_JKR = "借款人";
        private const string F_JKJE = "借款金额";
        private const string F_TZJE = "投资金额";
        private const string F_JKLL = "利率";
        private const string F_TZSJ = "投资时间";
        private const string F_JKQX = "借款期限";
        private const string F_YSBX = "应收本息";
        private const string F_YHJE = "已还金额";
        private const String F_HKRQ = "还款日期";
        private const string F_SFYQ = "是否逾期";
        private const string F_SFHQ = "是否还清";
        #endregion

        private DateTime updatedDate = new DateTime(0);
        private Dictionary<String, InvestInfo> investCollection = new Dictionary<string, InvestInfo>();

        public Form1()
        {
            InitializeComponent();
            // read_ppdai_detail("ppdai_detail.csv");

        }

        private void logInfo(String msg)
        {
            this.txtLog.AppendText(msg + "\r\n");
        }

        private void logWarning(String msg)
        {
            this.txtWarning.AppendText(msg + "\r\n");
        }

        #region 初始化，读取历史记录

        private Excel.Workbook book = null;
        private Excel.Application app = null;
        private void read_ppdai_excel(String filename)
        {
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            book = app.Workbooks.Open(filename);

            // 读取已还清
            readYiHuanQingSheet();

            // 读取黑名单
            readBlackListSheet();

            // 读取逾期收回
            readLateBack();

            // 读取投标记录
            readInvestRecord();

            // 读取资金记录最新时间
            updatedDate = readFinanceRecord();
            logInfo(String.Format("上次更新时间为： {0}", updatedDate));
            var t = DateTime.Now.Subtract(updatedDate).Days + 1;
            numDays.Value = Math.Min(numDays.Maximum, t);
            
        }

        private List<String> investRecords = new List<string>();
        private void readInvestRecord()
        {
            var sheet = getSheet("投标记录");
            for (int i = 4; i < sheet.Rows.Count; i++)
            {
                String tzbh = ((Excel.Range)sheet.Cells[i, 2]).Text;
                if (tzbh == null || tzbh.Length == 0) break;
                investRecords.Add(tzbh);
            }
            logInfo(String.Format("读取了{0}投标记录", investRecords.Count));
        }

        private List<String> lateBacks = new List<string>();
        private void readLateBack()
        {
            var sheet = getSheet("逾期收回");
            for(int i = 4; i < sheet.Rows.Count; i++)
            {
                String rq = ((Excel.Range)sheet.Cells[i, 1]).Text;
                String tzbh = ((Excel.Range)sheet.Cells[i, 2]).Text;
                if (tzbh == null || tzbh.Length == 0) break;
                lateBacks.Add(tzbh + rq);
            }
            logInfo(String.Format("读取了{0}逾期收回记录", lateBacks.Count));
        }

        private DateTime readFinanceRecord()
        {
            var sheet = getSheet("资金记录");
            var date = ((Excel.Range)sheet.Cells[4, 1]).Value;
            if (date == null) date = new DateTime(0);
            return date;
        }

        private void close_excel()
        {
            book.Save();
            book.Close();
            app.Quit();
        }

        private void read_ppdai_detail(String filename)
        {
            if (!File.Exists(filename))
            {
                logInfo(String.Format("上次更新时间为： {0}", updatedDate));
                return;
            }

            // 判断上次更新时间
            updatedDate = getLastUpdatedDateTime(filename);
            logInfo(String.Format("上次更新时间为： {0}", updatedDate));

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
                        TZJE = (int)Double.Parse(reader.GetField(F_TZJE)),
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
                        logWarning("读取本地记录，发现重复编号：" + ii.TZBH);
                    }
                    else
                    {
                        investCollection.Add(ii.TZBH, ii);
                        //Console.WriteLine("读取编号" + ii.TZBH);
                    }
                }
            }

        }

        private DateTime getLastUpdatedDateTime(String file)
        {
            FileInfo fi = new FileInfo(file);
            return fi.LastWriteTime;
        }
        #endregion


        #region 修改文件
        private void insertRecord(Excel.Worksheet sheet, int rowIndex)
        {
            ((Excel.Range)sheet.Rows[2]).Copy();
            var range = (Excel.Range)sheet.Rows[rowIndex, Missing.Value];
            range.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
        }

        #endregion

        #region 写数据到文件

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
                writer.WriteField(F_SFHQ);
                writer.NextRecord();

                foreach (var ii in investCollection.Values)
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
                    writer.WriteField(ii.SFHQ);
                    writer.NextRecord();
                }
            }
        }
        # endregion

        #region 从网站收集数据,

        private HttpClient client = new HttpClient();
        private String username = ""; //"13305278179";
        private String password = ""; //"1qaz2wsx";

        private void login()
        {
            username = txtUsername.Text.Trim();
            password = txtPassword.Text.Trim();
            String loginUrl = "https://ac.ppdai.com/User/Login";
            String body = String.Format("UserName={0}&Password={1}", username, password);
            String loginHtml = client.Post(loginUrl, body);
        }

        private void crawl(int days)
        {
            login();

            // 一直循环抓取，一直到抓取不到数据,退出循环
            // 爬取资金记录
            int i = 1;
            while(true)
            {
                logInfo(String.Format("爬取资金记录第{0}页数据", i));
                if (!crawlPage(days, i))
                {
                    logInfo(String.Format("爬取资金记录第{0}页数据, 未发现数据", i));
                    break;
                }
                i += 1;
            }

            // 爬投标
            //i = 1;
            //while(true)
            //{
            //    logInfo(String.Format("爬取投标第{0}页数据", i));
            //    if (!crawlPage3(days, i))
            //    {
            //        logInfo(String.Format("爬取投标第{0}页数据, 未发现数据", i));
            //        break;
            //    }
            //    i += 1;
            //}

            //// 爬还款
            //i = 1;
            //while (true)
            //{
            //    logInfo(String.Format("爬取还款第{0}页数据", i));
            //    if (!crawlPage5(days, i))
            //    {
            //        logInfo(String.Format("爬取还款第{0}页数据,未发现数据", i));
            //        break;
            //    }
            //    i += 1;
            //}

            // 爬取已还清
            i = 1;
            while(true)
            {
                logInfo(String.Format("爬取已还清第{0}页数据", i));
                if(!crawlPage01(i))
                {
                    logInfo(String.Format("爬取已还清第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }

            // 爬取黑名单
            i = 1;
            while(true)
            {
                logInfo(String.Format("爬取黑名单第{0}页数据", i));
                if (!crawlPage02(i))
                {
                    logInfo(String.Format("爬取黑名单第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }

            // 爬取逾期收回
            i = 1;
            while (true)
            {
                logInfo(String.Format("爬取逾期收回第{0}页数据", i));
                if (!crawlLateback(i))
                {
                    logInfo(String.Format("爬取逾期收回第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }
        }

        #region  爬取资金记录

        private bool crawlPage(int day, int page )
        {
            String 投标成功url = String.Format("http://www.ppdai.com/moneyhistory?Type=-1&Time={0}&page={1}", day, page);
            String html = client.Get(投标成功url, Encoding.UTF8);

            return parsePage(html);
        }

        private bool parsePage(string html)
        {
            // 开始分析页面
            // < table cellpadding = "0" cellspacing = "0" class="receivetab c666666">
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//table[@class=\"receivetab c666666\"]/tr/td");
            //只有一个tr行， 表示没有数据
            //if (nodes.Count == 1) return false;
            if (nodes == null || nodes.Count == 0) return false;

            // 对每一个tr 分析， 
            for (int i = 0; i < nodes.Count; i += 6)
            {
                var rq = nodes[i].InnerText.Trim();
                var dt = DateTime.Parse(rq);
                if (dt.CompareTo(updatedDate) <= 0)
                {
                    return false;
                }

                // 开始构建invest 数据
                var type = nodes[i + 1].InnerText.Trim();
                var pay = parseCurrency(nodes[i + 2].InnerText.Trim());
                var income = parseCurrency(nodes[i + 3].InnerText.Trim());
                var ye = parseCurrency(nodes[i + 4].InnerText.Trim());
                var sm = nodes[i + 5].InnerText.Trim();

                if (type == "投标成功")
                {
                    // 投标
                    var tzbh = getInvestID(sm);
                    var ii = parseLoan(tzbh);
                    ii.TZSJ = rq;
                    ii.TZJE = (int)pay;
                    ii.YSBX = compute(ii.TZJE, ii.JKQX, ii.JKLL / 1200);  // TODO 需要计算
                    if (investRecords.Contains(tzbh))
                    {
                        logWarning("发现重复投资标的" + tzbh + ". 请人工确认excel");

                        //var it = investCollection[tzbh];
                        //it.TZJE += ii.TZJE;
                        //it.YSBX += ii.YSBX;
                        //updateDuplicateInvestRecord(tzbh, ii.TZJE);
                    }
                    else
                    {
                        investRecords.Add(tzbh);
                        insertNewInvestRecord(tzbh, ii.JKJE, ii.TZJE, ii.JKLL, ii.JKQX, dt);
                    }
                }

                // 插入到资金记录sheet
                writeCashRecordToExcel(dt, type, pay, income, ye, sm);

                //else if (type == "收到还款")
                //{
                //    // 收款
                //    if (investCollection.ContainsKey(tzbh))
                //    {
                //        var ii = investCollection[tzbh];
                //        ii.YHJE += Double.Parse(income.Substring(6));
                //        ii.HKRQ = rq;
                //    }
                //    else
                //    {
                //        logWarning("");
                //        var ii = new InvestInfo()
                //        {
                //            TZBH = tzbh,
                //            JKR = "无此编号",
                //            YHJE = Double.Parse(income.Substring(6)),
                //            HKRQ = rq,
                //        };
                //        investCollection.Add(tzbh, ii);
                //    }
                //}

            }

            return true;
        }

        private double parseCurrency(String s)
        {
            if (s.Length == 0) return 0;
            return Double.Parse(s.Substring(6));
       
        }

        private void writeCashRecordToExcel(DateTime rq, String lx, Double zc, Double sr, Double ye, String sm)
        {
            var sheet = getSheet("资金记录");
            insertRecord(sheet, 4);

            sheet.Cells[4, 1] = rq;
            sheet.Cells[4, 2] = lx;
            sheet.Cells[4, 3] = zc;
            sheet.Cells[4, 4] = sr;
            sheet.Cells[4, 5] = ye;
            sheet.Cells[4, 6] = sm;
        }

        #endregion

        #region 爬取逾期收回
        private bool crawlLateback(int page)
        {
            String 逾期收回url = String.Format("http://invest.ppdai.com/account/lateback?pageIndex={0}", page);
            String html = client.Get(逾期收回url, Encoding.UTF8);

            return parseLateback(html);
        }

        private bool parseLateback(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//table[@class=\"receivetab c666666\"]/tr/td/a");

            if (nodes == null || nodes.Count == 0) return false;
            for(int i = 0; i < nodes.Count; i +=2)
            {
                String tzbh = nodes[i].InnerText;
                if (yuqish.Contains(tzbh)) return false;
                yuqish.Add(tzbh);
                writerYuQiSHToExcel(tzbh);
                //if (investCollection[tzbh].SFYQ == "N") return false;
                //investCollection[tzbh].SFHQ = "N";
            }

            return true;
            //return false;
        }

        private List<String> yuqish = new List<string>();
        private void readYuQiSHSheet()
        {
            var sheet = getSheet("逾期收回");
            for (int i = 4; i <= sheet.Rows.Count; i++)
            {
                String tzbh = ((Excel.Range)sheet.Cells[i, 1]).Text;
                if (tzbh == null || tzbh.Length == 0) break;
                yuqish.Add(tzbh);
            }
            // bls.Add(tzbh, new BlackList() { TZBH = tzbh });
        }

        private void writerYuQiSHToExcel(String tzbh) {
            var sheet = getSheet("逾期收回");
            insertRecord(sheet, 4);
            sheet.Cells[4, 1] = tzbh;
        }

        #endregion

        #region 爬取黑名单
        // private Dictionary<String, BlackList> bls = new Dictionary<String, BlackList>();
        private List<String> blackLists = new List<string>();
        private bool crawlPage02(int page)
        {
            String 黑名单url = String.Format("http://invest.ppdai.com/account/blacklist?pageIndex={0}", page);
            String html = client.Get(黑名单url, Encoding.UTF8);

            return parsePage02(html);
        }

        private bool parsePage02(String html)
        {
            // receivetab c666666
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//table[@class=\"receivetab c666666\"]/tr/td");

            if (nodes == null || nodes.Count == 0) return false;
            for (int i = 0; i < nodes.Count-1; i += 7)
            {
                //String tzbh = nodes[i].InnerText;
                var n = nodes[i].ChildNodes[2];
                String tzbh = n.Attributes["listingid"].Value;
                if(!blackLists.Contains(tzbh))
                {
                    String days = nodes[i + 3].InnerText;
                    //bls.Add(tzbh, new BlackList() { TZBH = tzbh, YHRQ = days });
                    blackLists.Add(tzbh);
                    writeBlackListToExcel(tzbh, days, "");
                } else
                {
                    return false;
                }

            }

            return true;
        }

        private void readBlackListSheet()
        {
            logInfo("读取黑名单记录");
            var sheet = getSheet("黑名单");
            for (int i = 4; i <= sheet.Rows.Count; i++)
            {
                String tzbh = ((Excel.Range)sheet.Cells[i, 1]).Text;
                if (tzbh == null || tzbh.Length == 0) break;
                blackLists.Add(tzbh);
            }
        }

        private void writeBlackListToExcel(String tzbh, String days, String jine)
        {
            // insert a record
            var sheet = getSheet("黑名单");
            insertRecord(sheet, 4);
            sheet.Cells[4, 1] = tzbh;
            sheet.Cells[4, 2] = jine;
            int day = int.Parse(days.Substring(0, days.IndexOf("天")).Trim());
            var xx = DateTime.Now.AddDays(-day);
            sheet.Cells[4, 5] = xx.ToString("yyyy/MM/dd");
        }

        #endregion

        #region 爬取已还清
        private bool crawlPage01(int page)
        {
            String 已还清url = String.Format("http://invest.ppdai.com/account/paybacklend?Type=1&pageIndex={0}", page);
            String html = client.Get(已还清url, Encoding.UTF8);

            // 分析已还清
            return parsePage01(html);
        }

        private bool parsePage01(String html)
        {
            // <div class="my-paidframe c666666">
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class=\"my-paidframe c666666\"]/div/div/span[@class=\"fr\"]/a");

            if (nodes == null || nodes.Count == 0) return false;
            foreach(var node in nodes)
            {
                String tzbh = node.Attributes["listid"].Value;
                if (yhqs.Contains(tzbh)) return false;
                yhqs.Add(tzbh);
                writeYiHuanQingToExcel(tzbh, 0, 0, 0);
                //if (investCollection[tzbh].SFHQ == "已还清") return false;
                //investCollection[tzbh].SFHQ = "已还清";
            }
            //只有一个tr行， 表示没有数据
            return true;
        }

        private List<String> yhqs = new List<string>();
        private void readYiHuanQingSheet()
        {
            logInfo("读取已还清记录");
            var sheet = getSheet("已还清");
            for(int i = 4; i <= sheet.Rows.Count; i++)
            {
                String tzbh = ((Excel.Range)sheet.Cells[i, 1]).Text;
                if (tzbh == null || tzbh.Length == 0) break;
                yhqs.Add(tzbh);
            }

        }

        private void writeYiHuanQingToExcel(String tzbh, Double ysbx, Double ysbx2, double jcje)
        {
            // insert a record
            var sheet = getSheet("已还清");
            insertRecord(sheet, 4);
            sheet.Cells[4, 1] = tzbh;
            sheet.Cells[4, 2] = ysbx;
            sheet.Cells[4, 3] = ysbx2;
            sheet.Cells[4, 4] = jcje;
        }
        #endregion

        #region 爬取投标成功 还款成功
        /// <summary>
        /// 爬取投标成功的数据
        /// </summary>
        /// <param name="day"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private bool crawlPage3(int day, int page)
        {
            // 投标成功
            String 投标成功url = String.Format("http://www.ppdai.com/moneyhistory?Type=3&Time={0}&page={1}", day, page);
            String html = client.Get(投标成功url, Encoding.UTF8);

            return parsePage(html);
        }

        /// <summary>
        /// 爬取还款成功的数据
        /// </summary>
        /// <param name="day"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private bool crawlPage5(int day, int page)
        {
            String 还款成功url = String.Format("http://www.ppdai.com/moneyhistory?Type=5&Time={0}&page={1}", day, page);
            String html = client.Get(还款成功url, Encoding.UTF8);
            return parsePage(html);
        }


        private double compute(int benjin, int months, double rate)
        {
            //[本金 x 月利率 x(1 + 月利率)贷款月数] / [(1 + 月利率)还款月数 - 1]
            var t = (benjin * rate * Math.Pow(1 + rate, months)/(Math.Pow(1+rate, months) - 1));
            t = Math.Floor(t*100) / 100;
            return t * months;
        }

        private InvestInfo parseLoan(String tzbh)
        {
            logInfo(String.Format("分析编号为{0}的借款数据", tzbh));
            String url = String.Format("http://invest.ppdai.com/loan/info?id={0}", tzbh);
            String html = client.Get(url, Encoding.UTF8);
            var jkr = "注销";
            var lendmoney = 0.0;
            var lendrate = 0.0;
            var lendspan = 0;

            if (html != null)
            {
                // 开始分析html
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                // 借款人
                jkr = doc.DocumentNode.SelectSingleNode("//a[@class=\"username\"]/text()").InnerText;

                // 借款金额
                var moneyNodes = doc.DocumentNode.SelectSingleNode("//div[@class=\"newLendDetailMoneyLeft\"]");
                lendmoney = Double.Parse(moneyNodes.SelectSingleNode("//dl[1]/dd/text()").InnerText);
                lendrate = Double.Parse(moneyNodes.SelectSingleNode("//dl[2]/dd/text()").InnerText);
                lendspan = int.Parse(moneyNodes.SelectSingleNode("//dl[3]/dd/text()").InnerText);
            }
            else
            {
                logWarning("借款人帐号已经注销");
            }

            var ii = new InvestInfo()
            {
                TZBH = tzbh,
                JKR = jkr,
                JKJE = lendmoney,
                TZJE = 0,
                JKLL = lendrate,
                TZSJ = "未填写",
                JKQX = lendspan,
                YSBX = 0,
                YHJE = 0,
                SFYQ = "N",
                SFHQ="N",

            };
            return ii;
        }

        private string getInvestID(string v)
        {
            return v.Substring(v.IndexOf("借款ID：") + 5);
        }

        private void insertNewInvestRecord(String tzbh, Double jkje, Double tzje, Double rate, int span, DateTime rq)
        {
            var sheet = getSheet("投标记录");
            insertRecord(sheet, 4);

            sheet.Cells[4, 2] = tzbh;
            sheet.Cells[4, 7] = jkje;
            // sheet.Cells[4, 7] = tzje;
            sheet.Cells[4, 10] = rate/100;
            sheet.Cells[4, 11] = span;
            sheet.Cells[4, 12] = rq;
        }

        private void updateDuplicateInvestRecord(String tzbh, Double tzje)
        {
            var sheet = getSheet("投标记录");
            var row = findRowViaTZBH(sheet, tzbh);
            row[7].value = (double)((Excel.Range)row[7]).Value + tzje;
        }

        private void updateInvestRecord(String tzbh, Double je)
        {
            var sheet = getSheet("投标记录");
            var row = findRowViaTZBH(sheet, tzbh);

            Double dsje = ((Excel.Range)row[8]).Value;
            // 如果金额 > 待收本金， 表示一次性还清

        }

        private Excel.Range findRowViaTZBH(Excel.Worksheet sheet, String tzbh)
        {
            for(int i = 4; i < sheet.Rows.Count; i++)
            {
                string temp = ((Excel.Range)sheet.Cells[i, 2]).Text;
                if (tzbh == temp)
                {
                    return sheet.Rows[i];
                }
            }

            return null;
        }

        #endregion

        #endregion 收集最近一个月的数据

        #region 按钮

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            read_ppdai_excel(String.Format("d:\\joe\\{0}_ppdai_detail.xlsx", txtUsername.Text.Trim()));
            crawl((int)numDays.Value);
            this.Cursor = Cursors.Default;
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            login();

            int i = 1;
            while (true)
            {
                logInfo(String.Format("爬取投标第{0}页数据", i));
                if (!crawlPage3((int)numDays.Value, i))
                {
                    logInfo(String.Format("爬取投标第{0}页数据, 未发现数据", i));
                    break;
                }
                i += 1;
            }

            i = 1;
            while (true)
            {
                logInfo(String.Format("爬取已还清第{0}页数据", i));
                if (!crawlPage01(i))
                {
                    logInfo(String.Format("爬取已还清第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }

            // 爬取黑名单
            i = 1;
            while (true)
            {
                logInfo(String.Format("爬取黑名单第{0}页数据", i));
                if (!crawlPage02(i))
                {
                    logInfo(String.Format("爬取黑名单第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }

            // 爬取逾期收回
            i = 1;
            while (true)
            {
                logInfo(String.Format("爬取逾期收回第{0}页数据", i));
                if (!crawlLateback(i))
                {
                    logInfo(String.Format("爬取逾期收回第{0}页数据,未发现数据", i));
                    break;
                }
                i += 1;
            }

            book.Save();
            book.Close();
            //app.Quit();
            //var sheet = getSheet("黑名单");
            //insertRecord(sheet, 3);
            //sheet.Cells[3, 1] = "FileName";
            //sheet.Cells[3, 2] = "xxx";
            //book.Save();
            //book.Close();
            //app.Quit();
        }

        private void EarlyAlert()
        {
            // 发现有多个借款的人， 最近有账户有逾期
            // 发现最近有大额借款 最近一次的借款额 是上次的2倍以上
            // 
        }
              
        private Excel.Worksheet getSheet(String name)
        {
            foreach(Excel.Worksheet sheet in book.Worksheets)
            {
                if (sheet.Name == name) return sheet;
            }

            return null;
        }
    }

    public class LateBack
    {
        public String TZBH { get; set; }
        public DateTime HKRQ { get; set; }
    }

    public class BlackList
    {
        public String TZBH { get; set; }
        public String YHRQ { get; set; }
    }

    public class InvestInfo
    {
        public String TZBH { get; set; }
        public String JKR { get; set; }
        public double JKJE { get; set; }
        public int TZJE { get; set; }
        public double JKLL { get; set; }
        public String TZSJ { get; set; }
        public int JKQX { get; set; }
        public double YSBX { get; set; }
        public double YHJE { get; set; }
        public String HKRQ { get; set; }
        public String SFYQ { get; set; }
        public String SFHQ { get; set; }

    }

}
