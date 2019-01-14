using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PC_Monitor {

    class Buffer {
        private char grupo;
        private int bufferOcupado = 0;

        public char Grupo {
            get {
                Monitor.Enter(this);

                if (bufferOcupado == 0) {
                    Console.WriteLine(Thread.CurrentThread.Name + "Tentando ler.");
                    Console.WriteLine("Buffer vazio." + Thread.CurrentThread.Name + "\nEsperando...");
                    Monitor.Wait(this);
                }
                --bufferOcupado;

                Console.WriteLine(Thread.CurrentThread.Name + "Consome: " + grupo);
                Monitor.Pulse(this);

                char copiaBuffer = grupo;

                Monitor.Exit(this);

                return copiaBuffer;
            }

            set {
                Monitor.Enter(this);


                if (bufferOcupado == 1) {
                    Console.WriteLine(
                        Thread.CurrentThread.Name + " Tentando escrever...");

                    Console.WriteLine("Buffer cheio. " +
                        Thread.CurrentThread.Name + " \nEsperando...");

                    Monitor.Wait(this);
                }


                grupo = value;


                ++bufferOcupado;

                Console.WriteLine(Thread.CurrentThread.Name + "\nProduz " + grupo);

                Monitor.Pulse(this);

                Monitor.Exit(this);
            }
        }

    }

    class Produtor {
        private Buffer localizacaoComp;
        private string dadosAlunos;

        public Produtor(Buffer buffer, string dados) {
            localizacaoComp = buffer;
            dadosAlunos = dados;
        }
        public void Produzir() {

            for (int i = 0; i < dadosAlunos.Length; i++) {

                Thread.Sleep(500);
                Console.Write("Produzindo...");
                localizacaoComp.Grupo = dadosAlunos[i];
            }

            Console.WriteLine(Thread.CurrentThread.Name + "Produção terminada " + Thread.CurrentThread.Name + ".");


        }

    }

    class Consumidor {
        private Buffer localizacaoComp;
        private int tamanho;

        public Consumidor(Buffer buf, int tamanho) {
            localizacaoComp = buf;
            this.tamanho = tamanho;
        }

        public void Consumir() {
            char caracter = ' ';

            for (int i = 0; i < tamanho; i++) {

                Thread.Sleep(500);
                Console.Write("Consumindo...");
                caracter += localizacaoComp.Grupo;

            }
            Console.WriteLine(Thread.CurrentThread.Name + "Consumo terminado " + Thread.CurrentThread.Name + ".");
        }

    }
    class Ex_6_1 {
        //Trabalho Final Sistemas Operacionais
        //Exercício 6.1
        //Data: 15/05/2018
        //Grupo: Alessandra Faria, Cleuba Alves, Lucas Amancio, Mirella Avelino e Yanna Paula 
        //Descrição: Usando monitores implementar o programa produtor consumidor.
        //Entrada: Nomes e números de matrícula dos componentes do grupo.
        //Para executar: Ex_6_1.exe AlessandraFariaAbreu573831 CleubaAlvesRibeiro LucasAmancioMantini590982 MirellaAvelinoSoares590983 YannaPaulaAraújoSilva601282
        //Saída: Mostrar processo de produção e consumo.


        static void Main(string[] args) {

            Console.WriteLine("********************************************************");
            Console.WriteLine("Exercício 6.1 - Produtor Consumidor com Monitor");
            Console.WriteLine("Grupo: Alessandra Faria Abreu 573831\nCleuba Alves Ribeiro\nLucas Amancio Mantini 590982\nMirella Avelino Soares 590983\nYanna Paula Araújo Silva 601282");
            Console.WriteLine("********************************************************");

            Buffer buf = new Buffer();

            string grupo = args[0] + args[1] + args[2] + args[3] + args[4];

            Produtor produtor = new Produtor(buf, grupo);
            Consumidor consumidor = new Consumidor(buf, grupo.Length);


            //https://social.msdn.microsoft.com/Forums/pt-BR/8d4cb076-491c-4f3d-bd4b-adbca042e4ae/parar-um-loop-pressionando-uma-tecla?forum=vscsharppt

            Thread thread = new Thread(produtor.Produzir);
            Thread thread2 = new Thread(consumidor.Consumir);

            thread.Start();
            thread2.Start();

            ConsoleKeyInfo tecla = Console.ReadKey();

            if (tecla.KeyChar == 't') {

                thread.Abort();
                thread2.Abort();
            }

            Console.ReadKey();

        }
    }
}
