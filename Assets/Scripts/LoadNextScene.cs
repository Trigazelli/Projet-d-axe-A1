using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("CameraTriggerTarget"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(_sceneName);
            }
        }
    }
}
