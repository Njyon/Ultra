using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Text combo;
    [SerializeField] Text score;
    [SerializeField] Text multiplier;

    //[SerializeField] Animation comboAnim;

    /// <summary>
    /// UpdateScore
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        this.score.text = "Score: " + score.ToString();
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
        combo.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1);
        multiplier.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1);
    }
    /// <summary>
    /// Make ComboCounter InVisible
    /// </summary>
    public void HiddeCombo()
    {
        combo.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        multiplier.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
    }
}