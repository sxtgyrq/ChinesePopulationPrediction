using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    public class commondindex
    {
        public static long GetCommondIndex(string address)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    long count;
                    {
                        string sQL = $"SELECT COUNT(*) FROM commondindex WHERE BitcoinAddress='{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            count = Convert.ToInt64(command.ExecuteScalar());
                        }

                    }
                    if (count == 0)
                    {
                        tran.Commit();
                        return 1;
                    }
                    else
                    {
                        string sQL = $"SELECT SuccessCount FROM commondindex WHERE BitcoinAddress='{address}';";
                        using (var command = new MySqlCommand(sQL, con, tran))
                        {
                            tran.Commit();
                            return Convert.ToInt64(command.ExecuteScalar());
                        }
                    }

                }
            }
        }

        internal static long GetCommondIndex(MySqlConnection con, MySqlTransaction tran, string address)
        {
            long count;
            {
                string sQL = $"SELECT COUNT(*) FROM commondindex WHERE BitcoinAddress='{address}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    count = Convert.ToInt64(command.ExecuteScalar());
                }

            }
            if (count == 0)
            {
                return 1;
            }
            else
            {
                string sQL = $"SELECT SuccessCount FROM commondindex WHERE BitcoinAddress='{address}';";
                using (var command = new MySqlCommand(sQL, con, tran))
                {
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }

        public static void AddAddressValue(MySqlConnection con, MySqlTransaction tran, string address, string[] signValue)
        {
            {
                int count;
                {
                    string sQL = $"SELECT COUNT(*) FROM commondindex WHERE BitcoinAddress='{address}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        count = Convert.ToInt32(command.ExecuteScalar());
                    }

                }
                if (count == 0)
                {
                    string sQL = $"INSERT INTO commondindex (BitcoinAddress,SuccessCount) VALUES ('{address}',2);";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sQL = $"UPDATE commondindex SET SuccessCount = SuccessCount + 1 WHERE BitcoinAddress = '{address}';";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            { 
                    string sQL = $"INSERT INTO signrecord (`addr`,`msg`,`signv`,`timestamp`) VALUES (@input1,@input2,@input3,@input4);";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@input1", signValue[0]);
                        command.Parameters.AddWithValue("@input2", signValue[1]);
                        command.Parameters.AddWithValue("@input3", signValue[2]);
                        command.Parameters.AddWithValue("@input4", DateTime.Now);
                        command.ExecuteNonQuery(); 
                }
            }
        }
    }
}
