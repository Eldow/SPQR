using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {

    void Update()
    {
        float aimUpDown = Input.GetAxis("Mouse Y");
        float aimLeftRight = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.right * -aimUpDown);
        transform.Rotate(Vector3.down * -aimLeftRight);
    }
}
