using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using ProjectForFileWrite = ProjectForFileWriteCore;
//using MySql = MysqlCore;

namespace MysqlCore
{
    public class md5reward
    {
        internal static decimal Add(string v, string address, MySqlConnection con, MySqlTransaction tran)
        {
            throw new Exception("");
            //decimal addMoneyCount = 0m;
            //{
            //    DateTime lastTime;
            //    DateTime now = DateTime.Now;

            //    {
            //        int count;
            //        string sQL = $"SELECT count(*) FROM rewardrecord WHERE recordType='{v}'";
            //        using (var command = new MySqlCommand(sQL, con, tran))
            //        {
            //            count = Convert.ToInt32(command.ExecuteScalar());
            //        }
            //        if (count < 1)
            //        {
            //            return 0;
            //        }
            //    }
            //    {
            //        string sQL = $"SELECT recordTime FROM rewardrecord WHERE recordType='{v}'";
            //        using (var command = new MySqlCommand(sQL, con, tran))
            //        {
            //            lastTime = Convert.ToDateTime(command.ExecuteScalar());
            //        }
            //    }


            //    if (now > lastTime)
            //    {
            //        int count;
            //        addMoneyCount = Convert.ToDecimal((now - lastTime).TotalMinutes);
            //        if (addMoneyCount >= 0.01m)
            //        {
            //            {
            //                string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    count = Convert.ToInt32(command.ExecuteScalar());
            //                }

            //            }
            //            if (count == 0)
            //            {
            //                string sQL = $"INSERT INTO moneycount (BitcoinAddress,SuccessCount) VALUES ('{address}',{addMoneyCount});";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            else
            //            {
            //                string sQL = $"UPDATE moneycount SET SuccessCount = SuccessCount + {addMoneyCount} WHERE BitcoinAddress = '{address}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            {
            //                string sQL = $"UPDATE rewardrecord SET recordTime=@recordTime WHERE recordType='{v}'";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.Parameters.AddWithValue("@recordTime", now);
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            addMoneyCount = 0;
            //        }
            //    }
            //}
            //return addMoneyCount;
        }

        public static string Check(string md5_30)
        {
            var m = Regex.Match(md5_30, "^[0-9A-Z]{30}$");
            if (m.Success) { }
            else
            {
                throw new Exception("调用方法前没有正则校验！！！");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int itemCount = 0;
                    {
                        var sQL = $"select Count(*) from md5reward where  `md5`='{md5_30}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            itemCount = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }
                    if (itemCount == 0)
                    {
                        return $"备注密文：{md5_30}，没有记录在册，可用！";
                    }
                    else
                    {
                        string plaintext;
                        {
                            var sQL = $"select `plaintext` from md5reward where  `md5`='{md5_30}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                plaintext = Convert.ToString(command.ExecuteScalar()).Trim();
                            }
                        }
                        if (plaintext == "")
                        {
                            return $"备注密文：{md5_30}，已经记录在册！但现在还没有人领取其奖励！。";
                        }
                        else
                        {
                            string address;
                            {
                                var sQL = $"select `address` from md5reward where  `md5`='{md5_30}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    address = Convert.ToString(command.ExecuteScalar()).Trim();
                                }
                            }
                            return $"备注密文：{md5_30}，已经记录在册，明文为{plaintext}，备注填写此密文将直接为{address}添加稀土！";
                        }
                    }
                }
            }
        }

        public static string AddItem(string md5_30, decimal addValue)
        {
            if (addValue >= 0.01m) { }
            else
            {
                throw new Exception("addValue输入错误");
            }
            var m = Regex.Match(md5_30, "^[0-9A-Z]{30}$");
            if (m.Success) { }
            else
            {
                throw new Exception("调用方法前没有正则校验！！！");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int itemCount = 0;
                    {
                        var sQL = $"select Count(*) from md5reward where  `md5`='{md5_30}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            itemCount = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }
                    if (itemCount == 0)
                    {
                        var sQL = $"INSERT INTO md5reward(`value`,`md5`,`isused`,`address`,`sign`,`msg`) VALUES({addValue},'{md5_30}',0,'','','');";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            if (command.ExecuteNonQuery() == 1)
                            {
                                tran.Commit();
                            }
                            else
                            {
                                tran.Rollback();
                                throw new Exception("意想不到");
                            }

                        }
                        ProjectForFileWrite.CsvWrite.Write(DateTime.Now, "", addValue, "", md5_30);
                        return $"{md5_30}新增成功！";
                    }
                    else
                    {
                        bool isused;
                        string address;
                        {
                            var sQL = $"select `value`,`md5`,`isused`,`address`,`sign`,`msg` from md5reward where md5='{md5_30}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        isused = Convert.ToBoolean(reader["isused"]);
                                        address = Convert.ToString(reader["address"]).Trim();
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        throw new Exception("reader.Read()没有数据");
                                    }
                                }
                            }
                        }
                        if (isused)
                        {
                            goldcount.Add(addValue, address, con, tran);
                            tran.Commit();
                            ProjectForFileWrite.CsvWrite.Write(DateTime.Now, address, addValue, "", md5_30);
                            return $"{md5_30}已经存在！给{address}完成充值!";
                        }
                        else
                        {
                            var sQL = $"update md5reward set `value`=`value`+{addValue} where md5='{md5_30}';";

                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                if (command.ExecuteNonQuery() != 1)
                                {
                                    throw new Exception("");
                                }
                            }

                            tran.Commit();
                            ProjectForFileWrite.CsvWrite.Write(DateTime.Now, address, addValue, "", md5_30);
                            return $"{md5_30}已经存在！完成充值，但现在还无人认领!";
                        }
                    }
                }
            }
            //throw new NotImplementedException();
        }

        internal static void Add(decimal v, string address, MySqlConnection con, MySqlTransaction tran)
        {
            throw new Exception("");
            //if (v >= 0.01m)
            //{
            //    decimal addMoneyCount = v;
            //    {
            //        int itemCount;
            //        if (addMoneyCount >= 0.01m)
            //        {
            //            {
            //                string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    itemCount = Convert.ToInt32(command.ExecuteScalar());
            //                }

            //            }
            //            if (itemCount == 0)
            //            {
            //                string sQL = $"INSERT INTO moneycount (BitcoinAddress,SuccessCount) VALUES ('{address}',{addMoneyCount});";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            else
            //            {
            //                string sQL = $"UPDATE moneycount SET SuccessCount = SuccessCount + {addMoneyCount} WHERE BitcoinAddress = '{address}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //    }
            //}

            //{

            //}
            //return;
        }

        //static string sheetName = "moneycount";
        //  static string xunzhangName = "元素周期表";
        public static string Change(string addressFrom, string addressTo, decimal goldValue)
        {
            throw new Exception("");
            //goldValue = Math.Round(goldValue, 2);
            //using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            //{
            //    con.Open();
            //    using (MySqlTransaction tran = con.BeginTransaction())
            //    {
            //        int count;
            //        decimal moneyCount = 0;
            //        {
            //            string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
            //            using (var command = new MySqlCommand(sQL, con, tran))
            //            {
            //                count = Convert.ToInt32(command.ExecuteScalar());
            //            }

            //        }
            //        if (count == 0)
            //        {
            //            tran.Commit();
            //            return $"您没有足够的金币来送给{addressTo}";
            //        }
            //        else
            //        {
            //            {
            //                string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    moneyCount = Convert.ToDecimal(command.ExecuteScalar());
            //                    moneyCount = Math.Round(moneyCount, 2);
            //                }
            //            }
            //            if (moneyCount - goldValue >= 0.01m)
            //            {
            //                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {goldValue} WHERE BitcoinAddress = '{addressFrom}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            else if (Math.Abs(moneyCount - goldValue) < 0.01m)
            //            {
            //                string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            else
            //            {
            //                tran.Commit();
            //                return $"您没有足够的{goldValue}金币送给{addressTo}";
            //            }
            //            {
            //                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressTo}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    count = Convert.ToInt32(command.ExecuteScalar());
            //                }

            //            }
            //            if (count == 0)
            //            {
            //                string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{addressTo}',{goldValue});";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            else
            //            {
            //                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + {goldValue} WHERE BitcoinAddress = '{addressTo}';";
            //                using (var command = new MySqlCommand(sQL, con, tran))
            //                {
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            commondindex.AddAddressValue(con, tran, addressFrom);
            //            tran.Commit();

            //            return $"您将{goldValue}金币给予了{addressTo}.";
            //        }

            //    }
            //}
        }

        public static string GetAndSet(string md5_mid30, string plaintext, string[] signValue)
        {
            //明文是 plaintext 密文是 ciphertext
            string address = signValue[0].Trim();
            PassObj ob = null;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = $"select `md5`,`value`,`isused`,`address`,`sign`,`msg` from md5reward where  `md5`='{md5_mid30}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    ob = new PassObj()
                                    {
                                        md5 = Convert.ToString(reader["md5"]).Trim(),
                                        value = Convert.ToDecimal(reader["value"]),
                                        isused = Convert.ToBoolean(reader["isused"]),
                                        address = Convert.ToString(reader["address"]).Trim(),
                                        sign = Convert.ToString(reader["sign"]).Trim(),
                                        msg = Convert.ToString(reader["msg"]).Trim(),
                                    };
                                }
                                else
                                {
                                    ob = null;
                                }
                            }
                        }
                    }

                    if (ob == null)
                    {
                        tran.Rollback();
                        return $"没有找到明文为{plaintext}(密文为{md5_mid30})的打赏记录！";
                    }
                    if (ob.isused)
                    {
                        tran.Rollback();
                        return $"密语{md5_mid30}对应的明文是{plaintext}。打赏权益已经被{ob.address}领取！";
                    }
                    else
                    {
                        {
                            MysqlCore.goldcount.Add(ob.value, address, con, tran);
                        }
                        {
                            var sQL = $"UPDATE md5reward SET `address`='{signValue[0]}',`msg`='{signValue[1]}',`sign`='{signValue[2]}',`isused`=1,`plaintext`='{plaintext}' WHERE md5='{md5_mid30}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    MysqlCore.commondindex.AddAddressValue(con, tran, address, signValue);
                                    tran.Commit();
                                    return $"{signValue[0]}获得了打赏收益{ob.value.ToString("f2")}kg稀土。";
                                }
                                else
                                {
                                    tran.Rollback();
                                    throw new Exception("");
                                }
                            }
                        }
                    }
                }
            }
        }

        public class PassObj
        {
            public string md5 { get; set; }
            public decimal value { get; set; }
            public bool isused { get; set; }
            public string address { get; set; }
            public string sign { get; set; }
            public string msg { get; set; }
        }

        internal static bool Decrease(MySqlConnection con, MySqlTransaction tran, string addressFrom, decimal goldValue)
        {
            throw new Exception("");
            //int count;
            //{
            //    string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        count = Convert.ToInt32(command.ExecuteScalar());
            //    }

            //}
            //if (count == 0)
            //{
            //    return false;
            //}

            //decimal moneyCount;
            //{
            //    string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        moneyCount = Convert.ToDecimal(command.ExecuteScalar());
            //        moneyCount = Math.Round(moneyCount, 2);
            //    }
            //}
            //if (moneyCount - goldValue >= 0.01m)
            //{
            //    string sQL = $"UPDATE {sheetName} SET SuccessCount = {moneyCount - goldValue} WHERE BitcoinAddress = '{addressFrom}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    return true;
            //}
            //else if (Math.Abs(moneyCount - goldValue) < 0.01m)
            //{
            //    string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        internal static decimal GetSum(MySqlConnection con, MySqlTransaction tran, string address)
        {
            throw new Exception("");
            //int count;
            //{
            //    string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        count = Convert.ToInt32(command.ExecuteScalar());
            //    }

            //}
            //if (count == 0)
            //{
            //    return 0.00m;
            //}
            //else
            //{
            //    string sQL = $"SELECT SuccessCount FROM moneycount WHERE BitcoinAddress='{address}';";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        return Convert.ToDecimal(command.ExecuteScalar());
            //    }
            //}
        }

        public static void SetUsed(string md5_mid30)
        {
            throw new NotImplementedException();
        }
    }
}
