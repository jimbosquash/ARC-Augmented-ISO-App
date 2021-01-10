
using UnityEngine;

/// <summary>
/// Responisble for running and controling 
/// simulation of ToolPathManagers ToolPath List. 
/// </summary>
[RequireComponent(typeof(ToolPathManagerBase))]
public class ToolPathSimulator : ToolPathSimulatorBase
{
    /// Dependencies
    [SerializeField] private ToolPathManagerBase _toolPathManager;
    [SerializeField] private TCPObject _TCPGoal;

    public float MaxSpeed = 2.1f;// Recommended max safety speed
    public float _movementSpeed = 0.4f;// 0.4 = 40cm per sec // m per second

    private bool _simulating = false;
    private int NextPtIndex = 0;

    private float _percentageComplete = 0.0f;
    private float _distanceTraveled = 0.0f;
    private float _totalTravelDistance = 0.0f;
    private float _startTravelDistance = 0;

    private Vector3 _startTCPPosition;
    private Quaternion _startTcpQuaternion;
    private float _timeTakenDuringLerp;
    private float _timeStartedLerping;
    private float _tempNewPercentage;
    
    public override float TotalTravelDistance
    {
        get { return _totalTravelDistance; }
    }
    
    public override float DistanceTraveled
    {
        get { return _distanceTraveled; }
    }
    
    public override float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }
    
    void Awake()
    {
        if (_TCPGoal != null)
            _TCPGoal = FindObjectOfType<TCPObject>();

        if (_toolPathManager != null)
            _toolPathManager = GetComponent<ToolPathManagerBase>();
        Debug.Log(" Tool path manager found : " + _toolPathManager.ToString());
    }
    
    void Update()
    {
        if (_simulating)
            ContinueSimulation();
    }

    public override void Execute()
    {
        _simulating = true;
        _distanceTraveled = 0f;
        _startTravelDistance = Vector3.Distance(_TCPGoal.transform.position, _toolPathManager.GetPathPts()[NextPtIndex].transform.position);
        StartLerp();
    }

    protected override void ContinueSimulation()
    {
        Vector3 tempPos =  new Vector3(_TCPGoal.transform.position.x, _TCPGoal.transform.position.y, _TCPGoal.transform.position.z);
        ContinueLerp();
        _distanceTraveled = _distanceTraveled + Vector3.Distance(_TCPGoal.transform.position ,tempPos);
        _totalTravelDistance = _toolPathManager.GetPathLength() + _startTravelDistance;
        _percentageComplete = _distanceTraveled / _totalTravelDistance;
        ToolPathSimulating?.Invoke();
    }

    public override void ResetSimulation()
    {
        NextPtIndex = 0;
        _TCPGoal.transform.position = _toolPathManager.GetPathPts()[NextPtIndex].transform.position;
        _TCPGoal.transform.rotation = _toolPathManager.GetPathPts()[NextPtIndex].transform.rotation;
    }

    private void StartLerp()
    {
        _startTCPPosition = _TCPGoal.transform.position;
        _startTcpQuaternion = _TCPGoal.transform.rotation;
        _timeTakenDuringLerp = Vector3.Distance(_toolPathManager.GetPathPts()[NextPtIndex].transform.position, _startTCPPosition) / MovementSpeed;
        _timeStartedLerping = Time.time;
    }

    private void ContinueLerp()
    {

        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / _timeTakenDuringLerp;
        Vector3 newPos = (Vector3.Lerp(_startTCPPosition, _toolPathManager.GetPathPts()[NextPtIndex].transform.position, percentageComplete));
        Quaternion newRot = Quaternion.Lerp(_startTcpQuaternion, _toolPathManager.GetPathPts()[NextPtIndex].transform.rotation, percentageComplete);

        _TCPGoal.transform.position = newPos;
        _TCPGoal.transform.rotation = newRot;

        if (percentageComplete >= 1.0f)
        {
            SetNextPt();
        }
    }

    private void SetNextPt()
    {
        NextPtIndex++;
        if (NextPtIndex >= _toolPathManager.GetPathPts().Count)
        {
            _simulating = false;
            NextPtIndex = 0;
        }
        StartLerp();
    }
}
