﻿using System.Collections;
using System.Collections.Generic;

//Photon network
using Photon.Pun;
using Photon.Realtime;

//Unity libs
using UnityEngine;

//Game libs
using com.Core.Configuration;

namespace com.Core.Client
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        //GameObjects
        [SerializeField] private GameObject playerOnlyArms; //For the local user
        [SerializeField] List<GameObject> FullPlayerOBJ; //For the network user

        //Camera
        [SerializeField] public Transform playerCamera; //Current player camera
        
        //Weapon
        [SerializeField] private GameObject playerWeapon; //Player laser gun
        [SerializeField] private Transform playerWeaponHand;
        [SerializeField] private Transform weaponFirePoint; //Player weapon starting point of the laser

        //Animation
        [SerializeField] private Animator playerAnimator; //Get Player Animator
        private protected Transform chest;

        //Checkers
        [SerializeField] private Transform wallChecker; //Check the material of the wall
        [SerializeField] private Transform groundChecker; //Check the material of the ground

        //Character
        [SerializeField] private CharacterController characterController; //Gets the component of the character Controller to be able to move
        
        //Bones
        [SerializeField] private Transform fullBodySpineBone;
        [SerializeField] private Transform headBone;

        //Player Settings
        private protected float MouseSensivity = 1f;

        //Player listener
        private protected AudioListener audioListener;

        //Solutions logic vars ////////////////// ----  V  V  V  V  V
        //Ground checker
        private protected RaycastHit groundRayHit;
        private protected float groundDistance = .3f;
        private protected bool isGrounded = true;

        //Wall checker
        private protected RaycastHit wallDetector;
        private protected float minWallDistance = 0.1f;
        private protected float maxWallDistance = 4f;

        private protected string currentWall;
        private protected string previousWall;
        private protected bool isOnWall;

        //Player Movement variables
        private protected Vector3 velocity; //Relative velocity
        private protected Vector3 move; //Direction of move
        private protected int availableJumps = 2; //Available jumps of the player in total
        private protected float jumpForce = -.6f; //0f - 1f  Jump force from the ground
        private protected float playerRunningSpeed = 20f; //Default speed of the player
        private protected float gravity = -28f; //World gravity for the player
        private protected float GroundSmoothTime = .05f; // Movement smooth for the player on the ground
        private protected float MidAirSmoothTime = .2f; // Movement smooth for the player on the air
        private protected Vector2 currentDir = Vector2.zero; // Stores the current direction of the player
        private protected Vector2 currentDirVelocity = Vector2.zero; // Stores the velocity of the player

        //Player view variables 
        private protected float CameraMaxAngle = 90f; // Max camera angle upwards
        private protected float CameraMinAngle = -90f; // Max camera angle downwards

        public bool allowMovementUpdate { get; set; } //Useful to block movement controls in certain time
        public bool allowCameraUpdate { get; set; }  //Useful to block camera controls in certain time

        private protected float cameraPitch = 0f; //Stores the camera pitch *Default: 0f

        private protected bool FPSMode = false; //Stores the information that if the player is in first person or not

        //Animator
        private protected float animatorFatigueDelay = 10f; //Delay until the player plays the fatigue animation
        private protected float animatorFatigueTimer = 0f; //Stores the player standing time

        //Syncing player transform and rotation variables
        private protected Quaternion RemotePlayerRotation;
        private protected Vector3 remotePlayerRotVel;

        private protected Vector3 RemotePlayerPosition;
        private protected Vector3 remotePositionVel;

        private protected Vector3 RemotePlayerLooking;
        private protected Vector3 remotePlayerLookVel;

       // DEVELOPMENT USE ONLY | IT WILL IGNORE ANY NETWORKING COMPONENT OF THE PLAYER
        /// //////// ----------------------------
        /// 

        // Start is called before the first frame update
        void Start()
        {
                    audioListener = GetComponent<AudioListener>();
                    allowMovementUpdate = true;
                    allowCameraUpdate = true;

                if (photonView.IsMine)
                {
                    toggleObjects(true);
                    FPSMode = true;
                    changePerspective(true);
                    playerCamera.gameObject.SetActive(true);
                    gameObject.layer = 10; // Set Player as local player

                    if (Cursor.visible)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }

                    audioListener.enabled = true; //Enable audio listener
                }
                else
                {

                    playerAnimator.SetBool("IsGrounded", true);
                    playerAnimator.SetFloat("Horizontal", 0f); //reset
                    playerAnimator.SetFloat("Vertical", 0f); //reset
                
                    FPSMode = false;
                    toggleObjects(false);
                    changePerspective(false);
                    playerCamera.gameObject.SetActive(false);
                    gameObject.layer = 11; // Set Player as network player
                    audioListener.enabled = false;

                };
 
        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine) return;
            updatePlayerMovementAnimations();
            if (isGrounded)
            {
                playerAnimator.SetBool("IsGrounded", true);
            } else
            {
                playerAnimator.SetBool("IsGrounded", false);
            }
            //Update player status
            updatePlayerMovement();
            updatePlayerView();
            checkUpdateKeybinds();
        }

        private void LateUpdate()
        {
            if(!photonView.IsMine)
            {
                syncNetworkClient();
                
            }
        }

        private void syncNetworkClient()
        {
            if(!photonView.IsMine)
            {
                if (transform.position == RemotePlayerPosition && transform.rotation == RemotePlayerRotation && fullBodySpineBone.localEulerAngles == RemotePlayerLooking) return; // If player is completry still it will not update 
                
                float distanceMagnitude = transform.position.magnitude - RemotePlayerPosition.magnitude;
                float localOffset = -45f;

                if (RemotePlayerLooking.x < 40 && RemotePlayerLooking.x >= 0)
                {
                    localOffset = 15f;
                } else if (RemotePlayerLooking.x < 91 && RemotePlayerLooking.x >= 40)
                {
                    localOffset = 30f;
                } else
                {
                    localOffset = -45f;
                }


                if (distanceMagnitude < 10f)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, RemotePlayerPosition, ref remotePositionVel, .2f);
                    transform.rotation = SmoothDampQuaternion(transform.rotation, RemotePlayerRotation, ref remotePlayerRotVel, .15f);
                    fullBodySpineBone.localEulerAngles = Vector3.SmoothDamp(fullBodySpineBone.localEulerAngles, new Vector3(localOffset, 0f, RemotePlayerLooking.x), ref remotePlayerLookVel, 0f);
                } else
                {
                    transform.position = RemotePlayerPosition;
                    transform.rotation = RemotePlayerRotation;
                    fullBodySpineBone.localEulerAngles = RemotePlayerLooking;
                }
            }
        }


        //Smooth damp for quaternion rotation
        private protected Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
        {
            Vector3 c = current.eulerAngles;
            Vector3 t = target.eulerAngles;
            return Quaternion.Euler(
              Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
              Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
              Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
            );

            
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(playerCamera.localEulerAngles);

            }
            else if (stream.IsReading)
            {
                RemotePlayerPosition = (Vector3)stream.ReceiveNext();
                RemotePlayerRotation = (Quaternion)stream.ReceiveNext();
                RemotePlayerLooking = (Vector3)stream.ReceiveNext();
            }
        }

        private protected void toggleObjects(bool hideObjects)
        {
            for (int i = 0; i < FullPlayerOBJ.Count; i++)
            {
                FullPlayerOBJ[i].gameObject.SetActive(!hideObjects);
            }
        }

        //Change first person perspective
        private void changePerspective(bool enableFPS)
        {
            
            playerOnlyArms.SetActive(false);

            if (enableFPS)
            {
                
                playerOnlyArms.SetActive(true);
            } else
            {
                
                playerOnlyArms.SetActive(false);
            }
        }
        private void checkUpdateKeybinds()
        {
            if (!allowMovementUpdate) return;

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = jumpForce * gravity;
                //JUMP EFFECCT
                availableJumps -= 1;
            }

            if (availableJumps >= 1 && !isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {

                    if (isOnWall)
                    {
                        //Doesn't allow player jump on the same wall of before
                        if (currentWall == previousWall) return;


                        //JUMP EFFECCT WIP
                        previousWall = currentWall; 

                        //Reflection 
                        Vector3 reflection = Vector3.Reflect(move, wallDetector.normal);

                        velocity = reflection * 15f;
                        velocity.y = jumpForce * gravity; // Jump force upwards went jump from the wall

                        playerAnimator.SetTrigger("WallJump");

                    }
                    else
                    {
                        //If player tries to jump in middle of the air
                        availableJumps = 0;
                        return;
                    }
                }

            }
        }

   
        //Send direction information to the animator
        private void updatePlayerMovementAnimations()
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            bool isWalking = (horizontalAxis > 0.2f || horizontalAxis < -0.2f) || (verticalAxis > 0.2f || verticalAxis < -0.2f); //Gets true if player is walking
           

            bool isPlayerMoving = isWalking && (isGrounded || !isGrounded);
            playerAnimator.SetBool("isPlayerWalking", isPlayerMoving);
            
            //Animation logic
            if(isWalking && !isGrounded) //Player is flying while running 
            {
                animatorFatigueTimer = 0f;
                playerAnimator.SetFloat("Horizontal", 0f);
                playerAnimator.SetFloat("Vertical", 0f);
            } else if(isWalking && isGrounded)
            {
                animatorFatigueTimer = 0f;
                playerAnimator.SetFloat("Horizontal", horizontalAxis);
                playerAnimator.SetFloat("Vertical", verticalAxis);
            } else if (!isWalking && !isGrounded)
            {
                animatorFatigueTimer = 0f;
                playerAnimator.SetFloat("Horizontal", 0f);
                playerAnimator.SetFloat("Vertical", 0f);
            } else if (!isWalking && isGrounded)
            {
                animatorFatigueTimer += Time.deltaTime;
                playerAnimator.SetFloat("Horizontal", horizontalAxis);
                playerAnimator.SetFloat("Vertical", verticalAxis);
            }

            //If player is standing still for a certain amount of time will play a animation
            if(animatorFatigueTimer > animatorFatigueDelay)
            {
                playerAnimator.SetTrigger("StandingFatigue");
                animatorFatigueTimer = 0f;
            }

            
        }

        private protected void updatePlayerMovement()
        {
            
            checkGround();
            checkWall();

            Vector2 targetDir = Vector2.zero;

            if (allowMovementUpdate)
            {
                targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                targetDir.Normalize();
            }

            if (isGrounded && velocity.y < 0)
            {
                availableJumps = 2;
                //Resets the reflects
                velocity = Vector3.zero;
                velocity.y = -2f;
                currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, GroundSmoothTime);

            }
            else
            {
                //mid air smooth    
                currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, MidAirSmoothTime);
            }

            //Move Player
            move = (transform.forward * currentDir.y + transform.right * currentDir.x);
            characterController.Move(move * playerRunningSpeed * Time.deltaTime);

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private protected void updatePlayerView()
        {

            if (!photonView.IsMine) return;
            if (!allowCameraUpdate) return; 

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            cameraPitch -= mouseDelta.y * MouseSensivity;

            //Locks the player camera in certain pitch
            cameraPitch = Mathf.Clamp(cameraPitch, CameraMinAngle, CameraMaxAngle); // Lock the camera in a certain angle upwards and downwards
            playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch; // rotates the camera up and down

            transform.Rotate(Vector3.up * mouseDelta.x * MouseSensivity); // rotates the player right - left

            //Rotates the onlyArms Object 
            if(FPSMode)
            {
                playerOnlyArms.transform.localEulerAngles = playerCamera.transform.localEulerAngles; //Rotates the arms  
            }

            //photonView.RPC("rotateViewPlayerGlobally", RpcTarget.All, playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, playerCamera.transform.localEulerAngles.z);
        }

        //Rotating the bone
        [PunRPC]
        private void rotateViewPlayerGlobally(float x, float y, float z)
        {
            fullBodySpineBone.transform.localEulerAngles = new Vector3(x, y, z);
        }

        private protected void checkGround()
        {
            BoxCollider box = groundChecker.GetComponent<BoxCollider>();
            Debug.DrawRay(box.transform.position, -Vector3.up * groundDistance, Color.red);
            if (Physics.Raycast(box.transform.position, -Vector3.up, out groundRayHit, groundDistance))
            {
                if (groundRayHit.collider.gameObject.layer == 10 || groundRayHit.collider.gameObject.layer == 11) return; //Ignores if it's above of any player

                Renderer collider = groundRayHit.collider.GetComponent<Renderer>();
            
                string MaterialName = collider.material.name;
                bool GroundMaterial = MaterialName.Contains("Ground");
                bool JumpMaterial = MaterialName.Contains("JumpPad");


                //Check map triggers

                if (GroundMaterial)
                {
                    isGrounded = true;
                    
                    previousWall = null;
                    return;
                }
                else if (JumpMaterial)
                {
                    Vector3 TempVelocity = groundRayHit.normal * 45f;
                    velocity = TempVelocity;
                    velocity.y = Vector3.up.y * 30f;
                    StartCoroutine(jumpPad(5f));
                    StartCoroutine(FreezeMovement(.5f));
                }
            } else
            {
                isGrounded = false;  
            }
        }

        

        //Check player is on wall 
        private void checkWall()
        {
            RaycastHit hit;
            

            if (Physics.SphereCast(wallChecker.position, minWallDistance, move.normalized, out hit, maxWallDistance))
            {
                //If detects a player on collision range ignores
                if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 10) return;

                bool WallMaterial = hit.collider.GetComponent<Renderer>().material.name.Contains("Wall");

                if (WallMaterial)
                {
                    currentWall = hit.collider.name;
                    isOnWall = true;
                    wallDetector = hit;
                    return;
                }
            }
            isOnWall = false;

        }

        #region Enums

        private IEnumerator jumpPad(float time)
        {
            float timeElapsed = 0f;
        
            while (timeElapsed < time)
            {
            
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            

        }

        //Freeze controls for a certain of time
        private IEnumerator FreezeCameraMovement(float time)
        {
            allowCameraUpdate = false;
            float timeElapsed = 0f;
            while (timeElapsed < time)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            allowCameraUpdate = true;
        }

        //Freeze controls for a certain of time
        private IEnumerator FreezeMovement(float time)
        {
            allowMovementUpdate = false;
            float timeElapsed = 0f;
            while (timeElapsed < time)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            allowMovementUpdate = true;
        }
        #endregion
    }
}
