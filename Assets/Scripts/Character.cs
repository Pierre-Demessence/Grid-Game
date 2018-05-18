using System;
using DG.Tweening;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private int _currentHealth;

    private Vector2Int _position;

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

    public Vector2Int Position
    {
        get { return _position; }
        private set
        {
            _position = value;
            var transformPosition = new Vector3(Position.x, Position.y);
            //transform.position = transformPosition;
            transform.DOMove(transformPosition, 0.1250f);
        }
    }

    protected abstract void OnDeath();

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
        Position = new Vector2Int((int) transform.position.x, (int) transform.position.y);
        GameManager = FindObjectOfType<GameManager>();
    }

    protected void Move(Vector2Int dir)
    {
        if (CanMove(dir))
            Position = DirToPos(dir);
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
        transform.DOMove(target.Position.ToVector3Int(), 0.125f).onComplete = () => transform.DOMove(Position.ToVector3Int(), 0.125f);
    }

    protected Vector2Int DirToPos(Vector2Int dir)
    {
        return Vector2Int.FloorToInt(Position + dir * GameManager.MoveOffset);
    }
    

    protected bool CanMove(Vector2Int dir)
    {
        return GameManager.CanMove(DirToPos(dir));
    }

    private void OnGUI()
    {
        var guiPos = Camera.main.WorldToScreenPoint(transform.position);

        const int guiWidth = 64;
        const int guiHeight = 20;
        GUI.Box(new Rect(guiPos.x - (guiWidth - 64) / 2, Screen.height - guiPos.y - guiHeight - 20, guiWidth, guiHeight), Name);
        GUI.Box(new Rect(guiPos.x - (guiWidth - 64) / 2, Screen.height - guiPos.y - guiHeight, guiWidth, guiHeight), _currentHealth + "/" + MaxHealth);
    }
}