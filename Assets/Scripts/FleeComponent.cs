using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Ship Components/Flee/Basic Flee")]
public class FleeComponent : ShipComponent
{
    public string Flee_Success_Message, Flee_Fail_Message;

    public override void Fire(ShipManager PlayerShip, ShipManager EffectedShip)
    {
        float random = UnityEngine.Random.value;
        float chance = PlayerShip.GetSails() / 12f;
        if (chance >= random)
        {
            FindObjectOfType<CombatManager>().ActiveFlee();
            CombatManager.PostToCombatLog(String.Format(Flee_Success_Message, Mathf.RoundToInt(random * 100f), Mathf.RoundToInt(chance * 100f)));
        }
        else
            CombatManager.PostToCombatLog(String.Format(Flee_Fail_Message, Mathf.RoundToInt(chance * 100f)));
    }
}
