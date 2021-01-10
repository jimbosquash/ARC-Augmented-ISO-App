using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class TrackedImageInfo : MonoBehaviour
{
    public ARTrackedImageManager _TrackedImageManager;
    public Text MarkerPos;

    void Update()
    {
            foreach (var trackedImage in _TrackedImageManager.trackables)
            {
                Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                          $"{trackedImage.transform.position}");
                MarkerPos.text = "MarkerPos :" + trackedImage.transform.position.ToString();
            }
    }
}

