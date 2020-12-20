using System;
using System.Threading;
using System.Threading.Tasks;

namespace semaphoreSlim
{
    class Program
    {
        static void Main(string[] args)
        {
            //new App().RunApp().Wait();
            //new App().RunApp2().Wait();
            new App().RunClient();
        }
    }
}
