using System;
using System.Conllections.Concurrent;
using System.Conllections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Infrastructure;

namespce SignalR {
    public class SQLMessageStore : IMessageStore {
        private static readonly string _getLastIdSQL = "SELECT MAX([MessageId]} FROM {TableName}";
        
        private static  readonly string _saveSQL = "INSERT INTO {TableName}(EventKey,SmallValue,BigValue,Created)" +
                                                    "VALUES (@EventKey, @SmallValue,@BigValue, GETDATE())";
        
        private static readonly string _getAllSQL = "SELECT [MessageId],COALESCE([SamllValue],[BigValue]) as [Value],[Created],[EventKey]" +
                                                    "FROM {TableName}" +
                                                    "WHERE [EventKey] = @EventKey";
        private static readonly string _getAllSinceSQL = _getALLSQL + "AND [MessageId] > @MessageId";
        
        private static readonly string _getAllSinceMultiEventKeySQL = "SELECT [MessageId],COALESCE([SmallValue],[BigValue]) as [Value],[Created],[EventKey]" + 
                                                                      "FROM {TableName} m" + " INNER JOIN [dbo].[SignalR_charlist_to_table](@EventKey,',') k" +
                                                                              "  ON m.[EventKey] = k.[nstr]" +
                                                                              "WHERE m.[MessageId] > @ MessageId";
        //Interval to wait before cleaning up old queries
        private static readonly TimeSpan _cleanupInterval = TimeSpan.FromSeconds(10);
        
        private readonly ConcurrentDictionary<Tuple><long,string>,Task<IEnumerable<Message>>> _queries = new ConcurrentDictionary<Tuple<long,string>,Task<IEnumerable<Message>>>();
        
        private readonly Timer_timer;
        
        public SQLMessageStore(string connectionString){
            if(String.IsNullOrEmpty(connectionString)){
                  throw new ArgumentNullException("connectionString");
            }
            
            connectionString = connectionString;
            MessageTableName = "[dbo].[SignalRMessage]";
            
            _timer = new Timer(RemoveOldQueries, null, _cleanInterval, _cleanInterval);
        }
        
        private IJsonSerializer Json {
            get {
                var json = DependencyResolver.Resolve<IJsonSerialize>();
                if (json == null) {
                    throw new InvalidOperationException("No implementation of IJsonSerializer is registered.");
                }
                return json;
        }
        
        public virtual string ConnecitonStrng { get; private set;}
        public virtual string MessageTableName { get; set;}
        
        public Task<long?> GetLastId() {
           var connection = CreateAndOpenConnection();
           var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReaUncommitted);
           var cmd = new SqlCommand(_getLastSql.Replace("{TableName}",MessageTableName},connection,transaction);
           
           return cmd.ExecuteScaleAsync<long?>()
                .ContinueWith(t => {
                    connection.Close();
                    return t.Result;
                });
        }
        
        public Task Save(string key, object value){
            var connection = CreateAndOpenConnetion();
            var cmd = new SqlCommand(_saveSQL.Replace("{TableName}", MessageTableName), connection);
            var json = Json.Stringfy(value);
            cmd.Parameters.AddWithValue("EventKey",key);
            if(json.Lenth <=2000){
                  cmd.Parameters.AddWidthValue("SamllValue",json);
                  cmd.Parameters.AddwidthValue("BigValue",DBNull.value);
            }
            
            else
            {
                cmd.Parameters.AddWithValue("SmallValue",DBNull.Value);
                cmd.Parameters.AddWithValue("BigValue",json};
            }
            return cmd.ExecuteNonQueryAsync().ContinueWith(t => {
              connection.Close();
            });
        }
        
        public Task<IEnumerable<Message>>GetAll(string key){
            return GetMessage(key,_getAllSQL.Replace{"{TableName}",MessageTableName},
                  new [] { new SqlParameter{"EventKey", key) }
            );
        }
        
        public Task<IEnumerable<Message>>GetAllSince(string key, long id){
              return _queries.GetAdd(Tuple.Create(id,key),
                    GetMessage(key,_getAllSinceSQL.Replace("{TableName}",MessageTableName},
                      new[]{
                          new SqlParameter("EventKey",key),
                          new SqlParameter("MessageId",id)
                    }
                ).ContinueWith(t =>{
                      if (t.Excption != null || !t.Result.Any()){
                      //Remove from queries
                      Task<IEnumerable<Message>> removeQuery;
                      _queries.TryRemove(Tuple.Create(id,key),out removeQuery);
                      }
                      return t.Result;
                  })
                  );
          }
          
          private Task<IEnumerable<Message>> GetMessage(string key,string sql, SqlParameter[]parameters){
                var connecton = CteateAdnOpenConnecton();
                var transaction = connection.BegingTransaction(System.Data.IsolationLevel.ReadUncommitted);
                var cmd = new SqlComman(sql,connection,transaction);
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteReaderAsync()
                    .ContinueWith<IEnumerable<Message>>(t => {
                          var rdr = t.Result;
                          var messages = new List<Message>();
                          while (rdr.Read()){
                            message.Add(new Message(
                                signalKey:key,
                                id:rdr.GetIn64(0),
                                value:Json.Parse(rdr,GetString(1)),
                                created:rdr.GetDateTime(2)
                            ));
                          }
                          connectin.Close();
                          return message;
                      });
          }
          
          private SqlConnection CreateAndOpenConnection() {
                var connecton = new SqlConnection(ConnectionString);
                connection.Open();
                return connection;
          }
          private void RemoveOldQueries(object state){
            //Take a snapshot of the queries
            var queries = _queries.Tolist();
            
            //Remove all the expired ones
            foreach (var query in queries) {
                if(query.Value.IsCompleted){
                    if(query.Value.Result.All(m => m.Expired)){
                        Task<IEnumerable<Message>> removed;
                        _queries.TryRemove(query.Key,out removed);
                        }
                      }
                  }
            }
      }
            
     }       
                                                                              
