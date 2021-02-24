using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.Network;

namespace com.Core.UI.PassiveMenu
{
    public class ServerRoomJoin : MonoBehaviour
    {
        [SerializeField] private Button joinButton;
        public static string selectedServer = null;
        private Color buttonOriginalColor;


        private void OnEnable()
        {
            buttonOriginalColor = joinButton.colors.normalColor;
        }

        private void Update()
        {
            var color = joinButton.colors;
            if(selectedServer == null)
            {
                color.normalColor = Color.gray;
            } else
            {
                color.normalColor = buttonOriginalColor;
            }
        }

        public void joinSelectedServer()
        {
            if (selectedServer == null) return;
            //if (selectedServer.Length < 1) return; //If there's no field selected will not do anything

            NetworkManager.JoinRoom(selectedServer);
        
        }
    }
}
