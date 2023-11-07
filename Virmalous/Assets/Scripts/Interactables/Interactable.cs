using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Animator anim;

    [Header("Connection")]
    [Tooltip("Connected animator.")]
    public Animator connectedAnimator;
    [Tooltip("Bool name to toggle.")]
    public string boolName = "";

    [Header("Config")]
    [Tooltip("Is it a switch or a button?")]
    public bool isToggle = true;
    [Tooltip("IF BUTTON, Time before reset to deactivated state.")]
    public float resetTime = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        anim.SetBool("active", !anim.GetBool("active"));
        connectedAnimator.SetBool(boolName, !connectedAnimator.GetBool(boolName));
    }
}