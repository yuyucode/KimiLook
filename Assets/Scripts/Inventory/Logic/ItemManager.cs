using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        [Header("ItemPrafab 预制件")]
        public Item itemPrefab;

        private Transform itemParent;

        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        /// <summary>
        /// 激活时，触发函数
        /// </summary>
        private void OnEnable()
        {
            EventHandler.OnInstantiateItenScene += OnInstantiateItenScene;
        }

        

        /// <summary>
        /// 取消激活时，触发函数
        /// </summary>
        private void OnDisable()
        {
            EventHandler.OnInstantiateItenScene -= OnInstantiateItenScene;
        }

        private void OnInstantiateItenScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;

        }
    }
}
