
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SpeechRecognizerPlugin;

public class UnitySpeechRecognizer : MonoBehaviour, ISpeechRecognizerPlugin
{
    [SerializeField]
    private SpeechKeyword[] keywords;
    private SpeechRecognizerPlugin plugin = null;
    public TextMeshPro test;
    void Start()
    {
        plugin = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        StartVoice();
    }

    public void StartVoice()
    {
        plugin.SetContinuousListening(true);
        plugin.SetMaxResultsForNextRecognition(1);
        plugin.StartListening();
        Debug.Log("voice started");
        Debug.Log(Microphone.devices[0]);
    }

    public void OnError(string recognizedError)
    {
        Debug.Log(recognizedError);
        if (test != null) test.text = recognizedError;
    }

    public void OnResult(string recognizedResult)
    {
        Debug.Log(recognizedResult);
        recognizedResult.ToLower();
        if(test != null) test.text = recognizedResult;
        foreach (SpeechKeyword speechKeyword in keywords)
        {
            if (speechKeyword.keyword == recognizedResult)
            {
                if (speechKeyword.isSaid) continue;
                speechKeyword.isSaid = true;
                speechKeyword.onRecognized.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        plugin.StopListening();
    }
}
