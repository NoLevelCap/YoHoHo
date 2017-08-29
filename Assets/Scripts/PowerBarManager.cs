using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PowerBarManager : MonoBehaviour {

    public float value;
    public string name;
    public PowerValues powerValue;

    private GameObject ButtonHolder;
    private bool load;
    private GameObject[] Buttons;

	// Use this for initialization
	void Start () {
        ButtonHolder = transform.GetChild(1).gameObject;
        name = transform.GetComponentInChildren<Text>().text;
        LoadButtons();
        
        ValueChange(GameManager.GetPowerValue(powerValue));

    }

    private void LoadButtons()
    {
        Buttons = new GameObject[ButtonHolder.transform.childCount];
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i] = ButtonHolder.transform.GetChild(i).GetChild(0).gameObject;
        }
    }

    private void Update()
    {
    }

    public void ValueChange(int nI)
    {
        for (int i = 0; i < nI; i++)
        {
            Buttons[i].SetActive(true);
        }

        for (int i = nI; i < ButtonHolder.transform.childCount; i++)
        {
            Buttons[i].SetActive(false);
        }

        value = nI;
    }

    public void OnButtonPress(int childIndex)
    {
        int val = 0;
        if (!(value == 1 && childIndex == 0)) {
            val = childIndex + 1;
        }

        int change = val - GameManager.GetPowerValue(powerValue);
        if (change < 0)
            GameManager.DecreaseValue(powerValue, -change);
        else
            GameManager.IncreaseValue(powerValue, change);

        ValueChange(GameManager.GetPowerValue(powerValue));

        //Debug.Log("Button Pressed with a index of "+ childIndex);
    }

}

[CustomEditor(typeof(PowerBarManager))]
[CanEditMultipleObjects]
public class PowerBarEditor : Editor
{
    SerializedProperty value, nameProp, powerValueProp;

    private void OnEnable()
    {
        value = serializedObject.FindProperty("value");
        nameProp = serializedObject.FindProperty("name");
        powerValueProp = serializedObject.FindProperty("powerValue");
    }

    public override void OnInspectorGUI()
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        if (!value.hasMultipleDifferentValues)
        {
            EditorGUI.ProgressBar(rect, value.floatValue / 12f, powerValueProp.enumNames[powerValueProp.enumValueIndex] + " (" + value.floatValue + "/12)");
        } else
        {
            EditorGUI.ProgressBar(rect, 1f, "Combined Values (Different)");
        }
        EditorGUILayout.Space();
        
        powerValueProp.enumValueIndex = (int) ((PowerValues) EditorGUILayout.EnumPopup("Power Bar Assignment: ", (PowerValues)powerValueProp.enumValueIndex));
        serializedObject.ApplyModifiedProperties();
    }


}
