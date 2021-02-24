using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Photon
using Photon.Pun;
using Photon.Realtime;
namespace com.Core.Client
{
    public class PlayerAbilities : MonoBehaviourPun
    {

        [SerializeField] private PlayerManager playerManager;
        
        [SerializeField] private Transform playerCamera;

        [SerializeField] private GameObject PlayerWeapon;

        //Laser variables
        private protected float maxLaserDistance = 200f;

        //Skills
        private protected float FireDelay = 3f; // Delay of the firing 
        private protected float TeleportDelay = 5f; // Delay of the teleport 

        private protected float FireTimer = 0f; //
        private protected float TelportTimer = 0f; // 

        private protected float currentTimeLapsedKickBack = 0f;

        private protected Vector3 originalCameraPosition;
        void Start()
        {
            originalCameraPosition = PlayerWeapon.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if(originalCameraPosition != PlayerWeapon.transform.localPosition) PlayerWeapon.transform.localPosition = Vector3.Lerp(PlayerWeapon.transform.localPosition, originalCameraPosition, Time.deltaTime * 2f);
            if(photonView.IsMine)
            {
                checkTeleport();
            }
        }

        private void checkTeleport()
        {
            //FIRE LASER
            if (Input.GetKeyDown(KeyCode.Mouse0) )
            {
                Debug.Log("TRY FIRE LASER");
                //Gun FX
                Debug.Log(PlayerWeapon.transform.localPosition.x + PlayerWeapon.transform.localPosition.y + PlayerWeapon.transform.localPosition.z);
                
                //Vector3 newWeaponPos = new Vector3(PlayerWeapon.transform.localPosition.x, PlayerWeapon.transform.localPosition.y, PlayerWeapon.transform.localPosition.z - .4f);
                //Vector3 kickback = Vector3.Lerp(PlayerWeapon.transform.localPosition, newWeaponPos, Time.deltaTime * 3f);
                //PlayerWeapon.transform.localPosition -= kickback;

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 500f))
                {
                    Debug.Log("FIRE LASER");
                    fireLaser(hit, true);
                    //If hitting a player 

                    if (hit.collider.gameObject.layer == 11) // kill player
                    {
                        //hit.collider.gameObject.GetPhotonView().RPC("death", RpcTarget.All);
                        //Debug.LogWarning("Collided with a player");
                    }

                }
                else
                {
                    fireLaser(hit, false);
                }
            }

            //TELEPORT
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {

            }

            //-------
        }

        [PunRPC]
        private void teleportSkill()
        {
            if (photonView.IsMine)
            {
                RaycastHit hit;

                //Teleport the player
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
                {
                    StartCoroutine(TeleportToPoint(gameObject.transform.localPosition, hit.point, .2f));
                }
            }
        }

        private void fireLaser(RaycastHit hit, bool isFinite)
        {
           

    
        }

        //Smooth teleportation
        private IEnumerator TeleportToPoint(Vector3 origin, Vector3 destination, float travelTime)
        {
            playerManager.allowCameraUpdate = false;
            playerManager.allowMovementUpdate = false;


            float timeElapsed = 0f;
            //While the teleport has not finished. Player cannot use controls.
            while (timeElapsed < travelTime)
            {
                transform.position = Vector3.Lerp(origin, destination, timeElapsed / travelTime);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
            playerManager.allowCameraUpdate = true;
            playerManager.allowMovementUpdate = true;

        }
    }
}
