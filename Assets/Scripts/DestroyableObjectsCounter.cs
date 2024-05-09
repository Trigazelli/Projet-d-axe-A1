using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DestroyableObjectsCounter : MonoBehaviour
{
    [SerializeField] private int _numberOfObjects;
    [SerializeField] private Sprite[] _dozensNumber;
    [SerializeField] private Sprite[] _unitsNumber;
    [SerializeField] private Text _counter;

    private DestroyableObjectsCounter _instance;
    public DestroyableObjectsCounter Instance { get { return _instance; } }

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
    }

    public void DecrementCounter()
    {
        _numberOfObjects--;
        DisplayCounter();
    }

    private void DisplayCounter()
    {
        _counter.text = _numberOfObjects.ToString();
    }
}
