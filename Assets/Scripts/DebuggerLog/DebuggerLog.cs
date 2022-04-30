using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerLog : MonoBehaviour
{
    public bool showLog = true;

    #region Events

    //Will display a log that can have a title and can be log type Normal,Warning,Error ("Error")
    public void Log(string title, string logData, LogType logType = LogType.Normal)
    {
        if (showLog)
        {
            string log = "";
            string colorUsed = "#000000";

            if (logType == LogType.Positive)
            {
                colorUsed = "#3F9617";
            }
            else if (logType == LogType.Warning)
            {
                colorUsed = "#F0F40F";
            }
            else if (logType == LogType.Alert)
            {
                colorUsed = "#961E8E";
            }
            else if (logType == LogType.Error)
            {
                colorUsed = "#D60A04";
            }
            else if (logType == LogType.Negitive)
            {
                colorUsed = "#F48A0F";
            }
            else if (logType == LogType.Info)
            {
                colorUsed = "#0F13F4";
            }

            if (title != null && title != "")
            {
                log += "<color=" + colorUsed + "><size=11><b>" + title + "</b></size></color>\n";
            }

            log += logData;

            Log(log, logType);
        }
    }

    public void Log(string logData, LogType logType = LogType.Normal)
    {
        if (showLog)
        {
            string log = "";

            log += logData;

            if (logType == LogType.Positive || logType == LogType.Normal || logType == LogType.Negitive || logType == LogType.Info)
            {
                Debug.Log(log);
            }
            else if (logType == LogType.Warning || logType == LogType.Alert)
            {
                Debug.LogWarning(log);
            }
            else if (logType == LogType.Error)
            {
                Debug.LogError(log);
            }
            else
            {
                Debug.Log(log);
            }
        }
    }

    public enum LogType
    {
        Normal,
        Alert,
        Error,
        Info,
        Negitive,
        Positive,
        Warning,
    }

    #endregion
}
