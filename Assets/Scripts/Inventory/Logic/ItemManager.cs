using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        [Header("ItemPrafab Ԥ�Ƽ�")]
        public Item itemPrefab;

        private Transform itemParent;

        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        /// <summary>
        /// ����ʱ����������
        /// </summary>
        private void OnEnable()
        {
            EventHandler.OnInstantiateItenScene += OnInstantiateItenScene;
        }

        

        /// <summary>
        /// ȡ������ʱ����������
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
