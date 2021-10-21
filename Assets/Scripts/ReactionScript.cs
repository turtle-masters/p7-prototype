using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScript : MonoBehaviour
{
    public GameObject[] reactionSpawnObjects;
    public string triggerName = "";
    public string triggerNewName = "";

    void React() {
        foreach(GameObject reactionObject in reactionSpawnObjects)
        {
            Vector3 randomOffset= new Vector3(Random.Range(0,0.1f),Random.Range(0,0.1f),Random.Range(0,0.1f));
            Instantiate(reactionObject,transform.position+randomOffset,Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision other) {
        ChemData data = other.gameObject.GetComponent<ChemData>();
        if(data!=null) {
            if(data.Name==triggerName) {
                React();
                if(triggerNewName!="") {
                    data.SetName(triggerNewName);
                }
            }
            
        }
    }
}
