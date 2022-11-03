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

    [SerializeField] private int explosionDamage = 5;
    [SerializeField] private GameObject explosionEffect;
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
            //explooooooooooooooooosion
            // only lower (smaller z) puck triggers explooooooooooooooosion
            if (this.transform.position.z < coll.transform.position.z)
            {
                float expRadius = .5f;     // radius of explosion ( collision detection area )
                int expLayer = 1 << 10;     // block : 10 layer ==> 10000_0000_00
                Collider[] expcoll = Physics.OverlapSphere(coll.contacts[0].point, expRadius, expLayer);

                foreach (Collider _expcoll in expcoll)
                {
                    _expcoll.GetComponent<Block>().GetDamage(explosionDamage);
                    Debug.Log("block damaged by explosion : " + _expcoll.GetComponent<Block>());
                }

                Instantiate(explosionEffect, coll.contacts[0].point, Quaternion.identity);
                Debug.Log("puck EXPLOSION!!");
                Debug.Log(expcoll.Length);
            }

            myPuck.Retrieve();
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

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        GetComponent<Rigidbody>().velocity = direction * Mathf.Max(speed, minVelocity);
        myPuck.AddBounce();
    }
}