using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to switch between different panel attached to menu
public class MenuPanelManager : MonoBehaviour 
{
    // Reference to panels and button in the scene
    [Header("Components Needed")]  
    public GameObject main_panel;
    public GameObject settings_panel;
    public Button switcher;
    [Header("Sounds")]
    public AudioSource select;

    // Used to set inital main panel active and secondary panel deactive
     void Start() 
    {
        main_panel.SetActive(true);
        settings_panel.SetActive(false);
    }

    #region unity ui functions
    // When setting button is clicked this function will run and switch panel
    public void OnSettingsClicked()
    {
        if(Values.MusicOn == true)
        { select.Play(); }

        if (main_panel.activeSelf == true)
        {
            switcher.GetComponentInChildren<Text>().text = "Back";
            settings_panel.SetActive(true);
            main_panel.SetActive(false);
        }
        else
        {
            switcher.GetComponentInChildren<Text>().text = "Settings";
            settings_panel.SetActive(false);
            main_panel.SetActive(true);
        }
    }
    #endregion
}
