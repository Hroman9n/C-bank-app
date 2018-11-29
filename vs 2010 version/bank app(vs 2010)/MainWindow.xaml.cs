using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using add_lib;

namespace bank_app_vs_2010_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = "Balance.txt";    // путь к файлу со счетами (bin/Debug)
        string transactions = "Transactions.txt";   // путь к файлу с историей баланса(bin/Debug)

        Dictionary<string, double> customers = new Dictionary<string, double>();    // создаём словарь, где ключ - имя счёта, значение - баланс

        public double CustomerAccount;  // для более удобного обращения

        public string name;     // для более удобного обращения

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (nameOfCustomer.Text != "")              // если поле с именем пустое, то отключаем возможность делать какие-либо операции
            {
                try
                {
                    using (StreamReader fr = new StreamReader(path, System.Text.Encoding.Default))      //чтение файла со счетами
                    {
                        string line;

                        while ((line = fr.ReadLine()) != "")
                        {
                            customers.Add(line.Split('|')[0], Convert.ToDouble(line.Split('|')[1]));
                        }
                    }
                }
                catch
                {

                }

                string currentCustomer = nameOfCustomer.Text;       // берём имя текущего счёта
                name = currentCustomer;                             // записываем его в глобальную переменную

                foreach (string c in customers.Keys)                // смотрим есть ли счёт в файле
                {
                    if (c == currentCustomer)                       // если есть, то
                    {
                        CustomerAccount = customers[c];             // берём его баланс
                        // здороваемся с пользователем
                        helloText.Content = "Добрый день";
                        helloText.Content += ',' + " " + name;
                        // включаем все функции
                        anotherCurrency.IsEnabled = true;
                        yesButton.IsEnabled = true;
                        noButton.IsEnabled = true;
                        changeBalance.IsEnabled = true;
                        changingBalance.IsEnabled = true;
                        Puting.IsEnabled = true;
                        Withdrawing.IsEnabled = true;
                        currentBalance.Content = CustomerAccount;
                        newCustomer.IsEnabled = false;
                        newCustomerApply.IsEnabled = false;
                        history.IsEnabled = true;
                        break;
                    }
                    else                                    // иначе
                    {
                        helloText.Content = "Простите, " + name + ", но вас нет в списке наших клиентов." + '\n' + "Введите баланс в строке рядом с именем";  //говорим, что такого счёта не существует
                        //отключаем все функции
                        anotherCurrency.IsEnabled = false;
                        yesButton.IsEnabled = false;
                        noButton.IsEnabled = false;
                        changeBalance.IsEnabled = false;
                        changingBalance.IsEnabled = false;
                        Puting.IsEnabled = false;
                        Withdrawing.IsEnabled = false;
                        history.IsEnabled = false;
                        currentBalance.Content = "";
                        // просим ввести текущий баланс для нового счёта
                        newCustomer.IsEnabled = true;
                        newCustomerApply.IsEnabled = true;
                    }
                }
            }
            else            // если поле с названием счёта пустое, отключаем все функции
            {
                anotherCurrency.IsEnabled = false;
                yesButton.IsEnabled = false;
                noButton.IsEnabled = false;
                changeBalance.IsEnabled = false;
                changingBalance.IsEnabled = false;
                Puting.IsEnabled = false;
                Withdrawing.IsEnabled = false;
                currentBalance.Content = "";
                newCustomer.IsEnabled = false;
                newCustomerApply.IsEnabled = false;
                history.IsEnabled = false;
                helloText.Content = "Пожалуйста, введите имя";
            }
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            Converter converter = new Converter();  //создание экзепляра класса
            inUSD.Content = Convert.ToString(converter.RUBtoUSD(Convert.ToDouble(currentBalance.Content)));     // конвертация текущего баланса в доллары по курсу 60:1
            inEUR.Content = Convert.ToString(converter.RUBtoEUR(Convert.ToDouble(currentBalance.Content)));     // конвертация баланса в евро по курсу 75:1
            USD.IsEnabled = true;
            EUR.IsEnabled = true;
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            USD.IsEnabled = false;
            EUR.IsEnabled = false;
            inEUR.Content = "";
            inUSD.Content = "";
        }

        private void Puting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerAccount += Convert.ToDouble(changingBalance.Text);
                currentBalance.Content = Convert.ToString(CustomerAccount);
                customers[name] = CustomerAccount;

                using (StreamWriter fw = new StreamWriter(transactions, true, System.Text.Encoding.Default))    // запись транзакции в историю
                {
                    fw.WriteLine(name + '|' + "на счёт " + name + " добавлено " + changingBalance.Text);
                }

                using (StreamWriter fw = new StreamWriter(path, false, System.Text.Encoding.Default))  // сохранение баланса
                {
                    foreach (string c in customers.Keys)
                    {
                        fw.Write(c + '|' + customers[c] + "\n");
                    }
                }
            }
            catch
            {
                MessageBox.Show("упс, похоже, вы ввели что-то кроме чисел");
            }
        }

        private void Withdrawing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerAccount -= Convert.ToDouble(changingBalance.Text);
                currentBalance.Content = Convert.ToString(CustomerAccount);
                customers[name] = CustomerAccount;

                using (StreamWriter fw = new StreamWriter(transactions, true, System.Text.Encoding.Default))    // запись транзакции в историю
                {
                    fw.WriteLine(name + '|' + "со счёта " + name + " списано " + changingBalance.Text);
                }

                using (StreamWriter fw = new StreamWriter(path, false, System.Text.Encoding.Default))           // сохранение баланса
                {
                    foreach (string c in customers.Keys)
                    {
                        fw.Write(c + '|' + customers[c] + "\n");
                    }
                }
            }
            catch
            {
                MessageBox.Show("упс, похоже, вы ввели что-то кроме чисел");
            }
        }

        private void newCustomerApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                customers.Add(nameOfCustomer.Text, Convert.ToDouble(newCustomer.Text));

                using (StreamWriter fw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (string c in customers.Keys)
                    {
                        fw.Write(c + '|' + customers[c] + "\n");
                    }
                }

                anotherCurrency.IsEnabled = true;
                yesButton.IsEnabled = true;
                noButton.IsEnabled = true;
                changeBalance.IsEnabled = true;
                changingBalance.IsEnabled = true;
                Puting.IsEnabled = true;
                Withdrawing.IsEnabled = true;
                currentBalance.Content = CustomerAccount;
                newCustomer.IsEnabled = false;
                newCustomerApply.IsEnabled = false;
                history.IsEnabled = true;
                CustomerAccount = Convert.ToDouble(newCustomer.Text);
                currentBalance.Content = CustomerAccount;
                helloText.Content = "Добрый день";
                helloText.Content += ", " + name;
            }
            catch
            {
                MessageBox.Show("упс, похоже, вы ввели недопустимое значение");
            }
        }

        private void history_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Message = "";

                using (StreamReader history = new StreamReader(transactions, System.Text.Encoding.Default))
                {
                    string line;

                    while ((line = history.ReadLine()) != null)
                    {
                        if (name == line.Split('|')[0])
                        {
                            Message += line.Split('|')[1] + '\n';
                        }
                    }
                }

                if (Message != "")
                {
                    MessageBox.Show(Message);
                }
                else
                {
                    MessageBox.Show("Баланс ни разу не менялся");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении истории транзакций");
            }
        }
    }
}
