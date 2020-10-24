using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ArScalingManager : MonoBehaviour
{
    ARSessionOrigin aRSessionOrigin;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        aRSessionOrigin = GetComponent<ARSessionOrigin>();
        slider.onValueChanged.AddListener(OnSliderMove);
    }

    #region user define methods
    void OnSliderMove(float value)
  {
        if(slider != null)
            aRSessionOrigin.transform.localScale = Vector3.one / value;
  }
    #endregion
}
