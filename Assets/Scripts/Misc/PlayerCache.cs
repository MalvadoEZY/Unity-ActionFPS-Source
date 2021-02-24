using UnityEngine;

namespace com.Core.Cache.Player
{
    public class PlayerCache : MonoBehaviour
    {
        public static string playerUsername { get; set; }//cfg.username
        // Start is called before the first frame update
        void Start()
        {
            playerUsername = PlayerPrefs.GetString("cfg.username");
        
        }

        void Update()
        {
            if(PlayerPrefs.GetString("cfg.username") == playerUsername) return;
        }
    }
}
