using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PromptTest
{
    Level testLevel;
    Prompt testPrompt;
    Prompt testPrompt2;

    [SetUp]
    public void Setup()
    {
        GameObject testLevelObject = new GameObject("TestLevel");
        testLevel = testLevelObject.AddComponent<Level>();
        GameObject testPromptObject = new GameObject("TestPrompt");
        GameObject testPromptObject2 = new GameObject("TestPrompt2");
        testPromptObject.transform.parent = testLevelObject.transform;  // set the parent before Start in Prompt is run
        testPromptObject2.transform.parent = testLevelObject.transform;
        testPrompt = testPromptObject.AddComponent<Prompt>();
        testPrompt2 = testPromptObject2.AddComponent<Prompt>();
    }

    [TearDown]
    public void Teardown()
    {
        Prompt.ResolveAll();
        Object.DestroyImmediate(testLevel.gameObject);  // running DestroyImmediate is ok since we're testing in edit mode and >should< also destroy child objects... hopefully...
    }

    [Test]
    public void FindsParentLevel()
    {
        Assert.IsNotNull(testPrompt.GetParentLevel());
    }

    [Test]
    public void Activates()
    {
        Assert.False(testPrompt.IsActive());
        testPrompt.Activate();
        Assert.True(testPrompt.IsActive());
    }

    [Test]
    public void ActivatesWithPrompt()
    {
        Assert.False(testPrompt.IsActive());
        testPrompt.Activate(testPrompt);
        Assert.True(testPrompt.IsActive());
    }

    [Test]
    public void Resolves()
    {
        Assert.False(testPrompt.IsActive());
        testPrompt.Activate();
        Assert.True(testPrompt.IsActive());
        testPrompt.Resolve();
        Assert.False(testPrompt.IsActive());
    }

    [Test]
    public void ManagesActivePrompts()
    {
        Assert.AreEqual(0, Prompt.activePrompts.Count);
        
        testPrompt.Activate();
        Assert.AreEqual(1, Prompt.activePrompts.Count);
        Assert.True(Prompt.activePrompts.Exists(p => p.Equals(testPrompt)));
        Assert.False(Prompt.activePrompts.Exists(p => p.Equals(testPrompt2)));

        testPrompt2.Activate();
        Assert.AreEqual(2, Prompt.activePrompts.Count);
        Assert.True(Prompt.activePrompts.Exists(p => p.Equals(testPrompt)));
        Assert.True(Prompt.activePrompts.Exists(p => p.Equals(testPrompt2)));

        testPrompt.Resolve();
        Assert.AreEqual(1, Prompt.activePrompts.Count);
        Assert.False(Prompt.activePrompts.Exists(p => p.Equals(testPrompt)));
        Assert.True(Prompt.activePrompts.Exists(p => p.Equals(testPrompt2)));

        testPrompt2.Resolve();
        Assert.AreEqual(0, Prompt.activePrompts.Count);
        Assert.False(Prompt.activePrompts.Exists(p => p.Equals(testPrompt)));
        Assert.False(Prompt.activePrompts.Exists(p => p.Equals(testPrompt2)));
    }

    [Test]
    public void InvokesOnActive()
    {
        testPrompt.OnActive.AddListener(p => Assert.True(testPrompt.IsActive()));
        Assert.False(testPrompt.IsActive());
        testPrompt.Activate();
    }

    [Test]
    public void InvokesOnResolve()
    {
        testPrompt.OnResolve.AddListener(p => Assert.False(testPrompt.IsActive()));
        testPrompt.Activate();
        Assert.True(testPrompt.IsActive());
        testPrompt.Resolve();
    }

    [Test]
    public void DoesNotActivateIfAlreadyActive()
    {
        testPrompt.Activate();
        Assert.True(testPrompt.IsActive());
        testPrompt.OnActive.AddListener(p => Assert.True(false));
        testPrompt.Activate();
        Assert.True(testPrompt.IsActive());
    }

    [Test]
    public void DoesNotResolveIfAlreadyResolved()
    {
        testPrompt.Activate();
        testPrompt.Resolve();
        Assert.False(testPrompt.IsActive());
        testPrompt.OnResolve.AddListener(p => Assert.True(false));
        testPrompt.Resolve();
        Assert.False(testPrompt.IsActive());
    }
}
