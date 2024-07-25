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

    private void Awake()
    {
        Instance = this;
    }

    private void Win(string winner)
    {
        alertT.text = winner + " is Win!";
    }


}
