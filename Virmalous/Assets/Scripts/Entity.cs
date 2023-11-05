using System.Collections;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public class EntityState
{
    [Header("States")]
    public float stunned = 0f;
    public float onFire = 0f;
    public float frozen = 0f;
    public float electrified = 0f;

    public float _stunned
    {
        get { return stunned; }
        set
        {
            stunned = value;
            lastStunTime = Time.time;
        }
    }

    public float _onFire
    {
        get { return onFire; }
        set
        {
            onFire = value;
            lastOnFireTime = Time.time;
        }
    }

    public float _frozen
    {
        get { return frozen; }
        set
        {
            frozen = value;
            lastFrozenTime = Time.time;
        }
    }

    public float _electrified
    {
        get { return electrified; }
        set
        {
            electrified = value;
            lastElectrifiedTime = Time.time;
        }
    }

    [Header("Config")]
    [Tooltip("How long will stunning this Entity last")]
    public float stunnedTime = 0f;
    [Tooltip("How long will setting this Entity on fire last")]
    public float onFireTime = 0f;
    [Tooltip("How long will freezing this Entity last")]
    public float frozenTime = 0f;
    [Tooltip("How long will electrifying this Entity last")]
    public float electrifiedTime = 0f;

    [HideInInspector] public float lastStunTime = 0f;
    [HideInInspector] public float lastOnFireTime = 0f;
    [HideInInspector] public float lastFrozenTime = 0f;
    [HideInInspector] public float lastElectrifiedTime = 0f;
}



public abstract class Entity : MonoBehaviour
{
    public EntityState entityState = new EntityState();

    public void Update()
    {
        //Cooldown checks
        if (Time.time - entityState.lastStunTime >= entityState.stunnedTime) { entityState.stunned = 0; }
        if (Time.time - entityState.lastOnFireTime >= entityState.onFireTime) { entityState.onFire = 0; }
        if (Time.time - entityState.lastFrozenTime >= entityState.frozenTime) { entityState.frozen = 0; }
        if (Time.time - entityState.lastElectrifiedTime >= entityState.electrifiedTime) { entityState.electrified = 0; }
    }
}