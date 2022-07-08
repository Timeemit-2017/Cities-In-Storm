using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// Piece在引擎中的具体实现
/// </summary>
public class PieceInstante : MonoBehaviour
{
    public MapSummon Summoner;

    public Piece piece;

    private bool isAbleShowOperation = false;

    public Position P
    {
        get
        {
            return piece.p;
        }
        set
        {
            piece.p = value;
        }
    }

    public void Update()
    {
        RaycastHit hitInfo;
        // 当按下鼠标右键时
        if (Input.GetMouseButtonUp(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            if (hitInfo.collider.gameObject == this.gameObject && isAbleShowOperation)
            {
                OnMouseDownBehaviour();
            }
        }
        else if(Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            if (hitInfo.collider.gameObject == this.gameObject)
            {
                isAbleShowOperation = true;
            }
            else
            {
                isAbleShowOperation = false;
            }
        }
        else if (Input.GetMouseButtonDown(0) && !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 9999, 1 << 8))
        {
            GameVar.blocks.ClearAllBlockWhickIsChecked();
            GameVar.blockBeingOperated = null;
        }
    }

    public void OnMouseDownBehaviour()
    {
        // 如果正在移动棋子（鼠标左键按下）
        if (Input.GetMouseButton(0))
        {
            // 不执行展开操作栏的逻辑
            return;
        }

        GameObject operation = Summoner.operation;
        bool target = !operation.activeSelf;
        if (target)  // 公开Block（即正在被操作的Block）
        {
            GameVar.blockBeingOperated = GameVar.blocks.GetBlock(P);
        }
        else
        {
            if (GameVar.blockBeingOperated.isChecked)
            {
                GameVar.blocks.ClearAllBlockWhickIsChecked();
            }
            GameVar.blockBeingOperated = null;
        }
        operation.SetActive(target);
        if (target)
        {
            operation.GetComponent<OperationControl>().positionControl();
        }
    }

    public void SetSize(Vector3 scale)
    {
        transform.localScale = scale;
    }

    /// <summary>
    /// 获取棋子所附属的方块
    /// </summary>
    /// <returns></returns>
    public Transform GetBlock()
    {
        return GameVar.blocks.GetBlock(P).transform;
    }

    public void OnMouseDown()
    {
        if(GameVar.blockBeingOperated != null && GameVar.blockBeingOperated.p != P)
        {
            // 有一个棋子正在被操作，但用户又选择了另一个棋子
            GameVar.blocks.ClearAllBlockWhickIsChecked();
        }
        GameVar.blockBeingOperated = GameVar.blocks.GetBlock(P);
        GameVar.pieceControl.ReadyToMovePiece();
    }

    public void OnMouseDrag()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(r,out RaycastHit hitInfo, 1000, (1<<9)))
        {
            //Block b;
            if(hitInfo.collider.gameObject == gameObject)
            {
                return;
            }
            else if (hitInfo.collider.CompareTag("BlockBound"))
            {
                //b = hitInfo.collider.GetComponentInParent<Block>();
                Transform blockT = hitInfo.collider.transform.parent;
                //transform.position = hitInfo.point + (blockT.position.z - blockT.localScale.z / 2 - hitInfo.point.z + transform.localScale.z / 2) * Vector3.back;
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, blockT.position.z -blockT.localScale.z / 2 - transform.localScale.z / 2);
                return;
            }
            else if (hitInfo.collider.CompareTag("Piece"))
            {
                transform.position = hitInfo.point + (transform.localScale.z / 2) * Vector3.back;
                return;
            }
            else
            {
                //b = hitInfo.collider.GetComponent<Block>();
            }
            /*List<TerrainID> terrianNear = GameVar.map.GetTerrainsInArray(b.p.Near(GameVar.map.Width, GameVar.map.Height));
            if(CIS_Terrain.oceans.Contains(b.id) && terrianNear.Contains(TerrainID.Land) || b.id == TerrainID.Land && terrianNear.Contains(TerrainID.LandLocked))
            {
                offset = 1 * GameVar.BlockSize.z;
            }
            else if(CIS_Terrain.oceans.Contains(b.id) && terrianNear.Contains(TerrainID.LandLocked))
            {
                offset = 2 * GameVar.BlockSize.z;
            }*/
            transform.position = hitInfo.point + (transform.localScale.z / 2) * Vector3.back;

        }
    }

    public void OnMouseUp()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hitInfo, 1000, 1 << 9))
        {
            Block b;
            if (hitInfo.collider.gameObject == gameObject)
            {
                return;
            }
            else if (hitInfo.collider.CompareTag("BlockBound"))
            {
                b = hitInfo.collider.GetComponentInParent<Block>();
            }
            else if (hitInfo.collider.CompareTag("Piece"))
            {
                b = hitInfo.collider.GetComponent<PieceInstante>().GetBlock().GetComponent<Block>();
            }
            else
            {
                b = hitInfo.collider.GetComponent<Block>();
            }
            bool result = GameVar.pieceControl.TryMovePiece(P, b.p);
            if (result)
            {
                GameVar.blocks.ClearAllBlockWhickIsChecked();
            }
        }
    }

    public void Delete()
    {
        GameVar.pieceControl.RemovePiece(P);
        Destroy(this.gameObject);
    }

}
