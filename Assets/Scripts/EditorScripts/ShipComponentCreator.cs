using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShipComponentCreator : EditorWindow {
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    ComponentType selectedComponent;
    string componentName = "";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Assets/Create/Custom/General Ship Component")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ShipComponentCreator window = (ShipComponentCreator)EditorWindow.GetWindow(typeof(ShipComponentCreator));
        window.Show();
    }
    // Add menu named "My Window" to the Window menu
    [MenuItem("Assets/Create/Custom/Ship Components/Fire Component")]
    static void StartFireComponent()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        selectedComponent = (ComponentType) EditorGUILayout.EnumPopup("Type", selectedComponent);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }

    public enum ComponentType
    {
        Fire, Flee, Flank
    }
}
