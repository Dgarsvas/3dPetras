using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAroundScript : MonoBehaviour
{
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public KeyCode HoldToLookAroundKeyCode = KeyCode.LeftAlt;

    public KeyCode MoveForwardsKeyCode = KeyCode.W;
    public KeyCode MoveBackwardsKeyCode = KeyCode.S;
    public KeyCode MoveLeftKeyCode = KeyCode.A;
    public KeyCode MoveRightKeyCode = KeyCode.D;

    public Transform parentTransform;

    float rotationX;
    float rotationY;



    Quaternion originalRotation;
    Quaternion parentOriginalRotation;

    public float CameraMoveSpeed = 1;

    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        originalRotation = transform.localRotation;
        parentOriginalRotation = parentTransform.localRotation;
    }

    void Update()
    {
        //Look around
        if (Input.GetKey(HoldToLookAroundKeyCode))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

            transform.localRotation = originalRotation * yQuaternion;
            parentTransform.localRotation = parentOriginalRotation * xQuaternion;
        }

        //Move around
        if (Input.GetKey(MoveForwardsKeyCode))
        {
            if (Input.GetKey(MoveLeftKeyCode))
            {
                MoveForwardsLeft();
            }
            else if (Input.GetKey(MoveRightKeyCode))
            {
                MoveForwardsRight();
            }
            else
            {
                MoveForwards();
            }

        }
        else if (Input.GetKey(MoveBackwardsKeyCode))
        {
            if (Input.GetKey(MoveLeftKeyCode))
            {
                MoveBackwardsLeft();
            }
            else if (Input.GetKey(MoveRightKeyCode))
            {
                MoveBackwardsRight();
            }
            else
            {
                MoveBackwards();
            }
        }
        else if (Input.GetKey(MoveLeftKeyCode))
        {
            MoveLeft();
        }
        else if (Input.GetKey(MoveRightKeyCode))
        {
            MoveRight();
        }
    }

    #region Camera Movement
    private void MoveRight()
    {
        parentTransform.Translate(Vector3.left * -CameraMoveSpeed);
    }

    private void MoveLeft()
    {
        parentTransform.Translate(Vector3.left * CameraMoveSpeed);
    }

    private void MoveBackwards()
    {
        parentTransform.Translate(Vector3.forward * -CameraMoveSpeed);
    }

    private void MoveForwards()
    {
        parentTransform.Translate(Vector3.forward * CameraMoveSpeed);
    }

    private void MoveForwardsLeft()
    {
        parentTransform.Translate(Vector3.forward * CameraMoveSpeed * Mathf.Sin(0.785f));
        parentTransform.Translate(Vector3.left * CameraMoveSpeed * Mathf.Sin(0.785f));
    }

    private void MoveForwardsRight()
    {
        parentTransform.Translate(Vector3.forward * CameraMoveSpeed * Mathf.Sin(0.785f));
        parentTransform.Translate(Vector3.left * -CameraMoveSpeed * Mathf.Sin(0.785f));
    }

    private void MoveBackwardsLeft()
    {
        parentTransform.Translate(Vector3.forward * -CameraMoveSpeed * Mathf.Sin(0.785f));
        parentTransform.Translate(Vector3.left * CameraMoveSpeed * Mathf.Sin(0.785f));
    }

    private void MoveBackwardsRight()
    {
        parentTransform.Translate(Vector3.forward * -CameraMoveSpeed * Mathf.Sin(0.785f));
        parentTransform.Translate(Vector3.left * -CameraMoveSpeed * Mathf.Sin(0.785f));
    }

    #endregion

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}