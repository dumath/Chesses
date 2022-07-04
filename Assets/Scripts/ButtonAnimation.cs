using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject backgroundButtonImage;
    private Text spriteButton;
    public float red = 0.01f;
    public float green = 0.01f;
    public float blue = 0.01f;
    private Color minValue = Color.black;
    private Color maxValue = Color.white;
    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteButton = button.GetComponent<Text>();
        spriteButton.color = currentColor = minValue;
    }

    // Update is called once per frame
    void Update()
    {
        currentColor.r += red;
        currentColor.g += green;
        currentColor.b += blue;
        spriteButton.color = currentColor;
        if (currentColor == maxValue || currentColor == minValue)
        {
            red = -red;
            blue = -blue;
            green = -green;
        }
    }

    public void OnMouseEnter()
    {
        Color enterColor = Color.cyan;
        enterColor.a = 0.2f;
        backgroundButtonImage.GetComponent<Image>().color = enterColor;
    }

    public void OnMouseExit()
    {
        Color exitColor = Color.white;
        exitColor.a = 0.0f;
        backgroundButtonImage.GetComponent<Image>().color = exitColor;
    }



}
