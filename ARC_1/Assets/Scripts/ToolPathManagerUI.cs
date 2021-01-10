using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for GUI to add/ edit toolPathManager List.
/// Responsible for Updating and managing Line Renderer.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
class ToolPathManagerUI : MonoBehaviour
{
    [SerializeField] private ToolPathManager _toolPathManager;
    [SerializeField] private Button SaveTCPButton;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector3[] _lrPositions;

    private void Start()
    {
        SaveTCPButton.onClick.AddListener(_toolPathManager.AddTCPPosition);
        _lineRenderer = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _toolPathManager.GetPathPts().Count;
        _lrPositions = new Vector3[_lineRenderer.positionCount];
        for (int i = 0; i < _lrPositions.Length; i++)
        {
            _lrPositions[i] = _toolPathManager.GetPathPts()[i].transform.position;

        }
        _lineRenderer.SetPositions(_lrPositions);
    }
}

