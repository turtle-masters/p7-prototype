using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastReaderScript : MonoBehaviour
{
    public Text displayText;
    public bool displayGoals = false;

    void FixedUpdate()
    {
        if(!displayGoals) {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit))
            {
                ChemData data = hit.collider.gameObject.GetComponent<ChemData>();
                if(data!=null) {
                    if(data.Formula == "") {
                        displayText.text = data.Name;
                    } else {
                        displayText.text = data.Name+"\n"+data.Formula;
                    }
                    
                } else {
                    displayText.text="No Data";
                }
                
            }
        } else {
            displayText.text = MinigameManagerScript.instance.GetGoalString();
        }
    }
}
