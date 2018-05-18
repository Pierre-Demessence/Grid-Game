using System;
using DG.Tweening;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private int _currentHealth;

    private Vector3 _guiPos;

    protected GameManager GameManager;
    
    public bool IsDead
    {
        get { return _currentHealth <= 0; }
    }

    protected abstract string Name { get; }
    protected abstract int MaxHealth { get; }

    protected virtual int Defense
    {
        get { return BaseDefense; }
    }

    protected virtual int Damage
    {
        get { return BaseDamage; }
    }
    protected abstract int BaseDefense { get; }

    protected abstract int BaseDamage { get; }

    protected abstract void OnDeath();

    protected virtual void Start()
    {
        _currentHealth = MaxHealth; 
        GameManager = FindObjectOfType<GameManager>();
    }

    protected void Move(Vector2 dir)
    {
        if (CanMove(dir))
            transform.DOMove(DirToPos(dir), 0.1250f);
            //transform.position = DirToPos(dir);
    }

    private void GetHit(int damage)
    {
        var finalDamage = Math.Max(damage - Defense, 0);
        _currentHealth = Math.Max(_currentHealth - finalDamage, 0);
        GameManager.Log(Name + " reduces dmg by " + Defense + " and takes " + finalDamage + " dmg.");

        if (IsDead)
        {
            GameManager.Log(Name + " dies.");
            OnDeath();
        }
    }

    protected void Attack(Character target)
    {
        GameManager.Log(Name + " attacks " + target.Name + " for " + Damage + " dmg.");
        target.GetHit(Damage);
    }

    protected Vector3Int DirToPos(Vector2 dir)
    {
        return Vector3Int.FloorToInt(transform.position + (Vector3) (dir * GameManager.MoveOffset));
    }
    
    protected Vector3Int DirToPosCeil(Vector2 dir)
    {
        return Vector3Int.CeilToInt(transform.position + (Vector3) (dir * GameManager.MoveOffset));
    }

    protected bool CanMove(Vector2 dir)
    {
        return GameManager.CanMove(DirToPosCeil(dir));
    }

    private void OnGUI()
    {
        _guiPos = Camera.main.WorldToScreenPoint(transform.position);

        const int guiWidth = 64;
        const int guiHeight = 20;
        GUI.Box(new Rect(_guiPos.x - (guiWidth - 64) / 2, Screen.height - _guiPos.y - guiHeight - 20, guiWidth, guiHeight), Name);
        GUI.Box(new Rect(_guiPos.x - (guiWidth - 64) / 2, Screen.height - _guiPos.y - guiHeight, guiWidth, guiHeight), _currentHealth + "/" + MaxHealth);
    }
}