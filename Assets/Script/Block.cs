using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    
    public int block_HP;
    [SerializeField] protected int block_score;
    [SerializeField] protected int block_damage;
    public float Block_speed => block_speed;
    [SerializeField] protected float block_speed;
    protected Material mat;
    [SerializeField] GameObject destroyParticle;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update() 
    {
        transform.position += Vector3.back * block_speed * Time.deltaTime;
        UpdateColor();
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
        Debug.Log(damage);
        if(block_HP <= 0) Destroy_By_Player();
    }

    public void UpdateColor()
    {
        float refNum = ((float)block_HP / 10) * 280;
        mat.color = Color.HSVToRGB(refNum / 360, 1f, 1f);
        //mat.color = new Color(refNum/255, 0, refNum / 255, 1);
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

    private void OnDestroy()
    {
        GameObject go = Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(go, go.GetComponent<ParticleSystem>().main.duration + .4f);
    }
}
