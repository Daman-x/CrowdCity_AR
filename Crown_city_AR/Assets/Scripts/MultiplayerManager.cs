
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


// Used to manager multiplayer behaviour like joining random rooms
public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text warning;

    private string _roomname;

    #region unity methods
    // Start is called before the first frame update
    void Start()
    {
        warning.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion


    #region User define methods

    // used to join room called from Uimethods class
    public void JoiningRooms()
    {

        PhotonNetwork.JoinRandomRoom(); // used to join random rooms 
    }

    // used to create a room
    void CreateRoom()
    {
        _roomname = "Room " + Random.Range(1, 100); // generate random room no.
        RoomOptions roomOptions = new RoomOptions(); // object of class roomoptions to specific room properties

        roomOptions.MaxPlayers = 6; // max player in a room 6 // can be change later
                                    //  roomOptions.EmptyRoomTtl = 60000; // room will stay active for 1 min after all the players leave
        roomOptions.CleanupCacheOnLeave = false; // dont destroy object if player leaves

        PhotonNetwork.CreateRoom(_roomname, roomOptions); // create a room with room options
    }


    #endregion

    #region photo callback methods

    public override void OnCreateRoomFailed(short returnCode, string message) // called if creating room fails
    {
        warning.text = "Unable To Create A Room ,Try Again Later";
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        if (PhotonNetwork.CountOfRooms == 0) // if no rooms in server
            CreateRoom(); // call create room method
        else
            warning.text = " Failed To Join Due to " + message;
    }
  
    public override void OnJoinedRoom() // called when room is joined
    {
        warning.text = "Joined To " + PhotonNetwork.CurrentRoom.Name + " Waiting for other players";
        
    }
    
    #endregion
}
