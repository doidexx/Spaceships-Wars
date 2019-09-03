using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Slider slider;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        SetUpHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, player.GetHealth(), 0.05f);
    }

    private void SetUpHealthBar()
    {
        slider = GetComponent<Slider>();
        player = FindObjectOfType<Player>();
        slider.maxValue = player.GetHealth();
    }
}
