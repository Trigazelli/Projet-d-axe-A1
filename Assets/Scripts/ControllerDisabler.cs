using UnityEngine;

public class ControllerDisabler : MonoBehaviour
{
    private static ControllerDisabler _instance;
    public static ControllerDisabler Instance {  get { return _instance; } }

    private HeroController _controller;

    [HideInInspector] public bool InDialogue;
    [HideInInspector] public bool InBlending;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null) _instance = this;
        _controller = FindObjectOfType<HeroController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InDialogue || InBlending)
        {
            _controller.enabled = false;
        } else
        {
            _controller.enabled = true;
        }
    }
}
