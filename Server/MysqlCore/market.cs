using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MysqlCore
{
    public class market
    {


        internal static void Sell(MySqlConnection con, MySqlTransaction tran, string sheetName, string addressFrom, decimal price, int countofxunzhang)
        {
            int recordCount;
            {
                string sQL = $"SELECT Count(*) FROM market WHERE BitcoinAddress='{addressFrom.Trim()}' AND ObjSoldType='{sheetName}' AND Price={price}";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    recordCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            if (recordCount == 0)
            {
                string sQL = $"INSERT into market (BitcoinAddress,ObjSoldType,Price,Count) VALUES('{addressFrom}','{sheetName}',{price},{countofxunzhang})";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                string sQL = $"UPDATE market SET Count=Count+{countofxunzhang} WHERE BitcoinAddress='{addressFrom}' AND ObjSoldType='{sheetName}' AND Price={price}";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new Exception("maket 错误");
                    }
                }
            }
        }

        internal static bool BuyOne(MySqlConnection con, MySqlTransaction tran, string sheetName, string addrv, decimal price)
        {
            throw new NotImplementedException();
        }



        class DBmodel
        {
            public string BitcoinAddress { get; set; }
            public decimal Price { get; set; }
            public string ObjSoldType { get; set; }
            public long Count { get; set; }
        }


        public enum BuyFailureReason { moneyIsNotEnough, amountIsNotEnough, Null };
        internal static bool Buy(MySqlConnection con, MySqlTransaction tran, string sheetName, string addrv, decimal priceMax, ref decimal money_left, ref long xunzhangCountNeedToBuy, out Dictionary<string, decimal> rewardMoney, out BuyFailureReason reason)
        {
            rewardMoney = new Dictionary<string, decimal>();
            var removeItem = new List<DBmodel>();
            long countFrom = 0;
            long countTo = 0;
            string updateBitcoinAddress = "";
            decimal updatePrice = 0.00m;
            bool needUpdate = false;
            {

                string sQL = $"SELECT BitcoinAddress,Price,Count FROM market WHERE ObjSoldType = '{sheetName}' AND Price<={priceMax} ORDER BY Price ASC";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (xunzhangCountNeedToBuy > 0)
                            {
                                var priceGet = Convert.ToDecimal(reader["Price"]);
                                var Count = Convert.ToInt64(reader["Count"]);
                                var BitcoinAddress = Convert.ToString(reader["BitcoinAddress"]).Trim();

                                if (Count > xunzhangCountNeedToBuy)
                                {
                                    needUpdate = true;
                                    updateBitcoinAddress = BitcoinAddress;
                                    countFrom = Count;
                                    countTo = Count - xunzhangCountNeedToBuy;

                                    updatePrice = priceGet;

                                    if (rewardMoney.ContainsKey(BitcoinAddress))
                                    {

                                    }
                                    else
                                    {
                                        rewardMoney.Add(BitcoinAddress, 0.00m);
                                    }
                                    rewardMoney[BitcoinAddress] += xunzhangCountNeedToBuy * priceGet;

                                    money_left -= xunzhangCountNeedToBuy * priceGet;
                                    if (money_left < 0.00m)
                                    {
                                        reason = BuyFailureReason.moneyIsNotEnough;
                                        return false;
                                    }
                                    xunzhangCountNeedToBuy = 0;
                                    break;
                                }
                                else
                                {
                                    removeItem.Add(new DBmodel()
                                    {
                                        BitcoinAddress = BitcoinAddress,
                                        ObjSoldType = sheetName,
                                        Price = priceGet,
                                        Count = Count
                                    });


                                    if (rewardMoney.ContainsKey(BitcoinAddress))
                                    {

                                    }
                                    else
                                    {
                                        rewardMoney.Add(BitcoinAddress, 0.00m);
                                    }
                                    rewardMoney[BitcoinAddress] += Count * priceGet;
                                    money_left -= Count * priceGet;
                                    if (money_left < 0.00m)
                                    {
                                        reason = BuyFailureReason.moneyIsNotEnough;
                                        return false;
                                    }
                                    xunzhangCountNeedToBuy -= Count;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (xunzhangCountNeedToBuy > 0)
                        {
                            reason = BuyFailureReason.amountIsNotEnough;

                            return false;
                        }
                    }
                }
            }
            if (needUpdate)
            {
                string sQL = $"UPDATE market set Count={countTo} WHERE BitcoinAddress='{updateBitcoinAddress}' AND Price={updatePrice} AND Count={countFrom}";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    if (command.ExecuteNonQuery() != 1)
                    {
                        tran.Rollback();
                        throw new Exception("sql update 逻辑错误！");
                    }
                }
            }
            for (int i = 0; i < removeItem.Count; i++)
            {
                string sQL = $"DELETE FROM market WHERE BitcoinAddress='{removeItem[i].BitcoinAddress}' AND Price={removeItem[i].Price} AND Count={removeItem[i].Count};";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    if (command.ExecuteNonQuery() != 1)
                    {
                        tran.Rollback();
                        throw new Exception("sql DELETE 逻辑错误！");
                    }
                }
            }
            reason = BuyFailureReason.Null;
            return true;
        }
    }
}
