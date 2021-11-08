using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UType = UnitController.UType;

public class CombatController : MonoBehaviour
{
    MapController mapCtrl;

    // damage[attacker, defender] = float
    float[,] damage = new float[4,4] {
        {  0,  0,  0,  0 },     // Villager
        { 70, 50, 40, 70 },     // Warrior
        { 60, 50, 30, 60 },     // Armor
        { 60, 50, 20, 60 }      // Archer
    };

    // -------------------------------------------

    void Awake() {
        mapCtrl = GameObject.FindWithTag("MapController").GetComponent<MapController>();
    }

    public void Attack(UnitController attacker, UnitController defender) {
        // attack
        int aHP = attacker.GetHP();
        int dHP = defender.GetHP();

        float baseDmg = damage[(int)attacker.GetUType(), (int)defender.GetUType()];
        float HP_modifier = (float)aHP / 100;
        float aDef = mapCtrl.GetTerrainDef(attacker.GetX(), attacker.GetY());
        float dDef = mapCtrl.GetTerrainDef(defender.GetX(), defender.GetY());

        int atkDmg = (int) (baseDmg * HP_modifier * dDef);
        Debug.Log($"{attacker.name} hit {defender.name} for base {baseDmg} damage!");
        defender.SetHP(dHP - atkDmg);

        if (defender.GetHP() <= 0)
            defender.Die();
        else {
            // counterattack
            if (defender.GetAtkRange() == attacker.GetAtkRange()) {
                dHP = defender.GetHP();

                baseDmg = damage[(int)defender.GetUType(), (int)attacker.GetUType()];
                HP_modifier = (float)dHP / 100;

                int defDmg = (int) (baseDmg * HP_modifier * aDef);
                attacker.SetHP(aHP - defDmg);
            }
        }
    }
}
