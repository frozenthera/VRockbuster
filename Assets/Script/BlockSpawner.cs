using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject block_prefab;
    [SerializeField] private float blockOffset = 0.2f;
    private int col = 6;
    [SerializeField] private float blockToSpawn = 0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float time = 0f;
        while(time < 6f)
        {
            yield return null;
            time += Time.deltaTime;
            blockToSpawn += GameManager.Instance.spawnRate * Time.deltaTime;
        }
        int toSpawn = (int)blockToSpawn;
        blockToSpawn -= toSpawn;
        SpawnBlock(toSpawn);
        StartCoroutine(SpawnRoutine());
    }
    
    private void SpawnBlock(int n)
    {
        bool[] dataSet= new bool[6] {false, false , false , false , false , false};
        for(int i=0; i<n; i++)
        {
            int rand = Random.Range(1, col);
            if(!dataSet[rand])
            {
                dataSet[rand] = !dataSet[rand];
                i--;
                continue;
            }
            Block block = Instantiate(block_prefab, new Vector3(colNumToXcord(rand), .025f, 1.55f), Quaternion.identity).GetComponent<Block>();
            block.block_HP = Random.Range(1, 10);
        }     
    }

    private float colNumToXcord(int num)
    {
        return blockOffset * (2 * num - 6);
    }
}
