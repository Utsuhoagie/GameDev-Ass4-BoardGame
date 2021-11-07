using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UType = UnitController.UType;

public class CombatController : MonoBehaviour
{
    // damage[attacker, defender] = float
    float[,] damage = new float[4,4] {
        {  0,  0,  0,  0 },
        { 70, 50, 40, 70 },
        { 60, 50, 30, 60 },
        { 60, 50, 20, 60 }
    };

    public void Attack(UnitController attacker, UnitController defender) {
        // attack
        int aHP = attacker.GetHP();
        int dHP = defender.GetHP();

        float baseDmg = damage[(int)attacker.GetUType(), (int)defender.GetUType()];
        float HP_modifier = (float)aHP / 100;
        float def_modifier = 1.0f - 0.0f;

        int atkDmg = (int) (baseDmg * HP_modifier * def_modifier);
        defender.SetHP(dHP - atkDmg);

        if (defender.GetHP() <= 0)
            defender.Die();
        else {
            // counterattack
            if (defender.GetAtkRange() == attacker.GetAtkRange()) {
                dHP = defender.GetHP();

                baseDmg = damage[(int)defender.GetUType(), (int)attacker.GetUType()];
                HP_modifier = (float)dHP / 100;
                def_modifier = 1.0f - 0.0f;

                int defDmg = (int) (baseDmg * HP_modifier * def_modifier);
                attacker.SetHP(aHP - defDmg);
            }
        }
    }
}
