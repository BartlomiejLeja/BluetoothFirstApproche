using System;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new MyCallBack();
            obj.callService();
            Console.ReadKey();
        }
    }
}
