
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if !UNITY_ANDROID
using UnityEngine.Windows.Speech;
#endif

public class UnitySpeechRecognizer : MonoBehaviour
{
    [SerializeField]
    //private string[] m_Keywords;
    private SpeechKeyword[] keywords;
#if !UNITY_ANDROID
    string[] keywordValues;
    private KeywordRecognizer m_Recognizer;
    Dictionary<string, SpeechKeyword> keywordDict = new();


    void Start()
    {
        keywordValues = new string[keywords.Length];
        for (int i = 0; i < keywords.Length; i++)
        {
            keywordValues[i] = keywords[i].keyword;
            keywordDict.Add(keywords[i].keyword, keywords[i]);
        }

        m_Recognizer = new KeywordRecognizer(keywordValues, ConfidenceLevel.Low);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        SpeechKeyword speechKeyword = keywordDict[args.text];
        speechKeyword.onRecognized.Invoke();
        speechKeyword.isSaid = true;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
    }
#endif
}
