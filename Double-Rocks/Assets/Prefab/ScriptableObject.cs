using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable/Health", order = 1)]
public class DataScriptable : ScriptableObject
{
    public int startHealth;
    public int damage;
}
