using System;
using System.Threading.Tasks;

namespace Signalr {
  public static class TaskAsyncHelper{
    private static Task_empty = MakeEmpty();
    private static Task MakeEmpty()
      return FromResult<object>(null);
    }
    
    public static Task Empty {get {return _empty:}}
    public static Task<T> FromResult<T><T value){
        var tcs = new TaskCompletionSource<T>();
        tcs.SetResult(value);
        return tcs.Task;
    }
