using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : Character
{
    private readonly int _armorDefense = 0;

    private readonly int _weaponDamage = 0;

    protected override int Defense
    {
        get { return base.Defense + _armorDefense; }
    }

    protected override int Damage
    {
        get { return base.Damage + _weaponDamage; }
    }
    protected override int BaseDefense
    {
        get { return 0; }
    }
    protected override int BaseDamage
    {
        get { return 1; }
    }
    
    protected override void OnDeath()
    {
            
    }

    protected override string Name
    {
        get { return "Player"; }
    }
    protected override int MaxHealth
    {
        get { return 20; }
    }

    public bool DoTurn()
    {
        var dir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow))
            dir = Vector2.down;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir = Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        
        // Wait
        if (Input.GetKey(KeyCode.Space))
            return true;

        if (dir == Vector2.zero) return false;

        // Attack
        Character target;
        if ((target = GameManager.GetCharacter(DirToPos(dir))) != null)
        {
            Attack(target);
            return true;
        }

        // Move
        if (CanMove(dir))
        {
            Move(dir);
            return true;
        }

        return false;
    }
}