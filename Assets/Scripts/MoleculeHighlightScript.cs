using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeHighlightScript : MonoBehaviour
{
    public GameObject highlightObject;
    
    void Start()
    {
        highlightObject.SetActive(false);
    }

    public void ToggleHighlight(bool toggleBool) {
        highlightObject.SetActive(toggleBool);
    }
}
