using System;
using System.Net;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var obj = new MyCallBack();
            //obj.callService();
            //Console.ReadKey();
            var web = new HttpListener();

            web.Prefixes.Add("http://+:8080/");

            Console.WriteLine("Listening..");

            web.Start();
            while (true)
            {
                Console.WriteLine(web.GetContext());

                var context = web.GetContext();

                var response = context.Response;

                const string responseString = "<html><body>Hello world</body></html>";

                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;

                var output = response.OutputStream;

                output.Write(buffer, 0, buffer.Length);

                Console.WriteLine(output);
               
                output.Close();
                Console.WriteLine(context.Request.Url);
            }

            web.Stop();

            Console.ReadKey();
        }
    }
}
