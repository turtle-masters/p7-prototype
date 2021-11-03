using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChemData : MonoBehaviour
{
    public GameObject chemTextPrefab = null;
    public string Name = "";
    public string Formula = "";
    private GameObject chemText;
    //private TextMesh textMesh;

    private void Start() {
        if(chemTextPrefab!=null) { //Add text object and set it as child
            chemText = Instantiate(chemTextPrefab,transform.position,Quaternion.identity);
            chemText.transform.SetParent(this.transform);
            //chemText.transform.localPosition.Set(chemText.transform.localPosition.x,chemText.transform.localPosition.y+textYOffset,chemText.transform.localPosition.z);
            SetName(Name);
        }
    }

    private void Update() {
        if(Vector3.Distance(transform.position,MinigameManagerScript.playerGameObject.transform.position)<MinigameManagerScript.chemTextDisableDistance) {
            SetName(Name);
            chemText.transform.LookAt(MinigameManagerScript.playerGameObject.transform.position);
        } else {
            SetName("");
        }
    }

    public void SetName(string _Name) {
        Name = _Name;
        if(chemText.GetComponent<TextMesh>()) {
            if(Name=="NAD+") {
                chemText.GetComponent<TextMesh>().text="Enzyme";
            } else if(Name=="NADH") {
                chemText.GetComponent<TextMesh>().text="Reduced\nEnzyme";
            } else {
                chemText.GetComponent<TextMesh>().text=Name;
            }
        }
    }
}
