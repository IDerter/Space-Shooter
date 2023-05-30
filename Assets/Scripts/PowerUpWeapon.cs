using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpWeapon : PowerUp
    {
        [SerializeField] private TurretPropetries _turretPropetries;

        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.AssignWeapon(_turretPropetries);
        }
    }

}
