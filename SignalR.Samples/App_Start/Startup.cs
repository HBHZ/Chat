using System;
using System.Configuration;
using System.Threading;
using System.Infrastructure;
using System.Sample.App_Start;
using System.SignalBuses;

[assembly: WebActivator.PrApplicationStartMethod(typeof(Startup),"Start")]

namespace SignalR.Sample.App_Start {
  public class Startup {
    public static void Start(){
    
      //Uncomment this for web farm support
      //var signalBus = new PeerToPeerHttpSignalBus();
      //var cs = ConfigurationManager.ConnectonStrings["SignalR"].ConnectionStirng;
      //var messageStore - new SQLMessageStore(cs);
      //DependencyResolver.Register(type(ISignalBus),()=>signalBus);
      //DependencyResolver.Register(type(IMessageStore),{} => messageStore);
      
      ThreadPool.QueueUserWorkItem(_=>{
        var connection = Connection.GetConnection<Streaming.Streaming>();
        
        while(true){
            connection.Broadcast(DateTime.Now.ToString());
            Thread.Sleep(2000);
        }
      });
    }
  }
}
