using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Text alertT;
    [SerializeField] float startDelay;
    [SerializeField] float endDelay;

    [SerializeField] Image hpBar_P1;
    [SerializeField] Image hpBar_P2;

    private void Awake()
    {
        Instance = this;
    }

    public void Win(string winner)
    {
        alertT.text = winner + " is Win!";
    }


    public void hpUpdate(int playerIndex, float hp)
    {
        if (playerIndex == 1)
            hpBar_P1.fillAmount = hp / 100;
        else
            hpBar_P2.fillAmount = hp / 100;
    }

}
