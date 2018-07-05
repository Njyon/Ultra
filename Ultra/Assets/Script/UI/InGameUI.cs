using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Text combo;
    [SerializeField] Text score;
    [SerializeField] Text multiplier;
    [SerializeField] Image border;

    Color color;
    
    public void SetHUDColor(Color color)
    {
        border.color = color;
        combo.color = color;
        multiplier.color = color;
        this.color = color;
        HiddeCombo();
    }

    /// <summary>
    /// UpdateScore
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        float digit;
        float decimals;
        if (score >= 1000000)
        {
            digit = score / 1000000;
            digit = Mathf.Round(digit * 100.0f) / 100.0f;
            decimals = digit - (int)digit;
            this.score.text = digit.ToString() + "M";
        }
        else if (score >= 1000)
        {
            digit = score / 1000f;
            Debug.Log("/1000: " + digit);
            digit = Mathf.Round(digit * 100.0f) / 100.0f;
            Debug.Log("Round: " + digit);
            decimals = digit - (int)digit;
            Debug.Log("Decimal = " + decimals);
            decimals *= 100;
            Debug.Log("*100: " + decimals);
            this.score.text = digit.ToString() + "K";
        }
        else
        {
            this.score.text = score.ToString();
        }
    }
    /// <summary>
    /// Update Combo
    /// </summary>
    /// <param name="combo"></param>
    public void UpdateCombo(int combo)
    {
        this.combo.text = combo.ToString() + "x";
    }
    /// <summary>
    /// Update Multiplier
    /// </summary>
    /// <param name="combo"></param>
    public void UpdateMultiplier(int multiplier)
    {
        this.multiplier.text = multiplier.ToString();
    }
    /// <summary>
    /// Make ComboCounter Visible
    /// </summary>
    public void ShowCombo()
    {
        combo.color = new Color(color.r, color.g, color.b, 1);
        multiplier.color = new Color(color.r, color.g, color.b, 1);
    }
    /// <summary>
    /// Make ComboCounter InVisible
    /// </summary>
    public void HiddeCombo()
    {
        combo.color = new Color(color.r, color.g, color.b, 0);
        multiplier.color = new Color(color.r, color.g, color.b, 0);
    }
}