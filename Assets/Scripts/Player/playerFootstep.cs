using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerFootstep : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<AudioClip> footsteps;
    [SerializeField] private Transform footstepSource;
    [SerializeField] private PhotonView PV;

    private protected float footstepMaxDistance = 30f;

    public void footstepSound()
    {
    
        if(PV.IsMine)
        {
            System.Random rndNumber = new System.Random();
            int index = rndNumber.Next(footsteps.Count);
            Transform ftTrans = footstepSource;

            PV.RPC("sendFootstep", RpcTarget.All, ftTrans.position.x, ftTrans.position.y, ftTrans.position.z, index);
        }
    }

    [PunRPC]
    public void sendFootstep(float x, float y, float z, int footStepIndex)
    {
        Vector3 pos = new Vector3(x, y, z);
        GameObject tempGO = new GameObject("Temporary Audio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        var aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = footsteps[footStepIndex]; // define the clip
        aSource.playOnAwake = false;
        aSource.volume = .3f;
        aSource.spatialBlend = 1f; //3D sound if is not the local player
        aSource.minDistance = 0f;
        aSource.maxDistance = footstepMaxDistance;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        if (PV.IsMine) aSource.spatialBlend = 0f; // 2D if is local player
        aSource.maxDistance = footstepMaxDistance;
                             // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, aSource.clip.length); // destroy object after clip duration


    }
}
