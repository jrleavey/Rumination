using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public GameObject _light;

    public bool isFlickering = false;
    public float timeDelay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }

        
    }

    public IEnumerator FlickeringLight()
    {
        isFlickering = true;
        _light.SetActive(false);
        timeDelay = Random.Range(.01f, .2f);
        yield return new WaitForSeconds(timeDelay);
        _light.SetActive(true);
        timeDelay = Random.Range(.01f, .2f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
