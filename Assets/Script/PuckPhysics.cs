using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPhysics : MonoBehaviour
{
    private Rigidbody rigid;
    private Puck myPuck;

    private Vector3 lastFrameVelocity;
    private float minVelocity = 1f;
    private float maxVelocity = 5f;

    private void Start()
    {
        myPuck = GetComponent<Puck>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rigid.velocity += Time.deltaTime * Vector3.forward * -0.0981f * 5f;
        lastFrameVelocity = GetComponent<Rigidbody>().velocity;
    }

    private void AddVelocity(float horizontal, float vertical)
    {
        rigid.velocity += new Vector3(horizontal, 0, vertical);
        rigid.velocity = rigid.velocity.normalized * Mathf.Min(maxVelocity, rigid.velocity.magnitude);
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
                AddVelocity(0, -block.Block_speed);
            }
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if(coll.transform.CompareTag("Block"))
        {
            Block block = coll.transform.GetComponent<Block>();
            block.GetDamage(myPuck.Puck_damage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (rigid.velocity.magnitude < 1f) myPuck.Retrieve();
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        GetComponent<Rigidbody>().velocity = direction * Mathf.Max(speed, minVelocity);
    }
}