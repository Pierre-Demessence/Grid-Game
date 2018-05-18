using UnityEngine;

public class Monster : Character
{
    private static int _id = 1;
    private string _name;

    protected override void Start()
    {
        base.Start();
        _name = name;// "Monster " + _id++;
    }

    public void DoTurn()
    {
        Vector2Int dir;
        switch (Random.Range(0, 4))
        {
            case 0:
                dir = Vector2Int.up;
                break;
            case 1:
                dir = Vector2Int.right;
                break;
            case 2:
                dir = Vector2Int.down;
                break;
            case 3:
                dir = Vector2Int.left;
                break;
            default:
                dir = Vector2Int.zero;
                break;
        }

        // Attack
        Character target;
        if ((target = GameManager.GetCharacter(DirToPos(dir))) is Player) 
            Attack(target);

        Move(dir);
    }

    protected override string Name
    {
        get { return _name; }
    }
    protected override int MaxHealth
    {
        get { return 1; }
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
        Destroy(gameObject);
    }
}