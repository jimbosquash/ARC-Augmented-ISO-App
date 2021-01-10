using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

/// <summary>
/// The IK is responsible, for solving the Inverse Kinematics of a robot arm
/// algorithm implemented by Yao Shu Chau and Integrated by James Hayward
/// The Base class implements the "TEMPLATE' pattern to controle calc and 
/// checking, refer to the IKSolverBase for details. if Overriding "Update" or "Start" 
/// method, be sure to call 'Tick()' or "SetUp()"
/// To use, Assign all [SerializeField] properties in the inspector
/// </summary>
public class IK_Solver_TCP : IKSolverBase
{
    //Call TCP postion & orientation from TCPObject.cs
    [SerializeField] private TCPObject TCP_input;

    ///////////////////////// Manualy insert positions in GUI //////////////////////////

    //KUKA_6Axis pivot position
    [SerializeField] private Vector3 pivotA1;
    [SerializeField] private Vector3 pivotA2;
    [SerializeField] private Vector3 pivotA3;
    [SerializeField] private Vector3 pivotA4;
    [SerializeField] private Vector3 pivotA5;
    [SerializeField] private Vector3 pivotA6;

    //Initalizing Gameobject Plane from input Planes
    [SerializeField] private GameObject TCP_GO_plane ;
    [SerializeField] private GameObject TCP_GO_plane0 ;
    [SerializeField] private GameObject TCP_GO_plane00 ;
    [SerializeField] private GameObject WorldPlane_XY;
    [SerializeField] private GameObject WorldXY;
    [SerializeField] private GameObject RobotOrigin;

    ////////////////////////////////////////////////////////////////////////////////////

    private Quaternion m_MyQuaternion;
    private Quaternion m2_MyQuaternion;
    private Quaternion m3_MyQuaternion;

    //Visulazation for real-time 6 Axis changing
    [SerializeField] private double A1;
    [SerializeField] private double A2;
    [SerializeField] private double A3;
    [SerializeField] private double A4;
    [SerializeField] private double A5;
    [SerializeField] private double A6;

    
    ////////////////////////////////////Temp Lists//////////////////////////////////////
    List<bool> AxisToggles = new List<bool>();
    private List<double> AxisValues = new List<double>();
    ////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Returns the axis val assuming that the user inputs the axis number and not the list index
    /// </summary>
    public override double GetAxisVal(int index)
    {
        if (index > 0)
            index = index - 1;
        return AxisValues[index];
    }

    public override TCPObject GetTCPGoal()
    {
        return TCP_input;
    }

    protected override void SetUp()
    {
        AxisToggles.Add(false);
        AxisToggles.Add(false);
        AxisToggles.Add(false);
        pivotA1 += RobotOrigin.transform.position * 1000;
        pivotA2 += RobotOrigin.transform.position * 1000;
        pivotA3 += RobotOrigin.transform.position * 1000;
        pivotA4 += RobotOrigin.transform.position * 1000;
        pivotA5 += RobotOrigin.transform.position * 1000;
        pivotA6 += RobotOrigin.transform.position * 1000;
    }

    protected override void Calculate()
    {
        Vector3 WorldVec_XAxis = new Vector3(1, 0, 0);
        Vector3 WorldVec_ZAxis = new Vector3(0, 0, 1);
        Vector3 WorldVec_YAxis = new Vector3(0, 1, 0);

        float num1 = Vector3.Distance(pivotA2, pivotA3);
        float num2 = Vector3.Distance(pivotA3, pivotA5);
        float num3 = Vector3.Distance(pivotA5, pivotA6);

        TCP_GO_plane.transform.position = TCP_input.transform.position * 1000;
        TCP_GO_plane.transform.rotation = TCP_input.transform.rotation;

        TCP_GO_plane.transform.localScale = new Vector3(100, 100, 100);

        double degree1 = ToDegree(GetAngleBetweenTwoVectors(pivotA2 - pivotA3, pivotA5 - pivotA3));

        TCP_GO_plane.transform.position -= TCP_GO_plane.transform.up * num3;

        Vector3 b1 = new Vector3(TCP_GO_plane.transform.position.x - pivotA1.x, 0.0f, TCP_GO_plane.transform.position.z - pivotA1.z);

        A1 = ToDegree(GetAngleBetweenTwoVectors(WorldVec_XAxis, b1, -WorldVec_YAxis, false));

        TCP_GO_plane.transform.RotateAround(pivotA1, WorldVec_YAxis, -(float)A1);

        Vector3 b2 = TCP_GO_plane.transform.position - pivotA2;
        double length = b2.magnitude;
        double d = Math.Acos((num1 * num1 + length * length - num2 * num2) / (2.0 * num1 * length));

        A2 = !AxisToggles[0] ? (!AxisToggles[1] ? -ToDegree(d + GetAngleBetweenTwoVectors(WorldVec_XAxis, b2, -WorldVec_ZAxis, false)) : -ToDegree(-d + GetAngleBetweenTwoVectors(WorldVec_XAxis, b2, -WorldVec_ZAxis, false))) :
            (!AxisToggles[1] ? -ToDegree(-d + GetAngleBetweenTwoVectors(WorldVec_XAxis, b2, -WorldVec_ZAxis, false)) : -ToDegree(d + GetAngleBetweenTwoVectors(WorldVec_XAxis, b2, -WorldVec_ZAxis, false)));
        A3 = !AxisToggles[0] ? (!AxisToggles[1] ? 90.0 + degree1 - ToDegree(Math.Acos((num1 * num1 + num2 * num2 - length * length) / (2.0 * num1 * num2))) : 90.0 + degree1 + ToDegree(Math.Acos((num1 * num1 + num2 * num2 - length * length) / (2.0 * num1 * num2)))) :
            (!AxisToggles[1] ? 90.0 + degree1 + ToDegree(Math.Acos((num1 * num1 + num2 * num2 - length * length) / (2.0 * num1 * num2))) : 90.0 + degree1 - ToDegree(Math.Acos((num1 * num1 + num2 * num2 - length * length) / (2.0 * num1 * num2))));

        Vector3 b3 = pivotA5 - pivotA4;
        b3 = Vector3.Normalize(b3);

        TCP_GO_plane0.transform.position = pivotA5;
        TCP_GO_plane0.transform.right = Vector3.Cross(b3, WorldVec_ZAxis);

        if (!float.IsNaN((float)A2) && !float.IsNaN((float)A3))
        {
            TCP_GO_plane0.transform.RotateAround(pivotA3, WorldVec_ZAxis, -(float)(A3 - 90));
            TCP_GO_plane0.transform.RotateAround(pivotA2, WorldVec_ZAxis, -(float)(A2 + 90.0));
        }

        /////////////////////// Rhinocommon => Transform.PlaneToPlane///////////////////////////////////
        TCP_GO_plane.transform.position = WorldPlane_XY.transform.position;
        TCP_GO_plane00.transform.rotation = TCP_GO_plane0.transform.rotation;
        TCP_GO_plane00.transform.position = WorldPlane_XY.transform.position;

        Quaternion z_localRot = Quaternion.FromToRotation(TCP_GO_plane0.transform.forward, WorldVec_ZAxis);

        TCP_GO_plane00.transform.rotation = z_localRot * TCP_GO_plane00.transform.rotation;
        TCP_GO_plane.transform.rotation = z_localRot * TCP_GO_plane.transform.rotation;
        Quaternion x_localRot = Quaternion.FromToRotation(TCP_GO_plane00.transform.right, WorldVec_XAxis);
        TCP_GO_plane.transform.rotation = x_localRot * TCP_GO_plane.transform.rotation;
        ////////////////////////////////////////////////////////////////////////////////////////////////

        Vector3 zaxis = TCP_GO_plane.transform.up;
        float x = zaxis.x;
        float z = zaxis.z;
        double degree2 = ToDegree(GetAngleBetweenTwoVectors(new Vector3(x, 0.0f, z), WorldVec_XAxis));

        zaxis = TCP_GO_plane.transform.up;
        A4 = degree2;
        if (zaxis.z > 0.0)
            A4 = -A4;

        A5 = ToDegree(GetAngleBetweenTwoVectors(TCP_GO_plane.transform.up, WorldVec_YAxis));

        WorldXY.transform.position = WorldPlane_XY.transform.position;
        WorldXY.transform.right = WorldPlane_XY.transform.forward;
        WorldXY.transform.forward = -WorldPlane_XY.transform.right;

        WorldXY.transform.RotateAround(WorldXY.transform.position, WorldVec_YAxis, -(float)(-A4));
        WorldXY.transform.RotateAround(WorldXY.transform.position, WorldXY.transform.forward, -(float)A5);

        A6 = -ToDegree(GetAngleBetweenTwoVectors(WorldXY.transform.right, TCP_GO_plane.transform.right, WorldXY.transform.up, true));

    }

    protected override void CheckAxisLimits()
    {
        // axis limits for different types of robots

        if (A1 < -185 || A1 > 185 || float.IsNaN((float)A1))
        {
            Debug.Log("A1 out of reach");
        }
        if (A2 < -170 || A2 > 60 || float.IsNaN((float)A2))
        {
            Debug.Log("A2 out of reach");
        }
        if (A3 < -120 || A3 > 165 || float.IsNaN((float)A3))
        {
            Debug.Log("A3 out of reach");
        }
        if (A4 < -180 || A4 > 180 || float.IsNaN((float)A4))
        {
            Debug.Log("A4 out of reach");
        }
        if (A5 < -125 || A5 > 125 || float.IsNaN((float)A5))
        {
            Debug.Log("A5 out of reach");
        }
    }

    protected override void SetNewValues()
    {
        AxisValues.Clear();

        AxisValues.Add(A1);
        AxisValues.Add(A2);
        AxisValues.Add(A3);
        AxisValues.Add(A4);
        AxisValues.Add(A5);
        AxisValues.Add(A6);

        for (int index = 0; index < AxisValues.Count; index++)
        {
            if (AxisValues[index] > 180.0)
                AxisValues[index] -= 360.0;
            else if (AxisValues[index] < -180.0)
                AxisValues[index] += 360.0;
        }
    }

    public static double GetAngleBetweenTwoVectors(Vector3 a,Vector3 b,Vector3 z,bool positive = true)
    {
        double d = (a.x * b.x + a.y * b.y + a.z * b.z) / (a.magnitude * b.magnitude);
        if (d > 1.0)
            d = 1.0;
        else if (d < -1.0)
            d = -1.0;
        double num = Math.Acos(d);
        Vector3 cross = Vector3.Cross(b, a);

        if (Vector3.Dot(cross, z) < 0.0)
            num = !positive ? -num : 2.0 * Math.PI - num;
        return num;
    }
    
    public static double GetAngleBetweenTwoVectors(Vector3 a, Vector3 b)
    {
        double d = (a.x * b.x + a.y * b.y + a.z * b.z) / (a.magnitude * b.magnitude);
        if (d > 1.0)
            d = 1.0;
        else if (d < -1.0)
            d = -1.0;
        return Math.Acos(d);
    }

    public static double ToDegree(double radian)
    {
        return radian * (180.0 / Math.PI);
    }

    public static double ToRadian(double degree)
    {
        return degree * (Math.PI / 180.0);
    }


}
