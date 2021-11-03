using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerScript : MonoBehaviour
{
    public GameObject spawnObject=null;
    public int objectAmount = 0;
    public float spawnMinRadius = 0f, spawnMaxRadius = 0f;
    public float spawnObjectDistance = 0f;
    public float spawnMinHeight = 0f, spawnMaxHeight = 0f;
    public bool extraSpawn = false;
    public GameObject extraSpawnObject = null;
    public float extraSpawnMinDistance = 0f, extraSpawnMaxDistance = 0f;
    public float extraSpawnMinHeight = 0f, extraSpawnMaxHeight = 0f;
    private GameObject[] spawnObjectArray;


    // Start is called before the first frame update
    void Start()
    {
        /*if(MinigameManagerScript.instance.GetCurrentLevel()==1) {
            extraSpawn = true;
        } else if(MinigameManagerScript.instance.GetCurrentLevel()==3) {
            extraSpawn = false;
        }*/
        spawnObjectArray = new GameObject[objectAmount];
        if(spawnObject!=null && spawnMinRadius<spawnMaxRadius) {
            for(int i=0;i<objectAmount;i++) {
                float spawnX, spawnY, spawnZ;
                Vector3 spawnPosition = Vector3.zero;
                bool isTooCloseToOthers = false;
                if(spawnObjectDistance>0) {
                    do {
                        spawnX = transform.position.x + RandomOperator() * Random.Range(spawnMinRadius,spawnMaxRadius);
                        spawnZ = transform.position.z + RandomOperator() * Random.Range(spawnMinRadius,spawnMaxRadius);
                        spawnY = transform.position.y + Random.Range(spawnMinHeight,spawnMaxHeight);
                        spawnPosition = new Vector3(spawnX,spawnY,spawnZ);
                        for(int j=0;j<i;j++) {
                            if(Vector3.Distance(spawnPosition,spawnObjectArray[j].transform.position)<spawnObjectDistance) {
                                isTooCloseToOthers = true;
                                break;
                            } else {
                                isTooCloseToOthers = false;
                            }
                        }
                    } while(isTooCloseToOthers);
                }
                spawnObjectArray[i]=Instantiate(spawnObject,spawnPosition,Quaternion.identity);
                if(extraSpawn) {
                    spawnX = spawnObjectArray[i].transform.position.x + RandomOperator() * Random.Range(extraSpawnMinDistance,extraSpawnMaxDistance);
                    spawnZ = spawnObjectArray[i].transform.position.z + RandomOperator() * Random.Range(extraSpawnMinDistance,extraSpawnMaxDistance);
                    spawnY = spawnObjectArray[i].transform.position.y + Random.Range(extraSpawnMinHeight,extraSpawnMaxHeight);
                    spawnPosition = new Vector3(spawnX,spawnY,spawnZ);
                    Instantiate(extraSpawnObject,spawnPosition,Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int RandomOperator() {
        int tempRandom = Random.Range(0,2);
        if(tempRandom==1) {
            return 1;
        } else {
            return -1;
        }
    }
}
