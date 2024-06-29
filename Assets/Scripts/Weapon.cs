using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Camera playerCamera;
    //shooting
    public bool isShooting, ReadyToShoot;
    bool allowReset = true; 
    public float shootingDelay = 2f;

    //burst
    public int bulletsPerBurst = 3;
    public int burstBulletsleft;

    // Spread
    public float spreadIntensity;


    //bullet
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefablifetime = 3f;
     
    public enum ShootingMode 
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        ReadyToShoot = true;
        burstBulletsleft = bulletsPerBurst;
    }

    // Update is called once per frame
    void Update()
    {
       if (currentShootingMode == ShootingMode.Auto)
       {
        //holding down mouse button to shoot
        isShooting = Input.GetKey(KeyCode.Mouse0);
       }
       else if (currentShootingMode == ShootingMode.Burst|| currentShootingMode == ShootingMode.Single)
       {
        //tapping down mouse button to shoot
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
       }

       if (isShooting && ReadyToShoot)
       {
           burstBulletsleft = bulletsPerBurst;
           FireWeapon();
       }
    }

    private void FireWeapon()
    {
        ReadyToShoot=false;

        Vector3 shootDirection = CalculateDirectionAndSpread().normalized;
        
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, Quaternion.identity);

        bullet.transform.position = shootDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootDirection * bulletVelocity,ForceMode.Impulse);

        StartCoroutine(DestroyBulletafterTime(bullet, bulletPrefablifetime));

        //checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }

        //burst MODE
        if (currentShootingMode == ShootingMode.Burst && burstBulletsleft > 1) //we already shoot once before this check 
        {
            burstBulletsleft--;
            Invoke("FireWeapon", shootingDelay);
        }



    }
    private void ResetShot()
    {
        ReadyToShoot = true;
        allowReset = true;
    }



    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - BulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBulletafterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
