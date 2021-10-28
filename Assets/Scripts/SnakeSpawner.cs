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
    
    // Start is called before the first frame update
    void Start()
    {
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

    /*GameObject[] GetJointsWithinCone(Transform coneSource, float coneSpreadAngle) {
        GameObject[] joints = GetAllSnakesJoints();
        GameObject[] targetedJoints;
        int returnArrLen = 0;
        for(int i=0;i<joints.Length;i++) {
            if(Vector3.Angle(joints[i].transform.position-coneSource.position,coneSource.transform.forward)>coneSpreadAngle) {
                joints[i]=null;
            } else {
                returnArrLen++;
            }
        }
        targetedJoints = new GameObject[returnArrLen];
        int indexCounter=0;
        foreach (GameObject joint in joints)
        {
            if(joint!=null) {
                targetedJoints[indexCounter] = joint;
                indexCounter++;
            }
        }
        return targetedJoints;
    }

    public GameObject[] GetAllSnakesJoints() {
        GameObject[] tempJointArray;
        int tempJointArrLen = 0;
        int[] tempPerSnakeJointArrLen = new int[snakeAmount];
        //Count number of joints for instatiating the return array
        for(int i=0;i<snakeAmount;i++) {
            tempPerSnakeJointArrLen[i]=snakeArray[i].GetComponent<SnakeLoader>().GetJointLength();
            tempJointArrLen+=tempPerSnakeJointArrLen[i];
        }
        tempJointArray = new GameObject[tempJointArrLen];
        int indexCounter = 0;
        for(int i=0;i<snakeAmount;i++) {
            for(int j=0;j<snakeArray[i].GetComponent<SnakeLoader>().GetJointLength();j++) {
                tempJointArray[indexCounter] = snakeArray[i].GetComponent<SnakeLoader>().GetJoint(j);
                indexCounter++;
            }
        }
        return tempJointArray;
    }*/
}
