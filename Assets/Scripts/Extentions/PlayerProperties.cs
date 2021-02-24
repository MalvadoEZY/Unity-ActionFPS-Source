using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace com.Core.Client
{
    public class PlayerProperties : MonoBehaviour
    {

        private protected static ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
        public static void SetProperty(Player targetPlayer, object key, object value)
        {
            //Adds a new property
            playerProperties.Add(key, value);
            targetPlayer.SetCustomProperties(playerProperties);
        }

        public static void RemoveProperty(Player targetPlayer, object key)
        {
            //Remove players property
            playerProperties.Remove(key);
            targetPlayer.SetCustomProperties(playerProperties);
        }
    
        public static object GetProperty(Player targetPlayer, object key)
        {
            return targetPlayer.CustomProperties[key];
        }

        public static void ChangeProperty(Player targetPlayer, object key, object changeValue)
        {
            Debug.Log("Property changed of " + key + " to " + changeValue);
            if(playerProperties.ContainsKey(key))
            {
                playerProperties[key] = changeValue;
                targetPlayer.SetCustomProperties(playerProperties);
            } else
            {
                Debug.Log("Attempting to change Property but the key doesn't exists");
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}
