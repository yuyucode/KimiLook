using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEditorInternal.VersionControl;

public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;  // 存储原数据
    private List<ItemDetails> itemList = new List<ItemDetails>(); // 物品列表
    private VisualTreeAsset itemRowTemplate; // itemListView内的单模板文件
    private ListView itemListView;
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem; // 选中的item
    private VisualElement iconPreview; // icon的预览
    private Sprite defaultIcon; // 默认预览Icon
    private Slider sellPercentageSlider; // 折扣模块

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("M STUDIO/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        // 拿到模板数据
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");

        // 拿默认图片数据
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        // 查找模板
        // 1. 查找到VisualElement元素下ListView元素并赋值
        // 根部先查询名为为ItemList的VisualElement元素，在查找下面的
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        // 2. 查找到ScrollView元素并赋值
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");

        // 3. 查找icon的物体位置
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        // 4. 获得Button

        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += onDeleteItemClicked;


        LoadDataBase();

        GenerateListView();
    }

    /// <summary>
    /// 删除 Item
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void onDeleteItemClicked()
    {
        if(itemList.Count == 0 ) return;

        int itemIndex = itemList.IndexOf(activeItem);
        Debug.Log(itemIndex);

        itemList.Remove(activeItem);



        if (itemIndex != 0)
        {
            itemListView.SetSelection(itemIndex - 1);
            itemListView.ScrollToItem(itemIndex - 1);
        }
         
        if(itemIndex == 0 && itemList.Count > 0)
        {
            itemListView.SetSelection(0);
            itemListView.ScrollToItem(0);
        }

        itemListView.Rebuild();
    }

    /// <summary>
    /// 添加 Item
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnAddItemClicked()
    {
        // 创建新的物品，添加到物品列表
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NET ITEM";
        newItem.itemID = itemList.Count == 0 ? 1000 :itemList[itemList.Count - 1].itemID + 1;
        itemList.Add(newItem);

        // 新的物品展示在信息栏，
        activeItem = newItem;

        // 重新绘制
        itemListView.Rebuild();

        // ListView焦点聚焦在新的物品上（最后一个物品）
        itemListView.SetSelection(itemList.Count - 1);
        itemListView.ScrollToItem(itemList.Count - 1);

    }

    private void LoadDataBase()
    {
        string[] dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        // 判断是否有数据
        if(dataArray.Length > 1 ) {
            // 根据GUID 找到dataBase的路径
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);

            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;

        }
        itemList = dataBase.itemDetailsList;
        
        // 如果不标记则无法保存数据
        EditorUtility.SetDirty(dataBase);
    }


    private void GenerateListView()
    {
        // 创建项目：将模板文件生成一份给到makeItem
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        // 绑定生成项目
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            // 判断列表数据的个数 > i
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon!= null)
                {
                    // Q用于查找UI Tookit 的UI Document 文件内部的命名
                    // element 查找对应的Icon，并且把backgroundImage 赋值
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                }

                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
            }
        };
        // 生成ListView
        // itemSource 源文件
        itemListView.fixedItemHeight = 48;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem= bindItem;


        itemListView.onSelectionChange += OnListSelectionChange;

        itemDetailsSection.visible = false; // 当前不显示
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails) selectedItem.First();
        GetItemDetails(); // 激活
        itemDetailsSection.visible = true; //  显示
    }

    private void GetItemDetails()
    {
        // MarkDirtyRepaint 右侧面板有数据的更改，撤销等其他动作都可以保存
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt => activeItem.itemID = evt.newValue);


        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild(); // 修改name时，重新刷新ListView
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(IItemType.Seed);
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (IItemType)evt.newValue;
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture; // 预览图赋值背景
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon; // 图片赋值
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild(); // 修改图片时，重新刷新ListView
        });

        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite; // 图片赋值
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemOnWorldSprite = newIcon;
        });

        // Bottom

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedUp").value = activeItem.canPickup;
        itemDetailsSection.Q<Toggle>("CanPickedUp").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickup= evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        // Price
        itemDetailsSection.Q<IntegerField>("ItemPrice").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("ItemPrice").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });


        itemDetailsSection.Q<Slider>("SellPercentage").value=activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });

        // Description

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });
    }
}
