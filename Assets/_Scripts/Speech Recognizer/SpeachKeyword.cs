using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.SerializableAttribute]
public class SpeechKeyword 
{
    [SerializeField] public string keyword;
    public bool isSaid = false;
    [SerializeField]
    [Tooltip("Events to fire when matching keyword is found")]
    public UnityEvent onRecognized = new UnityEvent();
}
