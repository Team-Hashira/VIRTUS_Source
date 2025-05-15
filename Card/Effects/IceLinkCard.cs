using Crogen.CrogenPooling;
using Hashira.StageSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Tilemaps;

namespace Hashira.Cards.Effects
{
    // 보류
    public class IceLinkCard : MagicCardEffect
    {
        protected override float DelayTime => 10;

        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Vector2 _range = new Vector2(8, 2);
        [SerializeField] private Vector2 _offset = new Vector2(0, -1);

        private int[] xDir = { 0, 1, 0, -1 };
        private int[] yDir = { 1, 0, -1, 0 };

        public override void OnUse()
        {
            //Debug.Log("OnUse");
            //List<Vector2Int> groundTileList = StageGenerator.Instance.GetCurrentStage().GroundTileList;
            //Tilemap tilemap = StageGenerator.Instance.GetCurrentStage().Tilemap;

            //Vector3 center = player.transform.position + (Vector3)_offset;
            //Quaternion playerInverseRotation = Quaternion.Inverse(player.transform.rotation);
            //Vector3 localCenter = playerInverseRotation * center;
            
            //foreach (Vector2 pos in groundTileList)
            //{
            //    Vector3 localPos = playerInverseRotation * pos;
            //    if (Mathf.Abs(localPos.x - localCenter.x) < _range.x && Mathf.Abs(localPos.y - localCenter.y) < _range.y)
            //    {
            //        List<Vector2> hitedPointList = new List<Vector2>();
            //        bool hitFlag = false;

            //        List<Vector2> colliderPointList = new List<Vector2>();
            //        int pointCount = tilemap.GetSprite(new Vector3Int((int)pos.x, (int)pos.y)).GetPhysicsShape(0, colliderPointList);
            //        for (int i = 0; i < pointCount; i++)
            //        {
            //            Vector3 targetPos = pos + colliderPointList[i];
            //            Vector3 direction = targetPos - player.transform.position;
            //            RaycastHit2D hit2D = Physics2D.Raycast(player.transform.position, direction.normalized, direction.magnitude + 0.1f, _whatIsGround);
            //            Debug.DrawLine(player.transform.position, targetPos, Color.red, 10f);
            //            if (Mathf.Abs(hit2D.distance - direction.magnitude) < 0.02f)
            //            {
            //                hitedPointList.Add(colliderPointList[i]);
            //                hitFlag = true;
            //            }
            //        }

            //        if (hitFlag)
            //            FreezeFacingSurfaces((Vector3)pos + new Vector3(0.5f, 0.5f), hitedPointList);
            //    }
            //}
        }

        //private void FreezeFacingSurfaces(Vector3 pos, List<Vector2> hitedPointList)
        //{

        //    List<Vector2Int> dirList = new List<Vector2Int>();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (hitedPointList[i] && hitedPointList[(i + 1) % 4])
        //        {
        //            Transform iceBlock = PopCore.Pop(CardSubPoolType.IceBlock, pos, Quaternion.identity).gameObject.transform;
        //            iceBlock.up = new Vector2(xDir[i], yDir[i]);
        //        }
        //    }
        //}
    }
}
