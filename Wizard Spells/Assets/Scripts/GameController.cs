﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public GameObject SpellUIObj;
    public PlayerInput GameInput;
    public LayerMask EnemyLayers;
    public EnemyLockOn enemyLockOn;
}
