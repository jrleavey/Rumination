using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool isMoving;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _speed;
    private float _horizMove;
    private float _vertMove;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private int _handgunAmmo;
    [SerializeField]
    private int _shotgunAmmo;
    [SerializeField]
    private float _currentHP;
    private float _maxHp;

    private void Awake()
    {
        
    }

    void Start()
    {
        _rotateSpeed = 150f;
        _speed = 4f;
    }

    void Update()
    {
        TankControls();
    }

    private void TankControls()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            isMoving = true;
            _horizMove = Input.GetAxis("Horizontal") * Time.deltaTime * _rotateSpeed;
            _vertMove = Input.GetAxis("Vertical") * Time.deltaTime * _speed;
            this.gameObject.transform.Rotate(0, _horizMove, 0);
            this.gameObject.transform.Translate(0, 0, _vertMove);
        }
        else
        {
            isMoving = false;

        }
    }

    public void Heal()
    {
        Debug.Log("Player has healed");
    }
    public void HandgunAmmoPickup()
    {
        Debug.Log("Player has picked up rounds");
    }
    public void ShotgunAmmoPickup()
    {
        Debug.Log("Player has picked up shells");
    }

    public void KeyPickup()
    {
        //Array needed for keys
        Debug.Log("Player has picked up Key");

    }
}
