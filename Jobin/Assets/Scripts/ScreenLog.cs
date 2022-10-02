
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLog : MonoBehaviour
{
    [SerializeField] Text[] logText;
    public void Log<T>(int logBox, T log)
    {
        logText[logBox].text = log.ToString();
    }    
    public void Log<T>(int logBox, T log,int fontSize)
    {
        logText[logBox].text = log.ToString();
        logText[logBox].fontSize = fontSize;
    }
    public void Log<T>(int logBox, T log,int fontSize,Color color)
    {
        logText[logBox].text = log.ToString();
        logText[logBox].fontSize = fontSize;
        logText[logBox].color = color; 
    }
}