using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class UIMoveControl : MonoBehaviour
{
    //Variables
    public float speed = 1.0F;
    public float jumpSpeed = 1.0F;
    public float gravity = 0.0F;
    public float yVal = 0;
    private Vector3 moveDirection = Vector3.zero;

    public float xVal;
    public float zVal;

    void Update()
    {
        xVal = Input.GetAxis("Horizontal");
        zVal = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Q))
        {
            yVal = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            yVal = -1;
        }
        else
        {
            yVal = 0;
        }
            moveDirection = new Vector3(xVal, yVal, zVal);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        //controller.Move(moveDirection * Time.deltaTime);
        transform.position += moveDirection * Time.deltaTime;
    }
}