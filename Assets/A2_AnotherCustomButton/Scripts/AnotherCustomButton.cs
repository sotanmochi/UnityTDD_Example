using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnotherCustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _duration;
    
    public event Action Tap;
    public event Action LongPress;

    private CustomButtonContext _context;

    private void Awake()
    {
        // Initialize
        _context = new CustomButtonContext();
        _context.SetLongPressDuration(_duration);

        // Debug
        _context.OnLongPressed += () => print("long!!!");
        _context.OnTapped += () => print("tap!");

        // Event
        _context.OnTapped += Tap;
        _context.OnLongPressed += LongPress;

        // Animation
        _context.OnPressed += () => _animator.SetTrigger("Pressed");
        _context.OnReleased += () => _animator.SetTrigger("Released");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _context.SetButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _context.SetButtonUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _context.SetButtonExit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _context.SetButtonEnter();
    }
}
