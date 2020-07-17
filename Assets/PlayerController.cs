using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform rightRayCastOrigin;
    [SerializeField] Transform leftRayCastOrigin;

    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;

    [SerializeField] float dist = 3;
    [SerializeField] float height = 6;

    bool rightMoved = false;

    private void Update ()
    {
        RaycastHit hit;
        int layerMask = LayerMask.NameToLayer ("Default");

        Vector3 averagePos = GetPos ();
        averagePos.y = 0f;
        Vector3 thisPos = transform.position;
        thisPos.y = 0;
        float d = Vector3.Distance (averagePos, thisPos);

        if (d > dist)
        {
            if (! rightMoved)
            {
                if (Physics.Raycast (rightRayCastOrigin.position, rightRayCastOrigin.TransformDirection (-Vector3.up), out hit, Mathf.Infinity))
                {
                    Debug.DrawRay (rightRayCastOrigin.position, rightRayCastOrigin.TransformDirection (-Vector3.up) * hit.distance, Color.yellow);
                    rightFoot.position = hit.point;
                    rightMoved = true;
                    updateHeight ();
                }
                else
                {
                    Debug.DrawRay (rightRayCastOrigin.position, rightRayCastOrigin.TransformDirection (-Vector3.up) * 1000, Color.white);
                }
            }
            else
            {
                if (Physics.Raycast (leftRayCastOrigin.position, leftRayCastOrigin.TransformDirection (-Vector3.up), out hit, Mathf.Infinity))
                {
                    Debug.DrawRay (leftRayCastOrigin.position, leftRayCastOrigin.TransformDirection (-Vector3.up) * hit.distance, Color.yellow);
                    leftFoot.position = hit.point;
                    rightMoved = false;
                    updateHeight ();
                }
                else
                {
                    Debug.DrawRay (leftRayCastOrigin.position, leftRayCastOrigin.TransformDirection (-Vector3.up) * 1000, Color.white);
                }
            }
        }
    }

    Vector3 GetPos ()
    {
        Vector3 result = Vector3.zero;
        result = (rightFoot.position + leftFoot.position) / 2f;
        return result;
    }

    void updateHeight ()
    {
        //Vector3 avgPos = GetPos ();
        //float y = avgPos.y + height;
        //Vector3 thisPos = transform.position;
        //thisPos.y = y;
        //transform.position = thisPos;
    }
}
