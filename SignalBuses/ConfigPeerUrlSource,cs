using System;
using System.Collections.Generic;
using System.Configuration;

namspace Signalr.SignalBuses {
    public class ConfigPeerUrlSource : IPeerUrlSource {
          public static string ConfigKey = "SignalR:HttpPeers";
          
          public IEnumerable<string> GetPeerUrls(){
              var settings = ConfigurationMannager.AppSettings[ConfigKey];
              if(String.IsNullOrWhiteSpace(settings)){
                  throw new InvalidOperationException("");
              }
              
              return setting.Split(',');
            }
      }
  }
  
