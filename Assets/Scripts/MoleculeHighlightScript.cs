using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeHighlightScript : MonoBehaviour
{
    public GameObject highlightObject;
    // Start is called before the first frame update
    void Start()
    {
        highlightObject.SetActive(false);
    }

    /*private void Update() {
        if(Input.GetKeyDown(KeyCode.L)) {
            ToggleHighlight(!highlightObject.activeSelf);
        }
    }*/

    public void ToggleHighlight(bool toggleBool) {
        highlightObject.SetActive(toggleBool);
    }
}
