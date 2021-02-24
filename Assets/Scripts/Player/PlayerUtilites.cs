using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace com.Core.Client.Utilities
{
    public class PlayerUtilites : MonoBehaviourPunCallbacks
{
    private static PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }

     
}
}