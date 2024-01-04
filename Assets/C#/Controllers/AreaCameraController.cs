using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCameraController : MonoBehaviour
{   
    // 실제 카메라가 부착된 오브젝트의 트랜스폼
    private Transform _cameraTransform;

    [SerializeField]
    // 카메라 이동 속도
    private float _moveSpeed;
    [SerializeField]
    // 카메라의 각종 값들이 새 값으로 변경되는 시간 (클수록 더 빨리 변경됨)
    private float _moveTime;
    [SerializeField]
    // 한 번 입력에 카메라 zoom되는 양
    private Vector3 _zoomAmount;
    [SerializeField]
    // 화면 이동을 위해 필요한 (마우스 위치 - 스크린 모서리 위치) 값
    private float _borderThickness;

    // 카메라가 이동할 수 있는 최대, 최소 x, z (Vector2.x: min, Vector2.y: max)
    [SerializeField]
    private Vector2 _posLimitX;
    [SerializeField]
    private Vector2 _posLimitZ;
    // 카메라 zoom 최대, 최소
    [SerializeField]
    private float _zoomoutLimit;
    [SerializeField]
    private float _zoominLimit;


    // 카메라의 다음 위치
    private Vector3 _newPosition;
    // 카메라의 다음 zoom
    private Vector3 _newZoom;

    // 마우스 드래그를 통한 화면 이동을 위해 사용되는 변수들
    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;
    private Plane _plane;
    private float _entry;

    // 줌 단계에 따라 카메라 이동속도 조정
    private float _zoomLevel;

    void Start()
    {
        _cameraTransform = transform.GetChild(0);
        if (_cameraTransform == null)
        {
            Debug.LogError("No camera attached to AreaCameraController");
        }
        _newPosition = transform.position;
        _newZoom = _cameraTransform.localPosition;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _entry = 0;

        _zoomLevel = CalculateZoomlevel();
    }

    void Update()
    {
        HandleMouseInput();
        _zoomLevel = CalculateZoomlevel();
        HandleMovementInput();
        UpdateCamera();
    }

    // 드래그를 통한 카메라 이동, 마우스 휠을 사용한 zoom
    private void HandleMouseInput()
    {   
        // zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            _newZoom += Input.mouseScrollDelta.y * _zoomAmount;
        }

        // 클릭, 드래그로 화면 이동
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out _entry)){
                _dragStartPosition = ray.GetPoint(_entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out _entry))
            {
                _dragCurrentPosition = ray.GetPoint(_entry);
                _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }
    }

    // 키보드 입력 or 스크린 모서리로 마우스 이동 시 카메라 이동
    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - _borderThickness)
        {
            _newPosition += transform.forward * _moveSpeed / _zoomLevel;
            Debug.Log("up");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= _borderThickness)
        {
            _newPosition += transform.right * -_moveSpeed / _zoomLevel;
            Debug.Log("left");
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= _borderThickness)
        {
            _newPosition += transform.forward * -_moveSpeed / _zoomLevel;
            Debug.Log("down");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - _borderThickness)
        {
            _newPosition += transform.right * _moveSpeed / _zoomLevel;
            Debug.Log("right");
        }
    }

    // 카메라 위치 및 zoom 업데이트
    private void UpdateCamera()
    {
        _newPosition.x = Mathf.Clamp(_newPosition.x, _posLimitX.x, _posLimitX.y);
        _newPosition.z = Mathf.Clamp(_newPosition.z, _posLimitZ.x, _posLimitZ.y);
        _newZoom.y = Mathf.Clamp(_newZoom.y, _zoominLimit, _zoomoutLimit);
        _newZoom.z = Mathf.Clamp(_newZoom.z, _zoominLimit, _zoomoutLimit);

        transform.position = Vector3.Lerp(transform.position, _newPosition, _moveTime * Time.deltaTime);
        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, _moveTime * Time.deltaTime);
    }

    private float CalculateZoomlevel()
    {
        return Mathf.Abs((_zoomoutLimit - _newZoom.y) / ((_zoomoutLimit - _zoominLimit) / _zoomAmount.y)) + 1;
    }
}
