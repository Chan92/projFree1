using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public string Sound;
    public GameObject DisableObject;

    private void Update()
    {
        if (DisableObject != null)
        {
            if (DisableObject.activeSelf == true)
            {
                gameObject.SetActive(false);
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Sound != null)
        {
            gameObject.SetActive(false);
            other.GetComponent<AudioManager>().playSound(Sound);
        }
    }
}
