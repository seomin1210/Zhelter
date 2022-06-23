using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float ry;
    public float rotSpeed = 200f;

    private void Update()
    {
        float my = Input.GetAxis("Mouse Y");

        ry += rotSpeed * my * Time.deltaTime;
        ry = Mathf.Clamp(ry, -70, 70);

        transform.localEulerAngles = new Vector3(-ry, 0, 0);
    }
}
