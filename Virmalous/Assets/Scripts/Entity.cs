using System.Collections;
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
        get
        {
            return stunned;
        }
        set
        {
            stunned = value;
            lastStunTime = Time.time;
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

    protected float lastStunTime = 0f;
    protected float lastOnFireTime = 0f;
    protected float lastFrozenTime = 0f;
    protected float lastElectrifiedTime = 0f;
}

public abstract class Entity : MonoBehaviour
{
    public EntityState entityState = new EntityState();
}