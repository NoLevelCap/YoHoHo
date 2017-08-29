using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Ship Components/Fire/Basic Fire")]
public class FireComponent : ShipComponent
{
    [SerializeField]
    public float FlankBonus, MaxHit;

    public string Hit_Message;

    public override void Fire(ShipManager PlayerShip, ShipManager EffectedShip)
    {
        float chance = PlayerShip.GetCannons() / 12f;
        int hitAmount = (EffectedShip.flanked) ? Mathf.RoundToInt(chance * MaxHit * FlankBonus) : Mathf.RoundToInt(chance * MaxHit);
        CombatManager.PostToCombatLog(String.Format(Hit_Message, hitAmount));
        EffectedShip.ChangeHealth(-hitAmount);
    }
}
