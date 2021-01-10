using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// RobotController is responsible for use interaction with each
/// robot instance and represents the robot. It contains robot models
/// To enable the swapping of robot models at runtime. the robot contains 
/// a "ToolPathSimulator" and "ToolPathManager" to control flow of access.
/// </summary>
public class RobotController : MonoBehaviour
{
    // Dependencies
    [SerializeField] private ToolPathSimulatorBase _tooLPathSimulator;
    [SerializeField] private ToolPathManagerBase _tooLPathManager;
    [SerializeField] private TCPObject _tCP;

    // Set in Inspector
    [SerializeField] private List<GameObject> _robots;
    private int _currentRobot;

    public ToolPathManagerBase TPManager
    {
        get { return _tooLPathManager; }
        set { _tooLPathManager = value; }
    }
    public ToolPathSimulatorBase TPSimulator
    {
        get { return _tooLPathSimulator; }
        set { _tooLPathSimulator = value; }
    }


    public void SetRobot(int index)
    {

        if (index >= _robots.Count)
            index = _robots.Count - 1;

        for (int i = 0; i < _robots.Count; i++)
        {
            if (i != index)
                _robots[i].SetActive(false);
            else
                _robots[i].SetActive(true);
        }

    }

    public GameObject GetRobot()
    {
        return _robots[_currentRobot];
    }

    public List<GameObject> GetRobotModels()
    {
        return _robots;
    }

}

