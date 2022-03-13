using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/UpSlashAbility")]
public class UpSlashAbility : MeleeAbilities
{
        private UpSlashTriggerable attack;

        public float meterProgression;
        public float knockBackX, knockBackY;
        public float moveForceY;
        public float hitStop;

        public override void Initialize(GameObject obj)
        {
            attack = obj.GetComponent<UpSlashTriggerable>();

            // overwrite melee attack values with the values for current attack
            attack.damage = damage;
            attack.meterProgression = meterProgression;
            attack.knockBackX = knockBackX;
            attack.knockBackY = knockBackY;
            attack.moveForceY = moveForceY;
            attack.hitStop = hitStop;
            attack.rumbleDurration = rumbleDurration;
            attack.rumbleLow = rumbleLow;
            attack.rumbleHigh = rumbleHigh;
            attack.triggerName = triggerName;
        }

        public override void TriggerAbility()
        {
            attack.Trigger();
        }
}
