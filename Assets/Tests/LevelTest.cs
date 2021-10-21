using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEditor.SceneTemplate;
using System;
using UnityEngine.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

public class LevelTest
{
    private Scene testScene;
    private Level testLevel;
    private PrivateType privateTypeLevel;
    private SceneTemplateAsset testSceneTemplate = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>("Assets/Tests/TestScene.scenetemplate");

    [SetUp]
    public void Setup()
    {
        this.testScene = SceneTemplateService.Instantiate(testSceneTemplate, true).scene;
        SceneManager.SetActiveScene(this.testScene);

        GameObject rootGameObject = (GameObject)this.testScene.GetRootGameObjects().GetValue(2);
        this.testLevel = rootGameObject.GetComponent<Level>();

        this.privateTypeLevel = new PrivateType(this.testLevel.GetType());
        this.privateTypeLevel.SetStaticField("totalSceneChanges", 0);
        this.privateTypeLevel.SetStaticField("activeLevel", this.testLevel);
    }

    [TearDown]
    public void Teardown()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        //SceneManager.UnloadScene(this.testScene);
#pragma warning restore CS0618 // Type or member is obsolete
        Prompt.ResolveAll();
    }

    [Test]
    public void ActivatesEntryPromptWhenActivated()
    {
        Prompt prompt = this.testLevel.gameObject.GetComponentInChildren<Prompt>();  // get the first Prompt in the Level
        NUnit.Framework.Assert.False(prompt.IsActive());
        this.testLevel.Activate();
        NUnit.Framework.Assert.True(prompt.IsActive());
    }

    [Test]
    public void GetsNextSceneName()
    {
        this.privateTypeLevel.SetStaticField("totalSceneChanges", 1);
        int totalSceneChanges = (int) this.privateTypeLevel.GetStaticField("totalSceneChanges");
        NUnit.Framework.Assert.AreEqual(1, totalSceneChanges);

        void TestGetNextSceneName(string expectedOutcome)
        {
            // compare expected with actual
            NUnit.Framework.Assert.AreEqual(
                expectedOutcome,
                (string) this.privateTypeLevel.InvokeStatic("GetNextSceneName")
            );

            // increase totalSceneChanges by 1
            int newTotalSceneChanges = (int) this.privateTypeLevel.GetStaticField("totalSceneChanges");
            this.privateTypeLevel.SetStaticField("totalSceneChanges", newTotalSceneChanges + 1);
        }

        TestGetNextSceneName("Village");
        TestGetNextSceneName("Microverse");

        TestGetNextSceneName("Village");
        TestGetNextSceneName("Microverse");

        TestGetNextSceneName("Village");
        TestGetNextSceneName("Microverse");

        TestGetNextSceneName("Village");
        TestGetNextSceneName("Room");
    }

    [Test]
    public void ResolvesAllActivePromptsOnCompletion()
    {
        Prompt prompt = this.testLevel.gameObject.GetComponentInChildren<Prompt>();  // get the first Prompt in the Level
        this.testLevel.Activate();
        NUnit.Framework.Assert.True(prompt.IsActive());
        this.testLevel.Complete(prompt);
        NUnit.Framework.Assert.False(prompt.IsActive());
    }

    [Test]
    public void ActivatesCorrectLevelsOnActiveSceneChange()
    {
        void TestOnActiveSceneChanged(int expectedIndexOfActiveLevel)
        {
            List<GameObject> rootGameObjects = new List<GameObject>(this.testScene.GetRootGameObjects());
            Level expectedLevel = rootGameObjects[expectedIndexOfActiveLevel + 2].GetComponent<Level>();
            rootGameObjects.RemoveAt(expectedIndexOfActiveLevel + 2);
            List<GameObject> onlyRootLevels = rootGameObjects.Skip(2).ToList();
            this.privateTypeLevel.SetStaticField("activeLevel", null);

            // activate next Level
            Scene oldScene = new Scene();
            this.privateTypeLevel.InvokeStatic("OnActiveSceneChanged", oldScene, this.testScene);

            // assert the expected Level is now active
            NUnit.Framework.Assert.AreEqual(
                expectedLevel,
                (Level)this.privateTypeLevel.GetStaticField("activeLevel")
            );

            // make sure none of the other Level objects are active
            foreach (GameObject go in onlyRootLevels)
                if (go != null)
                    NUnit.Framework.Assert.False(go.GetComponent<Level>().isActive);

            expectedLevel.SetVisibilityOfAllChildren(false);
        }

        TestOnActiveSceneChanged(0);

        TestOnActiveSceneChanged(0);
        TestOnActiveSceneChanged(0);

        TestOnActiveSceneChanged(1);
        TestOnActiveSceneChanged(1);

        TestOnActiveSceneChanged(2);
        TestOnActiveSceneChanged(2);

        TestOnActiveSceneChanged(3);

        TestOnActiveSceneChanged(1);
    }

    [Test]
    public void ManagesVisibilityOfChildRenderers()
    {
        void TestGameObjectRecursively(GameObject node, bool expectedState)
        {
            //Debug.Log(node.name);
            if (node.GetComponent<InteractionTarget>() != null)
                NUnit.Framework.Assert.False(node.GetComponent<Renderer>().enabled);
            else if (node.GetComponent<Renderer>() != null)
                NUnit.Framework.Assert.AreEqual(expectedState, node.GetComponent<Renderer>().enabled);
            if (node.GetComponent<Collider>() != null)
                NUnit.Framework.Assert.AreEqual(expectedState, node.GetComponent<Collider>().enabled);
            for (int i = 0; i < node.transform.childCount; i++)
                TestGameObjectRecursively(node.transform.GetChild(i).gameObject, expectedState);
        }

        // test level 1
        this.testLevel.SetVisibilityOfAllChildren(false);
        TestGameObjectRecursively(this.testLevel.gameObject, false);

        this.testLevel.SetVisibilityOfAllChildren(true);
        TestGameObjectRecursively(this.testLevel.gameObject, true);

        this.testLevel.SetVisibilityOfAllChildren(false);
        TestGameObjectRecursively(this.testLevel.gameObject, false);

        // test level 2
        GameObject level2GameObject = (GameObject) this.testScene.GetRootGameObjects().GetValue(3);
        Level level2 = level2GameObject.GetComponent<Level>();

        level2.SetVisibilityOfAllChildren(false);
        TestGameObjectRecursively(level2GameObject, false);

        level2.Activate();
        TestGameObjectRecursively(level2GameObject, true);

        level2.SetVisibilityOfAllChildren(false);
        TestGameObjectRecursively(level2GameObject, false);
    }
}
