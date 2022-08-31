using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [SerializeField]
    private GameObject _player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _player = GameObject.Find("Player");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
