using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Ship Components/Flank/Basic Flank")]
public class FlankComponent : ShipComponent
{
    public string Flank_Fail_Message, Flank_Succeed_Message, Flank_Pointless_Message;

    public override void Fire(ShipManager PlayerShip, ShipManager EffectedShip)
    {
        float random = UnityEngine.Random.value;
        float chance = PlayerShip.GetSails() / 12f;
        if (chance >= random && !EffectedShip.flanked)
        {
            CombatManager.PostToCombatLog(String.Format(Flank_Succeed_Message, Mathf.RoundToInt(random * 100f), Mathf.RoundToInt(chance * 100f)));
            EffectedShip.flanked = true;
        }
        else if (EffectedShip.flanked)
            CombatManager.PostToCombatLog(Flank_Pointless_Message);
        else
            CombatManager.PostToCombatLog(String.Format(Flank_Fail_Message, Mathf.RoundToInt(chance * 100f)));
    }
}
