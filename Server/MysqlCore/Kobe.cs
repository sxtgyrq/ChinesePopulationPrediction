using MySql.Data.MySqlClient;
using System;

namespace MysqlCore
{
    //public class Kobe
    //{

    //    //qianandyang
    //    public static void AddAddressValue(string address, out decimal moneycountAddV)
    //    {
    //        using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
    //        {
    //            con.Open();
    //            using (MySqlTransaction tran = con.BeginTransaction())
    //            {
    //                int count;
    //                {
    //                    string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{address}';";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        count = Convert.ToInt32(command.ExecuteScalar());
    //                    }

    //                }
    //                if (count == 0)
    //                {
    //                    string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{address}',1);";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        command.ExecuteNonQuery();
    //                    }
    //                }
    //                else
    //                {
    //                    string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + 1 WHERE BitcoinAddress = '{address}';";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        command.ExecuteNonQuery();
    //                    }
    //                }
    //                moneycountAddV = moneycount.Add(sheetName, address, con, tran);
    //                {
    //                    tran.Commit();
    //                }
    //            }
    //        }
    //    }

    //    const string sheetName = "kobe";
    //    const string xunzhangName = "黑曼巴";
    //    public static string Change(string addressFrom, string addressTo, int intNumber)
    //    {
    //        using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
    //        {
    //            con.Open();
    //            using (MySqlTransaction tran = con.BeginTransaction())
    //            {
    //                int count;
    //                {
    //                    string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        count = Convert.ToInt32(command.ExecuteScalar());
    //                    }

    //                }
    //                if (count == 0)
    //                {
    //                    tran.Commit();
    //                    return $"您没有足够的{xunzhangName}勋章来送给{addressTo}";
    //                }
    //                else
    //                {
    //                    {
    //                        string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            count = Convert.ToInt32(command.ExecuteScalar());
    //                        }
    //                    }
    //                    if (count > intNumber)
    //                    {
    //                        string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {intNumber} WHERE BitcoinAddress = '{addressFrom}';";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            command.ExecuteNonQuery();
    //                        }
    //                    }
    //                    else if (count == intNumber)
    //                    {
    //                        string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            command.ExecuteNonQuery();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        tran.Commit();
    //                        return $"您没有足够的{intNumber}枚{xunzhangName}勋章来送给{addressTo}";
    //                    }
    //                    {
    //                        string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressTo}';";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            count = Convert.ToInt32(command.ExecuteScalar());
    //                        }

    //                    }
    //                    if (count == 0)
    //                    {
    //                        string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{addressTo}',{intNumber});";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            command.ExecuteNonQuery();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + {intNumber} WHERE BitcoinAddress = '{addressTo}';";
    //                        using (var command = new MySqlCommand(sQL, con, tran))
    //                        {
    //                            command.ExecuteNonQuery();
    //                        }
    //                    }
    //                    commondindex.AddAddressValue(con, tran, addressFrom);
    //                    tran.Commit();

    //                    return $"您将{intNumber}枚{xunzhangName}勋章给予了{addressTo}.";
    //                }

    //            }
    //        }
    //    }

    //    public static string Sell(int countofxunzhang, string xunzhangType, decimal price, string addressFrom)
    //    {
    //        using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
    //        {
    //            con.Open();
    //            using (MySqlTransaction tran = con.BeginTransaction())
    //            {
    //                decimal sum = moneycount.GetSum(con, tran, addressFrom);
    //                var tradeMoeny = countofxunzhang * price / 20;
    //                tradeMoeny = Math.Round(tradeMoeny, 2);
    //                if (tradeMoeny < 0.01m)
    //                {
    //                    tradeMoeny = 0.01m;
    //                }
    //                if (tradeMoeny > sum)
    //                {
    //                    return $"您这次交易的{countofxunzhang}枚{xunzhangName}勋章总价为{countofxunzhang * price},你身上没有足够的金币来支付交易手续费共{tradeMoeny}";

    //                }
    //                else
    //                {
    //                    if (moneycount.Decrease(con, tran, addressFrom, tradeMoeny))
    //                    {
    //                        {
    //                            int xunzhangCountRecordInDB;
    //                            {
    //                                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
    //                                using (var command = new MySqlCommand(sQL, con, tran))
    //                                {
    //                                    xunzhangCountRecordInDB = Convert.ToInt32(command.ExecuteScalar());
    //                                }

    //                            }
    //                            if (xunzhangCountRecordInDB == 0)
    //                            {
    //                                tran.Rollback();
    //                                return $"您({addressFrom})没有足够的{countofxunzhang}枚{xunzhangName}勋章来上市场交易";
    //                            }
    //                            else
    //                            {
    //                                {
    //                                    string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
    //                                    using (var command = new MySqlCommand(sQL, con, tran))
    //                                    {
    //                                        xunzhangCountRecordInDB = Convert.ToInt32(command.ExecuteScalar());
    //                                    }
    //                                }
    //                                if (xunzhangCountRecordInDB > countofxunzhang)
    //                                {
    //                                    string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {countofxunzhang} WHERE BitcoinAddress = '{addressFrom}';";
    //                                    using (var command = new MySqlCommand(sQL, con, tran))
    //                                    {
    //                                        command.ExecuteNonQuery();
    //                                    }
    //                                }
    //                                else if (xunzhangCountRecordInDB == countofxunzhang)
    //                                {
    //                                    string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
    //                                    using (var command = new MySqlCommand(sQL, con, tran))
    //                                    {
    //                                        command.ExecuteNonQuery();
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    tran.Rollback();
    //                                    return $"您({addressFrom})没有足够的{countofxunzhang}枚{xunzhangName}勋章来上市场交易";
    //                                }
    //                            }
    //                        }
    //                        market.Sell(con, tran, sheetName, addressFrom, price, countofxunzhang);
    //                        commondindex.AddAddressValue(con, tran, addressFrom);
    //                        tran.Commit();
    //                        return $"您将{countofxunzhang}枚{xunzhangName}勋章以单价{price}金币在市场上架,共收取您{tradeMoeny}金币的手续费。";
    //                    }
    //                    else
    //                    {
    //                        return "系统错误";
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public static string Buy(string addrv, int xunzhangCount, decimal price)
    //    {
    //        return CommondClass.Buy(addrv, xunzhangCount, price, sheetName, xunzhangName);
    //    }
    //}
}
