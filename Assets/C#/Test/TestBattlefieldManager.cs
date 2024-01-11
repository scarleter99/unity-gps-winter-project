using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBattlefieldManager : MonoBehaviour
{
    private SquareGrid _playerGrid;
    private SquareGrid _enemyGrid;

    private Camera _mainCamera;

    private const string _playerPrefabPath = "Players/Player";
    private const string _monsterPrefabPath = "Monsters/FlyingDemon";

    void Start()
    {
        _playerGrid = new SquareGrid(new Vector3(-3, 0.1f, -4.75f), Define.GridOwner.Player);
        _enemyGrid = new SquareGrid(new Vector3(-3f, 0.1f, 2.25f), Define.GridOwner.Enemy);
        _mainCamera = Camera.main;
        GeneratePrefabs();
    }

    void Update()
    {
        GetMouseWorldPosition(out bool success, out Vector3 worldPosition);
        if (success)
        {
            _playerGrid.HandleMouseHover(worldPosition);
            _enemyGrid.HandleMouseHover(worldPosition);
        }
        else
        {
            _playerGrid.ResetMouseHover();
            _enemyGrid.ResetMouseHover();
        }
    }

    private void GetMouseWorldPosition(out bool success, out Vector3 position)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance:100f, layerMask:LayerMask.GetMask("BattleGrid")))
        {
            if (rayHit.transform.gameObject != null)
            {
                success = true;
                position = rayHit.point;
                Debug.Log(rayHit.point);
                Debug.DrawLine(_mainCamera.transform.position, rayHit.point);
            }
            else
            {
                success = false;
                position = Vector3.zero;
            }
        }
        else
        {
            success = false;
            position = Vector3.zero;
        }
    }

    private void GeneratePrefabs()
    {
        for (int z = 0; z < _playerGrid.Height; z++)
        {
            for (int x = 0; x < _playerGrid.Width; x++)
            {
                GameObject player = Managers.ResourceMng.Instantiate(_playerPrefabPath);
                player.transform.position = _playerGrid.GetWorldPosition(x, z);
            }
        }
        for (int z = 0; z < _enemyGrid.Height; z++)
        {
            for (int x = 0; x < _enemyGrid.Width; x++)
            {
                GameObject enemy = Managers.ResourceMng.Instantiate(_monsterPrefabPath);
                enemy.transform.position = _enemyGrid.GetWorldPosition(x, z);
                enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
