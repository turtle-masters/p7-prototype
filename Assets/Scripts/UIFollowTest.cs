using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowTest : MonoBehaviour
{

    public GameObject target;
    public Text reticle;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(cam.WorldToScreenPoint(target.transform.position));
        
        float minX = reticle.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = reticle.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minX;

        Vector2 pos = cam.WorldToScreenPoint(target.transform.position);

        if (Vector3.Dot((target.transform.position - cam.transform.position), cam.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }
        else
        {
            reticle.color = new Color(0, 0, 0, 1);
        }
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        reticle.transform.position = pos;
    }
}
