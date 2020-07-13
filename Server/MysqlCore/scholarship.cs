using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    public class scholarship
    {

        public delegate bool checkAddressIsOk(string address);
        public delegate long nextNum(long m);

        public enum AddAddressValueResult { hasNotScholarship, HasNotEnoughResource, costIsNotRight, notNeedTheType, Success, hasNotScholarshipOfAddress };

        public static string[] GetAll()
        {
            List<string> result = new List<string>();
            var dt = MysqlCore.MySqlHelper.ExecuteDataTable(Connection.ConnectionStr, "SELECT scholarshipstartdate,scholarshipaddress FROM scholarship ORDER BY scholarshipstartdate DESC");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(Convert.ToDateTime(dt.Rows[i]["scholarshipstartdate"]).ToString("yyyyMMdd"));
                result.Add(Convert.ToString(dt.Rows[i]["scholarshipaddress"]).Trim());
            }
            return result.ToArray();
        }

        public class PassObj
        {
            public string scholarshipaddress { get; set; }
            public DateTime scholarshipstartdate { get; set; }
            public DateTime scholarshipenddate { get; set; }
            public string owner { get; set; }
            public decimal btc { get; set; }
            public Dictionary<string, long> needCount { get; set; }
            public List<object> signMsgs { get; set; }
        }
        public static PassObj Get(string v)
        {
            //if (string.IsNullOrEmpty(v))
            {
                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string scholarshipaddress;
                        string columns;
                        string owner;
                        DateTime scholarshipstartdate;
                        DateTime scholarshipenddate;
                        decimal btc;
                        List<object> signMsgs = new List<object>();
                        {
                            string sQL;
                            if (string.IsNullOrEmpty(v))
                            {
                                sQL = $"SELECT scholarshipaddress,scholarshipstartdate,scholarshipenddate,`columns` ,`owner`,`btc` FROM scholarship WHERE scholarshipstartdate < '{date}' AND  scholarshipenddate>'{date}';";
                            }
                            else
                            {
                                sQL = $"SELECT scholarshipaddress,scholarshipstartdate,scholarshipenddate,`columns` ,`owner`,`btc` FROM scholarship WHERE scholarshipaddress ='{v}';";
                            }

                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        scholarshipaddress = Convert.ToString(reader["scholarshipaddress"]).Trim();
                                        columns = Convert.ToString(reader["columns"]).Trim();
                                        owner = Convert.ToString(reader["owner"]).Trim();
                                        scholarshipstartdate = Convert.ToDateTime(reader["scholarshipstartdate"]);
                                        scholarshipenddate = Convert.ToDateTime(reader["scholarshipenddate"]);
                                        btc = Convert.ToDecimal(reader["btc"]);
                                        // currentcolumn = Convert.ToString(reader["currentcolumn"]).Trim();

                                    }
                                    else
                                    {
                                        reader.Close();
                                        tran.Rollback();
                                        return null;
                                    }
                                }
                            }
                        }
                        var columnsArray = columns.Split(',');
                        Dictionary<string, long> needCount = new Dictionary<string, long>();
                        for (var i = 0; i < columnsArray.Length; i++)
                        {
                            ifNotExistThenInsert(con, tran, scholarshipaddress, columnsArray[i], ref needCount);
                        }
                        {
                            string sQL = $"SELECT recordaddress,recordmsg,recordsign,recordgiveaddress,recordgivetype,recordgivecount,recordgivedatetime FROM scholarshipdetailsign WHERE scholarshipaddress='{scholarshipaddress}' ORDER BY recordindex ASC;";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        signMsgs.Add(Convert.ToString(reader["recordaddress"]).Trim());
                                        signMsgs.Add(Convert.ToString(reader["recordmsg"]).Trim());
                                        signMsgs.Add(Convert.ToString(reader["recordsign"]).Trim());
                                        signMsgs.Add(Convert.ToString(reader["recordgiveaddress"]).Trim());
                                        signMsgs.Add(Convert.ToString(reader["recordgivetype"]).Trim());
                                        signMsgs.Add(reader["recordgivecount"]);
                                        signMsgs.Add(Convert.ToDateTime(reader["recordgivedatetime"]));
                                    }
                                }
                            }
                        }
                        return new PassObj()
                        {
                            scholarshipaddress = scholarshipaddress,
                            scholarshipstartdate = scholarshipstartdate,
                            scholarshipenddate = scholarshipenddate,
                            btc = btc,
                            needCount = needCount,
                            owner = owner,
                            signMsgs = signMsgs
                        };

                    }
                }
            }
            throw new NotImplementedException();
        }

        public static void AddItem(DateTime startTime, string scholarshipaddress, DateTime endTime, string columns, decimal btc, string[] signValue)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        string sQL = $@"INSERT INTO scholarship(scholarshipstartdate,scholarshipaddress,`owner`,scholarshipenddate,currentcolumn,`columns`,btc)VALUES('{startTime.ToString("yyyy-MM-dd HH:mm:ss")}','{scholarshipaddress}','','{endTime.ToString("yyyy-MM-dd HH:mm:ss")}','','{columns}',{btc});";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            if (command.ExecuteNonQuery() != 1)
                            {
                                tran.Rollback();
                                throw new Exception("command.ExecuteNonQuery() != 1");
                            }
                        }
                    }
                    var columnsArray = columns.Split(',');
                    Dictionary<string, long> needCount = new Dictionary<string, long>();
                    for (var i = 0; i < columnsArray.Length; i++)
                    {
                        ifNotExistThenInsert(con, tran, scholarshipaddress, columnsArray[i], ref needCount);
                    }

                    {
                        string sQL = $"INSERT INTO scholarshipdetailsign(scholarshipaddress,recordindex,recordaddress,recordmsg,recordsign) VALUES('{scholarshipaddress}',0,'{signValue[0]}','{signValue[1]}','{signValue[2]}')";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
            }
        }

        public static string GetAddressRecent()
        {
            var sQL = $"SELECT scholarshipaddress FROM scholarship WHERE scholarshipenddate<'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY scholarshipenddate DESC LIMIT 1";
            var result = MysqlCore.MySqlHelper.ExecuteScalar(Connection.ConnectionStr, sQL);
            if (result == DBNull.Value || result == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(result).Trim();
            }
            // throw new NotImplementedException();
        }

        public static AddAddressValueResult AddAddressValue(string indexv, string applyAddress, string scholarshipaddressInput, long count, string applyType, checkAddressIsOk checkAddr, string[] signValue)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string scholarshipaddress;
                    string columns;
                    string owner;
                    {
                        if (commondindex.GetCommondIndex(con, tran, applyAddress).ToString().Trim() == indexv.Trim())
                        { }
                        else
                        {
                            tran.Rollback();
                            throw new Exception("indexv 异常");
                        }
                        string sQL = $"SELECT scholarshipaddress,`columns` ,`owner`,currentcolumn FROM scholarship WHERE scholarshipstartdate < '{date}' AND  scholarshipenddate>'{date}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    scholarshipaddress = Convert.ToString(reader["scholarshipaddress"]).Trim();
                                    columns = Convert.ToString(reader["columns"]).Trim();
                                    owner = Convert.ToString(reader["owner"]).Trim();
                                    // currentcolumn = Convert.ToString(reader["currentcolumn"]).Trim();
                                }
                                else
                                {
                                    tran.Rollback();
                                    return AddAddressValueResult.hasNotScholarship;
                                }
                            }
                        }
                    }
                    if (scholarshipaddressInput == scholarshipaddress)
                    {

                    }
                    else
                    {
                        tran.Rollback();
                        return AddAddressValueResult.hasNotScholarshipOfAddress;
                    }

                    var columnsArray = columns.Split(',');
                    Dictionary<string, long> needCount = new Dictionary<string, long>();
                    for (var i = 0; i < columnsArray.Length; i++)
                    {
                        ifNotExistThenInsert(con, tran, scholarshipaddress, columnsArray[i], ref needCount);
                    }

                    //   string XunOrJiang;


                    if (needCount.ContainsKey(applyType))
                    {
                        if (needCount[applyType] == count)
                        {


                            if (checkAddr(owner))
                            {
                                if (applyType == "moneycount")
                                {
                                    moneycount.Add(count, owner, con, tran);
                                }
                                else
                                {
                                    BaseItem b = new BaseItem(applyType);
                                    b.AddByTran(count, owner, con, tran);
                                }
                            }
                            if (checkAddr(applyAddress))
                            {
                                bool decreseResult;
                                if (applyType == "moneycount")
                                {
                                    decreseResult = moneycount.Decrease(con, tran, applyAddress, count);
                                }
                                else
                                {
                                    BaseItem b = new BaseItem(applyType);
                                    decreseResult = b.Decrease(con, tran, applyAddress, count);
                                }
                                if (decreseResult)
                                {
                                    //{
                                    //    string sQL = $"UPDATE scholarshipdetail SET needcount=needcount+1 WHERE scholarshipaddress='{scholarshipaddress}';";
                                    //    using (var command = new MySqlCommand(sQL, con, tran))
                                    //    {
                                    //        if (command.ExecuteNonQuery() != columnsArray.Length)
                                    //        {
                                    //            tran.Rollback();
                                    //            throw new Exception("command.ExecuteNonQuery() != columnsArray.Length");
                                    //        }
                                    //    }
                                    //}
                                    {
                                        var addCount = count;
                                        //if (addCount == 0)
                                        //{

                                        //}
                                        //else 
                                        if (addCount > 0)
                                        {
                                            string sQL = $"UPDATE scholarshipdetail SET needcount=needcount+{addCount} WHERE scholarshipaddress='{scholarshipaddress}' AND columnvalue='{applyType}'";
                                            using (var command = new MySqlCommand(sQL, con, tran))
                                            {
                                                if (command.ExecuteNonQuery() != 1)
                                                {
                                                    tran.Rollback();
                                                    throw new Exception("command.ExecuteNonQuery() != 1");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            tran.Rollback();
                                            throw new Exception("addCount<0");
                                        }
                                    }
                                    {
                                        string sQL = $"UPDATE scholarship SET `owner`='{applyAddress}' WHERE scholarshipaddress='{scholarshipaddress}'";
                                        using (var command = new MySqlCommand(sQL, con, tran))
                                        {
                                            if (command.ExecuteNonQuery() != 1)
                                            {
                                                tran.Rollback();
                                                throw new Exception("command.ExecuteNonQuery() != 1");
                                            }
                                        }
                                    }
                                    {
                                        string sQL = $"UPDATE scholarshipdetailsign SET recordindex= recordindex+1 WHERE scholarshipaddress='{scholarshipaddress}'";
                                        using (var command = new MySqlCommand(sQL, con, tran))
                                        {
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    {
                                        string sQL = $"INSERT INTO scholarshipdetailsign(scholarshipaddress,recordindex,recordaddress,recordmsg,recordsign,recordgiveaddress,recordgivetype,recordgivecount,recordgivedatetime) VALUES('{scholarshipaddress}',0,'{signValue[0]}','{signValue[1]}','{signValue[2]}','{owner}','{applyType}',{count},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                                        using (var command = new MySqlCommand(sQL, con, tran))
                                        {
                                            command.ExecuteNonQuery();
                                        }
                                    }

                                    commondindex.AddAddressValue(con, tran, applyAddress, signValue);
                                    tran.Commit();
                                    return AddAddressValueResult.Success;
                                }
                                else
                                {
                                    tran.Rollback();
                                    return AddAddressValueResult.HasNotEnoughResource;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                throw new Exception($"{applyAddress}不是有效地址");
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return AddAddressValueResult.costIsNotRight;
                        }

                        //1 2 3 4 6 8 11 15 19 24 
                    }
                    else
                    {
                        tran.Rollback();
                        return AddAddressValueResult.notNeedTheType;
                    }

                }
            }
        }

        private static bool CheckUpdateIsOK(long v, int count)
        {
            throw new NotImplementedException();
        }

        private static bool ifNotExistThenInsert(MySqlConnection con, MySqlTransaction tran, string scholarshipaddress, string column, ref Dictionary<string, long> needCount)
        {
            bool hasValue;
            {
                string sQL = $"SELECT columnvalue,needcount from scholarshipdetail WHERE scholarshipaddress='{scholarshipaddress}' AND columnvalue='{column}';";

                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        hasValue = reader.Read();
                        if (hasValue)
                            needCount.Add(Convert.ToString(reader["columnvalue"]).Trim(), Convert.ToInt64(reader["needcount"]));
                    }
                }
            }
            if (hasValue)
            {
                return true;
            }
            else
            {
                string sQL = $"INSERT  INTO scholarshipdetail (scholarshipaddress,columnvalue,needcount) VALUES ('{scholarshipaddress}','{column}',1);";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new Exception("错误");
                    }
                }
                return ifNotExistThenInsert(con, tran, scholarshipaddress, column, ref needCount);
            }
        }
    }
}
