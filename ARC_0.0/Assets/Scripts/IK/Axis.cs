using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// To Use Axis, Place component on robot Axis model and assign axis number in inspector
/// as well as the correct "IK_Solver_TCP" instance.
/// </summary>
public class Axis : MonoBehaviour
{
    public GameObject Solver;
    [SerializeField] private int AxisNum;
    private IKSolverBase _ikSolver;
    private Vector3 _axisVal;

    void Start()
    {
        _ikSolver = Solver.GetComponent<IK_Solver_TCP>();
    }

    void Update()
    {
        float val = (float)_ikSolver.GetAxisVal(AxisNum);

        if (!float.IsNaN(val))
        {
            _axisVal.y = val;
            this.transform.localEulerAngles = _axisVal;
        }
    }
}

