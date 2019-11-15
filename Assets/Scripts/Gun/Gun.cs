using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    public int burstCount;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;

    float nextShotTime;

    public Transform shell;
    public Transform shellEjection;
    MuzzleFlash muzzleflash;

    public float viewAngle;
    public float FOVcoef;
    public float viewRadius;
    float currentAngle;
    float precision = 1.5f;

    void Start()
    {
        muzzleflash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
    }

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;

                Quaternion direction = projectileSpawn[i].rotation;
                
                if (currentAngle > 1f) {
                    currentAngle = currentAngle / precision;
                    float random = (Random.Range(-currentAngle, currentAngle) / 100);
                    //float random = 0.25f;
                    direction.y = direction.y + random;
                }

                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, direction) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleflash.Activate();
        }
    }

    public void OnTriggerHold(float viewAngle)
    {
        currentAngle = viewAngle;
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        currentAngle = 0;
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
