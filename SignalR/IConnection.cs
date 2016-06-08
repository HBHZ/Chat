using System;
using System.Threading.Tasks;

namespace SignalR {
    publish interface IConnection {
          TimeSpan ReceiveTimeout { get; set:}
          
          Task Send(object value);
          Task Broadcast(string message, object value);
          Task Broadcast(object value);
          
          Task<PersistenResponse> ReceiveAsync();
          Task<PersistenResponse> ReceiveAsync(long messageId);
    }
}
