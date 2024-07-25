using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Sprite[] arrows;
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform[] SpawnPoints;
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

    private void Start()
    {
        GameObject player1 =  Instantiate(playerPrefabs[ValueManager.Instance.P1CharacterIndex], SpawnPoints[0].transform.position, Quaternion.identity);
        GameObject player2 = Instantiate(playerPrefabs[ValueManager.Instance.P2CharacterIndex], SpawnPoints[1].transform.position, Quaternion.identity);
        player1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = arrows[0];
        PlayerController p1Con = player1.GetComponent<PlayerController>();
        p1Con.moveLeft = KeyCode.A;
        p1Con.moveRight = KeyCode.D;
        p1Con.jumpKey = KeyCode.W;
        p1Con.attackKey = KeyCode.G;
        p1Con.skillKey = KeyCode.H;
        p1Con.ultimateKey = KeyCode.F;

        player2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = arrows[1];
        PlayerController p2Con = player2.GetComponent<PlayerController>();
        p2Con.moveLeft = KeyCode.LeftArrow;
        p2Con.moveRight = KeyCode.RightArrow;
        p2Con.jumpKey = KeyCode.UpArrow;
        p2Con.attackKey = KeyCode.Comma;
        p2Con.skillKey = KeyCode.Period;
        p2Con.ultimateKey = KeyCode.M;
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
