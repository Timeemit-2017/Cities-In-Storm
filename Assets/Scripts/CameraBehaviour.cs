using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class CameraBehaviour : MonoBehaviour
{   
    /// <summary>
    /// 滚动速度
    /// </summary>
    public float scrollSpeed = 3;

    /// <summary>
    /// 滚动的最大速度
    /// </summary>
    public float scrollMaxSpeed = 15;

    /// <summary>
    /// 移动视角的最大速度
    /// </summary>
    public float moveSpeed = 0.2f;

    /// <summary>
    /// 上下改变角度的最大速度
    /// </summary>
    public float rotationSpeed = 0.01f;

    /// <summary>
    /// 摄像机的Transform组件
    /// </summary>
    private Transform t;

    /// <summary>
    /// 鼠标上一帧的位置
    /// </summary>
    private Vector3 lastMouse;

    /// <summary>
    /// 当前帧与鼠标上一帧位置的差
    /// </summary>
    public Vector3 temp;

    /// <summary>
    /// 当前帧与鼠标上一帧y值的差
    /// </summary>
    private float yOffset;

    private void Scroll()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, -scrollMaxSpeed, scrollMaxSpeed);
        t.position += Vector3.forward * distance;
        if (t.position.z > -2)
        {
            t.position += Vector3.forward * (-2 - t.position.z);
        }
    }

    private void RightClickBehaviour()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                yOffset = Input.mousePosition.y - lastMouse.y;
                t.rotation = new Quaternion(t.rotation.x + (-yOffset / Screen.height * 90) * rotationSpeed, t.rotation.y, t.rotation.z, t.rotation.w);
            }
            else
            {
                if (0 < Input.mousePosition.x && Input.mousePosition.x < Screen.width && 0 < Input.mousePosition.y && Input.mousePosition.y < Screen.height)
                {
                    temp = Input.mousePosition - lastMouse;
                    temp.Set(temp.x / Screen.width, temp.y / Screen.height, temp.z);
                    t.position += temp * -moveSpeed;
                }
            }
        }
    }

    private void PressButtonToMove()
    {
        t.position += Vector3.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        t.position += Vector3.up * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
    }

    private void Start()
    {
        t = GetComponent<Transform>();
        lastMouse = Input.mousePosition;
    }

    private void Update()
    {
        Scroll();

        RightClickBehaviour();

        PressButtonToMove();

        lastMouse = Input.mousePosition;
    } 
}
