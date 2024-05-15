using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    [SerializeField] private Transform[] _detectionPoints;
    [SerializeField] private LayerMask _layerMask;

    private bool _dialogueHasStarted = false;
    private bool _isInTrigger = false;

    public void TriggerDialogue()
    {
        Debug.Log("in trigger dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
        _dialogueHasStarted = true;
    }

    private void Update()
    {
        Debug.Log("in trigger 2d");
        if (!_isInTrigger) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_dialogueHasStarted)
            {
                Debug.Log("StartDialogue");
                TriggerDialogue();
            }
            else
            {
                if (!DialogueManager.Instance.DisplayNextSentence())
                {
                    _dialogueHasStarted = false;
                }
            }
            Debug.Log("in input F");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isInTrigger = true;
/*        Debug.Log(collision.gameObject.transform.tag);
        if (collision.gameObject.transform.CompareTag("CameraTriggerTarget"))
        {
            Debug.Log("in trigger 2d");
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!_dialogueHasStarted)
                {
                    Debug.Log("StartDialogue");
                    TriggerDialogue();
                }
                else
                {
                    if (!DialogueManager.Instance.DisplayNextSentence())
                    {
                        _dialogueHasStarted = false;
                    }
                }
                Debug.Log("in input F");
            }
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isInTrigger = false;
    }
}
