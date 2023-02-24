# KimiLook 《麦田物语》模拟经营游戏开发

# Unity笔记

## 常用工具（Package Manager & 内置工具插件）

1. 常用的摄像机管理 ：`Cinemachine`

   ![image-20230224163001478](/NoteIamges/Cinemachine)

2. 动画任何通过简化的代码。`DOTween`是一个快速、高效、完全类型安全的面向对象动画引擎。

   ![image-20230224163047680](/NoteIamges/DOTween)

3. `UI Toolkit`

  4. 在`Porject `下创建`Editor`文件夹，这个`Editor`文件夹不会经过打包

  5. 在`Editor`文件夹创建`UI Builder`文件夹

  6. 在`UI Builder`文件夹内，右键-> `UI Toolkit` -> `Editor Window` 输入对应的类名，这样会生成3个文件

     -  C#脚本
     -  样式文件
     -  `UI Builder`编辑器文件

  ![image-20230224163252755](/NoteIamges/Toolkit_UI_Builder)

### UI Toolkit 知识

1. 脚本查询`UI Toolkit`的元素
   在`uxml (Visual Tree Asset )`所有编辑的`UI`中，用到的`Library`元素，都可以通过以下方式进行查找，如下

  ![image-20230224163638297](/NoteIamges/search_ui)

  `root.Q< Standard下的元素名字 >("Hierarchy下的元素的name")`

  ```
#Container
   #ItemList（VisualElement）
  ```

  最后的查询语法`root.Q<VisualElement>("ItemList")` : 根元素查询名为 `ItemList `的 `VisualElement`

  



## 摄像机操作

1. 常用的摄像机管理工具`Cinemachine`，在Hierarchy目录下右键使用`Ciemachine`创建对象（名字默认可能是`CM Vcam1`）

2. 根据像素本身来处理相机的位置（配合`Cinemachine`）

   - `Main Camera` 对象：（主摄像机）

     > 在Main Camera 中添加组件 Pixel Perfect Camera，修改属性配置。

   - `CM Vcam1`对象：（需要`Chinemachine`工具创建）

     > 在`CinemachineVirtualCamera`组件中的`Extensions`属性选择`Add Extension` 下拉框，选择 `Cinemachine Pixel Perfect`，他会根据`Pixel Perfect Camera`来处理
     > 最后`Lens Ortho Size`属性 改为9

     

3. 设置摄像机缓冲

   在`CM vcam1 `对象中， 在`CinemachineVirtualCamera`组件的`Body`属性设置参数，看个人喜好

4. 设置摄像机边界

   1. 在`CM vcam1`对象中， 在`CinemachineVirtualCamera`组件的`Extensions` 属性添加新的扩展组件`Cinemachine Confiner`用来处理边界。

   2. 创建一个`Bounds`对象，添加`Collider 2D`组件，记得勾选`is Trigger`，防止顶出其他物体，然后再修改边界位置锚点
      把`Bounds`对象添加到` Cinemachine Confiner`组件的`Bounding Shape 2D`属性

   3. > 自动加载处理边界：为了防止切换场景，导致Bounds对象消失，可以编写C#脚本

      ```c#
      using Cinemachine;
      using System.Collections;
      using System.Collections.Generic;
      using UnityEngine;
      
      /// <summary>
      /// 切换场景
      /// </summary>
      public class SwitchBounds : MonoBehaviour
      {
          // TODO: 切换场景后更改调用
          private void Start()
          {
              SwitchConfinerShape();
          }
      
          private void SwitchConfinerShape()
          {
              // 获取 Collider2D
              PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
      
              // 获取 CinemachineConfiner 组件
              CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
      
              // 设置 CinemachineConfiner 的 BoundingShape2D 属性的值
              confiner.m_BoundingShape2D = confinerShape;
      
              // 如果边界形状的点在运行时发生变化，则调用此函数，防止被缓存
              confiner.InvalidatePathCache();		
          }
      }
      ```

![image-20230224163958999](/NoteIamges/CM vcam1)

## 图片操作

1. 图片统一预设处理

   - 当有N多个图片时，调整的参数配置都一样，那么就可以使用预设统一处理
   - 先在`Project`目录中点击图片，在`Inspector`窗口的右上角有个预设按钮![image-20230224154202679](/NoteIamges/preset)，创建新的`Preset`
   - 然后多选图片，选中`Preset`内的预设，进行处理

2. `Player`与树跟（或者其它物品）的遮挡关系

  3. 可以设置树根（或者其它物品）的`Sprite Renderer`组件的`Sprite Sort Point `属性改为`Pivot`

  4. 找到`Sprite Renderer`组件的`Sprite`属性图片位置，选择图片点击`Sprite Editor`进入`Sprite Editor`窗口 

     ![image-20230224164428288](/NoteIamges/image-2-2)

  5. 右下方的`Sprite`窗口，`Pivot`属性下拉选中`Custom`， 修改`Custom Pivot `的 `X || Y`，让其有遮挡的锚点位置![image-20230224164301095](/NoteIamges/image-2-4)


## 排序等级操作

1. `sorting group`组件

   - 对象中添加`Sorting Group`组件，选中对应的`Sorting Layer`可以让父物体下的所有子物体，整体一起渲染来排序。

   - 子物体也要选中对应的`Sorting Layer`

     ![image-20230224164714442](/NoteIamges/sorting-1-2)


2. 遮挡的功能效果，设置整个项目按照 Y 轴渲染

   1. `Unity`菜单栏：`Edit` -> `Project settings` -> `Graphics `

   2. 找到`Camera Settings`配置项，`Transparency Sort Mode` 透明度排序模式，选择 `Custom Axis`就是按照自定义轴的模式

   3. `Transparency Sort Axis`配置项为 `x=0,y=1,z=0` 这样根据Y轴就有遮挡关系

      > 注意事项：在`Sprite Renderer`组件下`Sorting Layer`必须是相同的，且`Player`的图片切割锚点需要在底部

      ![image-20230224164714442](/NoteIamges/sorting-2-3)

## Tilemap 操作

1. `Tile Palette` 的翻转功能（需要新的版本）

   1. 在 `Tile Palette ` 窗口中，在功能按钮旁边的区域，进行鼠标右键，点击  `open Tile palette Preferences`选项进行预设

      ![image-20230224165327011](/NoteIamges/Tilemap-1-1)

   2. 在`Tile Palette`窗口找到 `Default Tile Palatte Tools`，添加新的功能，然后`save`

      ![image-20230224165901649](/NoteIamges/Tilemap-1-2)

2. `Scene` 窗口网格区域的对焦模式（显示当前访问的绘画区域）

   1. `Scene `窗口网格区域最上方右边有个网格（对焦模式），选择`Tilemap`对焦
   2. 当`Tile Palette` 窗口的`Active Tilemap`下拉栏选择了`Tilemap（Ground）`那么只会显示`Ground`区域的`Tile`

   ![image-20230224165943865](/NoteIamges/Tilemap-2-2)


3. 绘制区域可以使用![image-20230224161959530](/NoteIamges/TilePalette_box)快速绘制，也可以使用`shift`+ ![image-20230224161959530](/NoteIamges/TilePalette_box)进行快速删除

4. 笔刷可以绘画出随机`Tile`, `Tile Palette`窗口下方有`Default Brush`下拉框，可以进行下拉选择`Random Brush`进行配置

   ![image-20230224170036896](/NoteIamges/Tilemap-4-1)

5. `Rule Tile` 可以进行规则绘制，减少绘制量。

   1. 在`Project`目录下鼠标右键 -> `Create` -> `2D` -> `Tiles`-> `Rule Tile`
   2. 根据不同图片进行规则绘制

![image-20230224170200939](/NoteIamges/Tilemap-5-2)



6. 当瓦片地图`(Tilemap)`出现缝隙，可以使用`Sprite Atlas`功能进行处理，这样可以把所有图片素材打包成一张图片，然后根据不同平台进行压缩优化。

   1. 在`Project`目录下鼠标右键 -> `Create` -> `2D` -> `Sprite Atlas` 新建`Sprite Atlas`
   2. 把需要打包的图片素材的`文件夹`拖到 `Objects for packing `
   3. 修改` Filter Mode` 为`Point `像素，` Compression` 为`None `最后进行`Pack preview`
   4. 注意：图片素材可能会干扰其他图片，可以把`Padding` 改大点

   ![image-20230224170418595](/NoteIamges/Tilemap-6-4)

7. 瓦片地图碰撞（整体）

   1. 给`Tilemap` 添加 `Tilemap Collider 2D`组件
   2. 为了让`Tilemap`整体，添加`Composite Collider 2D`，（给`Rigidbody 2D`的属性`Type` 改为`Staitc` , 看游戏需要）
   3. 最后`Tilemap Collider 2D`组件，勾选`Used By Composite`

   ![image-20230224171439947](/NoteIamges/Tilemap-7-3)

8. 对于不规则的碰撞`Tilemap`处理

  9. 可以回到碰撞体图片，点击`Sprite Editor`进行编辑 , `Sprite Editor` 下拉菜单选中 `Custom Physics Shape`

  10. 选中单独的`Tile`，点击上方的`Generate`，然后修改成自己想要的效果， 修改完成后点击`Apply`

  11. 关闭`Collision`，然后重新打开，就会完成绘制

  ![image-20230224172133256](/NoteIamges/Tilemap-8-3)

## 组件操作

1. 组件复制

   - 当组件配置好之后，可以右上角3点号进行Copy，可以复制到其他对象

   ![image-20230224173614771](/NoteIamges/component-1-1)


## 思路

1. `Player` 挂载`TriggerEnter`， 然后触发，调用其他对象所挂载的函数


## 代码

1. 让unity识别class对象，在Inspector显示的话，记得加上序列号

  ```c#
[System.Serializable]
public class ItemDetails（）{
    
}
  ```

  


## 文件操作

1. （SO文件）创建`ScriptableObject`的`C#`文件，用于存储数据

   ```c#
   [CreateAssetMenu(fileName ="ItemDataList_SO", menuName = "Inventory/ItemDataList")]
   
   public class ItemDataList_SO: ScriptableObject
   {
       public List<ItemDetails> itemDetailsList;
   }
   ```

   使用方式：在`Project` 目录先，右键 -> `Inventory` -> `ItemDataList`，生成`SO`文件

## 小知识

1. 组件右上方有圆圈问号，可以点击跳转到手册