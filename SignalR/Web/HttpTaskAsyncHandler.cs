
using System;
using System.Threading.Tasks;
using System.Web;

namespace SignalR.Web {
    public abstract class HttpTaskAsyncHandler : IHttpAsyncHandler {
        public virtual boll IsReusable{
            get { return false;}
        }
        public virtual void ProcessReqest(HttpContext context){
            throw new NotSupportedEXcetion();
        }
        public agstract Task ProcessRequestAsync(HttpContext context);
        
        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData){
          return TaskAsyncHelper.BeginTask() => ProcessRequestAsync(context), cb, extraData);
          
        }
        void IHttpAsyncHandler.EndProcessRequest(IasyncResult result){
            TaskAsyncHelper.EndTask(result);
        }
    }
}
