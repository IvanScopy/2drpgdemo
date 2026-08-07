using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private float cooldown=2f;
    [Space] 
    [SerializeField] private float cooldowndecreaterate = .05f;
    [SerializeField] private float cooldowncap = .7f;
    private float timer;
    
    private Transform player;

    private void Awake()
    {
           Player foundPlayer = FindAnyObjectByType<Player>();
           if (foundPlayer != null)
               player = foundPlayer.transform;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();
            
            cooldown = Mathf.Max(cooldowncap, cooldown - cooldowndecreaterate);
        }
    }

    private void CreateNewEnemy()
    {
        if (player == null)
            return;
        if (spawnPoints.Length == 0)
            return;
        int respawnPointIndex = Random.Range(0, spawnPoints.Length);
        
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoints[respawnPointIndex].position, Quaternion.identity);
        
        bool createOnTheRight = newEnemy.transform.position.x > player.position.x;
        if (createOnTheRight)
        {
            newEnemy.GetComponent<Enemy>().Flip();
        }
    }
}
