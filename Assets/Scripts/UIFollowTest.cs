using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowTest : MonoBehaviour
{

    public GameObject target;
    public Text reticle;
    public Text rightSideIndicator;
    public Text leftSideIndicator;
    public Camera cam;
    float offset = 1.12f;



    // Update is called once per frame
    void Update()
    {

        //Debug.Log(cam.WorldToScreenPoint(target.transform.position));
        //Debug.Log(cam.rect.width* canvas.scaleFactor);

        //Debug.Log(Vector3.Dot((target.transform.position - cam.transform.position), cam.transform.forward) < 0);



        Vector3 pos = cam.WorldToViewportPoint(target.transform.position);
        bool onScreen = pos.z > 0 && pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1;
        Vector3 dir = (target.transform.position - cam.transform.position).normalized;
        Debug.Log(Vector3.SignedAngle(cam.transform.forward, dir, Vector3.up));
        /*Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<RectTransform>(), pos, cam,out newPos);*/
        /*pos.x *= r.rect.width / (float)cam.pixelWidth;
        pos.y *= r.rect.height / (float)cam.pixelHeight;

        
        reticle.GetComponent<RectTransform>().anchoredPosition = pos - r.sizeDelta / 2f;*/
        /*
        float s = canvas.scaleFactor;
        float w = cam.rect.width * s;
        float h = cam.rect.height * s;
        float minX = (reticle.rectTransform.rect.width / 2) / s;
        float maxX = (w - minX);

        float minY = (reticle.rectTransform.rect.height / 2) / s;
        float maxY = (h - minY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        float x = pos.x - (w / 2);
        float y = pos.y - (h / 2);*/


        //textrect.anchoredPosition = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y)/s;



        if (!onScreen)
         {


            if (Vector3.SignedAngle(cam.transform.forward, dir,Vector3.up) > 0)
            {
                //right side
                rightSideIndicator.color = Color.white;
            }
            else
            {
                // left side
                leftSideIndicator.color = Color.white;
            }
        }
        else
        {
            rightSideIndicator.color = Color.clear;
            leftSideIndicator.color = Color.clear;
        }
        /*if (Vector3.Dot((target.transform.position - cam.transform.position), cam.transform.forward) < 0)
        {
            if (pos.y < Screen.height / 2)
            {
                pos.y = maxY;
            }
            else
            {
                pos.y = minY;
            }
        }
        */

        reticle.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + offset, target.transform.position.z);

    }
}
