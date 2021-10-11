using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameLoaderScript : MonoBehaviour
{
    public GameObject[] MiniGamePrefabs;

    void LoadMinigame(int index) {
        if(index<MiniGamePrefabs.Length) {
            Instantiate(MiniGamePrefabs[index],transform.position,Quaternion.identity);
        }
    }
}
