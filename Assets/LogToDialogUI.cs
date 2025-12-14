using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogToDialogUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text logText;
    public GameObject panel;

    [Header("Settings")]
    public int maxLines = 12;
    public bool showWarnings = true;
    public bool showErrors = true;
    public bool showLogs = true;

    private readonly Queue<string> lines = new Queue<string>();

    private void Awake()
    {
        // ✅ 如果你想一开始就显示面板，确保它开着
        if (panel != null) panel.SetActive(true);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void Start()
    {
        // ✅ 开机自检：不靠 Debug.Log，也能马上看到 UI 是否正常
        AddLine("LOG UI READY ✅");
        AddLine("If you can see this, UI is working.");
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Log && !showLogs) return;
        if (type == LogType.Warning && !showWarnings) return;
        if ((type == LogType.Error || type == LogType.Exception || type == LogType.Assert) && !showErrors) return;

        string prefix = type switch
        {
            LogType.Warning => "[WARN] ",
            LogType.Error => "[ERROR] ",
            LogType.Exception => "[EXCEPTION] ",
            LogType.Assert => "[ASSERT] ",
            _ => ""
        };

        AddLine(prefix + condition);
    }

    public void AddLine(string msg)
    {
        if (logText == null) return;

        lines.Enqueue(msg);
        while (lines.Count > maxLines)
            lines.Dequeue();

        logText.text = string.Join("\n", lines);
    }

    public void Clear()
    {
        lines.Clear();
        if (logText != null) logText.text = "";
    }

    public void TogglePanel()
    {
        if (panel != null) panel.SetActive(!panel.activeSelf);
    }
}
