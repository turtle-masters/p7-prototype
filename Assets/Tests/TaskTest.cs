using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TaskTest : PromptTest
{
    Task testTask;
    Task testTask2;

    public override void Setup()
    {
        base.Setup();

        GameObject testTaskObject = new GameObject("TestTask");
        GameObject testTaskObject2 = new GameObject("TestTask2");
        testTaskObject.transform.parent = this.testLevel.transform;  // set the parent before Start in Prompt is run
        testTaskObject2.transform.parent = this.testLevel.transform;
        this.testTask = testTaskObject.AddComponent<Task>();
        this.testTask2 = testTaskObject2.AddComponent<Task>();

        // audio source must be added manually in testing, even though it is done automatically when launching the game
        testTask.gameObject.AddComponent<AudioSource>();
        testTask2.gameObject.AddComponent<AudioSource>();
    }
}
