using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomatas
{
    internal class Transaction
    {
        private DateTime date;
        private double moneyOut;

        public Transaction(double sum)
        {
            date = DateTime.Now;
            moneyOut = sum;
        }
        public override String ToString()
        {
            return $"Transakcijos data: {date.ToShortTimeString()}\t\tNuimta pinigu: ${moneyOut}";
        }
    }
}
