using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;
    public GameObject targetObject;
    public float verticalSpread,horizontalSpread,spawnTime,snakeSpeed;
    public int snakeAmount = 10;
    private float spawnTimeCounter = 0f;
    private int activeSnakeIndex = 0;
    private GameObject[] prefabArray;
    
    // Start is called before the first frame update
    void Start()
    {
        prefabArray = new GameObject[snakeAmount];
        for(int i=0;i<prefabArray.Length;i++) {
            prefabArray[i]=Instantiate(snakePrefab,transform.position,Quaternion.identity);
        }
        foreach (var snake in prefabArray)
        {
            snake.GetComponent<SnakeMovement>().SetSpeed(snakeSpeed);
        }
        spawnTimeCounter=spawnTime;
    }

    // Update is called once per frame
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
        targetHorizontal = transform.forward * Random.Range(-horizontalSpread,horizontalSpread);
        targetVertical = transform.up * Random.Range(-verticalSpread,verticalSpread);
        Vector3 tempTargetPosition = targetObject.transform.position+targetHorizontal+targetVertical;
        prefabArray[activeSnakeIndex].transform.position=transform.position;
        prefabArray[activeSnakeIndex].transform.LookAt(tempTargetPosition);
        activeSnakeIndex++;
        if(activeSnakeIndex>=prefabArray.Length) {
            activeSnakeIndex=0;
        }
    }
}
