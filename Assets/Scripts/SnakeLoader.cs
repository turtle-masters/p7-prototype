using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLoader : MonoBehaviour
{
    public GameObject maltosePrefab;
    public int prefabLength;
    public float bodyWidth, prefabDist, rotationDegreePerPrefab;

    private GameObject[] maltoseChildArray;

    // Start is called before the first frame update
    void Start()
    {
        maltoseChildArray=new GameObject[prefabLength];
        for(int i=0;i<prefabLength;i++) {
            GameObject tempObject = Instantiate(maltosePrefab,transform.position,transform.rotation);
            tempObject.transform.SetParent(transform);
            maltoseChildArray[i]=tempObject;
        }
        for(int i=0;i<prefabLength;i++) {
            maltoseChildArray[i].transform.localPosition=new Vector3(-prefabDist*i,Mathf.Sin(Mathf.Deg2Rad*rotationDegreePerPrefab*i)*bodyWidth,Mathf.Cos(Mathf.Deg2Rad*rotationDegreePerPrefab*i)*bodyWidth);
            if(i>0) {
                maltoseChildArray[i].transform.localRotation=Quaternion.LookRotation(maltoseChildArray[i-1].transform.position-maltoseChildArray[i].transform.position);
            }
        }
        if(this.name=="Starch Snake Head") {
            BreakSnakeAtJoint(7);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject[] GetMaltoseChildArray() {
        return maltoseChildArray;
    }

    public void SetMaltoseChildArray(GameObject[] _maltoseChildArray) {
        maltoseChildArray=_maltoseChildArray;
    }

    public int GetPrefabLength() {
        return prefabLength;
    }

    public Vector3 GetJointPosition(int jointIndex) {
        if(0<=jointIndex && jointIndex<prefabLength) {
            return (maltoseChildArray[jointIndex].transform.position + maltoseChildArray[jointIndex+1].transform.position)/2;
        } else {
            Debug.Log("Incorrect Joint Index");
            return Vector3.zero;
        }
    }

    public void BreakSnakeAtJoint(int jointIndex) {
        //create new parents 
        //redistribute children
        //reindex children
        //add molecule component with dextrin
        GameObject newParent1, newParent2; //Use prefab?
        GameObject[] newParent1TempArray, newParent2TempArray;

        newParent1 = Instantiate(new GameObject(),transform.position,transform.rotation);
        newParent2 = Instantiate(new GameObject(),GetJointPosition(jointIndex),transform.rotation);

        SnakeLoader newParent1SnakeLoader = newParent1.AddComponent<SnakeLoader>();
        SnakeLoader newParent2SnakeLoader = newParent2.AddComponent<SnakeLoader>();

        newParent1TempArray = new GameObject[jointIndex+1];
        newParent2TempArray = new GameObject[prefabLength-jointIndex-1];
        for(int i=0;i<prefabLength;i++) {
            if(i<=jointIndex) {
                maltoseChildArray[i].transform.SetParent(newParent1.transform);
                newParent1TempArray[i]=maltoseChildArray[i];
            } else {
                maltoseChildArray[i].transform.SetParent(newParent2.transform);
                newParent2TempArray[i-jointIndex]=maltoseChildArray[i];
            }
        }

        newParent1SnakeLoader.SetMaltoseChildArray(newParent1TempArray);
        newParent2SnakeLoader.SetMaltoseChildArray(newParent2TempArray);

        Rigidbody rb1 = newParent1.AddComponent<Rigidbody>();
        Rigidbody rb2 = newParent2.AddComponent<Rigidbody>();
        rb1.isKinematic=true;
        rb2.isKinematic=true;
        rb1.drag=5;
        rb2.drag=5;
        rb1.AddForce(newParent1.transform.position-GetJointPosition(jointIndex));
        rb2.AddForce(newParent2.transform.position-GetJointPosition(jointIndex));
    }
}
