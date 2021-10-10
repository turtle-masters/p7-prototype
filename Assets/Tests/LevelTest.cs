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
        int totalSceneChanges = (int) this.privateTypeLevel.GetStaticField("totalSceneChanges");
        NUnit.Framework.Assert.AreEqual(0, totalSceneChanges);

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
            GameObject[] rootGameObjects = this.testScene.GetRootGameObjects();
            Level expectedLevel = rootGameObjects[expectedIndexOfActiveLevel + 2].GetComponent<Level>();
            this.privateTypeLevel.SetStaticField("activeLevel", null);
            /*NUnit.Framework.Assert.AreNotEqual(
                expectedLevel, 
                (Level) this.privateTypeLevel.GetStaticField("activeLevel")
            );*/

            // activate next Level
            Scene oldScene = new Scene();
            this.privateTypeLevel.InvokeStatic("OnActiveSceneChanged", oldScene, this.testScene);

            // assert the expected Level is now active
            NUnit.Framework.Assert.AreEqual(
                expectedLevel,
                (Level)this.privateTypeLevel.GetStaticField("activeLevel")
            );
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
        Task task = this.testLevel.gameObject.GetComponentInChildren<Task>();
        this.testLevel.SetVisibilityOfAllChildren(false);

        NUnit.Framework.Assert.False(task.gameObject.GetComponent<Renderer>().enabled);
        this.testLevel.Activate();
        NUnit.Framework.Assert.True(task.gameObject.GetComponent<Renderer>().enabled);
    }
}
