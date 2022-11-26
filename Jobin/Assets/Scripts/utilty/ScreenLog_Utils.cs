using UnityEngine;
using UnityEngine.UI;

namespace Abed.Utils
{
    public class ScreenLog_Utils : MonoBehaviour
    {
        [SerializeField] Text[] logText;
        [SerializeField] GameObject DbugTexts;

        bool DebugTextACitve;
        public void Log<T>(int logBox, T log)
        {

            logText[logBox].text = log.ToString();
            logText[logBox].fontSize = 25;
        }
        public void Log<T>(int logBox, T log, int fontSize)
        {
            logText[logBox].text = log.ToString();
            logText[logBox].fontSize = fontSize;
        }
        public void Log<T>(int logBox, T log, int fontSize, Color color)
        {
            logText[logBox].text = log.ToString();
            logText[logBox].fontSize = fontSize;
            logText[logBox].color = color;
        }
        public void HideText()
        {
            if (DebugTextACitve)
            {
                DebugTextACitve = false;
            }
            else { DebugTextACitve = true; }

            DbugTexts.SetActive(DebugTextACitve);
        }
    }
}