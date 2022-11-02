using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    Vector3 curpos;
    Vector3 oldpos;
    Vector3 velocity;

    public float bouncy_multiplier = .1f;

    [SerializeField] GameObject handleEcho;

    private void FixedUpdate()
    {
        curpos = transform.position;
        velocity = (curpos - oldpos) / Time.unscaledDeltaTime;
        velocity *= bouncy_multiplier;
        //print("velocity : " + velocity);
        oldpos = curpos;
    }

    public void movePos(Vector3 des){
        transform.position = des;
    }


    /*private void OnCollisionEnter(Collision coll){
        if(coll.transform.CompareTag("Puck"))
        {
            GameObject other = coll.gameObject;
            coll.gameObject.GetComponent<Puck>().Hit_By_Player(velocity);
            print("puck collision => velocity : " + velocity);
        }
    }*/

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Puck"))
        {
            GameObject other = coll.gameObject;
            coll.gameObject.GetComponent<Puck>().Hit_By_Player(velocity);
            print("puck collision => velocity : " + velocity);
        }
    }

    public IEnumerator StartEcho()
    {
        while(PlayerManager.Instance.IsBulletTime)
        {
            Instantiate(handleEcho, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
