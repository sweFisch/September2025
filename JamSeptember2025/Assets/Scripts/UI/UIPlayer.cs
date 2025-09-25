using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{

    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] Transform _livesBox;

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
        // Set UI sprite to player sprite
    }

    public void SetText(string newtext)
    {
        _textMeshProUGUI.text = newtext;
    }

    public void SetMaxlife(int nrOfLives)
    {
        Transform lifeChild = _livesBox.GetComponentInChildren<Transform>(true);
        
        for (int i = _livesBox.childCount; i < nrOfLives; i++) 
        {
            Instantiate(lifeChild, _livesBox);
        }
    }


    public void SetCurrentLife(int currentLife)
    {
        Transform[] allLives = _livesBox.GetComponentsInChildren<Transform>(true);
        
        currentLife = Mathf.Clamp(currentLife, 0, allLives.Length);

        for (int i = 0; i < allLives.Length; i++)
        {
            if (i < currentLife)
            {
                allLives[i].gameObject.SetActive(true);
            }
            else
            {
                allLives[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetDeath()
    {
        // Set blood puddle texture 
    }

}
