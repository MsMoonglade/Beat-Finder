using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceByValue : MonoBehaviour
{
    public static ReferenceByValue instance;

    [Space]
    [Header("Project References")]
    public AudioClip[] possibleClip;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Return Material At given Value
    /// </summary>
    /// <param name="i_value"><given value/param>
    /// <returns></returns>
    public T ReturnValue<T>(T[] possibleValue, float i_percentValue)
    {
        float arrayIndex = 1f / possibleValue.Length;

        int localArrayslot = Mathf.RoundToInt(i_percentValue / arrayIndex);

        localArrayslot = Mathf.Clamp(localArrayslot, 0, possibleValue.Length - 1);

        Debug.Log(localArrayslot + " with a percent of " + i_percentValue);

        return possibleValue[localArrayslot];
    }
}