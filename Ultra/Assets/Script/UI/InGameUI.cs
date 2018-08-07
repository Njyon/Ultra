using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Text & Images")]
    [SerializeField] Text combo;
    [SerializeField] Text comboTxt;
    [SerializeField] Text interimResultScore;
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
    Vector3 intermTextPos;
    Vector3 scoreTextPos;
    Vector3 scoreTextScale;

    MyCharacter character;
    float interimResult;

    void Start()
    {
        intermTextPos = interimResultScore.rectTransform.position;
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
        UptadeInterimResult();
        this.combo.text = combo.ToString() + "x";
        StopCoroutine(ComboAnim(comboObj));
        StartCoroutine(ComboAnim(comboObj));
    }
    /// <summary>
    /// Update Multiplier
    /// </summary>
    /// <param name="combo"></param>
    public void UpdateMultiplier(int multiplier)
    {
        UptadeInterimResult();
        this.multiplier.text = multiplier.ToString() + "x";
        StopCoroutine(ComboAnim(multiplierObj));
        StartCoroutine(ComboAnim(multiplierObj));
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
    public void GetCharacter(MyCharacter character)
    {
        this.character = character;
    }
    void UptadeInterimResult()
    {
        float oldInterimResult = interimResult;
        // Calc new Score
        interimResult = character.scoreFactor * (character.sqrtA * Mathf.Sqrt(character.hitFactor * character.hitCounter + character.combo + character.sqrtB) + character.sqrtC);
        if (interimResult < 1)
            return;
        if (oldInterimResult < 1)
            oldInterimResult = 0;

        StopCoroutine(CountUpInterScore(oldInterimResult));
        StartCoroutine(CountUpInterScore(oldInterimResult));
    }

    IEnumerator ComboAnim(RectTransform go)
    {
        go.localScale = new Vector3(1, 1, 1);
        float time = 0f;
        float duration = 1f;
        float speed = 5f;
        
        while(time < duration)
        {
            float xScale = xCurve.Evaluate(time);
            float yScale = yCurve.Evaluate(time);

            go.localScale = new Vector3(xScale, yScale, 1);
            //multiplierObj.position = Vector3.Lerp(objPos, posOffset, time);
            
            time += Time.deltaTime * speed;

            yield return null;
        }
        
        go.localScale = new Vector3(1, 1, 1);
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

    IEnumerator CountUpInterScore(float oldScore)
    {
        float outPut;
        float time = 0.5f;
        float speed = 1 * time;
        float lerp = 1;
        float wigleSpeed = 1000f;
        float wiglePower = 10f;

        int i_outPut;
        interimResultScore.text = "+" + interimResult.ToString();

        while (time > 0)
        {
            //outPut = Mathf.Lerp(interimResult, oldScore, lerp);
            //i_outPut = (int)outPut;

            //interimResultScore.text = "+" + i_outPut.ToString();

            interimResultScore.transform.position = new Vector3(this.intermTextPos.x + Mathf.PingPong(Time.time * wigleSpeed, wiglePower), this.intermTextPos.y, this.intermTextPos.z);
          
            lerp -= Time.deltaTime * speed;
            time -= Time.deltaTime;

            yield return null;
        }
        this.interimResultScore.rectTransform.position = intermTextPos;
        outPut = interimResult;
        i_outPut = (int)outPut;

        interimResultScore.text = "+" + i_outPut.ToString();

        yield return null;
    }
    IEnumerator CountUpScore(int score)
    {
        float time = 0;
        float endTime = 1f;
        float speed = 1f;
        float wigleSpeed = 1000f;
        float wiglePower = 10f;
        
        float outPut;
        int i_outPut;

        while (time < endTime)
        {
            float currentScore = Mathf.Lerp(i_score, score, time);
            outPut = Mathf.Lerp(interimResult, 0, time);

            DisplayScore((int)currentScore);

            i_outPut = (int)outPut;
            interimResultScore.text = "+" + i_outPut.ToString();

            interimResultScore.transform.position = new Vector3(this.intermTextPos.x + Mathf.PingPong(Time.time * wigleSpeed, wiglePower), this.intermTextPos.y, this.intermTextPos.z);
            
            this.score.transform.position = new Vector3(this.scoreTextPos.x + Mathf.PingPong(Time.time * wigleSpeed, wiglePower), this.scoreTextPos.y, this.scoreTextPos.z);
            this.score.transform.localScale = new Vector3(scoreTextScale.x, scoreTextScale.y + Mathf.PingPong(Time.time * wigleSpeed, 1.3f), scoreTextScale.z);

            time += Time.deltaTime * speed; ;
            yield return null;
        }
        i_score = score;
        outPut = 0;
        i_outPut = (int)outPut;

        DisplayScore(score);
        interimResultScore.text = "+" + i_outPut.ToString();

        this.interimResultScore.rectTransform.position = intermTextPos;

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