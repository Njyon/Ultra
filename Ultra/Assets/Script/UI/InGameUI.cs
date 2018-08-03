using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Text & Images")]
    [SerializeField] Text combo;
    [SerializeField] Text comboTxt;
    [SerializeField] Text score;
    [SerializeField] Text multiplier;
    [SerializeField] Text multiplierTxt;
    [SerializeField] Image Character;

    [Header("Rects")]
    [SerializeField] RectTransform comboHandel;
    [SerializeField] RectTransform comboObj;
    [SerializeField] RectTransform multiplierObj;
    [SerializeField] RectTransform cHCoolPos;

    [Header("Anim Curves")]
    [SerializeField] AnimationCurve xCurve;
    [SerializeField] AnimationCurve yCurve;

    Vector3 cHStartPos;
    Color color;
    bool comboActive = false;
    int i_score = 0;
    Vector3 scoreTextPos;
    Vector3 scoreTextScale;

    void Start()
    {
        scoreTextScale = score.transform.localScale;
        scoreTextPos = score.transform.position;
        cHStartPos = comboHandel.position;
        comboHandel.position = cHCoolPos.position;
    }

    public void SetHUDColor(Color color)
    {
        Character.color = color;
        combo.color = color;
        multiplier.color = color;
        comboTxt.color = color;
        multiplierTxt.color = color;
        this.color = color;
    }

    /// <summary>
    /// UpdateScore
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        if(score > 0)
            StartCoroutine(CountUpScore(score));
        else
            this.score.text = score.ToString();
        //float digit;
        //float decimals;
        //if (score >= 1000000)
        //{
        //    digit = score / 1000000f;
        //    digit = Mathf.Round(digit * 100.0f) / 100.0f;
        //    decimals = digit - (int)digit;
        //    this.score.text = digit.ToString() + "M";
        //}
        //else if (score >= 1000)
        //{
        //    digit = score / 1000f;
        //    digit = Mathf.Round(digit * 100.0f) / 100.0f;
        //    decimals = digit - (int)digit;
        //    this.score.text = digit.ToString() + "K";
        //}
        //else
        //{
        //    this.score.text = score.ToString();
        //}
    }
    /// <summary>
    /// Update Combo
    /// </summary>
    /// <param name="combo"></param>
    public void UpdateCombo(int combo)
    {
        this.combo.text = combo.ToString() + "x";
        StopCoroutine(ComboAnim());
        StartCoroutine(ComboAnim());
    }
    /// <summary>
    /// Update Multiplier
    /// </summary>
    /// <param name="combo"></param>
    public void UpdateMultiplier(int multiplier)
    {
        this.multiplier.text = multiplier.ToString() + "x";
    }
    /// <summary>
    /// Make ComboCounter Visible
    /// </summary>
    public void ShowCombo()
    {
        comboActive = true;
        StartCoroutine(MoveHandle(true));
        //combo.color = new Color(color.r, color.g, color.b, 1);
        //multiplier.color = new Color(color.r, color.g, color.b, 1);
    }
    /// <summary>
    /// Make ComboCounter InVisible
    /// </summary>
    public void HiddeCombo()
    {
        comboActive = false;
        StartCoroutine(MoveHandle(false));
        //combo.color = new Color(color.r, color.g, color.b, 0);
        //multiplier.color = new Color(color.r, color.g, color.b, 0);
    }
    /// <summary>
    /// Returns if Combo is Active or Not
    /// </summary>
    /// <returns></returns>
    public bool GetComboState()
    {
        return comboActive;
    }

    IEnumerator ComboAnim()
    {
        comboObj.localScale = new Vector3(1, 1, 1);
        float time = 0f;
        float duration = 1f;
        float speed = 5f;
        
        while(time < duration)
        {
            float xScale = xCurve.Evaluate(time);
            float yScale = yCurve.Evaluate(time);

            comboObj.localScale = new Vector3(xScale, yScale, 1);
            //multiplierObj.position = Vector3.Lerp(objPos, posOffset, time);
            
            time += Time.deltaTime * speed;

            yield return null;
        }
        
        comboObj.localScale = new Vector3(1, 1, 1);
        yield return null;
    }
    float cHPos = 0;
    IEnumerator MoveHandle(bool goIn)
    {
        if (!goIn)
        {
            yield return new WaitForSeconds(1f);
            if (GetComboState())
                StopCoroutine(MoveHandle(false));
                yield return null;
        }
        
        float time = 1f;
        float speed = 3f;

        while(time > 0f)
        {
            if (goIn)
            {
                comboHandel.position = Vector3.Lerp(cHCoolPos.position, cHStartPos, cHPos);
                cHPos += Time.deltaTime * speed;
            }
            else
            {
                comboHandel.position = Vector3.Lerp(cHCoolPos.position, cHStartPos, cHPos);
                cHPos -= Time.deltaTime * speed;
            }
            time -= Time.deltaTime * speed;
            yield return null;
        }

        if(cHPos > 1)
        {
            cHPos = 1;
        }
        else if(cHPos < 0)
        {
            cHPos = 0;
        }

        yield return null; 
    }

    IEnumerator CountUpScore(int score)
    {
        float time = 0;
        float endTime = 1f;
        float speed = 1f;
        float wigleSpeed = 1000f;
        float wiglePower = 10f;

        while(time < endTime)
        {
            float currentScore = Mathf.Lerp(i_score, score, time);

            DisplayScore((int)currentScore);

            this.score.transform.position = new Vector3(this.scoreTextPos.x + Mathf.PingPong(Time.time * wigleSpeed, wiglePower), this.scoreTextPos.y, this.scoreTextPos.z);
            this.score.transform.localScale = new Vector3(scoreTextScale.x, scoreTextScale.y + Mathf.PingPong(Time.time * wigleSpeed, 1.3f), scoreTextScale.z);

            time += Time.deltaTime * speed; ;
            yield return null;
        }
        i_score = score;

        DisplayScore(score);

        this.score.transform.position = this.scoreTextPos;
        this.score.transform.localScale = this.scoreTextScale;

        yield return null;
    }
    void DisplayScore(float score)
    {

        float digit;
        float decimals;
        if (score >= 1000000)
        {
            digit = score / 1000000f;
            digit = Mathf.Round(digit * 100.0f) / 100.0f;
            decimals = digit - (int)digit;
            this.score.text = digit.ToString() + "M";
        }
        else if (score >= 1000)
        {
            digit = score / 1000f;
            digit = Mathf.Round(digit * 100.0f) / 100.0f;
            decimals = digit - (int)digit;
            this.score.text = digit.ToString() + "K";
        }
        else
        {
            int iScore = (int)score;
            this.score.text = iScore.ToString();
        }
    }
}