using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static int HullValue, CannonValue, SailValue, CookValue;

    public static SailManager SailManager;
    public static GameManager gm;
    public static float ShipSpeed = 0.05f;
    public static int CrewUsage = 0, CrewAmount = 8, PlayerShipHealth;
    public static string ShipName;

    public static Node[,] nl;

    public static Node SelectedNode, LastVisitedNode;

    public static List<Item> Items;
    public FlankComponent EquippedFlank;
    public FleeComponent EquippedFlee;
    public FireComponent EquippedFire;

    [SerializeField]
    public ShipComponent[] StartingInventoryContents; 

    
    public static bool ValueChange;

    public int SHullValue, SCannonValue, SSailValue, SCookValue;

	// Use this for initialization
	void Start () {
        gm = this;
        DontDestroyOnLoad(gameObject);
        //UnloadAllScenes();

        SceneManager.LoadScene("PortScreen");
        Debug.ClearDeveloperConsole();


        Items = new List<Item>();
        PlayerShipHealth = 100;
        LoadBasicItems();
        Items.AddRange(StartingInventoryContents);
    }

    private void LoadBasicItems()
    {
        Debug.Log("3");
        Items.Add(EquippedFire);
        Items.Add(EquippedFlee);
        Items.Add(EquippedFlank);
    }

    private void UnloadAllScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            SceneManager.UnloadSceneAsync(i);
        }
    }
	
	// Update is called once per frame
	void Update () {
        RefreshValues();

        if (ValueChange)
            ValueChange = !ValueChange;
    }

    public void LoadShipVoyageScreen()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("PortScreen"))

        //SceneManager.UnloadSceneAsync("ShipManagementScreen");
        SceneManager.LoadScene("ShipVoyageScreen");

        SailManager = FindObjectOfType<SailManager>();
    }

    public void LoadShipManagementScreen()
    {
        LastVisitedNode = SelectedNode;

        SceneManager.LoadScene("ShipManagementScreen");
    }

    private void RefreshValues()
    {
        SHullValue = HullValue;
        SCannonValue = CannonValue;
        SSailValue = SailValue;
        SCookValue = CookValue;
    }

    public static void ChangeCrewTotal(int change)
    {
        CrewAmount += change;
    }

    public static void IncreaseValue(PowerValues pv, int amount)
    {
        int SelectedValue = GetPowerValue(pv);
        if (SelectedValue + amount <= 12)
        {
            if (CrewUsage + amount > CrewAmount)
            {
                amount = CrewAmount - CrewUsage;
            }

            CrewUsage += amount;
            SetPowerValue(pv, SelectedValue + amount);
            return;
        }

        if (AllMax())
        {
            return;
        }

        //If it is over 12
        amount -= 12 - SelectedValue;
        SetPowerValue(pv, 12);
        CrewUsage += amount;
        IncreaseValue(FindLowestValue(), amount);
    }

    public static void DecreaseValue(PowerValues pv, int amount)
    {
        int SelectedValue = GetPowerValue(pv);
        if (SelectedValue - amount >= 0)
        {
            CrewUsage -= amount;
            SetPowerValue(pv, SelectedValue - amount);
            return;
        }

        if (AllMin())
        {
            return;
        }

        //If it is under 0
        amount += 0 - SelectedValue;
        SetPowerValue(pv, 0);
        CrewUsage -= amount;
        DecreaseValue(FindHighestValue(), amount);
    }

    public static int GetPowerValue(PowerValues pv)
    {
        switch (pv)
        {
            case PowerValues.Hull:
                return HullValue;
            case PowerValues.Cannon:
                return CannonValue;
            case PowerValues.Cook:
                return CookValue;
            case PowerValues.Sail:
                return SailValue;
        }
        return 0;
    }

    private static void SetPowerValue(PowerValues pv, int amount)
    {
        ValueChange = true;
        switch (pv)
        {
            case PowerValues.Hull:
                HullValue = amount;
                break;
            case PowerValues.Cannon:
                CannonValue = amount;
                break;
            case PowerValues.Cook:
                CookValue = amount;
                break;
            case PowerValues.Sail:
                SailValue = amount;
                break;
        }
    }

    private static bool AllMax()
    {
        if (HullValue + CannonValue + CookValue + SailValue == 12 * 4)
        {
            return true;
        }
        return false;
    }

    private static bool AllMin()
    {
        if (HullValue + CannonValue + CookValue + SailValue == 0)
        {
            return true;
        }
        return false;
    }

    public static PowerValues FindLowestValue()
    {
        if (HullValue > CannonValue || HullValue > SailValue || HullValue > CookValue)
        {
            if (CannonValue > SailValue || CannonValue > CookValue)
            {
                if (SailValue > CookValue)
                {
                    return PowerValues.Cook;
                }
                return PowerValues.Sail;
            }
            return PowerValues.Cannon; 
        }
        return PowerValues.Hull;
    }

    public static PowerValues FindHighestValue()
    {
        if (HullValue < CannonValue || HullValue < SailValue || HullValue < CookValue)
        {
            if (CannonValue < SailValue || CannonValue < CookValue)
            {
                if (SailValue < CookValue)
                {
                    return PowerValues.Cook;
                }
                return PowerValues.Sail;
            }
            return PowerValues.Cannon;
        }
        return PowerValues.Hull;
    }
}

public enum PowerValues
{
    Hull = 0, Cannon = 1, Cook = 2, Sail = 3
}


[CustomEditor(typeof(GameManager))]
[CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    SerializedProperty hullValueProp, cannonValueProp, sailValueProp, cookValueProp;

    private void OnEnable()
    {
        hullValueProp = serializedObject.FindProperty("SHullValue");
        cannonValueProp = serializedObject.FindProperty("SCannonValue");
        sailValueProp = serializedObject.FindProperty("SSailValue");
        cookValueProp = serializedObject.FindProperty("SCookValue");
    }

    public override void OnInspectorGUI()
    {
        PowerBarProgress("Hull", hullValueProp.intValue, hullValueProp.hasMultipleDifferentValues);
        PowerBarProgress("Cannons", cannonValueProp.intValue, cannonValueProp.hasMultipleDifferentValues);
        PowerBarProgress("Sails", sailValueProp.intValue, sailValueProp.hasMultipleDifferentValues);
        PowerBarProgress("Cook", cookValueProp.intValue, cookValueProp.hasMultipleDifferentValues);

        EditorGUILayout.Space();

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void PowerBarProgress(string name, int value, bool mValue)
    {
        EditorGUILayout.BeginHorizontal();
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        if (!mValue)
        {
            EditorGUI.ProgressBar(rect, value / 12f, name + " (" + value + "/12)");
        }
        else
        {
            EditorGUI.ProgressBar(rect, 1f, "Combined Values (Different)");
        }
        
            

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }
}
