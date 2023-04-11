using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragThreshold : MonoBehaviour {

    private const float inchToCm = 2.54f;
    private EventSystem eventSystem = null;

    [SerializeField]
    private float dragThresholdCM = 0.5f;
    //For drag Threshold

    private void SetDragThreshold()
    {
        eventSystem = EventSystem.current;
        if (eventSystem != null)
            eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
    }


    void Awake()
    {
        SetDragThreshold();
    }
}
