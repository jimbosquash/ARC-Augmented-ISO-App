using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Base class for Detailed InverseKinimatics implementation
/// Implementing, 'TEMPLATE' pattern to control for correct 
/// sequential running including checking.
/// </summary>
public abstract class IKSolverBase : MonoBehaviour
{
    public abstract double GetAxisVal(int index);

    public abstract TCPObject GetTCPGoal();

    protected abstract void SetUp();
    protected abstract void Calculate();
    protected abstract void CheckAxisLimits();
    protected abstract void SetNewValues();

    protected void Start()
    {
        SetUp();
    }

    protected void Update()
    {
        Tick();
    }

    protected void Tick()
    {
        CheckAxisLimits();
        Calculate();
        SetNewValues();
    }
}

