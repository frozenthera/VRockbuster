using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RightHand : MonoBehaviour
{
    public Handle handle;               // control handle
    public GameObject boardPlane;       // to get normal
    public LayerMask layermask;         // get plane layer 

    public SteamVR_Action_Boolean Grab;

    void FixedUpdate()
    {
        HandleControlUpdate();
    }


    void HandleControlUpdate(){
        // grab && plane 위에 있을때 평면에 붙어 있는 핸들을 움직일 수 있다.
        // 그 외 --> 마지막으로 있던 위치에 그대로 있음.

        bool GrabBool = Grab.GetState(SteamVR_Input_Sources.RightHand);

        RaycastHit hit;
        Vector3 normal = new Vector3(0,0,0);
        normal = boardPlane.transform.up;

        if(Physics.Raycast(transform.position, -normal, out hit, Mathf.Infinity, layermask)){
            if(GrabBool){
                Debug.DrawRay(transform.position, -normal * hit.distance, Color.red);
                handle.movePos(hit.point);
            }
        }else{
            Debug.DrawRay(transform.position, -normal * 1000f, Color.red);
        }

    }
}
