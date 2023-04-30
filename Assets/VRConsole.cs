using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRConsole : MonoBehaviour
{
    [SerializeField] GameObject console;
    //s Start is called before the first frame update
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string output = logString +  stackTrace  + "- Type:-" + type.ToString();
        VRConsoleMessage(output);
    }


    public void VRConsoleMessage(string messageToPrint)
    {
        GameObject display = new GameObject($"PhoneConsoleMessage: {messageToPrint}");
        display.transform.parent = console.transform;
        display.transform.localPosition = Vector3.zero;
        display.transform.rotation= console.transform.rotation;

        display.AddComponent<RectTransform>();
        display.AddComponent<TextMeshProUGUI>();
        display.GetComponent<TextMeshProUGUI>().text = messageToPrint;
        display.transform.localScale = Vector3.one;

    }
}
