using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Responsible for defining user interaction with simulation implimentation
/// </summary>
public abstract class ToolPathSimulatorBase : MonoBehaviour
{
    public UnityEvent ToolPathSimulating; // used for updating GUI

    public abstract void Execute();
    protected abstract void ContinueSimulation();
    public abstract void ResetSimulation();

    public abstract float TotalTravelDistance{ get; }
    public abstract float DistanceTraveled { get; }
    public abstract float MovementSpeed { get; set; }
    


}

