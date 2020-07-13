using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    public class CommondClass
    {

        static void addCount(MySqlConnection con, MySqlTransaction tran, string sheetName, string addressV, long intNumber)
        {
            {
                int count;
                {
                    string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressV}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        count = Convert.ToInt32(command.ExecuteScalar());
                    }

                }
                if (count == 0)
                {
                    string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{addressV}',{intNumber});";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + {intNumber} WHERE BitcoinAddress = '{addressV}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }


        }
        public static string Buy(string addrv, long xunzhangCount, decimal price, string sheetName, string xunzhangName, string[] signs)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    decimal sum = moneycount.GetSum(con, tran, addrv);
                    decimal left = sum;
                    long xunzhangCountCopy = xunzhangCount;
                    Dictionary<string, decimal> rewardMoney;
                    market.BuyFailureReason reason;

                    if (market.Buy(con, tran, sheetName, addrv, price, ref left, ref xunzhangCountCopy, out rewardMoney, out reason))
                    {
                        foreach (var item in rewardMoney)
                        {
                            moneycount.Add(item.Value, item.Key, con, tran);
                        }
                        if (moneycount.Decrease(con, tran, addrv, sum - left))
                        {
                            commondindex.AddAddressValue(con, tran, addrv, signs);
                            addCount(con, tran, sheetName, addrv, xunzhangCount);
                            tran.Commit();
                            return $"您花费{sum - left}金币，获得了{xunzhangCount}枚{xunzhangName}勋章。";
                        }
                        else
                        {
                            tran.Rollback();
                            return $"您金币不足。";
                        }

                    }
                    else
                    {
                        tran.Rollback();
                        if (reason == market.BuyFailureReason.moneyIsNotEnough)
                        {
                            return "您的钱不足以支持此次收购";
                        }
                        else if (reason == market.BuyFailureReason.amountIsNotEnough)
                        {
                            return $"市面上没有这么多的{xunzhangName}勋章供您收购。满足您收购条件的只有{xunzhangCountCopy}枚。";
                        }
                        else
                        {
                            return "系统错误";
                        }
                    }
                }

            }
        }
    }
}
