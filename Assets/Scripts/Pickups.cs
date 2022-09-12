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
        _player.GetComponent<PlayerController>().Heal(1);
    }
    private void PistolAmmoPickup()
    {
        _player.GetComponent<PlayerController>().HandgunAmmoPickup();

    }
    private void ShotgunAmmoPickup()
    {
        _player.GetComponent<PlayerController>().ShotgunAmmoPickup();

    }
    private void KeyPickup(int keyID)
    {
        // Need to decide if we have multiple keys, or if there is only one key (or one key at a time)
        // If the player only has one key at a time, we can use a bool "_playerHasKey" and flip it when the key is consumed
        // If multiple keys are needed and utilized at the same time (player has 2 keys that open different areas, for instance) 
        // Then we need a series of bools on a switch statement based on keyID
        _player.GetComponent<PlayerController>().KeyPickup();

    }
}
