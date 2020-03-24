using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {
    PlayerController player;
    public Ability(PlayerController _player) {
        player = _player;
    }

    public class AbilityStats
    {
        public Ability abilityParent;
        public Abilities abilityType;
        public float TimeAbilityLastUsed;
        public float energyCost = 1;
        public float cooldown = 1;
        public bool canUseAbility { get { return Time.time - TimeAbilityLastUsed >= cooldown; } }
        public float chargePercentage { get { return (Time.time - TimeAbilityLastUsed) / cooldown; } }

        public AbilityStats(Ability _abilityParent, Abilities _abilityType, float _energyCost, float _cooldown)
        {
            this.abilityParent = _abilityParent;
            this.abilityType = _abilityType;
            this.energyCost = _energyCost;
            this.cooldown = _cooldown;
        }
    }
}
