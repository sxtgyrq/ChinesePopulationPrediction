using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    public class DelegateF
    {
        public delegate void AddAddressValue(string address, out decimal moneycountAddV);
        // moneycountAddV
        public delegate void AddAddressValueWithOutTimeLimit(string address, decimal moneycountAddV);
    }
}
