using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            String[] Nome = new String[30];
            String[] Exe = new String[30];
            String[] ID = new String[30];
            int i = 0;
            ManagementClass management = new ManagementClass("Win32_Process");
            ManagementObjectCollection mCollection = management.GetInstances();

            foreach (ManagementObject process in mCollection)
            {
                Nome[i] = process["Name"].ToString();
                ID[i] = process["ProcessId"].ToString();

                try
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo((string)process["ExecutablePath"]);
                    Exe[i] = info.FileDescription;
                }
                catch
                {
                    Exe[i] = "Não Disponível";
                }

                Console.WriteLine("ID " + ID[i] + " - Nome " + Nome[i] + " - Execução " + Exe[i]);
                i++;

            }

        }


    }
    }

