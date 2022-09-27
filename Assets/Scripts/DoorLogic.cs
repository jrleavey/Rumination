using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
   public bool amIOpen = false;
   public bool canFlipamIOpen = true;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _openDoor;
    [SerializeField]
    private AudioClip _closeDoor;


    public GameObject innerDoor;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void DoorStuff()
    {
        if (amIOpen == false && canFlipamIOpen == true)
        {
            OpenMe();
            canFlipamIOpen = false;
            StartCoroutine(DoorOpenTimer());
            AudioSource.PlayClipAtPoint(_openDoor, transform.position);
        }
        else if (amIOpen == true && canFlipamIOpen == true)
        {
            CloseMe();
            canFlipamIOpen = false;
            StartCoroutine(DoorCloseTimer());
            AudioSource.PlayClipAtPoint(_closeDoor, transform.position);

        }
    }

    public void OpenMe()
    {
        amIOpen = true;
        innerDoor.GetComponent<Animator>().SetBool("amIOpenAnim", true);
        innerDoor.GetComponent<BoxCollider>().enabled = false;
    }

    public void CloseMe()
    {
        amIOpen = false;
        //play animation of door closing
        innerDoor.GetComponent<Animator>().SetBool("amIOpenAnim", false);
        innerDoor.GetComponent<BoxCollider>().enabled = true;


    }

    public IEnumerator DoorOpenTimer()
    {
        yield return new WaitForSeconds(1f);
        canFlipamIOpen = true;
    }
    public IEnumerator DoorCloseTimer()
    {
        yield return new WaitForSeconds(1.5f);
        canFlipamIOpen = true;
    }

}
