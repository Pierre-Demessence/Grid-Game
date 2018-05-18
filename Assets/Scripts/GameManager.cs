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

    public bool CanMove(Vector2Int pos)
    {
        if (_tilemap.GetColliderType((pos - new Vector2Int(0, 1)).ToVector3Int()) != Tile.ColliderType.None) return false;
        if (GetCharacter(pos)) return false;
        return true;        
    }
    
    public Character GetCharacter(Vector2Int pos)
    {
        if (_player.Position.Equals(pos)) return _player;
        return (_monsters.GetComponentsInChildren<Monster>().FirstOrDefault(monster => monster.Position.Equals(pos)));
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