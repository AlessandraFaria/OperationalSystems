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
        static int fatorial;
        static int fibonacci;
        static int n;
        static void Main(string[] args)
        {
            Console.WriteLine("Digite o valor de N ");
            //n = int.Parse(args[0]);
            n = Convert.ToInt32(Console.ReadLine());

            Thread thread1 = new Thread(ImprimeK19);
            thread1.Start(); //inicia a rotina da thread
            thread1.Join(); //espera o fim da execução da thread para seguir o código

            Thread thread2 = new Thread(ImprimeK31);
            thread2.Start();
            thread2.Join();

            int resultado;
            resultado = fibonacci + fatorial;
            Console.WriteLine(" A soma do fibonacci de N e do Fatorial de N é :" + resultado);

            Console.ReadKey();
        }
        public static void ImprimeK19()
        {
            fatorial = n;

            for (int i = n - 1; i > 1; i--)
            {
                fatorial *= i;
                
                if (i % 10 == 0) Thread.Sleep(10000);
                
            }
            System.Console.WriteLine("Fatorial "+fatorial);
           
        }
        public static void ImprimeK31()
        {
            int numeroAnterior = 0;
            int numeroAtual = 1;
            int novoNumero;


            for (int i = 0; i < n; i++)
            {
                fibonacci = numeroAnterior + numeroAtual;               
                numeroAnterior = numeroAtual;
                numeroAtual = fibonacci;
                if (i % 10 == 0) Thread.Sleep(1000);
            }
            System.Console.Write(" enesimo termo fibonacci " + fibonacci);
        }
    }
}
