using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Combat
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] int _hitDamage;

        public int HitDamage { get => _hitDamage; }

        public void HitTarget(Health health)
        {
            health.TakeHit(this);
        }

        public void reduceEnergy(Health health)  //same as above but no sound effect for jump energy reduce
        {
            health.reduceEnergy(this);
        }
    }

}
