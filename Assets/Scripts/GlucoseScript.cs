using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlucoseScript : MonoBehaviour
{
    public GameObject[] deathSpawnObjects;
    
    private List<GameObject> nearbyADPs = new List<GameObject>();

    void Die() {
        //On Death
        //Energize ADP
        //Instantiate death objects
        //Destroy
        if(nearbyADPs.Count>0) {
            nearbyADPs[0].SendMessage("Energize");
        }

        foreach(GameObject deathObject in deathSpawnObjects)
        {
            Vector3 randomOffset= new Vector3(Random.Range(0,0.1f),Random.Range(0,0.1f),Random.Range(0,0.1f));
            Instantiate(deathObject,transform.position+randomOffset,Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision other) {
        ChemData data = other.gameObject.GetComponent<ChemData>();
        if(data!=null) {
            if(data.Name == "NAD+") {
                data.SetName("NADH");
                Die();
            }
            if(data.Name == "ADP") {
                nearbyADPs.Add(other.gameObject);
            }
            if(data.Name == "Amylase" && this.transform.parent!=null) {
                if(transform.parent.GetComponent<SnakeLoader>()) {
                    transform.parent.GetComponent<SnakeLoader>().BreakAtSegmentPosition(transform.position,other.transform.position);
                }
            }
        }
    }

    //This could be on an ADP detection aura that can be read
    private void OnCollisionExit(Collision other) {
        ChemData data = other.gameObject.GetComponent<ChemData>();
        if(data!=null) {
            if(data.Name == "ADP") {
                nearbyADPs.Remove(other.gameObject);
            }
        }
    }
}