using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static float playerHealth;
    private float takeDamageCoolDown = 2f;
    private float lerpTimer;
    private bool restingFromAttack = false;
    public static float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHB;
    public Image backHB;

    void Start()
    {
        lerpTimer = 0f;
        takeDamageCoolDown += Time.deltaTime;
        playerHealth = maxHealth;

    }
    //private void OnCollisionEnter(Collision collision) {
    //    Debug.Log(collision.gameObject.tag.ToString());
    //    if (collision.gameObject.CompareTag("HealthObject"))
    //    {
    //        RecoverHealth(20f);
    //        Debug.Log("Estoy en PlayerHealth.cs");
    //        Debug.Log(playerHealth);
    //    }
    //}


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("HealthObject"))
        {
            RecoverHealth(20f);
            Destroy(hit.gameObject);
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            if(!restingFromAttack)
            {
                TakeDamage(ZombieBehaviour.zombieDamage);
                lerpTimer = takeDamageCoolDown;
                restingFromAttack = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(restingFromAttack)
        {
            lerpTimer -= Time.deltaTime;
            if(lerpTimer < 0f)
            {
                restingFromAttack = false;
            }
        }
        playerHealth = Mathf.Clamp(playerHealth, 0 , maxHealth);
    }

   
    public static void TakeDamage(float damage) {

        playerHealth -= damage;
        Debug.Log(playerHealth);
    }

    public static void RecoverHealth(float health) {
        if(playerHealth < maxHealth) {
            playerHealth += health;
        }
        Debug.Log(playerHealth);
    }
}
