using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Transform[] characterImages;
    int p1SelectIndex = 0;
    int p2SelectIndex = 1;

    [SerializeField] private Sprite[] SelectImages;
    [SerializeField] private Image p1SelectImage;
    [SerializeField] private Image p2SelectImage;

    [SerializeField] private Transform SelectMark1P;
    [SerializeField] private Transform SelectMark2P;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) && p1SelectIndex < characterImages.Length - 1)
            p1SelectIndex++;
        if(Input.GetKeyDown(KeyCode.A) && p1SelectIndex > 0)
            p1SelectIndex--;

        if (Input.GetKeyDown(KeyCode.RightArrow) && p2SelectIndex < characterImages.Length - 1)
            p2SelectIndex++;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && p2SelectIndex > 0)
            p2SelectIndex--;

        SelectMark1P.position = characterImages[p1SelectIndex].position + new Vector3 (-80, 50, 0);
        SelectMark2P.position = characterImages[p2SelectIndex].position + new Vector3(80, -50, 0);

        p1SelectImage.sprite = SelectImages[p1SelectIndex];
        p2SelectImage.sprite = SelectImages[p2SelectIndex];

        ValueManager.Instance.P1CharacterIndex = p1SelectIndex;
        ValueManager.Instance.P2CharacterIndex = p2SelectIndex;
    }
}
