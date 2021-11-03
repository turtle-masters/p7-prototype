using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChemData))]
public class EnzymeColoringScript : MonoBehaviour
{
    [System.Serializable]
    public struct EnzymeColoring {
        public string name;
        public Color color;
    }
    public ChemData chemDataRef;
    public MeshRenderer meshRendererRef;
    public EnzymeColoring[] enzymeColoringArray;
    // Start is called before the first frame update
    void Start()
    {
        chemDataRef=gameObject.GetComponent<ChemData>();
        if(gameObject.GetComponent<MeshRenderer>())
            meshRendererRef=gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(EnzymeColoring tempEnzymeColoring in enzymeColoringArray) {
            if(chemDataRef.Name==tempEnzymeColoring.name && meshRendererRef.material.color!=tempEnzymeColoring.color) {
                meshRendererRef.material.color=tempEnzymeColoring.color;
            }
        }
    }
}
