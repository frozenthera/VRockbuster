using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    Vector3 curpos;
    Vector3 oldpos;
    Vector3 velocity;

    public float bouncy_multiplier = 2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        curpos = transform.position;
        velocity = curpos - oldpos;
        velocity.x *= bouncy_multiplier;
        velocity.y = 0;
        velocity.z *= bouncy_multiplier;
        //print("velocity : " + velocity);
        oldpos = curpos;
    }


    public void movePos(Vector3 des){
        transform.position = des;
    }


    private void OnCollisionEnter(Collision coll){
        if(coll.transform.CompareTag("Puck"))
        {
            GameObject other = coll.gameObject;
            coll.gameObject.GetComponent<Puck>().Hit_By_Player(velocity);
            print("puck collision => velocity : " + velocity);
        }
    }

}
