using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnButtonPressed = new UnityEvent<int>();
    [SerializeField] PressableButton[] m_buttons;

    void OnEnable()
    {
        for (int i = 0; i < m_buttons.Length; i++)
        {
            int buttonIndex = i;
            m_buttons[i].OnClicked.AddListener(() => OnButtonPressed.Invoke(buttonIndex));
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < m_buttons.Length; i++)
        {
            // Using RemoveAllListeners incase RemoveListener is unable
            // to properly remove an arrow function
            m_buttons[i].OnClicked.RemoveAllListeners();
        }
    }
}
