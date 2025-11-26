using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectNavigationMouseButtons
{
    static ProjectNavigationMouseButtons()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        // Mouse Back
        if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            SendCommand("SelectPrev");
        }
        // Mouse Forward
        else if (Input.GetKeyDown(KeyCode.Mouse5))
        {
            SendCommand("SelectNext");
        }
    }

    private static void SendCommand(string command)
    {
        EditorWindow window = EditorWindow.focusedWindow;

        if (window != null)
        {
            Event e = EditorGUIUtility.CommandEvent(command);
            window.SendEvent(e);
        }
    }
}