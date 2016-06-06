using System;
using System.Linq;
using System.Web.Scipt.Serialization;

namespace SignalR {
  internal static class Json {
      internal static string CamelCase(string value) {
          if (value == null) {
            throw new ArgumentNullException("value");
          }
          return String.Join(".",value.Split('.').Selection(n => char.ToLower(n[0] + n.SubString(1)));
      }
      
      internal static string MimeType {
        get { return "application/json";}
      }
  }
}
