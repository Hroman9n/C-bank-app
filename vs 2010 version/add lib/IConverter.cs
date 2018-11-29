using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace add_lib
{
    interface IConverter  // интерфейс для конвертации валют
    {
        // конвертация из рублей в доллары
        double RUBtoUSD(double RUB);

        // конвертация из рублей в евро
        double RUBtoEUR(double RUB);
    }
}
