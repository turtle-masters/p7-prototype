using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemData : MonoBehaviour
{
    public GameObject chemTextPrefab = null;
    public float textYOffset = 1f;
    public string Name = "";
    public string Formula = "";
    private GameObject chemText;

    private void Start() {
        if(chemTextPrefab!=null) { //Add text object and set it as child
            chemText = Instantiate(chemTextPrefab,transform.position,Quaternion.identity);
            chemText.transform.SetParent(this.transform);
            //chemText.transform.localPosition.Set(chemText.transform.localPosition.x,chemText.transform.localPosition.y+textYOffset,chemText.transform.localPosition.z);
            chemText.GetComponent<TextMesh>().text=Name;
        }
    }

    public void SetName(string _Name) {
        Name = _Name;
        chemText.GetComponent<TextMesh>().text=Name;
    }
}
