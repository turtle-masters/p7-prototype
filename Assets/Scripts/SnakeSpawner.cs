using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;
    public GameObject targetObject;
    public float verticalSpread,horizontalSpread,spawnTime,snakeSpeed;
    private float spawnTimeCounter = 0f;
    public float snakeDuration = 0f;
    
    void Start()
    {
        spawnTimeCounter=spawnTime;
    }

    void Update()
    {
        spawnTimeCounter+=Time.deltaTime;
        if(spawnTimeCounter>spawnTime) {
            spawnTimeCounter-=spawnTime;
            SpawnSnake();
        }
    }

    void SpawnSnake() {
        Vector3 targetHorizontal,targetVertical;
        targetHorizontal = transform.right * Random.Range(-horizontalSpread,horizontalSpread);
        targetVertical = transform.up * Random.Range(-verticalSpread,verticalSpread);
        Vector3 tempTargetPosition = targetObject.transform.position+targetHorizontal+targetVertical;
        GameObject newSnake = Instantiate(snakePrefab);
        newSnake.transform.position = transform.position;
        newSnake.transform.LookAt(tempTargetPosition);
        newSnake.GetComponent<SnakeMovement>().SetSpeed(snakeSpeed);
        newSnake.AddComponent<DecayScript>().decayTime=snakeDuration;
        newSnake.GetComponent<SnakeLoader>().SetupSegmentPositions();
    }
}
