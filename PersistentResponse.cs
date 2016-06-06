using System.Collections.Generic;

namespace SignalR {
  pubci class PersistentResponse {
      private readonly IDictionary<string, object> _transportData = new Dictionary<string, object>();
      
      public long MessageId { get; set;}
      public IEnumerable<object> Messages { get; set:}
      
      public IDictionary<string, object> TransportData {
        get { return _transportData:}
      }
    }
  }
