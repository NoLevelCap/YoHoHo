using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    private GameObject InfoBar;
    private Text FirstText, SecondText;

    private List<EventTrigger> EventTriggers;
    private EventTrigger activeEvent;

    public TextAsset EventsFile;
    public int id;

    // Use this for initialization
    void Start() {
        InfoBar = GameObject.Find("Canvas").transform.Find("InfoBar").gameObject;
        FirstText = InfoBar.GetComponentsInChildren<Text>()[0];
        SecondText = InfoBar.GetComponentsInChildren<Text>()[1];

        EventTriggers = EventParser.LoadEvents(EventsFile);
    }

    // Update is called once per frame
    void Update() {

    }

    public void FireEvent()
    {
        InfoBar.SetActive(true);
        activeEvent = EventTriggers.ToArray()[id];
        string[] td = activeEvent.getTextDialog();
        FirstText.text = td[0];
        SecondText.text = td[1];
    }

    public void DismissEvent()
    {
        SailManager.UnPauseMovement();
        activeEvent.FireEvent();
    }
}

public class EventParser{
    public static List<EventTrigger> LoadEvents(TextAsset eventList)
    {
        List<EventTrigger> et = new List<EventTrigger>();
        string[] EventSplitList = eventList.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in EventSplitList)
        {
            et.Add(CreateEvent(item.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)));
        }
        return et;
    }

    private static EventTrigger CreateEvent(string[] EventData)
    {
        string[] cData = ReplaceQuotes(ReplaceQuotes(EventData, @"""", string.Empty), @"{nl}", "\n");
        bool goodEvent = (cData[4] == "true");
        string fdialog = cData[0];
        string sdialog = cData[3];
        string variabletxt = cData[2];
        EventTrigger.Type type = (EventTrigger.Type) System.Enum.Parse(typeof(EventTrigger.Type), cData[1]);
        return new EventTrigger(fdialog, sdialog, variabletxt, goodEvent, type);
    }

    private static string[] ReplaceQuotes(string[] info, string a, string o)
    {
        for (int i = 0; i < info.Length; i++)
        {
            info[i] = info[i].Replace(a, o);
        }

        return info;
    }
}

public class EventTrigger
{
    public enum Type
    {
        gold, skip, lowest_stat, all_stats, highest_stat, health, sails, cannons, cooks, hull  
    }

    private string ftext, stext, vtext;
    private int cReturn;
    private bool preCalculated;

    public bool isGood;
    public Type type;

    public EventTrigger(string ftext, string stext, string vtext, bool good, Type type)
    {
        this.ftext = ftext;
        this.stext = stext;
        this.vtext = vtext;
        this.isGood = good;
        this.type = type;

        preCalculated = int.TryParse(vtext, out cReturn);     
    }

    public string[] getTextDialog()
    {
        if (!preCalculated)
        {
            cReturn = calculateAmount();
        }
        string[] r = new string[3];
        r[0] = ftext.Replace(@"{val}", cReturn.ToString());
        r[1] = stext.Replace(@"{val}", cReturn.ToString());
        r[0] = r[0].Replace(@"{invval}", (-cReturn).ToString());
        r[1] = r[1].Replace(@"{invval}", (-cReturn).ToString());
        r[2] = cReturn.ToString();
        return r;
    }

    private int calculateAmount()
    {
        string d = vtext;
        d = d.Replace("{ran(", string.Empty);
        d = d.Replace(")}", string.Empty);
        string[] a = d.Split(':');
        int f = int.Parse(a[0]);
        int s = int.Parse(a[1]);
        if (f > s)
        {
            int t = s;
            s = f;
            f = t;
        }
        return Random.Range(f, s);
    }

    public void FireEvent()
    {
        switch (type)
        {
            case Type.gold:
                throw new System.Exception("Type " + type.ToString() + " Not Implemented Yet");
                break;
            case Type.skip:
                throw new System.Exception("Type " + type.ToString() + " Not Implemented Yet");
                GameManager.SailManager.SkipToNextLocation();
                break;
            case Type.lowest_stat:
                ChangeLowestStat();
                break;
            case Type.all_stats:
                ChangeAllStats();
                break;
            case Type.highest_stat:
                ChangeHighestStat();
                break;
            case Type.health:
                throw new System.Exception("Type " + type.ToString() + " Not Implemented Yet");
                break;
            case Type.sails:
                ChangeStat(PowerValues.Sail);
                break;
            case Type.cannons:
                ChangeStat(PowerValues.Cannon);
                break;
            case Type.cooks:
                ChangeStat(PowerValues.Cook);
                break;
            case Type.hull:
                ChangeStat(PowerValues.Hull);
                break;
            default:
                throw new System.Exception("Type " + type.ToString() + " Not Implemented Yet");
        }
    }

    private void ChangeLowestStat()
    {
        GameManager.ChangeCrewTotal(cReturn);
        if (cReturn < 0)
            GameManager.DecreaseValue(GameManager.FindLowestValue(), -(cReturn));
        else
            GameManager.IncreaseValue(GameManager.FindLowestValue(), (cReturn));
    }

    private void ChangeAllStats()
    {
        GameManager.ChangeCrewTotal(cReturn * 4);
        if (cReturn < 0) {
            GameManager.DecreaseValue(PowerValues.Cannon, -(cReturn));
            GameManager.DecreaseValue(PowerValues.Cook, -(cReturn));
            GameManager.DecreaseValue(PowerValues.Hull, -(cReturn));
            GameManager.DecreaseValue(PowerValues.Sail, -(cReturn));
        }
        else
        {
            GameManager.IncreaseValue(PowerValues.Cannon, (cReturn));
            GameManager.IncreaseValue(PowerValues.Cook, (cReturn));
            GameManager.IncreaseValue(PowerValues.Hull, (cReturn));
            GameManager.IncreaseValue(PowerValues.Sail, (cReturn));
        }
    }

    private void ChangeHighestStat()
    {
        GameManager.ChangeCrewTotal(cReturn);
        if (cReturn < 0)
            GameManager.DecreaseValue(GameManager.FindHighestValue(), -(cReturn));
        else
            GameManager.IncreaseValue(GameManager.FindHighestValue(), (cReturn));
    }

    private void ChangeStat(PowerValues pv)
    {
        GameManager.ChangeCrewTotal(cReturn);
        if (cReturn < 0)
            GameManager.DecreaseValue(pv, -(cReturn));
        else
            GameManager.IncreaseValue(pv, (cReturn));
    }
}
