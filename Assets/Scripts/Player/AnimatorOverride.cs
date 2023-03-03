using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    [Header("各部分动画列表")]
    public List<AnimatorType> animatorsType;

    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

    private void Awake()
    {
        animators= GetComponentsInChildren<Animator>();

        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }


    private void OnEnable()
    {
        EventHandler.OnItemSelectedEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.OnItemSelectedEvent -= OnItemSelectedEvent;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSeleted)
    {
        //WORKFLOW: 不同的工具返回不同的动画在这里补全
        PartType currentType = itemDetails.itemType switch
        {
            IItemType.Seed => PartType.Carry,
            IItemType.Commodity => PartType.Carry,
            _ => PartType.None
        };

        if(isSeleted == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }else
        {
            if(currentType == PartType.Carry) {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
        }

        SwitchAnimator(currentType);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach (var item in animatorsType)
        {
            if(item.partType == partType)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
