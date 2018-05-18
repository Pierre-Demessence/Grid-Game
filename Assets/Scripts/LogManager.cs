using System;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private Text _textArea;

    private void Start()
    {
        _textArea.text = "";
    }

    public void Log(string log)
    {
        if (!string.IsNullOrEmpty(_textArea.text)) _textArea.text += "\n"; 
        _textArea.text += log;
    }
}