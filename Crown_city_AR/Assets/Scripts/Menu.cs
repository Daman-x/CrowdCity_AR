
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

// Used to connect to photon server after taking player name and other button functions
public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    public InputField player_name;
    public Button play;
    public GameObject warning; // Used to show warnings a text gameobject
    public Image Loader; // used to show the loading
    [Header("Sounds")]
    public AudioSource select;

    #region unity methods and user define methods
    void Start()
    {
        warning.SetActive(false);
        play.enabled = true;
        Loader.fillAmount = 0;
        player_name.text = PlayerPrefs.GetString("name");
    }

    // used to load scene asynchronously
    void LoadSceneAfterDone(string scene)
    {
        var LoadingScene = SceneManager.LoadSceneAsync(scene); // used for loading scene and also aloowing to do some background activity 
        LoadingScene.allowSceneActivation = false;

        while(!LoadingScene.isDone)
        {
            Loader.fillAmount = LoadingScene.progress;  // setting fill amount equal to the amount of loading done
            if (LoadingScene.progress >= 0.9f) // when scene is loaded 90%
            {
                LoadingScene.allowSceneActivation = true;  // allow it to show it to the user
                break;
            }
        }

    }

    #endregion

    #region UI&Network Functions
    
    // Runs when play button is clicked on main_panel
    public  void OnPlayClicked() 
    {
        string name = player_name.text;
        play.enabled = false;

        if (Values.MusicOn == true)
        { select.Play(); }

        if (!string.IsNullOrEmpty(name)) // if input field is not empty
        {
            PlayerPrefs.SetString("name", name);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = name;
                PhotonNetwork.ConnectUsingSettings(); // Connecting to photon servers
            }
           
        }
        else 
        {
            warning.SetActive(true);
        }

    }


    #endregion

    #region PUN Callbacks

    public override void OnConnected() // when connected to internet
    {
        Debug.Log("connected on internet");
    }

    public override void OnDisconnected(DisconnectCause cause) // if disconnect from servers
    {
        Debug.Log("disconnected" + cause);
    }

    public override void OnConnectedToMaster() // connected to photon servers
    {
        Debug.Log("connected to servers");
        LoadSceneAfterDone("Main Scene");  // loading main scene after connecting to main servers
    }
    #endregion
}
