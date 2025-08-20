using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Collider doorCollider;        
    public GameObject exitTrigger;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip openGate;

    private bool isOpen = false;

    private void Start()
    {

        // Ban đầu cửa đóng
        if (doorCollider == null) doorCollider = GetComponent<Collider>();
        exitTrigger.SetActive(false);
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        SoundManager.Instance.PlaySFX(openGate);

        doorCollider.enabled = false;
        anim.SetTrigger("OpenGate");
        //gameObject.SetActive(false);
        exitTrigger.SetActive(true);
    }
}
