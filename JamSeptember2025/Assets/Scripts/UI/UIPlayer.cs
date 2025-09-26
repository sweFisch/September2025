using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{

    [SerializeField] Image _image;
    [SerializeField] Image _livesImage;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] Transform _livesBox;
    
    [SerializeField] private Transform deathSprite;

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
        // Set UI sprite to player sprite
    }

    public void SetText(string newtext)
    {
        _textMeshProUGUI.text = newtext;
    }

    public void SetMaxLife(int nrOfLives)
    {
        Transform lifeChild = _livesBox.GetComponentInChildren<Transform>();

        for (int i = _livesBox.childCount; i < nrOfLives; i++) 
        {
            //print("making box??");
            Instantiate(_livesImage, _livesBox);
        }
    }


    public void SetCurrentLife(int currentLife)
    {
        Transform[] allLives = _livesBox.GetComponentsInChildren<Transform>(true);

        //print(allLives.Length); // TODO

        currentLife = Mathf.Clamp(currentLife, 0, allLives.Length);

        for (int i = 0; i < allLives.Length; i++)
        {
            if (i <= currentLife)
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
        // Set player UI status to Dead
        //Debug.Log("Player UI object set death");
        deathSprite.gameObject.SetActive(true);
    }

    public void SetAlive()
    {
        deathSprite.gameObject.SetActive(false);
    }

}
