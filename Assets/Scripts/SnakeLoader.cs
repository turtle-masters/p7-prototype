using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLoader : MonoBehaviour
{
    public static float jointBreakForce=100f;
    public GameObject segmentPrefab;
    public int prefabLength;
    public float bodyWidth, prefabDist, rotationDegreePerPrefab;

    private List<GameObject> segmentArray = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate Segment
        for(int i=0;i<prefabLength;i++) {
            GameObject tempObject = Instantiate(segmentPrefab,transform.position,Quaternion.identity);
            tempObject.transform.SetParent(transform);
            segmentArray.Add(tempObject);
        }
        SetupSegmentPositions();
        for(int i=0;i<prefabLength;i++) {
            segmentArray[i].transform.SetParent(transform);
        }
    }
    
    public void SetupSegmentPositions() {
        for(int i=0;i<segmentArray.Count;i++) {
            segmentArray[i].transform.localPosition=this.transform.forward*-prefabDist*i + this.transform.up*Mathf.Sin(Mathf.Deg2Rad*rotationDegreePerPrefab*i)*bodyWidth+this.transform.right*Mathf.Cos(Mathf.Deg2Rad*rotationDegreePerPrefab*i)*bodyWidth;
            if(i>0)
                segmentArray[i].transform.LookAt(segmentArray[i-1].transform);
        }
        if(segmentArray.Count>1)
            segmentArray[0].transform.LookAt(this.transform.forward*-prefabDist*-1 + this.transform.up*Mathf.Sin(Mathf.Deg2Rad*rotationDegreePerPrefab*-1)*bodyWidth+this.transform.right*Mathf.Cos(Mathf.Deg2Rad*rotationDegreePerPrefab*-1)*bodyWidth);
     }

    public List<GameObject> GetSegmentChildArray() {
        return segmentArray;
    }

    public void SetSegmentChildArray(List<GameObject> _segmentArray) {
        segmentArray=_segmentArray;
        foreach (GameObject segment in segmentArray)
        {
            segment.transform.SetParent(this.transform);
        }
    }

    public int GetPrefabLength() {
        return prefabLength;
    }

    public int GetJointLength() {
        return Mathf.Max(segmentArray.Count-1,0);
    }

    public Vector3 GetJointPosition(int jointIndex) {
        if(0<=jointIndex && jointIndex<GetJointLength()) {
            return (segmentArray[jointIndex].transform.position + segmentArray[jointIndex+1].transform.position)/2;
        } else {
            Debug.LogError("Incorrect Joint Index");
            return Vector3.zero;
        }
    }

    public Vector3[] GetAllJointPositions() {
        Vector3[] tempJointPositions= new Vector3[GetJointLength()];
        for(int i=0;i<GetJointLength();i++) {
            tempJointPositions[i] = GetJointPosition(i);
        }
        return tempJointPositions;
    }

    public void BreakSnakeAtJoint(int jointIndex) {
        //When an endpiece is hit, unparent, unarray, and give force
        segmentArray[jointIndex].transform.SetParent(null);
        Rigidbody segmentRb=segmentArray[jointIndex].GetComponent<Rigidbody>();
        segmentRb.isKinematic=false;
        segmentRb.AddForce(Random.onUnitSphere*jointBreakForce);
        MinigameManagerScript.instance.GoalUpdate(segmentArray[jointIndex]);
        segmentArray.RemoveAt(jointIndex);
    }

    public void BreakAtSegmentPosition(Vector3 hitSegmentPosition,Vector3 projectilePosition) {
        for(int i=0;i<=GetJointLength();i++) {
            if(segmentArray[i].transform.position==hitSegmentPosition) {
                BreakSnakeAtJoint(i);
            }
        }
    }
}
