using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool _dialogueHasStarted = false;
    private bool _keyDown = false;

    public void TriggerDialogue()
    {
        Debug.Log("in trigger dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
        _dialogueHasStarted = true;
    }

    private void Update()
    {
        _keyDown = Input.GetKeyDown(KeyCode.F);
        Debug.Log("keydown" + _keyDown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.transform.tag);
        if (collision.gameObject.transform.CompareTag("CameraTriggerTarget"))
        {
            Debug.Log("in trigger 2d");
            if (_keyDown)
            {
                if (!_dialogueHasStarted)
                {
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
        
    }
}
