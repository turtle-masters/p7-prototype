using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public class DontRunThese
{
    private void LoadLevel(int levelIndex)
    {
        PrivateType privateTypeLevel = new PrivateType(typeof(Level));
        privateTypeLevel.SetStaticField("totalSceneChanges", levelIndex - 1);
        string sceneName = (string) privateTypeLevel.InvokeStatic("GetNextSceneName");
        if (SceneManager.GetActiveScene().name != sceneName)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        else
            privateTypeLevel.SetStaticField("totalSceneChanges", levelIndex);
    }

    [UnityTest]
    public IEnumerator PlayRoomLevel1()
    {
        this.LoadLevel(1);
        yield return new WaitForDomainReload();
    }

    [UnityTest]
    public IEnumerator PlayVillageLevel1()
    {
        this.LoadLevel(2);
        yield return new WaitForDomainReload();
    }

    [UnityTest]
    public IEnumerator PlayRoomLevel2()
    {
        this.LoadLevel(8);
        yield return new WaitForDomainReload();
    }
}
