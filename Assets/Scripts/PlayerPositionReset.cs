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
        (Vector3, Quaternion) spawnPoint = GetSpawnPoint();
        Vector3 spawnPointVector = spawnPoint.Item1;

        this.playerObject.transform.position = new Vector3(spawnPointVector.x, this.playerObject.transform.position.y, spawnPointVector.z);
        this.playerObject.transform.rotation = spawnPoint.Item2;
    }

    private (Vector3, Quaternion) GetSpawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        return (spawnPoints[0].transform.position, spawnPoints[0].transform.rotation);
    }
}
