using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to save data in player prefabs
public class DataSaver : MonoBehaviour
{

    public Toggle music;
    public Toggle graphics;
    private int yes = 1;
    private int no = 0;
    public  static int musicIson = 1 ;
    public static int graphicsIson = 1;
    // Start is called before the first frame update


    private void Awake()
    {
        musicIson = PlayerPrefs.GetInt("music"); // set the previous setting the user set
        graphicsIson = PlayerPrefs.GetInt("graphics");
    }
    void Start()
    {
        music.isOn = Values.MusicOn = musicIson == 1 ? true : false ; // refered to music is on in values script
        graphics.isOn = graphicsIson == 1 ? true : false;
       
    }
    public void musicOn() // if  music toggle is changed
    {
        if(!music.isOn)
        {
            PlayerPrefs.SetInt("music", no);
            Values.MusicOn = false;
        }
        else
        {
            PlayerPrefs.SetInt("music", yes);
            Values.MusicOn = true;
        }
    }


    public void graphicsOn() // if graphics toggle is changed
    {
        if (!graphics.isOn)
        {
            PlayerPrefs.SetInt("graphics", no);
        }
        else
        {
            PlayerPrefs.SetInt("graphics", yes);
        }
    }

    
   
}
