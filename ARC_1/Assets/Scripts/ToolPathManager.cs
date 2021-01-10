using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ToolPathManager is a storage container for ToolPath Points
/// and store the prefab of the Toolpath Point Objects. 
/// It should exist as a component on a robot Game object
/// with a controller
/// </summary>
public class ToolPathManager : ToolPathManagerBase
{
    [SerializeField] private List<GameObject> _toolPathPts;
    [SerializeField] private GameObject _pathPtPrefab;
    [SerializeField] private TCPObject _TCPGoal;
    [SerializeField] private float _totalPathLength = 0.0f;

    public override List<GameObject> GetPathPts()
    {
        return _toolPathPts;
    }

    public override float GetPathLength()
    {
        UpdatePathLength();
        return _totalPathLength;
    }

    private void Awake()
    {
        _toolPathPts = new List<GameObject>();
        if (_TCPGoal != null)
            _TCPGoal = FindObjectOfType<TCPObject>();

    }

    private void UpdatePathLength()
    {
        _totalPathLength = 0;
        for (int i = 0; i < _toolPathPts.Count - 1; i++)
        {
            _totalPathLength = _totalPathLength + Vector3.Distance(_toolPathPts[i].transform.position, _toolPathPts[i + 1].transform.position);
        }
        Debug.Log("Total Path Length = " + _totalPathLength);
    }

    public override void AddTCPPosition()
    {
        GameObject newPathPt = Instantiate(_pathPtPrefab, _TCPGoal.transform.position, _TCPGoal.transform.rotation,this.transform);
        _toolPathPts.Add(newPathPt);
    }

    public override void AddPointToPath(Transform newPosition)
    {
        GameObject newPathPt = Instantiate(_pathPtPrefab, newPosition.position, newPosition.rotation, this.transform);
        _toolPathPts.Add(newPathPt);
    }
}
