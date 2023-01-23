using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Inspector to add a models list as a drop-down selection UI 
/// and add a "Generate" button for the StableDiffusionMaterial.
/// </summary>
[CustomEditor(typeof(StableDiffusionMaterial))]
public class StableDiffusionMaterialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StableDiffusionMaterial myComponent = (StableDiffusionMaterial)target;

        // Draw the drop-down list
        myComponent.selectedModel = EditorGUILayout.Popup("Model List", myComponent.selectedModel, myComponent.modelList);
        
        // Apply the changes to the serialized object
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Generate"))
            myComponent.Generate();
    }
}
