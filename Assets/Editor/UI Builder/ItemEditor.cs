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
    private ItemDataList_SO dataBase;  // �洢ԭ����
    private List<ItemDetails> itemList = new List<ItemDetails>(); // ��Ʒ�б�
    private VisualTreeAsset itemRowTemplate; // itemListView�ڵĵ�ģ���ļ�
    private ListView itemListView;
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem; // ѡ�е�item
    private VisualElement iconPreview; // icon��Ԥ��
    private Sprite defaultIcon; // Ĭ��Ԥ��Icon
    private Slider sellPercentageSlider; // �ۿ�ģ��

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

        // �õ�ģ������
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");

        // ��Ĭ��ͼƬ����
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");


        // ����ģ��
        // 1. ���ҵ�VisualElementԪ����ListViewԪ�ز���ֵ
        // �����Ȳ�ѯ��ΪΪItemList��VisualElementԪ�أ��ڲ��������
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        // 2. ���ҵ�ScrollViewԪ�ز���ֵ
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");

        // ��ʼ��Type��ѡ��Ĭ������
        itemDetailsSection.Q<EnumField>("ItemType").Init(IItemType.Seed);

        // 3. ����icon������λ��
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        // 4. ���Button

        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += onDeleteItemClicked;


        LoadDataBase();

        GenerateListView();
    }

    /// <summary>
    /// ɾ�� Item
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void onDeleteItemClicked()
    {
        if(itemList.Count == 0 ) return;

        int itemIndex = itemList.IndexOf(activeItem);

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
    /// ��� Item
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnAddItemClicked()
    {
        // �����µ���Ʒ����ӵ���Ʒ�б�
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NET ITEM";
        newItem.itemID = itemList.Count == 0 ? 1000 :itemList[itemList.Count - 1].itemID + 1;
        itemList.Add(newItem);

        // �µ���Ʒչʾ����Ϣ����
        activeItem = newItem;

        // ���»���
        itemListView.Rebuild();

        // ListView����۽����µ���Ʒ�ϣ����һ����Ʒ��
        itemListView.SetSelection(itemList.Count - 1);
        itemListView.ScrollToItem(itemList.Count - 1);

    }

    private void LoadDataBase()
    {
        string[] dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        // �ж��Ƿ�������
        if(dataArray.Length > 1 ) {
            // ����GUID �ҵ�dataBase��·��
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);

            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;

        }
        itemList = dataBase.itemDetailsList;
        
        // �����������޷���������
        EditorUtility.SetDirty(dataBase);
    }


    private void GenerateListView()
    {
        // ������Ŀ����ģ���ļ�����һ�ݸ���makeItem
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        // ��������Ŀ
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            // �ж��б����ݵĸ��� > i
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon!= null)
                {
                    // Q���ڲ���UI Tookit ��UI Document �ļ��ڲ�������
                    // element ���Ҷ�Ӧ��Icon�����Ұ�backgroundImage ��ֵ
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                }

                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
            }
        };
        // ����ListView
        // itemSource Դ�ļ�
        itemListView.fixedItemHeight = 48;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem= bindItem;


        itemListView.onSelectionChange += OnListSelectionChange;

        itemDetailsSection.visible = false; // ��ǰ����ʾ
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails) selectedItem.First();
        GetItemDetails(); // ����
        itemDetailsSection.visible = true; //  ��ʾ
    }

    private void GetItemDetails()
    {
        // MarkDirtyRepaint �Ҳ���������ݵĸ��ģ��������������������Ա���
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt => activeItem.itemID = evt.newValue);


        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild(); // �޸�nameʱ������ˢ��ListView
        });

        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (IItemType)evt.newValue;
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture; // Ԥ��ͼ��ֵ����
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon; // ͼƬ��ֵ
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild(); // �޸�ͼƬʱ������ˢ��ListView
        });

        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite; // ͼƬ��ֵ
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
