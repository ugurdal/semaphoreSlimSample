using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace semaphoreSlim
{
    public class App
    {
        //initialCount -> items thats passed from gate at a time
        SemaphoreSlim _gate = new SemaphoreSlim(0);
        SemaphoreSlim _gate2 = new SemaphoreSlim(10); //10 item can pass from gate at a time

        public async Task RunApp()
        {
            System.Console.WriteLine("Start");

            //Code awaits until we release the gate
            await _gate.WaitAsync();

            System.Console.WriteLine("Work Work Work");

            System.Console.WriteLine("Stop");
        }

        public async Task RunApp2()
        {


            for (int i = 0; i < 100; i++)
            {
                System.Console.WriteLine("Start - {0}", i);

                await _gate2.WaitAsync(); //Code awaits until we release the gate
                System.Console.WriteLine("Work - {0}", i);

                await Task.Delay(200); //do some work
                _gate2.Release(); // release the gate
                System.Console.WriteLine("Stop - {0}", i);
            }
        }


        ///B2C ApiClient vb uygulamalar için örnek olabilir

        HttpClient _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        //we hava to figureout how many request completed succesfuly before task cancells, semaphore init value??
        SemaphoreSlim _gate3 = new SemaphoreSlim(20);

        public void RunClient()
        {
            Task.WaitAll(CreateCalls().ToArray());
        }

        private IEnumerable<Task> CreateCalls()
        {
            //At some point newtork card cannot handle requests and cancels some tasks
            //we hava to figureout how many request completed succesfuly before task cancells, semaphore init value??
            for (int i = 0; i < 200; i++)
            {
                yield return CallGoogle(i);
            }
        }

        private async Task CallGoogle(int i)
        {
            try
            {
                await _gate3.WaitAsync();
                var response = await _client.GetAsync("https://google.com");
                _gate3.Release();

                System.Console.WriteLine($"{i} -> " + response.StatusCode);

            }
            catch (Exception e)
            {
                System.Console.WriteLine($"{i} -> " + e.Message.PadRight(100).Substring(0, 100));
            }
        }
    }
}