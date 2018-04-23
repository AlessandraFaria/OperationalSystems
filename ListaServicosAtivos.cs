using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            String[] texto1 = new String[100];
            String[] texto2 = new String[100];
            String[] texto3 = new String[100];
            String[] texto4 = new String[100];
            int i = 0;
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
              
                try
                {  
                    texto1[i] = service.ServiceName.ToString();
                    texto2[i] = service.Status.ToString();
                    
                    RegistryKey regKey1 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\" + service.ServiceName);
                    texto3[i] = regKey1.GetValue("ImagePath").ToString();
                    texto4[i] =  regKey1.GetValue("Description").ToString();
                    regKey1.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Nome "+texto1[i]+" - Status "+texto2[i]+" - Localização "+" - Valor "+texto3[i]+" - Descrição "+texto4[i]);

                i++;
            }

        }


    }
    }

