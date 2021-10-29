using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryMoleculeScript : MonoBehaviour
{
    //Material myMaterial;
    public GameObject emptyBatteryObj, chargedBatteryObj;

    // Start is called before the first frame update
    void Start()
    {
        emptyBatteryObj.SetActive(true);
        chargedBatteryObj.SetActive(false);
        //myMaterial=GetComponent<MeshRenderer>().material;
    }

    public void Energize() {
        GetComponent<ChemData>().SetName("ATP");
        //myMaterial.color=Color.blue;
        emptyBatteryObj.SetActive(false);
        chargedBatteryObj.SetActive(true);
        MinigameManagerScript.instance.GoalUpdate(this.gameObject);
    }
}
