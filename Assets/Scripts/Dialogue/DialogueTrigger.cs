using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool _dialogueHasStarted = false;

    public void TriggerDialogue()
    {
        Debug.Log("in trigger dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
        _dialogueHasStarted = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.CompareTag("CameraTriggerTarget")) return;
        Debug.Log("in trigger 2d");
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_dialogueHasStarted)
            {
                TriggerDialogue();
                _dialogueHasStarted = true;
            } else
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
