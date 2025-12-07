using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance { get; set; }

    // ----- Player Health ----- //
    public float currentHealth;
    public float maxHealth;


    // ----- Player Energy ----- //
    public float currentEnergy;
    public float maxEnergy;

    // ----- Player Mana ----- //
    public float currentMana;
    public float maxMana;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }

    }
}
