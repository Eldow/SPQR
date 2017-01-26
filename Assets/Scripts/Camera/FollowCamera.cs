using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject target = null;
    private Pivot pivotPoint = null;
    public float damping = 0.1f;
    Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if(pivotPoint != null)
        {
            float offsetBack = 3;

            transform.rotation = (pivotPoint.transform.rotation);
            transform.position = pivotPoint.transform.position + offsetBack * -transform.forward;
        }

    }

    public void SetTarget(GameObject player)
    {
        target = player;
        pivotPoint = target.GetComponentInChildren<Pivot>();
    }
}
