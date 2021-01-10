using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
/// <summary>
/// A generic implimentation of ARImage tracking to Move Robot TCP 
/// to Tracked Image Pose
/// </summary>
public class ARimag_TCP : MonoBehaviour
{
    public Text MarkerPos;
    public GameObject TCP_Frame;

    private ARTrackedImageManager _arTrackedImageManager;
    private ARTrackedImage arTrackedImage;
    private Transform _arTrackedImageTransform;

    void Awake()
    {
        _arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    public void OnEnable()
    {
        _arTrackedImageManager.enabled = true;
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            Debug.Log(trackedImage.name);
            arTrackedImage = trackedImage;
        }

        foreach (var trackedImage in args.updated)
        {
            Debug.Log(trackedImage.name);
            arTrackedImage = trackedImage;
        }
        foreach (var trackedImage in args.removed)
        {
            Debug.Log(trackedImage.name);
            arTrackedImage = null;
        }
    }

    public void Update()
    {

        if (arTrackedImage != null)
        {
            ARTrackedImage trackedImage;

            _arTrackedImageManager.trackables.TryGetTrackable(arTrackedImage.trackableId, out trackedImage);
            // check if tracking state on
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Debug.Log("ARImage found and tracking");
                Debug.Log($"TCP : Found Image : setting transform : {_arTrackedImageTransform}");
                gameObject.transform.position = arTrackedImage.transform.position;
                gameObject.transform.rotation = arTrackedImage.transform.rotation;
                TCP_Frame.transform.position= arTrackedImage.transform.position;
                TCP_Frame.transform.rotation = arTrackedImage.transform.rotation;
                Debug.Log($"Image: {arTrackedImage.referenceImage.name} is at " + $"{arTrackedImage.transform.position}");
                MarkerPos.text = "TCP: [" + arTrackedImage.transform.position.ToString() + arTrackedImage.transform.rotation.ToString()+"]";
            }
            else
            {
                Debug.Log("TCP : ARImage : not found");
            }
        }
        else
        {
            gameObject.transform.position = Vector3.back;
        }
    }
}
