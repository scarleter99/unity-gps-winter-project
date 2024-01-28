using UnityEngine;
using static Define;

// 전투 씬에서 사용되는 사각형 그리드 (zx축)
public class SquareGrid
{   
    // player or enemy
    private GridSide _side;
    private int _width;
    private int _height;
    public int Width 
    {
        get => _width;
        set => _width = value;
    }
    public int Height
    {
        get => _height;
        set => _height = value;
    }
    private float _cellsize;
    private float _cellgap;

    // 그리드 가장 왼쪽 밑 셀의 월드 좌표, 이곳이 그리드 좌표로는 (0,0)이 됨.
    // y가 0이면 지면과 겹쳐서 그리드가 보이지 않으므로, y는 0보다 약간 크게 설정해야 함.
    private Vector3 _originPosition;

    private SquareGridCell[,] _gridArray;

    private const string _gridObjectPath = "Area/grid_square_indicator";
    private Transform _cellParent;

    private SquareGridCell _currentMouseoverCell;

    public SquareGrid(Vector3 originposition, GridSide side, int width = 3, int height = 2, float cellsize = 2.5f, float cellgap = 0.1f)
    {
        _originPosition = originposition;
        _side = side;
        _width = width;
        _height = height;
        _cellsize = cellsize;
        _cellgap = cellgap;
        _gridArray = new SquareGridCell[height, width];
        AssignParent();
        InitializeGridObject();
    }

    // 그리드 나타내는 사각형 타일 생성
    private void InitializeGridObject()
    {
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                GameObject gridCellObject = Managers.ResourceMng.Instantiate(_gridObjectPath, _cellParent);
                gridCellObject.transform.position = GetWorldPosition(x, z);
                SquareGridCell gridCell =
                    new(x, z, _cellsize, gridCellObject, _side);
                SetGridCell(x, z, gridCell);
            }
        }
    }

    // 그리드 좌표를 월드 좌표로 변환
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * (_cellsize + _cellgap) + _originPosition;
    }

    // 월드 좌표를 그리드 좌표로 변환
    public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.RoundToInt((worldPosition - _originPosition).x / (_cellsize + _cellgap));
        z = Mathf.RoundToInt((worldPosition - _originPosition).z / (_cellsize + _cellgap));
    }

    public void SetGridCell(int x, int z, SquareGridCell gridCell)
    {
        _gridArray[z, x] = gridCell;
    }

    public SquareGridCell GetGridCell(Vector3 worldPosition)
    {   
        GetGridPosition(worldPosition, out int x, out int z);
        return GetGridCell(x, z);
    }

    public SquareGridCell GetGridCell(int x, int z)
    {
        return _gridArray[z, x];
    }

    // 게임오브젝트 부모 설정
    private void AssignParent()
    {
        Transform grandparent = GameObject.Find("Grid")?.transform;

        if (grandparent == null)
        {
            grandparent = new GameObject("Grid").transform;
        }

        _cellParent = GameObject.Find(_side.ToString() + "grid")?.transform;

        if (_cellParent == null)
        {
            _cellParent = new GameObject(_side.ToString() + "grid").transform;
            _cellParent.SetParent(grandparent);
        }
    }

    // 마우스를 그리드 셀 위로 가져다 대면 해당 셀 색을 바꿈
    public void HandleMouseHover(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out int x, out int z);
        //Debug.Log($"{z}, {x}");
        if (x >= 0 && x < _width && z >= 0 && z < _height)
        {
            _currentMouseoverCell?.OnMouseExit();
            _currentMouseoverCell = _gridArray[z, x];
            _currentMouseoverCell?.OnMouseEnter();
        }
        else
        {
            ResetMouseHover();
        }
    }

    public void ResetMouseHover()
    {
        _currentMouseoverCell?.OnMouseExit();
        _currentMouseoverCell = null;
    }

    // 해당 grid 좌표에 프리팹 생성
    public void InstantiatePrefab(string prefabPath, Define.WorldObject objectType, int x, int z, float rotationY = 0)
    {
        GameObject prefab = Managers.GameMng.Spawn(objectType, prefabPath);
        prefab.transform.position = GetWorldPosition(x, z);
        prefab.transform.rotation = Quaternion.Euler(0, rotationY, 0);
        GetGridCell(x, z).OnCellObject = prefab;
    }
    
}
