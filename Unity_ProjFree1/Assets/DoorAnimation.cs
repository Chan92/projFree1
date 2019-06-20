using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorAnimation : MonoBehaviour
{
    public Animator anim;
    bool isOpen = false;
    AudioSource SoundDoor;
    public GameObject keycard;
    
    void Start()
    {
        SoundDoor = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isOpen && keycard.activeSelf)
        {
            isOpen = true;
            SoundDoor.Play(0);
            anim.Play("DoorOpen", -1, float.NegativeInfinity);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isOpen && keycard.activeSelf)
        {
            isOpen = false;
            SoundDoor.Play(0);
            anim.Play("DoorClose", -1, float.NegativeInfinity);
        }
    }
}
