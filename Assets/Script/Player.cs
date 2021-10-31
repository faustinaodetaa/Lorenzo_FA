using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public int maxHealth = 1000;
    public int maxSkill = 200;

    public int currentHealth;
    public int currentSkill;

    public HealthBar healthBar;
    public SkillPointsBar skillBar;
    //public GameObject inventories;
    public Inventory inventory;

    public int coreItems;
    public Text coreItemDisplay;

    public int ammo;
    public bool isFiring;
    public Text ammoDisplay;
    public GameObject deathMenu;
    public GameObject bloodSplatter;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentSkill = maxSkill;
        skillBar.SetMaxSkill(maxSkill);
        deathMenu.SetActive(false);
        bloodSplatter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TakeDamage(20);
        }

        ammoDisplay.text = ammo.ToString();
        if (Input.GetMouseButtonDown(0) && !isFiring && ammo > 0)
        {
            isFiring = true;
            ammo--;
            isFiring = false;
        }

        coreItemDisplay.text = coreItems.ToString();
       
        if(currentHealth == 0)
        {
            //Debug.Log("mati");
            deathMenu.SetActive(true);
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject g = hit.gameObject;
        if (g.tag.Equals("Item"))
        {
            if (g.name.Contains("CoreItem"))
            {
                Debug.Log("core item");
                coreItems++;
            }
            else
            {
                inventory.AddItem(g.name);
            }
            Destroy(g);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        //bloodSplatter.SetActive(true);

        if(currentHealth <= 0)
        {
            Debug.Log("is ded");
            deathMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }

        //currentSkill -= damage;
        //skillBar.SetSkill(currentSkill);
    }

    public void useAmmo()
    {
        ammo += 30;
    }

    public void useShield()
    {

    }

    public void useHealthPotion()
    {
        currentHealth += 200;
    }

    public void useSkillPotion()
    {
        currentSkill += 75;
    }

    public void usePainKiller()
    {

    }

    public void useDamageMultiplier()
    {

    }

    
}
