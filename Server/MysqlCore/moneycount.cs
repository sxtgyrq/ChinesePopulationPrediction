using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MysqlCore
{
    public class moneycount
    {
        /// <summary>
        /// 通过时间和表，获取增加的额度。
        /// </summary>
        /// <param name="v"></param>
        /// <param name="address"></param>
        /// <param name="con"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        internal static decimal Add(string v, string address, MySqlConnection con, MySqlTransaction tran)
        {
            decimal addMoneyCount = 0m;
            {
                DateTime lastTime;
                DateTime now = DateTime.Now;

                {
                    int count;
                    string sQL = $"SELECT count(*) FROM rewardrecord WHERE recordType='{v}'";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        count = Convert.ToInt32(command.ExecuteScalar());
                    }
                    if (count < 1)
                    {
                        return 0;
                    }
                }
                {
                    string sQL = $"SELECT recordTime FROM rewardrecord WHERE recordType='{v}'";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        lastTime = Convert.ToDateTime(command.ExecuteScalar());
                    }
                }


                if (now > lastTime)
                {
                    int count;
                    addMoneyCount = Convert.ToDecimal((now - lastTime).TotalMinutes);
                    if (addMoneyCount >= 0.01m)
                    {
                        {
                            string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                count = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (count == 0)
                        {
                            string sQL = $"INSERT INTO moneycount (BitcoinAddress,SuccessCount) VALUES ('{address}',{addMoneyCount});";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sQL = $"UPDATE moneycount SET SuccessCount = SuccessCount + {addMoneyCount} WHERE BitcoinAddress = '{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        {
                            string sQL = $"UPDATE rewardrecord SET recordTime=@recordTime WHERE recordType='{v}'";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@recordTime", now);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        addMoneyCount = 0;
                    }
                }
            }
            return addMoneyCount;
        }

        public static void Add(decimal v, string address) 
        { 
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    Add(v, address, con, tran);

                }
            }
        }

        /// <summary>
        /// 直接给地址加，不通过时间进行校验
        /// </summary>
        /// <param name="v"></param>
        /// <param name="address"></param>
        /// <param name="con"></param>
        /// <param name="tran"></param>
        internal static void Add(decimal v, string address, MySqlConnection con, MySqlTransaction tran)
        {
            if (v >= 0.01m)
            {
                decimal addMoneyCount = v;
                {
                    int itemCount;
                    if (addMoneyCount >= 0.01m)
                    {
                        {
                            string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                itemCount = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (itemCount == 0)
                        {
                            string sQL = $"INSERT INTO moneycount (BitcoinAddress,SuccessCount) VALUES ('{address}',{addMoneyCount});";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sQL = $"UPDATE moneycount SET SuccessCount = SuccessCount + {addMoneyCount} WHERE BitcoinAddress = '{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            {

            }
            return;
        }

        static string sheetName = "moneycount";
        //  static string xunzhangName = "元素周期表";
        public static string Change(string addressFrom, string addressTo, decimal goldValue, string[] signs)
        {
            goldValue = Math.Round(goldValue, 2);
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    if (moneycount.Decrease(con, tran, addressFrom) || goldcount.Decrease(con, tran, addressFrom))
                    {
                        int count;
                        decimal moneyCount = 0;
                        {
                            string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                count = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (count == 0)
                        {
                            tran.Commit();
                            return $"您没有足够的金币来送给{addressTo}";
                        }
                        else
                        {
                            {
                                string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    moneyCount = Convert.ToDecimal(command.ExecuteScalar());
                                    moneyCount = Math.Round(moneyCount, 2);
                                }
                            }
                            if (moneyCount - goldValue >= 0.01m)
                            {
                                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {goldValue} WHERE BitcoinAddress = '{addressFrom}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else if (Math.Abs(moneyCount - goldValue) < 0.01m)
                            {
                                string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                tran.Commit();
                                return $"您没有足够的{goldValue}金币送给{addressTo}";
                            }
                            {
                                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressTo}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    count = Convert.ToInt32(command.ExecuteScalar());
                                }

                            }
                            if (count == 0)
                            {
                                string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{addressTo}',{goldValue});";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + {goldValue} WHERE BitcoinAddress = '{addressTo}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            commondindex.AddAddressValue(con, tran, addressFrom, signs);
                            tran.Commit();

                            return $"您将{goldValue}金币给予了{addressTo}.";
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return $"执行签名至少需要0.01金币或者0.01kg稀土。";
                    }


                }
            }
        }

        internal static bool Decrease(MySqlConnection con, MySqlTransaction tran, string addressFrom, decimal goldValue)
        {
            goldValue = Math.Round(goldValue, 2);
            int count;
            {
                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

            }
            if (count == 0)
            {
                return false;
            }

            decimal moneyCount;
            {
                string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    moneyCount = Convert.ToDecimal(command.ExecuteScalar());
                    moneyCount = Math.Round(moneyCount, 2);
                }
            }
            if (moneyCount - goldValue >= 0.01m)
            {
                string sQL = $"UPDATE {sheetName} SET SuccessCount = {moneyCount - goldValue} WHERE BitcoinAddress = '{addressFrom}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    command.ExecuteNonQuery();
                }
                return true;
            }
            else if (Math.Abs(moneyCount - goldValue) < 0.01m)
            {
                string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    command.ExecuteNonQuery();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static decimal GetSum(MySqlConnection con, MySqlTransaction tran, string address)
        {
            int count;
            {
                string sQL = $"SELECT COUNT(*) FROM moneycount WHERE BitcoinAddress='{address}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

            }
            if (count == 0)
            {
                return 0.00m;
            }
            else
            {
                string sQL = $"SELECT SuccessCount FROM moneycount WHERE BitcoinAddress='{address}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    return Convert.ToDecimal(command.ExecuteScalar());
                }
            }
        }

        internal static bool Decrease(MySqlConnection con, MySqlTransaction tran, string addressFrom)
        {
            return Decrease(con, tran, addressFrom, 0.01m);
        }
    }
}
