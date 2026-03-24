using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetChanceItem : MonoBehaviour
{
    public Image m_petImage;
    public TMP_Text m_petChance;
    public GameObject m_questionMark;
    public GameObject m_image;

    public void SetPetChanceItem(Sprite petSprite, int chance)
    {
        m_petImage.sprite = petSprite;
        m_petChance.text = ZString.Concat(chance, "%");    
    }

    public void SetPetUnlocked(bool state)
    {
        this.LogError(state);
        m_questionMark.SetActive(!state);
        m_image.SetActive(state);
    }
}
