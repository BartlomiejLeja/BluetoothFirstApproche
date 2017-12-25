using Android.App;
using Android.Widget;
using Android.OS;
using System.ServiceModel;



namespace IotXamarinApp
{
    [Activity(Label = "IotXamarinApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private TextView _sayHelloWorldTextView;
        private Button _testButton;
        Service1Client client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

          //  WSDualHttpBinding myUplexBinding = new WSDualHttpBinding();
            // BasicHttpBinding myBinding = new BasicHttpBinding();
         //   EndpointAddress myEndpoint = new EndpointAddress("http://iotservice1992.azurewebsites.net/Service1.svc");

            //client = new Service1Client(myBinding, myEndpoint);

            //var context = new InstanceContext(this);
            //client = new Service1Client(context,myBinding, myEndpoint);
            //client.RegisterClients();

            ////  var test =client.WelcomeMessage("Bart");
            _sayHelloWorldTextView = FindViewById<TextView>(Resource.Id.sayHelloWorldTextView);
            _testButton = FindViewById<Button>(Resource.Id.testButton);
            _testButton.Click += _testButton_Click;
            // RunOnUiThread(() =>  _sayHelloWorldTextView.Text = test);


        }

        private void _testButton_Click(object sender, System.EventArgs e)
        {
            //var context = new InstanceContext(this);
            //client = new Service1Client(context);
           
            client.NormalFunction();
            //  var testVal=proxy.WelcomeMessage("hehe");
            //  Console.WriteLine(testVal);
            
        }

        public void CallBackFunction(string pirStatus)
        {
            RunOnUiThread(() => _sayHelloWorldTextView.Text = pirStatus);
        }
    }
}

