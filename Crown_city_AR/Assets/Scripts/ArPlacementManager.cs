using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// used to place platform on the detected planes
public class ArPlacementManager : MonoBehaviour
{
    List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>(); // hitlist of objects and planes
    ARRaycastManager aRRaycastManager;

    public GameObject Platform;
    public Camera cam;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    // detect and place object in the closest detected plane
    void Update()
    {
        Vector3 screencenter = new Vector3(Screen.width / 2, Screen.height / 2); // center of the screen
        Ray ray = cam.ScreenPointToRay(screencenter);

        if(aRRaycastManager.Raycast(ray,aRRaycastHits,TrackableType.PlaneWithinPolygon))
        {
            Pose positin_to_place = aRRaycastHits[0].pose;

            Platform.transform.position = positin_to_place.position; // placing gameobject at the place of closest raycast hit
        }
    }
}
