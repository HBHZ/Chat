using System.Web;
usint System.Web.Script.Serialization;
using SignalR.Infrastructure;
usint SignalR.SQL;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]

namespace SignalR.SQL {
    public static class PreApplicationStart {
        
            public static void Start() {
                var serializer = new JavaScirptSerializerAdapter(new JavaScriptSerializer {
                    MaxJsonlength = 30 * 1024 * 1024
                });
                DependencyResolver.Register(typeof(IJsonSerializer), () => serializer);
            }
            
    }
    
}
