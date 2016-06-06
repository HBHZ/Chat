using System.Web.Script.Serializtion;
using System;

namespace SignalR {
    public class JavaScriptSerializerAdapter : IJsonStringifier {
        private JavaScriptSerializer _serializer;
        
        public JavaScriptSerializerAdapter(JavaSciptSerializer serializer) {
              _serializer = serializer;
        }
        public string Stringify(object obj) {
              return _serializer.Serialize(obj);
        }
     }
}
