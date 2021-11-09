using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButtonScript : MonoBehaviour
{
    public void Activate()
    {
        Destroy(this.gameObject);
    }

    public void Activate(Prompt p)
    {
        this.Activate();
    }
}
