using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace add_lib
{
    public class Converter : IConverter // класс, реализующий интерфейс IConverter
    {
        public double RUBtoEUR(double RUB)
        {
            return RUB / 60;
        }

        public double RUBtoUSD(double RUB)
        {
            return RUB / 75;
        }
    }
}
