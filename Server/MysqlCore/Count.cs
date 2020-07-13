using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    public class Count
    {
        public static Dictionary<string, int> GetSheet(string address, string[] sheets)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    for (int i = 0; i < sheets.Length; i++)
                    {
                        {
                            var sheetName = sheets[i];
                            string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{address}';";
                            using (var command = new MySqlCommand(sQL, con, tran))
                            {
                                var rr = command.ExecuteScalar();
                                if (rr == null || rr == DBNull.Value)
                                {
                                    result.Add(sheets[i], 0);
                                }
                                else
                                {
                                    result.Add(sheets[i], Convert.ToInt32(rr));
                                }

                            }
                        }
                    }


                }
            }
            return result;
        }

        public static decimal GetSheetOfGoldcount(string address)
        {
            string sheetName = "goldcount";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{address}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        var rr = command.ExecuteScalar();
                        if (rr == null || rr == DBNull.Value)
                        {
                            return 0m;
                        }
                        else
                        {
                            return Math.Round(Convert.ToDecimal(rr), 2);
                        }

                    }
                }
            }
        }

        public static decimal GetSheetOfMoneycount(string address)
        {
            string sheetName = "moneycount";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    string sQL = $"SELECT SuccessCount FROM {sheetName} WHERE BitcoinAddress='{address}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        var rr = command.ExecuteScalar();
                        if (rr == null || rr == DBNull.Value)
                        {
                            return 0m;
                        }
                        else
                        {
                            return Math.Round(Convert.ToDecimal(rr), 2);
                        }

                    }
                }
            }

        }
    }
}
