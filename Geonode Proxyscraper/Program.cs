using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace Geonode_Proxyscraper
{
    internal class Program
    {
        public static string currentTmie = DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss");
        public static int scraped;

        static void Main(string[] args)
        {

             string title = @"
                                                   
                                                ░██╗░░░░░░░██╗██╗░░░██╗██╗░░░░░██╗░░░██╗
                                                ░██║░░██╗░░██║██║░░░██║██║░░░░░██║░░░██║
                                                ░╚██╗████╗██╔╝██║░░░██║██║░░░░░██║░░░██║
                                                ░░████╔═████║░██║░░░██║██║░░░░░██║░░░██║
                                                ░░╚██╔╝░╚██╔╝░╚██████╔╝███████╗╚██████╔╝
                                                ░░░╚═╝░░░╚═╝░░░╚═════╝░╚══════╝░╚═════╝░
             ";

            Console.Title = "Geonode Scraper by wulu#0827";

            Console.WriteLine(title, Console.ForegroundColor = ConsoleColor.Red);

            Console.WriteLine("[!] Enter the URL", Console.ForegroundColor = ConsoleColor.DarkYellow);
            Console.Write("> ", Console.ForegroundColor = ConsoleColor.DarkGreen);
            string url = Console.ReadLine();

            Console.Clear();
            Console.WriteLine(title, Console.ForegroundColor = ConsoleColor.Red);
            work(url);
        }

        public static void writeproxy(string proxy, string proxytype)
        {
            string file = Environment.CurrentDirectory + $@"\\Results\\{currentTmie}\\{proxytype}" + ".txt";
            File.AppendAllText(file, proxy + Environment.NewLine);
        }

        public static void writeproxyall(string proxy )
        {
            string file = Environment.CurrentDirectory + $@"\\Results\\{currentTmie}\\All protocols" + ".txt";
            File.AppendAllText(file, proxy + Environment.NewLine);
        }

        public static void work(string url)
        {
            Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest();
            httpRequest.UserAgent = Http.ChromeUserAgent();

            var get = httpRequest.Get(url);

            try
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                var json = JsonConvert.DeserializeObject<dynamic>(get.ToString(), jsonSerializerSettings);

                if(json.total == 0)
                {
                    Console.WriteLine($"[-] There is nothing to scrape!", Console.ForegroundColor = ConsoleColor.Red);
                    Console.ReadLine(); 
                }
                else
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\\Results\\" + currentTmie);
                    foreach (var obj in json.data)
                    {
                        scraped++;
                        try
                        {

                            string ip = obj.ip;
                            string port = obj.port;
                            string uptime = obj.uptime;
                            string city = obj.city;
                            string speed = obj.speed;

                            string protocolsa = string.Empty;

                            foreach (var protocl in obj.protocols)
                            {
                                protocolsa = protocl;
                            }

                            Console.WriteLine($"[+] {ip}:{port} | Speed: {speed}ms | Uptime: {uptime} | City: {city} | Protocol: {protocolsa}", Console.ForegroundColor = ConsoleColor.Yellow);
                            writeproxy($"{ip}:{port}", protocolsa);
                            writeproxyall($"{ip}:{port}");
                        }
                        catch (Exception)
                        {

                        }

                    }
                    Console.WriteLine($"[!] Finished scraping {scraped} proxies. Saved results in: {Environment.CurrentDirectory}\\Results\\{currentTmie}", Console.ForegroundColor = ConsoleColor.Green);
                    Process.Start($@"{Environment.CurrentDirectory}\\Results\\{ currentTmie}\\");
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
