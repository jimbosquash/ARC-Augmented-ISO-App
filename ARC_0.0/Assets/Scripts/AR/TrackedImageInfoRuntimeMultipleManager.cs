using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoRuntimeMultipleManager : MonoBehaviour
{
    public GameObject LocateRobotPanel;

    [SerializeField] private GameObject _welcomePanel;

    [SerializeField] private Button _dismissButton;

    [SerializeField] private Text _imageTrackedText;

    [SerializeField] private GameObject[] _arObjectsToPlace;

    [SerializeField] private Vector3 _scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);

    private ARTrackedImageManager _TrackedImageManager;

    private Dictionary<string, GameObject> _arObjects = new Dictionary<string, GameObject>();


    void Awake()
    {
        _dismissButton.onClick.AddListener(Dismiss);
        _TrackedImageManager = GetComponent<ARTrackedImageManager>();

        // setup all game objects in dictionary
        foreach (GameObject arObject in _arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            _arObjects.Add(arObject.name, newARObject);
        }
    }

    void OnEnable()
    {
        _TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        _TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void Dismiss() => _welcomePanel.SetActive(false);

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            _arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        _imageTrackedText.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);

        Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    private void AssignGameObject(string name, Vector3 newPosition)
    {
        if (_arObjectsToPlace != null)
        {

            GameObject goARObject = _arObjects[name];

            LocateRobotPanel.SetActive(true);

            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = _scaleFactor;
            foreach (GameObject go in _arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if (go.name != name)
                {

                    go.SetActive(false);
                }
            }
        }
    }
}
