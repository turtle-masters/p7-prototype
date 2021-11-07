using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionReset : MonoBehaviour
{
    private GameObject playerObject;

    public void OnEnable()
    {
        this.playerObject = this.gameObject;
        SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Vector3 spawnPointVector = GetSpawnPointVector();
        this.playerObject.transform.position = new Vector3(spawnPointVector.x, this.playerObject.transform.position.y, spawnPointVector.z);
    }

    private Vector3 GetSpawnPointVector()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        return spawnPoints[0].transform.position;
    }
}
