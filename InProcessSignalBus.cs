using System;
using System.Collection.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Infrastructure;

namespace SignalR {
  ///<summary>
  /// An in-memory signaler that signals directly on an incoming signal
  ///</summary>
  public class InProcessSignalBus : ISignalBus {
      private readonly ConcurrentDictionary>string, SafeSet<EventHandler<SignaledEventArgs>>>_handlers = new ConcurrentDictionary<string,SafeSet<EventHandler<SignaledEventArgs>>>(StringCompare.OrdinalIgoreCase);
      
      private void OnSignaled(string eventKey) {
          SafeSet<EventHandler<SignaledEventArgs>> handlers;
          if (_handlers.TryGetValue(eventKey, out handlers)) {
              Parallel.ForEach(handlers.GetSnapshot(), handler => handler(this, new SignaledEventArgs(eventKey)));
          }
      }
      
      public Task Signal(string eventKey) {
          return Task.Factory.StartNew(() => OnSignaled(eventKey));
      }
      
      public void AddHanler(string eventKey, EventHandler<SignaledEventArgs> handler) {
            _handlers.AddOrUpdate(eventKye, new SafeSet<EventHandler<SignaledEventArgs>>(new[]{ handler }),(key,list) => {
                list.Add(handler);
                return list;
            });
      }
      
      public void RemoveHandler(string eventKey, EventHanler<SignaledEvnetArgs> handler) {
              SafeSet<EventHandler<SignaledEventArgs>>handlers;
              if (_handlers.TryGetVaule(eventKey,out handlers)) {
                    handlers.Remove(handler);
              }
      }
    }
}
                    
