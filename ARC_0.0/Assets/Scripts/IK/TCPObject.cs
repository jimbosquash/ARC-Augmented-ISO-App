using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TCPObject class handles the correct method of working
/// with tcp Poses and is used by the IKSolver classes 
/// and by other robot related classes
/// </summary>
public class TCPObject : MonoBehaviour
{
    public Vector3 TCP;
    public Vector3 TCP_Quaternion;
    public Vector3 normal;
    private Matrix4x4 matrix;
    private Vector3 XAxis;
    private Vector3 YAxis;
    private Vector3 ZAxis;

    void Update()
    {
        normal = this.transform.up;
        SetLocalPosition();
        SetMatrix();
        TCP_Quaternion = this.transform.eulerAngles;

    }

    public void Set(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public Transform Get()
    {
        return transform;
    }

    private void SetLocalPosition()
    {
        TCP.x = this.transform.localPosition.x;
        TCP.y = this.transform.localPosition.y;
        TCP.z = this.transform.localPosition.z;
    }

    private void SetMatrix()
    {
        matrix = this.transform.localToWorldMatrix;
        XAxis.z = matrix.m00;
        XAxis.x = matrix.m10;
        XAxis.y = -matrix.m20;

        ZAxis.z = matrix.m01;
        ZAxis.x = matrix.m11;
        ZAxis.y = -matrix.m21;

        YAxis.z = matrix.m02;
        YAxis.x = matrix.m12;
        YAxis.y = -matrix.m22;
    }
}
