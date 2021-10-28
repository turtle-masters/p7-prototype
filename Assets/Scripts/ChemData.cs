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
    private TextMesh textMesh;

    private void Start() {
        if(chemTextPrefab!=null) { //Add text object and set it as child
            chemText = Instantiate(chemTextPrefab,transform.position,Quaternion.identity);
            chemText.transform.SetParent(this.transform);
            //chemText.transform.localPosition.Set(chemText.transform.localPosition.x,chemText.transform.localPosition.y+textYOffset,chemText.transform.localPosition.z);
            textMesh = chemText.GetComponent<TextMesh>();
            textMesh.text=Name;
        }
    }

    private void Update() {
        if(Vector3.Distance(transform.position,MinigameManagerScript.playerGameObject.transform.position)<MinigameManagerScript.chemTextDisableDistance) {
            textMesh.text=Name;
            chemText.transform.LookAt(MinigameManagerScript.playerGameObject.transform.position);
        } else {
            textMesh.text = "";
        }
    }

    public void SetName(string _Name) {
        Name = _Name;
        chemText.GetComponent<TextMesh>().text=Name;
    }
}
