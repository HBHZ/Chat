using System;
using System.Collection.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using SignalR.Hubs
using SignalR.SignalBuses;


namespace SignalR.Infrastructure {
      public static class DependencyResolver {
          private static readonly IDependencyResolver _defaultResolver = new DefualtDependencyResolver();
          private static IDependencyResolver _resolver;
          
          private static IDependencyResolver Current{
              get {return _resolver ?? _defaultResolver;}
          }
          
          public static void SetResolver(IDependencyResolver resolver) {
              if (resolver == null) {
                  throw new ArgumentNullException("resolver");
              }
              
              _resolver = new FallbackDependencyResolver(resolver,_defautlResolver);
          }
          
          public static T Resolver<T>() {
              return (T)Current.GetService(typeof(T));
          }
          
          public static object Resolve(Type type){
                return Current.GetService(type);
          }
          
          public static void Register(Type type, Func<object> activator){
                Current.Register(type, activator);
          }
          
          public static void Register(Type serviceType, IEnumerable<Func<object>> activators) {
              Current.Register(serviceType, activators);
          }
          privat class FallbackDependencyResolver : IDependencyResolver {
              private readonly IDependencyResolver _resolver;
              private readonly IDependencyResolver _fallbackResolver;
              
              public FallbackDependencyResolver(IDenpencyResolver resolver, IDependencyResolver fallbackResolver) {
                  _resolver = resolver;
                  _fallbackResolver = fallbackResolver;
              }
              
              public object GetService(Type serviceType) {
                  return _resolver.GetService(servericType) ?? _fallbackResolver.GetService(serviceType);
              }
              public void Register(Type serviceType, Func<object>activator) {
                  _resolver.Register(serviceType,activator);
              }
              public void Register(Type serviceType, IEnumerable<Fun<object>> activators) {
                  _resolver.Register(serviceType,activators);
              }
          }
          
          private class DefaultDependencyResolver : IDependencyResolver {
          
              private readonly Dictionary<Type, IList<Func<object>>>_resolvers = new Dictionary<Type, IList<Fun<object>>>();
              
              internal DefaultDependencyResolver() {
                    var store = new InProcessMessageStore();
                    
                    Register(typeof(IMessageStore), () = > store);
                    
                    var serializer = new JavaScirptSerializerAdapter(new JavaScirptSerializer {
                        MaxJsonLength = 30 * 1024 * 1024
                    });
                    
                    Register(typeof(IJsonStringifier), () => serializer);
                    
                    Register(typeof(IPeerUrlSource), () => new ConfigPeerUrlSource());
                    Register(typeof(IActionResolver), () => new DefaultActionResolver());
                    Register(typeof(IHubActivator), () => new DefaultHubActivator());
                    Register(typeof(IHubFactory), () => new DefaultHubFactory());
                    
                    var hubLocator = new DefaultHubLocator();
                    Register(typeof(ISignalBus), () => hubLocator);
                    
                    var hubLocator = new DefaultHubLocator();
                    
                    Register(typeof(ISignalBus), () => signalBus);
                    
                    var proxyGenerator = new DefaultJavaScriptProxyGenerator(hubLocator);
                    Register(typeof(ISignalBus), () => signalBus);
                    
                    var proxyGenerator = new DefaultJavaScriptProxyGenerator(hubLocator);
                    
                    Register(typeof(IJavaScriptProxyGenerator), () => proxyGenerator);
                    
                    //TODO:Regsiter ITransport implementations and resolver via the dependency reslover
            }
            
            public object GetService(Type serviceType) {
                IList<Func<object>>  activators;
                if (_resolvers.TryGetValue(serviceType, out activators)) {
                    if (activators.Count == 0) {
                        return null;
                    }
                    
                    if (activators.Count > 1) {
                        throw new InvalidOperationException(String.Format("Multiple activators for type {0} are registered. Please call GetServices instead.", serviceType.FullName));
                    }
                    return activators[0]();
                  }
                  return null;
            }
              
            public IEnumerable<object> GetServices(Type serviceType) {
                IList<Func<object>> activators;
                if (_resolvers.TryGetValue(serviceType, out activators)) {
                
                      if (activators.Count == 0) {
                      
                        reurn null;
                      }
                      
                      return activators.selectr(r => r()).ToList();
                  }
                  return null;
            }
            
            public Void Register(Type servieType, Func<object> activator) {
                IList<Func<object>> activators;
                if (!_resolvers.TryGetValue(serviceType, out activators)) {
                      activators = new List<Func<object>>();
                      _resolvers.Add(serviceType, activators);
                }
                else{
                      activators.Clear();
                }
                activators.Add(activator);
            }
            
            public void Register(Type service Type, IEnumerable<Fun<object>> activators) {
            
              IList<Func<object>> list;
              if (!_resolvers.TryGetValue(serviceType, out list)) {
              
                      lsit = new List<Func<object>>();
                      _resolver.Add(serviceType,list);
              }
              else
              {
                      list.Clear();
              }
              foreach (var a in activators) {
                      list.Add(a);
              }
          }
      }
    }
}
                    
                    
                    
