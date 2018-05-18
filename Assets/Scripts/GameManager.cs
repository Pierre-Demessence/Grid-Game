using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private float _lastTurnTime;
    [SerializeField] private GameObject _monsters;
    [SerializeField] private Player _player;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private LogManager _logManager;
    private int _turn = 1;

    public enum TileType
    {
        Player,
        Monster,
        Passable,
        Impassable
    }

    public void Log(string log)
    {
        _logManager.Log(log);
    }

    public int MoveOffset
    {
        get { return (int) _tilemap.layoutGrid.cellSize.x; }
    }

    public bool CanMove(Vector3Int pos)
    {
        if (_tilemap.GetColliderType(pos - new Vector3Int(0, 1, 0)) != Tile.ColliderType.None) return false;
        if (GetCharacter(pos)) return false;
        return true;        
    }
    
    public Character GetCharacter(Vector3Int pos)
    {
        if (_player.transform.position.Ceil().Equals(pos)) return _player;
        return (_monsters.GetComponentsInChildren<Monster>().FirstOrDefault(monster => monster.transform.position.Ceil().Equals(pos)));
    }

    private void Update()
    {
        if (Time.time - _lastTurnTime < 0.150f) return;
        _lastTurnTime = Time.time;
        
        if (!_player.DoTurn()) return;
        foreach (var monster in _monsters.GetComponentsInChildren<Monster>())
            if (!monster.IsDead) monster.DoTurn();
    }
}

public static class Vector3Ex
{
    public static Vector3 Ceil(this Vector3 vector3)
    {
        return new Vector3(Mathf.Ceil(vector3.x), Mathf.Ceil(vector3.y), Mathf.Ceil(vector3.z));
    }
}