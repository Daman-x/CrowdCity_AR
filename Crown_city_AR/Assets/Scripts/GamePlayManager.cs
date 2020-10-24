using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;
// Used to spawn players in game and other game objects ,basically manage the gameplay
public class GamePlayManager : MonoBehaviourPunCallbacks
{
    [Header("U.I Elements")] // basic u.i elements
    public Text warning;
    public GameObject joystick;
    public GameObject Adjust;
    [Header("Spawn Points")]
    public GameObject[] spawnpoints; // refer to different spwanpoints
    [Header("Prefab To Spawn")]
    public GameObject[] prefabs; // reference to different players

    public enum RaiseEvent
    {
        PlayerSpawnCode = 0
    }

    public GameObject Platform;

    #region unity methods
    // Start is called before the first frame update
    void Start()
    {
        joystick.SetActive(false);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && Values.Started == true)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1) // if in the current player are more or equal to 2
            {
                warning.text = "";
                Values.MATCHISON = true;
              //  PhotonNetwork.CurrentRoom.IsOpen = false; // lock the room so noone can join after after match starts
                GetComponentInParent<PlayerWon>().enabled = true; // enable the playerwon script
            }
            else
            {
              //  Values.MATCHISON = false;
            }
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }


    #endregion

    #region phton callback methods

    public override void OnJoinedRoom() // when room is joined
    {
       if(PhotonNetwork.IsConnectedAndReady) // is photon servers are connected
        {
            joystick.SetActive(true);  // josytick is visible to control
            Adjust.SetActive(false);
            SpawnPlayer();     
        }
       else
        {
            joystick.SetActive(false); // disable joystick
        }
    }
    #endregion


    #region user define methods
    void SpawnPlayer()
    {
        int random_spawn = Random.Range(0, spawnpoints.Length - 1); // random positions
        Vector3 position_to_spawn = spawnpoints[random_spawn].transform.position; // get the spawn points position
        GameObject Player = Instantiate(prefabs[0], position_to_spawn, Quaternion.identity); // instanitate gameobject
        PhotonView photonView = Player.GetComponent<PhotonView>();

        if(PhotonNetwork.AllocateViewID(photonView))
        {

            object[] data = new object[]
            {
                Player.transform.position - Platform.transform.position ,Player.transform.rotation,photonView.ViewID
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };
            SendOptions sendOptions = new SendOptions
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent((byte)RaiseEvent.PlayerSpawnCode,data,raiseEventOptions,sendOptions);
        }
        else
        {
            Debug.Log("unable to create object");
            Destroy(Player);
        }

    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)RaiseEvent.PlayerSpawnCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 Otherposition = (Vector3)data[0];
            Quaternion OtherRotation = (Quaternion)data[1];
           
            GameObject OtherPlayer = Instantiate(prefabs[0], Otherposition + Platform.transform.position, OtherRotation);
            OtherPlayer.GetComponent<PhotonView>().ViewID = (int)data[2];
        }
    }

    #endregion
}
