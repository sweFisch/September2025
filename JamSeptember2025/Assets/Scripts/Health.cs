using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 100;
    [SerializeField] private int _maxHealth = 100;

    [SerializeField] GameObject _deathEffect;

    private void Start()
    {
        // Get from stats?
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int value)
    {
        _currentHealth -= value;
        if ( _currentHealth <= 0)
        {
            Debug.Log($"Dead : {gameObject.name}");
            if (_deathEffect != null) 
            {
                GameObject deathGO = Instantiate(_deathEffect, transform.position,Quaternion.identity);
                Destroy(deathGO,0.5f); 
            }

            GameManager.Instance.PlayerDied(this.gameObject);

        }
    }


    public void Heal(int value)
    {
        _currentHealth += value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }
}
