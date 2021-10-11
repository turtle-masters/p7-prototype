using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryMoleculeScript : MonoBehaviour
{
    Material myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial=GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Energize() {
        gameObject.tag="ATP";
        myMaterial.color=Color.blue;
    }
}
