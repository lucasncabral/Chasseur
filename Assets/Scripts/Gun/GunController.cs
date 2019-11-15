using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;
    FieldOfView playerFOV;

    void Start()
    {
        playerFOV = GetComponent<FieldOfView>();
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
        UpdateFOV();
    }

    public void UpdateFOV()
    {
        playerFOV.updateValues(equippedGun.viewRadius, equippedGun.viewAngle, equippedGun.FOVcoef);
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold(playerFOV.viewAngle);
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }
}