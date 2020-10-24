using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

// Used to Spawn A.I bodies in the scene
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Platform;
    [SerializeField] GameObject[] Spawnpoints;  // spawing points
    [SerializeField] GameObject objects; // prefab to spawn
     private float _timer = 0;

    public enum RaiseEvent
    {
        ObjectSpawnCode = 1
    }

    #region unity methods
    void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    // Update is called once per frame
    void Update()
    {

        if(_timer > 5f && Values.MATCHISON == true && PhotonNetwork.IsMasterClient)
        {
            _timer = 0;
            // int value = Random.Range(0,Spawnpoints.Length-1); // gives random value between 1 to 4
            // PhotonNetwork.InstantiateRoomObject(objects.name, Spawnpoints[value].transform.position, Quaternion.identity);
            Sppawner();
        }
        _timer += Time.deltaTime;
    }
    // when destroy
    private void OnDestroy()
    {
       PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    #endregion

    #region photon Event
    // CUSTOM ISTANTIATE SCRIPT
    void Sppawner()
    {
        int random_spawn = Random.Range(0, Spawnpoints.Length - 1); // random positions
        Vector3 position_to_spawn = Spawnpoints[random_spawn].transform.position; // get the spawn points position
        GameObject Player = Instantiate(objects, position_to_spawn, Quaternion.identity); // instanitate gameobject
        PhotonView photonView = Player.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView))
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

            PhotonNetwork.RaiseEvent((byte)RaiseEvent.ObjectSpawnCode, data, raiseEventOptions, sendOptions);
        }
        else
        {
            Debug.Log("unable to create object");
            Destroy(Player);
        }

    }

    public void OnEvent(EventData photonEvent) // called at recevier side
    {
        if (photonEvent.Code == (byte)RaiseEvent.ObjectSpawnCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 Otherposition = (Vector3)data[0];
            Quaternion OtherRotation = (Quaternion)data[1];

            GameObject OtherPlayer = Instantiate(objects, Otherposition + Platform.transform.position, OtherRotation);
            OtherPlayer.GetComponent<PhotonView>().ViewID = (int)data[2];
        }
    }

    #endregion
}
