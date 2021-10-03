using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InheritanceTest
{
    PromptController target;

    [SetUp]
    public void Setup()
    {
        GameObject testObject = new GameObject("TestObject");
        target = testObject.AddComponent<PromptController>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(target.gameObject);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void InheritanceTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual("PromptController 1", target.GetVar());
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator InheritanceTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
