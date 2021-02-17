using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.Configuration;
public class ClientInfoLoader : MonoBehaviour
{
    [SerializeField]
    private Text gameVersion;

    // Start is called before the first frame update
    void Start()
    {
        
        //Set game version bottom right corner
        gameVersion.text = ConfigurationFile.GameVersion;
    }
}
