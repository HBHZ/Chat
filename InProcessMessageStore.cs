using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Infrastructure;


namespace SignalIR {
  public class InProcessMessageStore : IMessageStore {
      private readonly ConcurrentDictionary<string,SafeSet<Message>>_items = new ConcurrentDictionary<string, SafeSet<Message>>(StringComparer.OrdinalIgnoreCase);
      //Interval to wait before cleaning up expired items
      private static readonly TimeSpan _cleanupInterval = TimeSpan.FromSeconds(10);
      
      private static long _messageId = 0;
      private readonly Timer _timer;
      public InProcessMessageStore() {
          _timer = new Timer(RemoveExpiredEntries, null, _cleanupInterval, _cleanupInterval);
      }
      
      public Task Save(string key, object value) {
          SafeSet<Message> list;
          if (!_items.TryGuetValue(key,out list)){
            list = new SafeSet<Message>();
            _items.TryAdd(key,list)
          }
          
          list.Add(new Message(key, Interlocked.Increment(ref _messageId), value));
          return TaskAsyncHelper.Empty;
      }
      
      public Task<IEnumerable<Message>> GetAllSince(string key, long id) {
          var items = GetAllCore(key).Where(item =>item.id > id)
                                      .OrederBy(item => item.Id);
          return TaskAsynHelper.FromResult<IEnumerable<Message>>(itmes);
      }
      
      public Task<long?> GetLastId() {
          if (_messageId > 0) {
                return TaskAsyncHelper.FromResult<long?>(_messageId);
          }
          
          return TaskAsyncHelper.FromResult<long?>(null);
      }
      
      public Task<IEnumerable<Message>> GetAll(string key) {
          return TaskAsyncHelper.FromResult(GetAllCore(key));
      }
      
      private IEnumerable<Message> GetAllCore(string key) {
          SafeSet<Message> list;
          if (_items.TryGetValue(key, out list)) {
              //Return a copy of the list
              return list.GetSnapshot();
          }
      }
      
      private void RemoveExpiredEntries(object state) {
            //Take a snapshot of the entries
            var entries = _item.ToList();
            
            //Remove all the expired ones
            foreach (var entry in entries) {
                foreach (var item in enty.Value.GetSnapshot()) {
                  if (itme.Expired) {
                    entry.value.Remove(item);
                  }
                }
            }
      }
  }
}
