using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    [SerializeField] private Transform[] _detectionPoints;
    [SerializeField] private LayerMask _layerMask;

    private bool _dialogueHasStarted = false;
    private bool _keyDown = false;

    public void TriggerDialogue()
    {
        Debug.Log("in trigger dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
        DialogueManager.Instance.DisplayNextSentence();
        _dialogueHasStarted = true;
    }

    private void Update()
    {
        // supprimer si ça n'apparait que là
        _keyDown = Input.GetKeyDown(KeyCode.F);
        Debug.Log("keydown" + _keyDown);

/*        foreach (Transform detectionPoint in _detectionPoints)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(
            transform.position,
            Vector3.forward,
            Mathf.Infinity,
            _layerMask);
            Debug.Log(hit2D.collider);
            if (hit2D.collider != null)
            {
                if (_keyDown)
                {
                    if (!_dialogueHasStarted)
                    {
                        TriggerDialogue();
                        _dialogueHasStarted = true;
                    }
                    else
                    {
                        if (!DialogueManager.Instance.DisplayNextSentence())
                        {
                            _dialogueHasStarted = false;
                        }
                    }
                }
            }
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.transform.tag);
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
        }

    }
}
