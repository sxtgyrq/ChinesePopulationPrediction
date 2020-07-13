using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    //public class morsecode
    //{
    //    public static void AddAddressValue(string address, out decimal moneycountAddV)
    //    {
    //        using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
    //        {
    //            con.Open();
    //            using (MySqlTransaction tran = con.BeginTransaction())
    //            {
    //                int count;
    //                {
    //                    string sQL = $"SELECT COUNT(*) FROM morsecode WHERE BitcoinAddress='{address}';";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        count = Convert.ToInt32(command.ExecuteScalar());
    //                    }

    //                }
    //                if (count == 0)
    //                {
    //                    string sQL = $"INSERT INTO morsecode (BitcoinAddress,SuccessCount) VALUES ('{address}',1);";
    //                    using (var command = new MySqlCommand(sQL, con, tran))
    //                    {
    //                        command.ExecuteNonQuery();
    //                    }
    //                }
    //                else
    //                {
    //                    string sQL = $"UPDATE morsecode SET SuccessCount = SuccessCount + 1 WHERE BitcoinAddress = '{address}';";
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

    //    const string sheetName = "morsecode";
    //    const string xunzhangName = "摩斯密码";
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
    //        BaseItem b = new BaseItem(sheetName);
    //        return b.Buy(addrv, xunzhangCount, price);
    //        return CommondClass.Buy(addrv, xunzhangCount, price, sheetName, xunzhangName);
    //    }
    //}

    public class BaseItem
    {
        public BaseItem(string sheetname_Input)
        {
            string xunOrJiang;
            var sIndex = sheetname_Input.IndexOf('_');
            if (sIndex == -1)
            {
                xunOrJiang = "奖";
            }
            else
            {
                if (sheetname_Input.Substring(sIndex, 5) != "_copy")
                {
                    throw new Exception("");
                }
                xunOrJiang = "勋";
                sheetname_Input = sheetname_Input.Substring(0, sIndex);
            }


            initial(sheetname_Input, xunOrJiang);
        }

        public BaseItem(string sheetname_Input, string xunOrJiang)
        {
            initial(sheetname_Input, xunOrJiang);
        }
        void initial(string sheetname_Input, string xunOrJiang)
        {
            InitialSucess = true;
            this.sheetName = sheetname_Input;
            switch (this.sheetName)
            {
                case "morsecode":
                    {
                        this.xunzhangName = "摩斯密码";
                    }; break;
                case "kekonghejubian":
                    {
                        this.xunzhangName = "人造小太阳";
                    }; break;
                case "kobe":
                    {
                        this.xunzhangName = "黑曼巴";
                    }; break;
                case "periodictable":
                    {
                        this.xunzhangName = "元素周期表";
                    }; break;
                case "bitcoinquestionpage":
                    {
                        this.xunzhangName = "椭圆加密";
                    }; break;
                case "qianandyang":
                    {
                        this.xunzhangName = "杨钱";
                    }; break;
                case "identitycard":
                    {
                        this.xunzhangName = "身份证";
                    }; break;
                case "selfsoftware":
                    {
                        this.xunzhangName = "本软件";
                    }; break;
                case "yilingersi":
                    {
                        this.xunzhangName = "一零二四";
                    }; break;
                case "mapfighttaiyuan":
                    {
                        this.xunzhangName = "太原好人";
                    }; break;
                default:
                    {
                        InitialSucess = false;
                        return;
                    }
                    break;
            }

            switch (xunOrJiang)
            {
                case "奖":
                    {
                        this.jiangType = xunOrJiang;
                    }; break;
                case "勋":
                    {
                        this.jiangType = xunOrJiang;
                        this.sheetName += "_copy";
                    }; break;
                default:
                    {
                        InitialSucess = false;
                        return;
                    }
            }
        }

        public bool InitialSucess { get; private set; }
        public string jiangType { get; private set; }
        public BaseItem(string xunZhangName, string xunOrJiang, bool byXunzhangName)
        {
            InitialSucess = true;
            this.xunzhangName = xunZhangName;
            switch (xunZhangName)
            {
                case "椭圆加密":
                    {
                        this.sheetName = "bitcoinquestionpage";
                        InitialSucess = true;
                    }; break;
                case "人造小太阳":
                    {
                        this.sheetName = "kekonghejubian";
                        InitialSucess = true;
                    }; break;
                case "摩斯密码":
                    {
                        this.sheetName = "morsecode";
                        InitialSucess = true;
                    }; break;
                case "元素周期表":
                    {
                        this.sheetName = "periodictable";
                        InitialSucess = true;
                    }; break;
                case "杨钱":
                    {
                        this.sheetName = "qianandyang";
                        InitialSucess = true;
                    }; break;
                case "黑曼巴":
                    {
                        this.sheetName = "kobe";
                        InitialSucess = true;
                    }; break;
                case "身份证":
                    {
                        this.sheetName = "identitycard";
                        InitialSucess = true;
                    }; break;
                case "本软件":
                    {
                        this.sheetName = "selfsoftware";
                        InitialSucess = true;
                    }; break;
                case "一零二四":
                    {
                        this.sheetName = "yilingersi";
                        InitialSucess = true;
                    }; break;
                case "太原好人":
                    {
                        this.sheetName = "mapfighttaiyuan";
                        InitialSucess = true;
                    }; break;
                default:
                    {
                        InitialSucess = false;
                        return;
                    };

            }
            switch (xunOrJiang)
            {
                case "奖":
                    {
                        this.jiangType = xunOrJiang;
                    }; break;
                case "勋":
                    {
                        this.jiangType = xunOrJiang;
                        this.sheetName += "_copy";
                    }; break;
                default:
                    {
                        InitialSucess = false;
                        return;
                    }
            }
        }


        /// <summary>
        /// 给指定的表和地址添加奖章和金钱奖励，金钱是其他方法来计算的。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="moneycountAddV"></param>
        public void AddAddressValue(string address, decimal moneycountAddV)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int count;
                    {
                        string sQL = $"SELECT COUNT(*) FROM {this.sheetName} WHERE BitcoinAddress='{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            count = Convert.ToInt32(command.ExecuteScalar());
                        }

                    }
                    if (count == 0)
                    {
                        string sQL = $"INSERT INTO {this.sheetName} (BitcoinAddress,SuccessCount) VALUES ('{address}',1);";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sQL = $"UPDATE {this.sheetName} SET SuccessCount = SuccessCount + 1 WHERE BitcoinAddress = '{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    moneycount.Add(moneycountAddV, address, con, tran);
                    {
                        tran.Commit();
                    }
                }
            }
        }


        /// <summary>
        /// 给指定的表和地址添加奖章和金钱奖励，金钱是通过时间来计算的。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="moneycountAddV"></param>
        public void AddAddressValue(string address, out decimal moneycountAddV)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int count;
                    {
                        string sQL = $"SELECT COUNT(*) FROM {this.sheetName} WHERE BitcoinAddress='{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            count = Convert.ToInt32(command.ExecuteScalar());
                        }

                    }
                    if (count == 0)
                    {
                        string sQL = $"INSERT INTO {this.sheetName} (BitcoinAddress,SuccessCount) VALUES ('{address}',1);";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sQL = $"UPDATE {this.sheetName} SET SuccessCount = SuccessCount + 1 WHERE BitcoinAddress = '{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    moneycountAddV = moneycount.Add(sheetName, address, con, tran);
                    {
                        tran.Commit();
                    }
                }
            }
        }

        //public void AddAddressValue(string address, decimal moneycountAddV) { }

        public string sheetName
        {
            get;
            private set;
        }
        public string xunzhangName
        {
            get;
            private set;
        }
        public string Change(string addressFrom, string addressTo, long intNumber, string[] signValue)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            if (intNumber <= 0)
            {
                return "送给的竖向不能小于等于0";
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    if (moneycount.Decrease(con, tran, addressFrom) || goldcount.Decrease(con, tran, addressFrom))
                    {
                        int itemCount_;
                        {
                            string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                itemCount_ = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (itemCount_ == 0)
                        {
                            tran.Commit();
                            return $"您没有足够的{xunzhangName}{this.jiangType}章来送给{addressTo}";
                        }
                        else
                        {
                            long successCount;
                            {
                                string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    successCount = Convert.ToInt64(command.ExecuteScalar());
                                }
                            }
                            if (successCount > intNumber)
                            {
                                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {intNumber} WHERE BitcoinAddress = '{addressFrom}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else if (successCount == intNumber)
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
                                return $"您没有足够的{intNumber}枚{xunzhangName}{this.jiangType}章来送给{addressTo}";
                            }
                            int targetItemCount;
                            {

                                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressTo}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    targetItemCount = Convert.ToInt32(command.ExecuteScalar());
                                }

                            }
                            if (targetItemCount == 0)
                            {
                                string sQL = $"INSERT INTO {sheetName} (BitcoinAddress,SuccessCount) VALUES ('{addressTo}',{intNumber});";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount + {intNumber} WHERE BitcoinAddress = '{addressTo}';";
                                using (var command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            commondindex.AddAddressValue(con, tran, addressFrom, signValue);
                            tran.Commit();

                            return $"您将{intNumber}枚{xunzhangName}{this.jiangType}章给予了{addressTo}.";
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return "执行签名至少要花费0.01金币或者0.01kg稀土。";
                    }



                }
            }
        }
        public string Sell(int countofxunzhang, string xunzhangType, decimal price, string addressFrom, string[] signs)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    //if (moneycount.Decrease(con, tran, addressFrom) || goldcount.Decrease(con, tran, addressFrom))
                    //本省交易费就能代替签名费。
                    {
                        decimal sum = moneycount.GetSum(con, tran, addressFrom);
                        var tradeMoeny = countofxunzhang * price / 20;
                        tradeMoeny = Math.Round(tradeMoeny, 2);
                        if (tradeMoeny < 0.01m)
                        {
                            tradeMoeny = 0.01m;
                        }
                        if (tradeMoeny > sum)
                        {
                            return $"您这次交易的{countofxunzhang}枚{xunzhangName}勋章总价为{countofxunzhang * price},你身上没有足够的金币来支付交易手续费共{tradeMoeny}";

                        }
                        else
                        {
                            if (moneycount.Decrease(con, tran, addressFrom, tradeMoeny))
                            {
                                {
                                    int itemCountRecordInDB;
                                    {
                                        string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                                        using (var command = new MySqlCommand(sQL, con, tran))
                                        {
                                            itemCountRecordInDB = Convert.ToInt32(command.ExecuteScalar());
                                        }

                                    }
                                    if (itemCountRecordInDB == 0)
                                    {
                                        tran.Rollback();
                                        return $"您({addressFrom})没有足够的{countofxunzhang}枚{xunzhangName}勋章来上市场交易";
                                    }
                                    else
                                    {
                                        long objectCount;
                                        {
                                            string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{addressFrom}';";
                                            using (var command = new MySqlCommand(sQL, con, tran))
                                            {
                                                objectCount = Convert.ToInt64(command.ExecuteScalar());
                                            }
                                        }
                                        if (objectCount > countofxunzhang)
                                        {
                                            string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {countofxunzhang} WHERE BitcoinAddress = '{addressFrom}';";
                                            using (var command = new MySqlCommand(sQL, con, tran))
                                            {
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        else if (objectCount == countofxunzhang)
                                        {
                                            string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{addressFrom}';";
                                            using (var command = new MySqlCommand(sQL, con, tran))
                                            {
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            tran.Rollback();
                                            return $"您({addressFrom})没有足够的{countofxunzhang}枚{xunzhangName}勋章来上市场交易";
                                        }
                                    }
                                }
                                market.Sell(con, tran, sheetName, addressFrom, price, countofxunzhang);
                                commondindex.AddAddressValue(con, tran, addressFrom, signs);
                                tran.Commit();
                                return $"您将{countofxunzhang}枚{xunzhangName}勋章以单价{price}金币在市场上架,共收取您{tradeMoeny}金币的手续费。";
                            }
                            else
                            {
                                tran.Rollback();
                                return "系统错误";
                            }
                        }
                    }
                }
            }
        }

        public string ChangeFromXunzhangToGold(long count, string address)
        {
            if (!InitialSucess)
            {

                throw new Exception("!InitialSucess");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    if (this.Decrease(con, tran, address, count))
                    {
                        //this.AddToCopy(con, tran, address, count);
                        goldcount.Add(count, address, con, tran);
                        tran.Commit();
                        return $"您用{count}个{this.xunzhangName}勋章提炼了{count.ToString("F2")}kg稀土";
                    }
                    else
                    {
                        tran.Rollback();
                        return $"{address}下现在没有{count}个{this.xunzhangName}勋章";
                    }
                }
            }
        }

        public string Buy(string addrv, long xunzhangCount, decimal price, string[] signs)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            return CommondClass.Buy(addrv, xunzhangCount, price, sheetName, xunzhangName, signs);
        }

        internal void AddByTran(long count, string address, MySqlConnection con, MySqlTransaction tran)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            if (count >= 1)
            {

                {
                    int itemCount;
                    //if (addMoneyCount >= 0.01m)
                    {
                        {
                            string sQL = $"SELECT COUNT(*) FROM {this.sheetName} WHERE BitcoinAddress='{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                itemCount = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (itemCount == 0)
                        {
                            string sQL = $"INSERT INTO {this.sheetName} (BitcoinAddress,SuccessCount) VALUES ('{address}',{itemCount});";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sQL = $"UPDATE {this.sheetName} SET SuccessCount = SuccessCount + {itemCount} WHERE BitcoinAddress = '{address}';";
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

        public string ChangeToCopy(long count, string address)
        {
            if (!InitialSucess)
            {

                throw new Exception("!InitialSucess");
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    if (this.Decrease(con, tran, address, count))
                    {
                        this.AddToCopy(con, tran, address, count);
                        if (goldcount.Decrease(con, tran, address, count))
                        {
                            tran.Commit();
                            return $"您用{count}个{this.xunzhangName}奖章和{count.ToString("F2")}kg稀土合成了{count}个{this.xunzhangName}勋章。";
                        }
                        else
                        {
                            return $"您现在没有合成{this.xunzhangName}勋章所需的{count.ToString("F2")}kg稀土。";
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return $"您现在没有合成{this.xunzhangName}勋章所需的{count}个{this.xunzhangName}奖章。";
                    }
                }
            }
        }

        private void AddToCopy(MySqlConnection con, MySqlTransaction tran, string address, long inputCount)
        {
            {
                {
                    int itemCount;
                    {
                        string sQL = $"SELECT COUNT(*) FROM {this.sheetName}_copy WHERE BitcoinAddress='{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            itemCount = Convert.ToInt32(command.ExecuteScalar());
                        }

                    }
                    if (itemCount == 0)
                    {
                        string sQL = $"INSERT INTO {this.sheetName}_copy (BitcoinAddress,SuccessCount) VALUES ('{address}',1);";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sQL = $"UPDATE {this.sheetName} SET SuccessCount = SuccessCount + {inputCount} WHERE BitcoinAddress = '{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        internal bool Decrease(MySqlConnection con, MySqlTransaction tran, string applyAddress, long count1)
        {
            if (!InitialSucess)
            {
                throw new Exception("InitialSucess");
            }
            int itemCount;
            {
                string sQL = $"SELECT COUNT(*) FROM {sheetName} WHERE BitcoinAddress='{applyAddress}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    itemCount = Convert.ToInt32(command.ExecuteScalar());
                }

            }
            if (itemCount == 0)
            {
                return false;
            }
            else
            {
                long countInDB;
                {
                    string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{applyAddress}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        countInDB = Convert.ToInt64(command.ExecuteScalar());
                    }
                }
                if (count1 < countInDB)
                {
                    string sQL = $"UPDATE {sheetName} SET SuccessCount = SuccessCount - {count1} WHERE BitcoinAddress = '{applyAddress}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                else if (count1 == countInDB)
                {
                    string sQL = $"DELETE FROM {sheetName} WHERE BitcoinAddress = '{applyAddress}';";
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
        }
    }
}
