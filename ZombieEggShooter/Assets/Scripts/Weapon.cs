using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    public bool isShooting;
    public bool readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    public int bulletsPerBurst = 3;
    public static int cargador = 10;
    public int burstEggsLeft;
    public float spreadIntensity;

    public GameObject eggPrefab;
    public Transform eggSpawn;
    public float eggVelocity = 30;
    public float eggPrefabLifetime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstEggsLeft = bulletsPerBurst;
    }

    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting && cargador > 0)
        {
            burstEggsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (Input.GetKey(KeyCode.R))
        {
            cargador = 10;
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        
        cargador--;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject egg = Instantiate(eggPrefab, eggSpawn.position, Quaternion.identity);
        egg.transform.forward = shootingDirection;

        egg.GetComponent<Rigidbody>().AddForce(shootingDirection * eggVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyEgg(egg, eggPrefabLifetime));

        if(allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && burstEggsLeft > 1)
        {
            burstEggsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
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

        Vector3 direction = targetPoint - eggSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyEgg(GameObject egg, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(egg);
    }
}
