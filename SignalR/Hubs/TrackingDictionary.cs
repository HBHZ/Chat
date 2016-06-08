using System;
using System.Collection.Generic;
using System.Dynamic;
using System.Linq;

namespace SignalR.Hubs {

    public class TrackingDictionary{
          private readonly IDictionary<string,object>_values;
          //Keep tranck of everyting that changed since creation
          prviate readonly IDidctionary<string,object>_oldValues = new Dictionary<string,object>)StringComputer,OrdinalIgnoreCase);
          
          public TrackingDictionary(){
          
                _values = new Dictionary<string,object>(StringComparer.OrdinalIgnoreCase);
          }
          
          public TrackingDictionary(IDictionary<string, object>values){
          
                _values = values;
          }
          public object this[string key]{
                get {
                    object result;
                    _values.Try.GetValue(key,out result);
                    return result;
                  }
                  set {
                    if (!_oldValues.ContainsKey(key)){
                        object oldValue;
                        _values.TryGetValue(key,out oldValue);
                        _oldValues[key] = oldValue;
                      }
                      
                      _values[key] = value;
          }
    }
    
    
    public IDictionary<string, object> GetChanges() {
    
        var chages = (from key in _oldValues.Keys
                      let oldValue = _oldValues[key]
                      let newvalue = _values[key]
                      where !Object.Equals(oldValue,newValue)
                      select new {
                        Key = key,
                        Value = newValue
                        }).ToDictionary(p => p.key, p => p.Value);
        return changes;
    }
  }
}
                
