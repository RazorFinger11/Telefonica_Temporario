using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointManager : MonoBehaviour
{
    public float defaultPoints;
    public float curPoints;
    public float requiredPoints;

    public Text pointsText;

    // Start is called before the first frame update
    void Start()
    {
        curPoints = defaultPoints;
    }

    // Update is called once per frame
    void Update()
    {
        // PLACEHOLDER 
        pointsText.text = "Pontuação: " + curPoints;
    }

    public void AddPoints(int value)
    {
        curPoints += value;
    }
}
