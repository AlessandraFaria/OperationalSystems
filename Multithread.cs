using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            Thread thread1 = new Thread(ImprimeK19);
            Thread thread2 = new Thread(ImprimeK31);
            thread1.Start();
            thread2.Start();

            Console.ReadKey();
        }
        public static void ImprimeK19()
        {
            int n=5;
            int fatorial;
            fatorial = n;

            for (int i = n - 1; i > 1; i--)
            {
                fatorial *= i;
                System.Console.Write(fatorial+" ");
                if (i % 10 == 0) Thread.Sleep(10000);
            }
        }
        public static void ImprimeK31()
        {
            int n = 5;
            int numeroAnterior = 0;
            int numeroAtual = 1;
            int novoNumero;
            int fibonacci;


            for (int i = 0; i < n; i++)
            {
                fibonacci = numeroAnterior + numeroAtual;
                System.Console.Write(fibonacci+" ");
                numeroAnterior = numeroAtual;
                numeroAtual = fibonacci;
                if (i % 10 == 0) Thread.Sleep(1000);
            }

            //for (int i = 0; i < 100; i++)
            //{
            //    System.Console.WriteLine(" K31 ");
            //    if (i % 10 == 0) Thread.Sleep(100);
            //}
        }
    }
}
