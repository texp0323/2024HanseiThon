using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Text alertT;
    [SerializeField] float startDelay;
    [SerializeField] float endDelay;

    [SerializeField] Image hpBar_P1;
    [SerializeField] Image hpBar_P2;
    [SerializeField] Image mpBar_P1;
    [SerializeField] Image mpBar_P2;

    bool isGaming = false;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(GameStart());
    }

    public bool getIsGaming()
    {
        return isGaming;
    }

    public void Win(string winner)
    {
        alertT.text = winner + " Win!";
        isGaming = false;
        StartCoroutine(GameEnd());
    }

    IEnumerator GameStart()
    {
        alertT.text = "Ready";
        yield return new WaitForSeconds(startDelay);
        alertT.text = "FIGHT";
        isGaming = true;
        yield return new WaitForSeconds(0.5f);
        alertT.text = "";
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(endDelay);
        SceneManager.LoadScene(0);
    }


    public void hpUpdate(int playerIndex, float hp)
    {
        if (playerIndex == 1)
            hpBar_P1.fillAmount = hp / 100;
        else
            hpBar_P2.fillAmount = hp / 100;
    }
    public void mpUpdate(int playerIndex, float mp)
    {
        if (playerIndex == 1)
            mpBar_P1.fillAmount = mp / 100;
        else
            mpBar_P2.fillAmount = mp / 100;
    }

}
