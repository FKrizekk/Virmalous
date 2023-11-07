using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

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
    [Space]
    public float stunDamageMultiplier = 1f;
    public float fireDamageMultiplier = 1f;
    public float freezeDamageMultiplier = 1f;
    public float electricityDamageMultiplier = 1f;

    [HideInInspector] public float lastStunTime = 0f;
    [HideInInspector] public float lastOnFireTime = 0f;
    [HideInInspector] public float lastFrozenTime = 0f;
    [HideInInspector] public float lastElectrifiedTime = 0f;
}



public abstract class Entity : MonoBehaviour
{
    public EntityState entityState = new EntityState();

    [Tooltip("References to the VFX parents")]
    public GameObject[] VFXParents;

    public GameObject electricityArcPrefab;
    public float electricitySpreadDistance = 5f;

    Dictionary<GameObject[], Entity> arcs = new Dictionary<GameObject[], Entity>();

    void HandleElectricity()
    {
        //Electricity spreading
        if (entityState.electrified > 0 && gameObject.tag != "Player")
        {
            Entity[] entities = GameObject.FindObjectsOfType<Entity>();
            foreach (var entity in entities)
            {
                if (Vector3.Distance(entity.transform.position, transform.position) < electricitySpreadDistance)
                {
                    if (entity.entityState.electrified == 0 && entity.tag != "Player" && !arcs.ContainsValue(entity))
                    {
                        entity.entityState.electrified = entityState.electrified;
                        entity.entityState.lastElectrifiedTime = entityState.lastElectrifiedTime;
                        Transform arc = Instantiate(electricityArcPrefab, transform).transform;
                        Transform pos1 = arc.GetChild(0);
                        Transform pos2 = arc.GetChild(1);
                        arcs.Add(new GameObject[]{ arc.gameObject, pos1.gameObject, pos2.gameObject}, entity);

                        pos1.parent = transform;
                        pos2.parent = entity.transform;

                        pos1.position = GetComponentInChildren<Renderer>().bounds.center;
                        pos2.position = entity.GetComponentInChildren<Renderer>().bounds.center;
                    }
                }
            }
        }
        else
        {
            var tempArcs = new Dictionary<GameObject[], Entity>(arcs);
            foreach (var arc in arcs.Keys)
            {
                tempArcs.Remove(arc);
                foreach(var subArc in arc)
                {
                    Destroy(subArc);
                }
            }
            arcs = tempArcs;
        }
    }

    public void Update()
    {
        //VFXParents[0].SetActive(entityState.stunned > 0);
        //VFXParents[1].SetActive(entityState.onFire > 0);
        //VFXParents[2].SetActive(entityState.frozen > 0);
        VFXParents[3].SetActive(entityState.electrified > 0);

        HandleElectricity();

        //Cooldown checks
        if (Time.time - entityState.lastStunTime >= entityState.stunnedTime) { entityState.stunned = 0; }
        if (Time.time - entityState.lastOnFireTime >= entityState.onFireTime) { entityState.onFire = 0; }
        if (Time.time - entityState.lastFrozenTime >= entityState.frozenTime) { entityState.frozen = 0; }
        if (Time.time - entityState.lastElectrifiedTime >= entityState.electrifiedTime) { entityState.electrified = 0; }
    }
}