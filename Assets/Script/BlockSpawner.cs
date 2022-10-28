using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject block_prefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float blockOffset = 0.2f;
    private int col = 6;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float time = 0f;
        while(time < 1 / spawnRate)
        {
            yield return null;
            time += Time.deltaTime;
        }
        SpawnBlock();
        StartCoroutine(SpawnRoutine());
    }
    
    private void SpawnBlock()
    {
        int rand = Random.Range(1, col);
        Instantiate(block_prefab, new Vector3(colNumToXcord(rand), .025f, 1.55f), Quaternion.identity);
    }

    private float colNumToXcord(int num)
    {
        return blockOffset * (2 * num - 6);
    }
}
