using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static float energy = 100;
    public float energyDecayTime = 5;
    private float lastRemovedTime;
    private int currentDisplayedEnergy = 100;
    private int frameUpdateCount;
    // Start is called before the first frame update
    void Start()
    {
        lastRemovedTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastRemovedTime > energyDecayTime)
        {
            lastRemovedTime = Time.time;
            RemoveEnergy(1);
        }
        UpdateDisplay();

    }

    void UpdateDisplay()
    {
        if(currentDisplayedEnergy != (int)energy)
        {
            int dif = Mathf.Abs(currentDisplayedEnergy - (int)energy);
            int dir = (int) -Mathf.Sign(currentDisplayedEnergy - (int)energy);
            if(dif > 10)
            {
                currentDisplayedEnergy += dir;
                frameUpdateCount = 0;
            }
            else
            {
                int frames = (int) (10 / Mathf.Sqrt(dif));
                if(frameUpdateCount > frames)
                {
                    currentDisplayedEnergy += dir;
                    frameUpdateCount = 0;
                }
            }
        }
        frameUpdateCount++;

        text.text = currentDisplayedEnergy.ToString();
    }

    public static void AddEnergy(float amount)
    {
        energy += amount;
    }

    public static void RemoveEnergy(float amount)
    {
        energy -= amount;
    }
}
