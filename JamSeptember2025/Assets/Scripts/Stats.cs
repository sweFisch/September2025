using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] Sprite[] _playerSprites;
    [SerializeField] int _index = 0;
    [SerializeField] SpriteRenderer _spriteRenderer;

    float health;
    [SerializeField] float movespeedBase;
    float movespeedMod;
    float jumpPower; //jumpaccelleration
    float gravity; //fall acelleration
    float friction; //



    private void Start()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        _spriteRenderer.sprite = _playerSprites[_index];
    }

    public void SetSprite(int index)
    {
        _index = index;
        SetSprite();
    }



    float movespeedBuffTimer;
    public void AddMSBuff(float time)
    {
        print(gameObject.name + "used speed buff");
        movespeedBuffTimer = Math.Max(movespeedBuffTimer, time) + Time.time;
    }

    private void Update()
    {
        movespeedMod = movespeedBase;

        if ( Time.time < movespeedBuffTimer) { movespeedMod *= 2; }
        // if (( Time.time < movespeedDebuffTimer) { movespeedmod /= 2; }

    }
}
