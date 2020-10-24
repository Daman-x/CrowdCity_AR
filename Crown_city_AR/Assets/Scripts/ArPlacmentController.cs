using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// used to control to whether to active and disable plane detection to fix the gameobject at one place
public class ArPlacmentController : MonoBehaviour
{
    [Header("U.I buttons")]
    public GameObject place;
    public GameObject adjust;
    public GameObject searchMatch;
    public GameObject slider;

  //  public static bool placed = false;

    ARPlaneManager aRPlaneManager;
    ArPlacementManager arPlacementManager;
    // Start is called before the first frame update
    void Start()
    {
        aRPlaneManager = GetComponent<ARPlaneManager>();
        arPlacementManager = GetComponent<ArPlacementManager>();
        searchMatch.SetActive(false);
        adjust.SetActive(false);
        place.SetActive(true);
        slider.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region user define methods

    // called when place button clicked
    public void OnPlaceButtonClicked()
    {
        DisableorActivePlanes(false);
        searchMatch.SetActive(true);
        adjust.SetActive(true);
        place.SetActive(false);
        slider.SetActive(false);
      //  placed = true;
    }

    // called when adjust button clicked
    public void OnAdjustButtonClicked()
    {
        DisableorActivePlanes(true);
        searchMatch.SetActive(false);
        adjust.SetActive(false);
        place.SetActive(true);
        slider.SetActive(true);
       // placed = false;
    }

    // active and deactive all planes detected by plane manager and also the scripts
    void DisableorActivePlanes(bool value)
    {
        arPlacementManager.enabled = value;
        aRPlaneManager.enabled = value;

        foreach(var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }    

    #endregion
}
