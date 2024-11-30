using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerUIMAnager : MonoBehaviour
{
    private TextMeshProUGUI txtHealth;
    private TextMeshProUGUI txtEggs;
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        txtHealth = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
        txtEggs = GameObject.Find("EggsLeft").GetComponent<TextMeshProUGUI>();
        txtHealth.text = "Vida: " + PlayerHealth.maxHealth.ToString();
        txtEggs.text = "Huevos: " + Weapon.cargador.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        txtHealth.text = "Vida: " + PlayerHealth.playerHealth.ToString();
        if(Weapon.cargador > 0)
        {
            txtEggs.text = "Huevos: " + Weapon.cargador.ToString();
        } else
        {
            txtEggs.text = "Pulsa 'R' para recargar";
        }
    }
}
