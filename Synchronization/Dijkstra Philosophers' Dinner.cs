using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jantar_Filosofos {
    //Trabalho Final Sistemas Operacionais
    //Exercício 6.3
    //Data: 01/06/2018
    //Grupo: Alessandra Faria, Cleuba Alves, Lucas Amancio, Mirella Avelino e Yanna Paula 
    //Descrição: Problema do jantar dos filósofos utilizando semáforo

    public class Jantar {
        // Cria padrões de comportamento dos filósofos
        public
        const int pensando = 0;
        public
        const int faminto = 1;
        public
        const int comendo = 2;

        // O semáforo mutex que recebe o valor incial 1 para o contador
        // e é o semáforo principal da nossa aplicação
        public static Semaphore mutex = new Semaphore(1, 1);

        // O vetor semáforos são normais e existe um semáforo para cada filósofo
        // que será criado, esses semafóros não recebem valores de inicialização
        // portanto iniciando o contador em 0
        public static Semaphore[] semaforos = new Semaphore[] {
            new Semaphore(0, 1),
                new Semaphore(0, 1),
                new Semaphore(0, 1),
                new Semaphore(0, 1),
                new Semaphore(0, 1)
        };

        // Define um vetor para o estado de cada um dos filósofos presentes
        // na aplicação
        public static int[] estado = new int[5];

        // Cria 5 filósofos em um vetor para a aplicação
        public static Filosofo[] filosofo = new Filosofo[5];

        static void Main(string[] args) {

            Console.WriteLine("********************************************************");
            Console.WriteLine("Exercício 6.3 - Jantar dos Filósofos com Semáforo");
            Console.WriteLine("Grupo: Alessandra Faria Abreu 573831\nCleuba Alves Ribeiro612542\nLucas Amancio Mantini 590982\nMirella Avelino Soares 590983\nYanna Paula Araújo Silva 601282");
            Console.WriteLine("********************************************************");



            // Inicializa todos estados para pensando
            for (int i = 0; i < estado.Length; i++) {
                estado[i] = pensando;
            }
            // Inicializa todos filósofos
            filosofo[0] = new Filosofo(0);
            filosofo[1] = new Filosofo(1);
            filosofo[2] = new Filosofo(2);
            filosofo[3] = new Filosofo(3);
            filosofo[4] = new Filosofo(4);


            // Inicializa todas suas threads
            Thread t0 = new Thread(new ThreadStart(filosofo[0].run));
            Thread t1 = new Thread(new ThreadStart(filosofo[1].run));
            Thread t2 = new Thread(new ThreadStart(filosofo[2].run));
            Thread t3 = new Thread(new ThreadStart(filosofo[3].run));
            Thread t4 = new Thread(new ThreadStart(filosofo[4].run));

            // Inicia a execução de todos filósofos
            t0.Start();
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
        }
    }


    //CRIA UM OBJETO REPRESENTATIVO PARA O FILÓSOFO QUE PODERÁ COMER, PENSAR E ESTAR COM FOME.
    public class Filosofo {
        //número do filosofo
        private int cadeira;

        // Método construtor que recebe um código de identificação do filósofo
        public Filosofo(int cadeira) {
            this.cadeira = cadeira;
        }

        // Método de execução da classe, onde o ambiente do filósofo será rodado
        public void run() {
            try {
                // Coloca o filósofo para pensar
                pensar();

                // Então realiza uma vida infinita para o filósofo onde inicialmente
                // ele executa os procedimentos de pergar os garfos da mesa, posteriormente
                // ele descansa um pouco, e por fim, ele largar os garfos que ele pegou
                do {
                    pegarGarfo();
                    Thread.Sleep(1000);
                    mostraEstados();
                    largarGarfos();
                    Thread.Sleep(1000);
                    mostraEstados();
                }
                while (true);
            } catch (ThreadInterruptedException e) {
                // Exibe uma mensagem de controle de erro
                Console.WriteLine("ERROR>" + e.Message);
                // E da um retorno de cancelamento
                return;
            }
        }

        // Método para definir que o filósofo está pensando
        public void pensar() {
            // Seta o estado deste filósofo na classe Grade para PENSANDO
            Jantar.estado[this.cadeira] = Jantar.pensando;

            // Será criado um controle para o filósofo permanecer pensando
            // durante certo período de tempo
            try {
                // Fica parado neste estado por 1000 milisegundos
                Thread.Sleep(1000);
            } catch (ThreadInterruptedException e) {
                // Exibe uma mensagem de controle de erro
                Console.WriteLine("ERROR>" + e.Message);
            }
        }

        // Método para definir que o filósofo está comendo
        public void comer() {
            // Seta o estado deste filósofo na classe Grade para COMENDO
            Jantar.estado[this.cadeira] = Jantar.comendo;

            try {
                Thread.Sleep(1000);
            } catch (ThreadInterruptedException e) {
                Console.WriteLine("ERROR>" + e.Message);
            }
        }

        // Método para o filósofo pegar um garfo na mesa
        public void pegarGarfo() {
            // Decrementa o semáforo mutex principal da classe, isso permite
            // informar que o atual método está operando na mesa dos filósofos
            try {
                Jantar.mutex.WaitOne();
            } catch (ThreadInterruptedException e) {
                Console.WriteLine("ERROR>" + e.Message);
            }

            Jantar.estado[this.cadeira] = Jantar.faminto;

            // Após o filósofo o período de fome, ele vai verificar com seus
            // vizinhos se ele pode pegar os garfos
            testar();

            // Após operar, volta o semáforo mutex para o estado normal
            // indicando que já realizou todos procedimentos na mesa
            Jantar.mutex.Release();

            // Decrementa seu semáforo
            try {
                Jantar.semaforos[this.cadeira].WaitOne();
            } catch (ThreadInterruptedException e) {
                Console.WriteLine("ERROR>" + e.Message);
            }
        }

        // Método para o filósofo soltar um garfo que ele pegou
        public void largarGarfos() {
            // Decrementa o semáforo mutex principal da classe, isso permite
            // informar que o atual método está operando na mesa dos filósofos
            try {
                Jantar.mutex.WaitOne();
            } catch (ThreadInterruptedException e) {
                Console.WriteLine("ERROR>" + e.Message);
            }

            // Coloca o filósofo para pensar determinado tempo
            pensar();

            // Após o filósofo pensar, ele vai informar para os seus vizinhos
            // que podem tentar pegar os garfos que já estão disponíveis
            Jantar.filosofo[VizinhoEsquerda()].testar();
            Jantar.filosofo[VizinhoDireita()].testar();

            // Após operar, volta o semáforo mutex para o estado normal
            // indicando que já realizou todos procedimentos na mesa
            Jantar.mutex.Release();
        }

        // Método para verificar se o filósofo pode pegar um garfo disposto na mesa
        public void testar() {
            // Verifica se este filósofo está com fome, e se o vizinho da esquerda
            // e da direita não estão comendo
            if (Jantar.estado[this.cadeira] == Jantar.faminto &&
                Jantar.estado[VizinhoEsquerda()] != Jantar.comendo &&
                Jantar.estado[VizinhoDireita()] != Jantar.comendo) {
                // Então este filósofo pode comer
                comer();

                // E incrementa o seu semáforo
                Jantar.semaforos[this.cadeira].Release();
            }
        }

        // Método para obter o filósofo vizinho da direita
        public int VizinhoDireita() {
            return (this.cadeira + 1) % 5;
        }

        // Método para obter o filósofo vizinho da esquerda
        public int VizinhoEsquerda() {
            return (this.cadeira + 5 - 1) % 5;
        }

        //Mostrar o estado de todos os filósofos na mesa
        public void mostraEstados() {
            try {
                Jantar.mutex.WaitOne();
            } catch (ThreadInterruptedException e) {
                Console.WriteLine("ERROR>" + e.Message);
            }

            for (int n = 0; n < 5; n++) {
                Console.WriteLine("Filósofo {0} ", (n + 1));
                switch (Jantar.estado[n]) {
                    case Jantar.pensando:
                        {
                            Console.WriteLine("Está PENSANDO ");
                            break;
                        }
                    case Jantar.faminto:
                        {
                            Console.WriteLine("Está FAMINTO ");
                            break;
                        }
                    case Jantar.comendo:
                        {
                            Console.WriteLine("Está COMENDO ");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            Console.WriteLine("\n");

            Jantar.mutex.Release();
        }
    }
}
