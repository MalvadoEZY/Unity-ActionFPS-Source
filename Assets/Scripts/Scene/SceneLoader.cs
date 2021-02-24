using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

namespace com.Core.SceneHelper
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        public void LoadMultiplayerLevel(int sceneIndex)
        {
            //int sceneIndex = getSceneIndexFromName(sceneName);
            //if (sceneIndex == -1) return;
            
            StartCoroutine(LoadAsyncScene(sceneIndex));
        }

        private int getSceneIndexFromName(string name)
        {
            Debug.Log("Searching build index by name: " + name + " of how many indexes: " + SceneManager.sceneCountInBuildSettings);
            for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                Scene getSceneName = SceneManager.GetSceneAt(i);
                Debug.LogWarning("SCENE NAME: " + getSceneName.name + " of " + i);
                if(getSceneName.name == name)
                {
                    Debug.Log("SCENE NAME SAME !!!");
                    return getSceneName.buildIndex;
                }

            }
            return -1;
        }

        IEnumerator LoadAsyncScene(int sceneIndex)
        {
         
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneIndex);

            

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(sceneIndex);
            yield return null;
        }
    }
}
