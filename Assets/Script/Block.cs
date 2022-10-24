using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected int block_HP;
    [SerializeField] protected int block_score;
    [SerializeField] protected int block_damage;
    [SerializeField] protected float block_speed;

    private void Update() 
    {
        transform.position += Vector3.back * block_speed * Time.deltaTime;
    }

    public Block(int _block_HP, int _block_score, int _block_damage)
    {
        block_HP = _block_HP;
        block_score = _block_score;
        block_damage = _block_damage;
    }

    public void GetDamage(int damage)
    {
        block_HP -= damage;
        Debug.Log(damage + ", " +  block_HP);
        if(block_HP <= 0) Destroy_By_Player();
    }

    public void Destory_By_Descend()
    {
        PlayerManager.Instance.GetDamage(block_damage);
        Destroy(gameObject);
    }

    public void Destroy_By_Player()
    {
        GameManager.Instance.GetScore(block_score);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.CompareTag("Bottom"))
        {
            Destory_By_Descend();
        }
        
    }
}
