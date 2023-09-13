using System;
using System.Linq;
using Microsoft.Web.Administration;
namespace DotNetVersionScanner
{
    class Program
    {
        static void Main(string[] args)
        {



            using (ServerManager serverManager = new ServerManager())
            {

                foreach (var site in serverManager.Sites)
                {



                    Console.WriteLine($"\nSite: {site.Name}");
                    foreach (var app in site.Applications)
                    {

                        foreach (var pool in serverManager.ApplicationPools.Where(p => p.Name == app.ApplicationPoolName))
                        {
                            Console.WriteLine($"Application pool: {pool.Name}");
                            Console.WriteLine($".NET version: {pool.ManagedRuntimeVersion}\n");



                        }
                    }


                }
            }






        }
    }
}