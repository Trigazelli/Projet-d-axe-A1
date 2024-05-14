using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScene : MonoBehaviour
{
    [SerializeField] private Canvas _canva;
    [SerializeField] private Text _text;
    [SerializeField] private string _dialog;

    private bool _showDialog = false;
    // Start is called before the first frame update
    void Start()
    {
        _canva.enabled = false;
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        _toggleDialog();
    }

    private void _toggleDialog()
    {
        Debug.Log("in toggle");
        Debug.Log(Input.GetKeyDown(KeyCode.F));
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("input f");
            StartCoroutine(_waitOut());
            _showDialog = !_showDialog;
            Debug.Log(_showDialog);
            if (_showDialog)
            {
                _text.text = _dialog;
                _canva.enabled = true;
            }
            else
            {
                _canva.enabled = false;
            }
    }

    }

    private IEnumerator _waitOut()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
