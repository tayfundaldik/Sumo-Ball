using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject [] enemyPrefab;
    public GameObject[] powerupPrefabs;
    private int enemyCount;
    private int waveNumber;
    private float spawnRange = 9;
    // Start is called before the first frame update
    void Start()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GeneratePosition(),powerupPrefabs[randomPowerup].transform.rotation);
        SpawnEnemyWave(waveNumber);
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            int randomPowerup = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], GeneratePosition(), powerupPrefabs[randomPowerup].transform.rotation);

        }
    }
    void SpawnEnemyWave(int enemies){

        for(int i = 0 ; i<enemies ; i++){
        int randomIndex = Random.Range(0,enemyPrefab.Length);
        Instantiate(enemyPrefab[randomIndex],GeneratePosition(), enemyPrefab[randomIndex].transform.rotation);
        }
    }
    private Vector3 GeneratePosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
