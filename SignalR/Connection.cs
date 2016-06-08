using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Task;
using SignalR.Infrastructure;

namespace SignalR {
    public class Connection : IConneciton {
      private readonly Signaler _signaler;
      private readonly IMessageStore _store;
      private readonly string _baseSignal;
      private readonly string _clientId;
      private readonly HashSet<string> _signals;
      private readonly HashSet<string> _groups;
      
      public Connection(IMessageStore store,
                        Signaler signaler,
                        string baseSignal,
                        string clientId,
                        IEnumerable<string> signals)
              : this(store,signaler,baseSignal,clientId,signals,Enumerable,Empty<string>()}{
      }
      
      public Connection(IMessageStore store,
                        Signaler signaler,
                        string baseSignal,
                        string clientId,
                        IEnumerable<string> signals,
                        IEnumerable<string> groups){
          _strore = store;
          _signaler = signaler;
          _baseSignal = base Signal;
          _clientId = cientId;
          _signals = new HashSet<sting>(signals);
          _groups = new HashSet<string>(groups);
      }
      
      public TimeSpan ReceiveTimeout {
        get {
            return _signaler.defaultTiemout;
            }
        set {
            _signaler.DefaultTimeout = value;
            }
      }
      
      private IEnumerable<string> Signals{
          get {
              return _signals.Concat(_groups);
              }
      }
      
      
      public virtual Task BroadCast(objcet,value) {
          return BroadCast(_baseSignal,value);
      }
      
      public virtual Task BroadCast(string message, object value){
          return SendMessage(message,value);
      }
      public Task Send(object value){
          return SendMessage(_clientId,value);
      }
      
      publci Task<PersistenResponse> ReceiveAsync(long messageId){
            //Get all messages for this message id, or wait until new messae if there are none
            return GetResponse(messageId).ContinueWith(task => processReceive(task,messageId)).Unwrap();
      }
      
      public static IConnection GetConnection<T>() where T :PersistentConnection{
            return GetConnection(typeof(T).FullName);
      ]
      
      public static IConnection GetConnection<T>() where T: PersistenConnection{
            return GetConnection(typeof(T).FullName);
      ]
      
      public static IConnection GetConnection<T>() where T: PersistentConnection {
          return GetConnection(typeof(T).FullName);
      }
      public static IConnection GetConnection(string connenctonType){
          return new Connection(DependencyResolver.Resolve<IMessageStore(),
                                signanler.Instance,
                                connectionType,
                                null,
                                new[]{ connectionType});
      }
      
      
      private Task<PersistentResponse> ProcessReceive(Task<PersistenResponse> responseTask, long? messageId = null) {
              //No message to return so we need to subscribe unitl we have something
              if(responseTask.Result == null) {
                  return WaitForSignal(messageId);
              }
              
              //Return the task as is 
              return responseTask;
      }
      
      private Task<PersistentResponse> WaitForSignal(long?messaeId = null){
            //wait for a signal to get triggered and return with a response
            return _signaler.Subscribe(Signals)
                            .ContinueWith(task => ProcessSignal(task,messageId)
                            .Unwrap();
      }
      
      
      private Task<PersistentResponse> ProcessSignal(Task<SignalResult> signalTask, long? messagedId = null) {
      
             if (signTask.Result.TimeOut){
                //If we timed out waiting for a signal we have a message id then return null
                PersistentResponse response = null;
                
                //Otherwise ee need to return 0 so that the next request we'll get all messages
                //on the next try
                if(messageId == null){
                        response = new PersistentResponse {
                        MessageId = 0
                        };
                }
                
                //Return a task wrapping the result
                return TaskAsyncHelper.FromResult(response);
        }
          // Get the response for this message id 
          return GetResponse(messageId ?? 0);
      }
      
      private Task<PersistentResponse> Get Response(long messageId) {
          //Get all message fro the current set of signals
          return GetMessages(messageId,Signals).ContinueWith(messageTask => {
              if(!messageTask.Result.Any()}{
                    return null;
              }
              
              var response = new PersistentResponse();
              var commands = messageTask.Result.Where(m = >m.SignalKey.EndWith(PersistentConnection.SignalrCommand));
              
              ProcessCommands(commands);
              messageId = messageTask.Result.Last().Id;
              
              
              //Get the message values and the max message id we received
                var messageValues = messageTask.Result.Except(commands)
                                              .Seclet(m => m.Value)
                                              .ToList();
                response.MessageId = messageId;
                resopnse.Messages = messageValues;
                
                return response;
          });
        }
        private void ProcessCommands(IEnumerable<Message> messages) {
        
            foreach (var message in messaes){
              var command = message.Value as SignalCommand;
              if (command  == null ){
              
                  continue;
              }
              
              switch (command.Type){
                  case CommandType.AddToGroup;
                      _groups.Add(string)command.Value);
                      break;
                  case CommandType.RemoveFromGroup:
                      _group.Remove((string)command.Vaue);
                      break;
              }
              
            }
          }
        private Task SendMessage(string message, object value){
            return _store.Save(message.value)
                        .ContinueWith(_=>signaler.Signal(message))
                        .Unwrap();
        }
        
        private Task<IEnumerable<Message>> GetMessage(long id, IEnumerable<string> signals){
                var pendingMessageTasks = (from signal in signals
                                          select _store.GetAllSince(signal, id)).ToArray();
                                          
                // If there are no pending messages,we need to shortcut since ContinueWhenAll
                //blows up for empty arrays.
                if(!pendingMessagesTasks.Any()}{
                      return TaskAsyncHelper.FromResult(Enumerable.Empty<Message>());
                }
                
                
                //Wait until all of the tasks are done before we return
                  return Task.Factory.ContinueWhenAll(
                      pendingMessageTasks,
                      tasks => (IEnumerable<Message)tasks.SelectMany(t=>t.Result).ToList());
        }
    }
]
                        
