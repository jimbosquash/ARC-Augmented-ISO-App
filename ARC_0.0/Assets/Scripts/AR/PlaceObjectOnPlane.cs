using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _RaycastManager;
    [SerializeField] private GameObject _objectToPlace;


    private static List<ARRaycastHit> Hits = new List<ARRaycastHit>();


    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (_RaycastManager.Raycast(touch.position, Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = Hits[0].pose;

                    Instantiate(_objectToPlace, hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}
