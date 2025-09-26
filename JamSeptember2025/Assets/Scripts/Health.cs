using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _currentHealth = 100;
    [SerializeField] private float _maxHealth = 100;

    [SerializeField] GameObject _deathEffect;
    [SerializeField] GameObject _damageParticle;
    [SerializeField] GameObject _deathParticle;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(float maxValue) {  _maxHealth = maxValue; }


    public void TakeDamage(float value)
    {
        _currentHealth -= value;
        GameManager.Instance._controlCam.CameraShake(0.2f, 0.3f);
        if ( _currentHealth <= 0)
        {
            Debug.Log($"Dead : {gameObject.name}");
            if (_deathEffect != null) 
            {
                GameObject deathGO = Instantiate(_deathEffect, transform.position,Quaternion.identity);
                Destroy(deathGO,0.5f); 
            }

            if (_deathParticle != null)
            {
                Instantiate(_deathParticle, transform.position, Quaternion.identity);
            }
            GameManager.Instance.PlayerDied(this.gameObject);
        }
        else { Instantiate(_damageParticle, transform.position, Quaternion.identity); }
    }


    public void Heal(float value)
    {
        _currentHealth += value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }
}
