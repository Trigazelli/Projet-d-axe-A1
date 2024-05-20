using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroyable : MonoBehaviour
{
    private bool _isInTrigger = false;

    private string _name;

    // Update is called once per frame

    private void Start()
    {
        _name = SceneManager.GetActiveScene().name + gameObject.name;

        if (PlayerPrefs.GetInt(_name) == 1)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _isInTrigger)
        {
            _DestroyObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isInTrigger = false;
    }

    private void _DestroyObject()
    {
        PlayerPrefs.SetInt(_name, 1);
        Destroy(gameObject);
        CounterOfDestroyable.Instance.DecrementCounter();
    }
}
