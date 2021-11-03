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
    public int spareAmmo;
    public bool isFiring;
    public Text currentAmmoDisplay;
    public Text spareAmmoDisplay;
    public GameObject deathMenu;
    public GameObject bloodSplatter;
    RaycastWeapon weapon;
    public Text messageDisplay;
    public Text currentWeaponDisplay;
    public string message;
    public string currentWeapon;
    public GameObject messageUI;
    public bool isRestart;
    public GameObject shield;
    bool shieldActive;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentSkill = maxSkill;
        skillBar.SetMaxSkill(maxSkill);
        deathMenu.SetActive(false);
        bloodSplatter.SetActive(false);
        weapon = GetComponentInChildren<RaycastWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    TakeDamage(20);
        //}

        currentAmmoDisplay.text = ammo.ToString();
        if (Input.GetMouseButtonDown(0) && !isFiring && ammo > 0)
        {
            isFiring = true;
            ammo--;
            isFiring = false;
            
        }
        if(ammo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            message = "Reloading...";
            ammo += 30;
            spareAmmo -= 30;
            messageDisplay.text = message;
            Invoke(nameof(displayMessage), 5);
        }
        spareAmmoDisplay.text = spareAmmo.ToString();
        currentWeaponDisplay.text = currentWeapon.ToString();
        coreItemDisplay.text = coreItems.ToString();
       
        if(currentHealth == 0)
        {
            //Debug.Log("mati");
            deathMenu.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.UseItem(1);
        }else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.UseItem(2);
        }else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.UseItem(3);
        }else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventory.UseItem(4);
        }else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            inventory.UseItem(5);
        }else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            inventory.UseItem(6);
        }

    }

    void displayMessage()
    {
        messageUI.SetActive(true);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject g = hit.gameObject;
        if (g.tag.Equals("Item"))
        {
            if (g.name.Contains("CoreItem"))
            {
                coreItems++;
                if(coreItems >= 9)
                {
                    coreItems = 9;
                }
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
        

        if(currentHealth <= 0)
        {
            Debug.Log("is ded");
            deathMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isRestart = true;
            //Time.timeScale = 0f;
        }

        if (shieldActive)
        {

        }else if (!shieldActive)
        {
            currentHealth -= damage;
            Debug.Log("get " + damage);
            healthBar.SetHealth(currentHealth);
            //bloodSplatter.SetActive(true);
        }

        //currentSkill -= damage;
        //skillBar.SetSkill(currentSkill);
    }

    public void useAmmo()
    {
        ammo += 30;
    }

    public IEnumerator useShield()
    {
        shield.SetActive(true);
        shieldActive = true;
        yield return new WaitForSeconds(7f);
        shieldActive = false;
        shield.SetActive(false);
    }

    public void useHealthPotion()
    {
        currentHealth += 200;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetHealth(currentHealth);
    }

    public void useSkillPotion()
    {
        currentSkill += 75;
        if(currentSkill > maxSkill)
        {
            currentSkill = maxSkill;
        }
        skillBar.SetSkill(currentSkill);
    }

    public IEnumerator usePainKiller()
    {
        currentHealth += 450;
        healthBar.SetHealth(currentHealth);
        for (int i = 0; i <= 5; i++)
        {
            yield return new WaitForSeconds(1f);
            TakeDamage(90);
        }
    }

    public IEnumerator useDamageMultiplier()
    {
        weapon.damage *= 2;
        yield return new WaitForSeconds(5f);
        weapon.damage /= 2;
    }

    
}
