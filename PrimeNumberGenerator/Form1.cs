using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumberGenerator
{
    public partial class Form1 : Form
    {
        private Thread primeThread;
        private bool isGenerating;//лог переменная работы потока


        public Form1()
        {
            InitializeComponent();
        }
        //метод генерации простых чисел

        private void GeneratePrimeNumbers(int start, int end)
        {
            for (int i = start; i <= end && isGenerating; i++) // перебираем числа внутри диапазона (если  лог переменная isGenerating true )
            {
                if (IsPrime(i)) // отправляем в метод проверки
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        listBoxPrimes.Items.Add(i);// простое число добавляем  в лист бокс
                    });
                }
            }

            isGenerating = false;
            this.Invoke((MethodInvoker)delegate
            {
                startButton.Text = "Start";
            });
        }
        //метод проверки числа на простое 
        private bool IsPrime(int number)
        {
            if (number <= 1)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)// метод проверки деления на все числа от 2 до квадр корня из числа
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }


        [Obsolete]
        private void startButton_Click_1(object sender, EventArgs e)
        {
            if (!isGenerating)
            {// если процесс не запущен, метод получает нач и конечное значения и начинает генерацию в потоке
                int startNumber = string.IsNullOrEmpty(txtStart.Text) ? 2 : int.Parse(txtStart.Text);
                int endNumber = string.IsNullOrEmpty(txtEnd.Text) ? int.MaxValue : int.Parse(txtEnd.Text);

                isGenerating = true;
                primeThread = new Thread(() => GeneratePrimeNumbers(startNumber, endNumber));
                primeThread.Start();
                startButton.Text = "Stop";
            }
            else
            {//иначе останавливает процесс
                isGenerating = false;
                startButton.Text = "Start";
                primeThread.Suspend(); 
            }
        }

       
        [Obsolete]
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {// обработчик события на случай закрытия формы. 
            {
                isGenerating = false;
                primeThread.Suspend();
            }
        }
    }
}
