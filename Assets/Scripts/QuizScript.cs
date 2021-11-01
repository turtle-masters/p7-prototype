using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[ExecuteInEditMode]
public class QuizScript : MonoBehaviour
{
    [System.Serializable]
    public struct Question {
        public string text;
        public string [] responseTextArray;
        
    }

    public Question[] questionArray;
    public TextMesh quizText;
    public GameObject buttonPrefab = null;
    private int activeQuestionIndex = 0;
    private int[] responseIndexArray;
    private string saveFileString;
    
    void Start()
    {
        
        responseIndexArray = new int[questionArray.Length];
        for(int i=0;i<questionArray.Length;i++) {
            responseIndexArray[i]=0;
        }
        Answer("");
    }

    // Update is called once per frame
    void Update()
    {
        quizText.text = questionArray[activeQuestionIndex].text;
        if(Input.GetKeyDown(KeyCode.Q)) {
            Answer("");
            Debug.Log("Answered");
        }
    }

    public void Answer(string answerID) {
        activeQuestionIndex++;
        SaveInventory();
        if(activeQuestionIndex>=questionArray.Length) {
            activeQuestionIndex=0;
        }
    }

    void SaveInventory ()
    {
        string filePath = getPath ();
        
        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter (filePath);
        
        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine ("QuestionIndex,QuestionText,AnswerText,AnswerIndex");
        //This loops through everything in the inventory and sets the file to these.
        for (int i = 0; i <= activeQuestionIndex; i++) {
            writer.WriteLine (i.ToString() + 
                "," + questionArray[i].text +
                "," + questionArray[i].responseTextArray[responseIndexArray[i]] +
                "," + responseIndexArray[i]);
        }
        writer.Flush ();
        //This closes the file
        writer.Close ();
    }
    private string getPath ()
    {
        #if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_Questionnaire.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_Questionnaire.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_Questionnaire.csv";
        #else
        return Application.dataPath +"/"+"Saved_Questionnaire.csv";
        #endif
    }    
}
