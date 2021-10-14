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
        Debug.Log("ActiveVisorSet");
        
        texts.Clear();
        GameObject newPlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerVisor.activeVisor = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerVisor>();
        PlayerVisor.FindPlayerCamera();
    }

    private void Update()
    {
        //Debug.Log(cam.WorldToScreenPoint(target.transform.position));
        foreach (Prompt targetObject in texts.Keys)
        {
            Text TextReticle = texts[targetObject];

            // hide/show text depending on the state of the associated Prompt
            if (!targetObject.IsActive())
                TextReticle.font = null;
            else
                TextReticle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            
            float minX = TextReticle.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = TextReticle.GetPixelAdjustedRect().height / 2;
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
                TextReticle.color = new Color(0, 0, 0, 1);

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            TextReticle.transform.position = pos;
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
        text.fontStyle = FontStyle.Bold;

        // make text available for display
        newTextObject.transform.parent = PlayerVisor.activeVisor.transform;
        PlayerVisor.texts.Add(prompt, newTextObject.GetComponent<Text>());

        //Debug.Log("Added text from Prompt " + prompt.name);
    }

    public static void FindPlayerCamera()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerVisor.playerCamera = playerObject.GetComponentInChildren<Camera>();
    }
}
