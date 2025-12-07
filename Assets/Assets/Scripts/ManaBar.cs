using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    private Slider slider;
    public Text manaCounter;

    public GameObject playerState;

    private float currentMana, maxMana;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentMana = playerState.GetComponent<PlayerState>().currentMana;
        maxMana = playerState.GetComponent<PlayerState>().maxMana;

        float fillValue = currentMana / maxMana;
        slider.value = fillValue;

        manaCounter.text = currentMana + " / " + maxMana;

    }
}