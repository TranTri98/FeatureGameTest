using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited!");
            // SceneManager.LoadScene("NextLevel");
            GameManager.Instance.NotifyPlayerWon();
        }
    }
}