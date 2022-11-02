using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPhysics : MonoBehaviour
{
    private Rigidbody rigid;
    private Puck myPuck;

    private Vector3 lastFrameVelocity;
    private float minVelocity = 0f;
    private float maxVelocity = 2f;

    private void Start()
    {
        myPuck = GetComponent<Puck>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!myPuck.isFirstSpawned) rigid.velocity += Time.deltaTime * Vector3.forward * -1f;
        lastFrameVelocity = rigid.velocity;
        rigid.velocity = rigid.velocity.normalized * Mathf.Min(maxVelocity, rigid.velocity.magnitude);
    }

    private void AddVelocity(float horizontal, float vertical)
    {
        rigid.velocity += new Vector3(horizontal, 0, vertical);
    }

    private void OnCollisionEnter(Collision coll)
    {
        Bounce(coll.contacts[0].normal);

        if(coll.transform.CompareTag("Puck"))
        {
            //추가 효과
        }

        if(coll.transform.CompareTag("Block"))
        {
            Block block = coll.transform.GetComponent<Block>();    
            if(coll.transform.position.z > transform.position.z)
            {
                AddVelocity(0, -block.Block_speed - .01f);
            }
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if(coll.transform.CompareTag("Block"))
        {
            Block block = coll.transform.GetComponent<Block>();
            block.GetDamage(myPuck.Puck_damage * (int)myPuck.powerMultiplier);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (rigid.velocity.magnitude < 1f) myPuck.Retrieve();
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        GetComponent<Rigidbody>().velocity = direction * Mathf.Max(speed, minVelocity);
        myPuck.AddBounce();
    }
}