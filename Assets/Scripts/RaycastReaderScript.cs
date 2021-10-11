using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RaycastReaderScript : MonoBehaviour
{
    public Text displayText;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("this is working");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 10000f, Color.yellow);
    }
}
