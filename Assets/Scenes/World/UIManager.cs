using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Text xpText;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject menu;
    [SerializeField] private bool airstrike= false;

    private void Awake()
    {
        Assert.IsNotNull(xpText);
        Assert.IsNotNull(levelText);
        Assert.IsNotNull(menu);
    }

    public void updateLevel(int level)
    {
        levelText.text = level.ToString();
    }
    public void updateXP(int currentXP, int requiredXP)
    {
        xpText.text = currentXP.ToString() + " / " + requiredXP.ToString();
    }

    public void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSecondsRealtime(5);
        print(Time.time);
    }   

    public void toggleAirStrike(){
        airstrike= !airstrike;
        Debug.Log("a:" + airstrike);

        if(airstrike){
            // StartCoroutine(Example());
            SpawnOnCoordinates.airstrike= this.airstrike;
            SpawnOnCoordinates.count++;
        }
        else {
            // StartCoroutine(Example());
            SpawnOnCoordinates.airstrike= false;
        }
    }
}