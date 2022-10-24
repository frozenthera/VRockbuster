using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] private int puck_Damage = 5;
    private Rigidbody rigid;
    private Collider coll;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        //StartCoroutine(InitialRetreive());
        //rigid.AddForce(Vector3.right * 30f, ForceMode.Impulse);
    }

    private IEnumerator InitialRetreive()
    {
        float time = 0f;
        while(time < 5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        if(rigid.velocity.sqrMagnitude == 0) Retrieve();
    }

    public void Hit_By_Player(Vector3 dir)
    {
        rigid.AddForce(new Vector3(dir.x, 0, dir.z), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.CompareTag("Puck"))
        {
            //추가 효과
        }

        if(coll.transform.CompareTag("Block"))
        {
            Block block = coll.transform.GetComponent<Block>();
            block.GetDamage(puck_Damage);
        }

        rigid.velocity = .9f * rigid.velocity;
        if(rigid.velocity.sqrMagnitude <= 9)
        {
            Retrieve();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.CompareTag("Bottom"))
        {
            Retrieve();
        }
    }

    private void Retrieve()
    {
        Debug.Log("retrieved");
        PlayerManager.Instance.GetPuck(1);
        Destroy(gameObject);
    }
}
