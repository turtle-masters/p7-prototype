using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class PlayerVisor : MonoBehaviour
{
    public static PlayerVisor activeVisor;
    public static Camera playerCamera;
    public static Dictionary<Prompt, Text> texts = new Dictionary<Prompt, Text>();

    static PlayerVisor()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        //Debug.Log("ActiveVisorSet");
        
        texts.Clear();

        GameObject[] visorObjects = GameObject.FindGameObjectsWithTag("HUD");
        foreach (GameObject vo in visorObjects)
            if (vo.scene == newScene) 
                PlayerVisor.activeVisor = vo.GetComponent<PlayerVisor>();

        Debug.Log("Found PlayerVisor in Scene " + PlayerVisor.activeVisor.gameObject.scene.name);
        PlayerVisor.FindPlayerCamera();
    }

    private void Update()
    {
        //Debug.Log(cam.WorldToScreenPoint(target.transform.position));
        foreach (Prompt targetObject in texts.Keys)
        {
            Text textReticle = texts[targetObject];

            // hide/show text depending on the state of the associated Prompt
            if (targetObject.IsActive()) textReticle.enabled = true;
            else
            {
                textReticle.enabled = false;
                continue;
            }

            float minX = textReticle.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = textReticle.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minX;

            Vector2 pos = playerCamera.WorldToScreenPoint(targetObject.transform.position);

            // TODO: refine corners of screen for VR (possible use a circle instead of square)
            if (Vector3.Dot((targetObject.transform.position - playerCamera.transform.position), playerCamera.transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                    pos.x = maxX;
                else
                    pos.x = minX;
            }
            else
                textReticle.color = new Color(255, 255, 255, 1);

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            textReticle.transform.position = pos;
        }
    }

    public static void AddMyText(Prompt prompt)
    {
        if (prompt.promptText == "") return;

        // add Text GameObject with required components
        GameObject newTextObject = new GameObject(prompt.promptText);
        newTextObject.layer = 5;  // UI layer
        newTextObject.AddComponent<RectTransform>();
        newTextObject.AddComponent<CanvasRenderer>();
        newTextObject.AddComponent<Text>();

        // style text
        Text text = newTextObject.GetComponent<Text>();
        text.text = prompt.promptText;
        text.fontSize = 16;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.fontStyle = FontStyle.Bold;

        // make text available for display
        newTextObject.transform.parent = PlayerVisor.activeVisor.transform;
        PlayerVisor.texts.Add(prompt, newTextObject.GetComponent<Text>());
        text.enabled = false;
    }

    public static void FindPlayerCamera()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject po in playerObjects)
            if (po.scene == SceneManager.GetActiveScene())
                PlayerVisor.playerCamera = po.GetComponentInChildren<Camera>();
    }
}
