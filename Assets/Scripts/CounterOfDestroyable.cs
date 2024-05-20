using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CounterOfDestroyable : MonoBehaviour
{
    [SerializeField] private int _numberOfObjects;
    [SerializeField] private Text _counter;

    private static CounterOfDestroyable _instance;
    public static CounterOfDestroyable Instance { get { return _instance; } }

    /*On peut itérer un string comme une liste, donc pour les objets, faire un truc ou je convertis
    le compteur en string, puis je récupère la valeur [0] et [1], et je peux les afficher avec des chiffres comme pour le projet transversal*/

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetInt("Counter") != 1)
        {
            PlayerPrefs.SetInt("Counter", 1);
        } else
        {
            Destroy(gameObject);
        }
        Debug.Log(_numberOfObjects);
        DisplayCounter();
    }

    public void DecrementCounter()
    {
        _numberOfObjects--;
        Debug.Log(_numberOfObjects);
        DisplayCounter();
        if (_numberOfObjects == 0) {
            SceneManager.LoadScene("Victory");
        }
    }

    private void DisplayCounter()
    {
        _counter.text = _numberOfObjects.ToString();
    }
}
