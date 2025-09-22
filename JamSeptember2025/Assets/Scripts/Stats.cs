using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] Sprite[] _playerSprites;
    [SerializeField] int _index = 0;
    [SerializeField] SpriteRenderer _spriteRenderer;



    private void Start()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        _spriteRenderer.sprite = _playerSprites[_index];
    }
}
