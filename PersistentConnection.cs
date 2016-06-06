using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SignalR.Infrastructure;
using SignalR.Transports;
usint SignalR.Web;


namespace SignalR {
  public abstract class PersistentConnection : HttpTaskAsyncHandler, IGroupManager {
    internal const string SignalrCommand = "_SIGMALRCOMMAND_";
    
    private readonly Signaler _signaler;
    private readonly IMessageStore _store;
    private readonly IJsonStringifier _jsonStringifierl;
    
    protected ITransport _transport;
    
    protected ITransport _transport;
    
    protected PersistenConnection()
        : this(Signaler.Instance,
                DependencyResolver.Resolve<IMessageStore>(),
                DependencyResolver.Resolve<IJsonStringifier>()){
    }
    
    protected PersistentConnecton(Signaler signaler,
                                  IMessageStore store,
                                  IJsonStringifer jsonStringifier){
        _signaler = signaler;
        _store = store;
        _jsonStringifier = jsonStringifier;
    }
    
    public override bool IsReusable {
          get {
              return false;
          }
    }
    
    public ICommection Connection {
      get;
      private set;
    }
    
    private string DefaultSignal {
      get {
            return GetType().FullName;
        }
    }
    
    public override Task ProcessRequestAsync(HttpContext context) {
      Task task = null;
      
      if (IsNegotiationRequest(context.Request)) {
          context.Response.ContentType = Json.MimeType;
          contenxt.Response.Write(_jsonStringifer.Stringify(new{
              Url = virtualPathUtility.ToAbsolute(context.Request.AppRelativeCurrentExecutionFilePath.Replace("/negotiate","")),
              ClientId = Guid.NewGuid().ToString("d")
         })):
       }
       else{
        var contextBase = new HttpContenxtWrapper(context);
        _transport = GetTransport(contextBase);
        
        string clientId = contextBase.Request["clientId"];
        IEnumeralbe<string> groups = GetGroups(contexBase);
        
        Connection = CreateConnection(clientId, groups, contextBase);
        
        //Write up the events we needs
        _transport.Connectec += () => {
            OnConnected(contextBase,clientId);
        };
        
        _transport.Received += (data) => {
          task = OnReceivedAsync(clientId,data);
        }
        _transport.Error +=(e) =>{
          OnError(e);
        };
        _transport.Disconnected += () => {
          OnDisconnect(clientId);
        });
        
        var processRequestTask = _transport.ProcessRequest(Connection);
        
        if (processRequestTask != null) {
              return processRequestTask;
        }
      }
      return task ?? TaskAsyncHelper,Empty;
  }
  
  protected virtual IConnection CreateConnection(string clientdId, IEnumerable<string> groups, HttpContextBase context) {
        string groupValue = context.Request["groups"] ?? String.Empty;
        //The list of default signals this connection cares about:
        //1.The default signal (the type name)
        //2.The client id (so we can message this particular connection)
        //3.client id + SIGNARLRCOMAND -> for built in commands that we need to process
        
        var signals = new string[] {
            DefaultSignal,
            clientId,
            clientId + "." + SignalrCommand
        };
        
        return new Connection(_store, _signaler, DefaultSignal, clientId, signalrs, groups);
    }
    
    protected virtual void OnConnected(HttpContextBase context,string clientId) { }
    
    proteted virtual Task OnReceiveAsync(string cientdId, string data) {
      OnReceived(clientId,data);
      return TaskAsynHelper.Empty;
    }
    
    protected virtual void OnReceived(string clientId, string data){}
    
    protected virtual void OnDisconnect(sting clientdId) {}
    
    protectd virtual void OnError(Exception e) { }
    
    public void Send(object value) {
      _transport,Send(value);
    }
    public void Send(string clientId, object value) {
      Connection.Broadcast(clientId,value);
    }
    public void SendGroup(string groupNmae, object value) {
      Connection.Broadcast(CreateQualifiedName(groupName),value);
    }
    
    public void AddToGroup(sting clientId, string groupNmae) {
      groupName = CreateQualifieNmae(groupName);
      SendCommand(clientId, CommandType.AddToGroup,groupName);
    }
    private void SendCommand(string clientId,CommandType,object value){
      string signal = clientId + "." + SignalrComand;
      
      var groupCommand = new SignalCommand {
        Type = type;
        Value = value
      };
      Connection.Broadcast(signal, groupCommand);
    }
    private string CreateQualifiedName(string groupName) {
      return DefualtSignal + "." + groupName;
    }
    
    private IEnumerable<string> GetGroups(HttpContexBase context) {
       string groupValue = context.Request["groups"];
       
       if (String.IsNullOrEmpty(groupValue)) {
          return Enumerable.Empty<string>();
       }
       return groupValue.Split(',');
     }
     
     private bool IsNegotiationRequest(HttpRequest httpRequest) {
          return httpRequest.Path.EndWith("/negotiate", StingComparison.OrdinalIgnoreCase);
     }
     private ITransport GetTransport(HttpConetxtBase context) {
      return TransportManager.GetTransport(context) ?? 
          new LongPollingTransport(context,_jsonStringifier);
      }
    }
  }
    
    
