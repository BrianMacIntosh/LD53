using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBubble : MonoBehaviour
{
    [SerializeField] GameObject BubbleHolder;
    [SerializeField] TextMeshProUGUI orderText;

    public void EnableBubble()
    {
        BubbleHolder.SetActive(true);
    }

    public void DisableBubble()
    {
        BubbleHolder.SetActive(false);
    }

    public void SetText(string text)
    {
        orderText.text = text;
    }
}
