using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Used to synchronize the movement across all the client ..( Custom script )
public class CustomNetworkSynchronzation : MonoBehaviour , IPunObservable
{

    CharacterController characterController;
    PhotonView PhotonView;
   
    Vector3 networkposition;
    Quaternion networkrotation;
    GameObject Platform;

   // public float MovementSpeed = 2f;

    Vector3 velocity;
    Vector3 NetworkVelocity;

    #region unity methods

    // called before start method
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PhotonView = GetComponent<PhotonView>();
        Platform = GameObject.Find("Platform");
    }
    // Update is called once per frame
    // used to change the position of remote player over network
    void Update()
    {
        Vector3 old_position = characterController.transform.position;
        velocity = characterController.transform.position - old_position;

        if(!PhotonView.IsMine) // if gameobject is the current user
        {
            characterController.transform.position = Vector3.MoveTowards(characterController.transform.position, networkposition ,Time.deltaTime);
            characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation , networkrotation, Time.deltaTime * 720f );
        }
    }

    #endregion

    #region photon methods

    // called by photon view
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if playeris mine and moves
        if(stream.IsWriting)
        {
            stream.SendNext(characterController.transform.position - Platform.transform.position);
            stream.SendNext(characterController.transform.rotation);
            stream.SendNext(velocity);
              
        }
        else // remote player so they will receive data
        {
            networkposition = (Vector3)stream.ReceiveNext() + Platform.transform.position;
            networkrotation = (Quaternion)stream.ReceiveNext();     
            
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)); // caculating lag 
            NetworkVelocity = (Vector3)stream.ReceiveNext();
            networkposition += NetworkVelocity * lag; 
                 
        }
    }

    #endregion
}
