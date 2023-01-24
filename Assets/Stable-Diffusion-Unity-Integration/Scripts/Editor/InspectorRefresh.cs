using UnityEditor;
using System;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class InspectorRefresh
{
    public static event Action OnEnterEditMode;
    static InspectorRefresh()
    {
        EditorApplication.playModeStateChanged += PlaymodeStateChanged;
    }

    private static void PlaymodeStateChanged(PlayModeStateChange stateChange)
    {
        if (stateChange == PlayModeStateChange.EnteredEditMode)
        {
            OnEnterEditMode?.Invoke();
        }
    }

    public static void ForceInspectorToRefresh()
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        EditorApplication.QueuePlayerLoopUpdate();
        EditorSceneManager.MarkAllScenesDirty();
        EditorUtility.RequestScriptReload();
    }
}

