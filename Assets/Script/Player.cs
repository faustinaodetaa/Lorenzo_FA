using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int maxHealth = 1000;
    public int maxSkill = 200;

    public int currentHealth;
    public int currentSkill;

    public HealthBar healthBar;
    public SkillPointsBar skillBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentSkill = maxSkill;
        skillBar.SetMaxSkill(maxSkill);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        currentSkill -= damage;
        skillBar.SetSkill(currentSkill);
    }
}
