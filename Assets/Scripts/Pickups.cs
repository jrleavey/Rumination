using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _pickupID;

    void Start()
    {
        _player = GameObject.Find("Player");

       
    }

    void Update()
    {
        
    }

    private void HealthPickup()
    {
        _player.GetComponent<PlayerController>().Heal();
    }
    private void PistolAmmoPickup()
    {
        _player.GetComponent<PlayerController>().HandgunAmmoPickup();

    }
    private void ShotgunAmmoPickup()
    {
        _player.GetComponent<PlayerController>().ShotgunAmmoPickup();

    }
    private void KeyPickup()
    {
        _player.GetComponent<PlayerController>().KeyPickup();

    }
}
