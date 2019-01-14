using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

//Trabalho Final Sistemas Operacionais
//Exercício 6.2
//Grupo: Alessandra Faria, Cleuba Alves, Lucas Amancio, Mirella Avelino e Yanna Paula 
//Descrição: programa de sincronização de threads, utilizando a classe Semaphore para tratamento de deadlocks. 
//Entrada: nomes dos componentes do grupo e seus números de matrícula.
//Saída: informações sobre a produção e o consumo.
//Para executar: SO_6_2_2.exe Lucas Mantini 590982, ao pressionar a tecla T, o programa finaliza a produção e o consumo.

namespace Produtor_Consumidor_Sem {
    class Program {

        const int tamBuffer = 2; //Alterar tamanho do Buffer
        static char[] buffer = new char[tamBuffer];
        static string entrada;
        static int producao;
        static int getValor {
            get {
                return producao;
            }
            set {
                producao = value;
            }
        }
        static string getEntrada {
            get {
                return entrada;
            }
            set {
                entrada = value;
            }
        }

        static void Main(string[] args) {
            string aux = args[0] + args[1] + args[2] + args[3] + args[4];
            getValor = aux.Length;
            getEntrada = aux;

            Console.WriteLine("********************************************************");
            Console.WriteLine("Exercício 6.2.2 - Produtor-Consumidor Utilizando Semáforos");
            Console.WriteLine("Grupo: Alessandra Faria Abreu 573831\nCleuba Alves Ribeiro " +
                "612542\nLucas Amancio Mantini 590982\nMirella Avelino Soares 590983\nYanna Paula Araújo Silva 601282");
            Console.WriteLine("********************************************************");

            Thread p = new Thread(new ThreadStart(produz));
            Thread c = new Thread(new ThreadStart(consome));
            Thread s = new Thread(new ThreadStart(para));

            s.Start();
            p.Start();
            c.Start();

        }

        private static Semaphore isFull = new Semaphore(tamBuffer, tamBuffer);
        private static Semaphore isEmpty = new Semaphore(0, tamBuffer);

        static void produz() {
            for (int i = 0; i < producao; i++) {
                isFull.WaitOne();
                Console.WriteLine("Produzindo...");
                Thread.Sleep(2000);
                buffer[i % tamBuffer] = entrada[i];
                Console.WriteLine("Produziu: {0}", buffer[i % tamBuffer]);
                isEmpty.Release(1);
            }
        }

        static void consome() {
            for (int i = 0; i < producao; i++) {
                isEmpty.WaitOne();
                Console.WriteLine("Consumindo...");
                Thread.Sleep(2000);
                char c = buffer[i % tamBuffer];
                Console.WriteLine("Consumiu: {0}", c);
                isFull.Release(1);
            }
        }

        static void para() {
            while (true) {
                ConsoleKeyInfo verifica = Console.ReadKey();
                if (verifica.KeyChar == 't' || verifica.KeyChar == 'T') {
                    isFull.WaitOne();
                    isEmpty.WaitOne();
                    Console.WriteLine("\n T foi pressionado.");
                } else {
                    isFull.WaitOne();
                    Thread.Sleep(10);
                    isFull.Release();
                }
            }
        }
    }
}
