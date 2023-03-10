# KimiLook 《麦田物语》模拟经营游戏开发（笔记）

## 生命周期 !!!!!!!!!!!

>  https://docs.unity.cn/cn/2022.1/Manual/ExecutionOrder.html

![生命周期](/NoteIamges/生命周期/smzq.svg)

### 1. 加载第一个场景

场景开始时将调用以下函数（为场景中的每个对象调用一次）。

- **Awake：**始终在任何 Start 函数之前并在实例化预制件之后调用此函数。（如果游戏对象在启动期间处于非活动状态，则在激活之后才会调用 Awake。）
- **OnEnable：**（仅在对象处于激活状态时调用）在启用对象后立即调用此函数。在创建 MonoBehaviour 实例时（例如加载关卡或实例化具有脚本组件的游戏对象时）会执行此调用。

请注意，对于添加到场景中的对象，在为任何对象调用 Start 和 Update 等函数之前，会为_所有_ 脚本调用 Awake 和 OnEnable 函数。当然，在游戏运行过程中实例化对象时，不能强制执行此调用。

### 2. Editor

- **Reset：**调用 Reset 可以在脚本首次附加到对象时以及使用 *Reset* 命令时初始化脚本的属性。
- **OnValidate：**每当设置脚本的属性时都会调用 OnValidate，包括反序列化对象时，这可能发生在不同的时间，例如在编辑器中打开场景时和域重新加载后。

### 3. 在第一次帧更新之前

- **Start：**仅当启用脚本实例后，才会在第一次帧更新之前调用 Start。

对于场景资源中的对象，在为任何脚本调用 Update 等函数之前，将在所有脚本上调用 Start 函数。当然，在游戏运行过程中实例化对象时，不能强制执行此调用。

### 4. 帧之间

- **OnApplicationPause：**在帧的结尾处调用此函数（在正常帧更新之间有效检测到暂停）。在调用 **OnApplicationPause** 之后，将发出一个额外帧，从而允许游戏显示图形来指示暂停状态。

### 5. 更新顺序

跟踪游戏逻辑和交互、动画、摄像机位置等的时候，可以使用一些不同事件。常见方案是在 **Update** 函数中执行大多数任务，但是也可以使用其他函数。

- **FixedUpdate：**调用 **FixedUpdate** 的频度常常超过 **Update**。如果帧率很低，可以每帧调用该函数多次；如果帧率很高，可能在帧之间完全不调用该函数。在 **FixedUpdate** 之后将立即进行所有物理计算和更新。在 **FixedUpdate** 内应用运动计算时，无需将值乘以 **Time.deltaTime**。这是因为 **FixedUpdate** 的调用基于可靠的计时器（独立于帧率）。
- **Update：**每帧调用一次 **Update**。这是用于帧更新的主要函数。
- **LateUpdate：**每帧调用一次 **LateUpdate__（在** Update__ 完成后）。**LateUpdate** 开始时，在 **Update** 中执行的所有计算便已完成。**LateUpdate** 的常见用途是跟随第三人称摄像机。如果在 **Update** 内让角色移动和转向，可以在 **LateUpdate** 中执行所有摄像机移动和旋转计算。这样可以确保角色在摄像机跟踪其位置之前已完全移动。

通常，不应依赖为不同的游戏对象调用相同事件函数的顺序 - 除非顺序明确记录或可设置。（如果需要对播放器循环进行更细粒度的控制，可以使用 [PlayerLoop API](https://docs.unity.cn/cn/2022.1/ScriptReference/LowLevel.PlayerLoop.html)。）

不能指定为同一 `MonoBehaviour` 子类的不同实例调用事件函数的顺序。例如，一个 `MonoBehaviour` 的 **Update** 函数可能会在另一个游戏对象（包括其父级或子级游戏对象）上的相同 `MonoBehaviour` 的 **Update** 函数之前或之后调用。

可以指定一个 `MonoBehaviour `子类的事件函数应在不同子类的事件函数之前调用（使用 Project Settings 窗口的 Script Execution Order 面板）。例如，如果有两个脚本，`EngineBehaviour` 和 `SteeringBehaviour`，可以设置 Script Execution Order，这样 `EngineBehaviours `始终在` SteeringBehaviours` 之前更新。

### 6. 动画更新循环

Unity 评估动画系统时，将调用以下函数和 [Profiler](https://docs.unity.cn/cn/2022.1/Manual/Profiler.html) 标记。

- **OnStateMachineEnter：**在**状态机更新 (State Machine Update)** 步骤中，当控制器的状态机进行流经 Entry 状态的转换时，将在第一个更新帧上调用此回调。在转换到 **StateMachine** 子状态时不会调用此回调。

  仅当动画图中存在控制器组件（例如，[**AnimatorController**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorController.html)、[**AnimatorOverrideController**](https://docs.unity.cn/cn/2022.1/ScriptReference/AnimatorOverrideController.html) 或 [**AnimatorControllerPlayable**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorControllerPlayable.html)）时才会发生此回调。

  **注意：**将此回调添加到 [**StateMachineBehaviour**](https://docs.unity.cn/cn/2022.1/ScriptReference/StateMachineBehaviour.html) 组件会禁用多线程的状态机评估。

- **OnStateMachineExit：**在**状态机更新 (State Machine Update)** 步骤中，当控制器的状态机进行流经 Exit 状态的转换时，将在最后一个更新帧上调用此回调。在转换到 **StateMachine** 子状态时不会调用此回调。

  仅当动画图中存在控制器组件（例如，[**AnimatorController**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorController.html)、[**AnimatorOverrideController**](https://docs.unity.cn/cn/2022.1/ScriptReference/AnimatorOverrideController.html) 或 [**AnimatorControllerPlayable**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorControllerPlayable.html)）时才会发生此回调。

  **注意：**将此回调添加到 [**StateMachineBehaviour**](https://docs.unity.cn/cn/2022.1/ScriptReference/StateMachineBehaviour.html) 组件会禁用多线程的状态机评估。

- **触发动画事件 (Fire Animation Events)：**调用在上次更新时间和当前更新时间之间采样的所有剪辑中的所有动画事件。

- **StateMachineBehaviour (OnStateEnter/OnStateUpdate/OnStateExit)：**一个层最多可以有 3 个活动状态：当前状态、中断状态和下一个状态。使用一个定义 **OnStateEnter**、**OnStateUpdate** 或 **OnStateExit** 回调的 StateMachineBehaviour 组件为每个活动状态调用此函数。

  依次针对当前状态、中断状态和下一个状态调用此函数。

  仅当动画图中存在控制器组件（例如，[**AnimatorController**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorController.html)、[**AnimatorOverrideController**](https://docs.unity.cn/cn/2022.1/ScriptReference/AnimatorOverrideController.html) 或 [**AnimatorControllerPlayable**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorControllerPlayable.html)）时才会执行此步骤。

- **OnAnimatorMove：**在每个更新帧中为每个 Animator 组件调用一次此函数来修改根运动 (Root Motion)。

- **StateMachineBehaviour(OnStateMove)：**使用定义此回调的 [**StateMachineBehaviour**](https://docs.unity.cn/cn/2022.1/ScriptReference/StateMachineBehaviour.html) 在每个活动状态中调用此函数。

- **OnAnimatorIK：**设置动画 IK。为每个启用 **IK pass** 的 Animator Controller 层进行一次此调用。

  仅当使用人形骨架时才会执行此事件。

- **StateMachineBehaviour(OnStateIK)：**使用在启用 **IK pass** 的层上定义此回调的 [**StateMachineBehaviour**](https://docs.unity.cn/cn/2022.1/ScriptReference/StateMachineBehaviour.html) 组件在每个活动状态中调用此函数。

- **WriteProperties：**从主线程将所有其他动画属性写入场景。

### 7.有用的性能分析标记

[脚本生命周期流程图](https://docs.unity.cn/cn/2022.1/Manual/ExecutionOrder.html#ScriptLifecycleFlowchart)中显示的某些动画函数不是可以调用的事件函数；它们是 Unity 处理动画时调用的内部函数。

这些函数具有 Profiler 标记，因此您可以使用 [Profiler](https://docs.unity.cn/cn/2022.1/Manual/Profiler.html) 查看 Unity 在帧中调用这些函数的时间。知道 Unity 调用这些函数的时间有助于准确了解所调用的事件函数的具体执行时间。

例如，假设在 **FireAnimationEvents** 回调中调用 [Animator.Play](https://docs.unity.cn/cn/2022.1/ScriptReference/Animator.Play.html)。如果知道只有在执行**状态机更新 (State Machine Update)** 和**流程图 (Process Graph)** 函数后才会触发 **FireAnimationEvents** 回调，就可以预期动画剪辑会在下一帧播放，而不是马上播放。

- **状态机更新 (State Machine Update)：**在执行序列的此步骤中评估所有状态机。仅当动画图中存在控制器组件（例如，[**AnimatorController**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorController.html)、[**AnimatorOverrideController**](https://docs.unity.cn/cn/2022.1/ScriptReference/AnimatorOverrideController.html) 或 [**AnimatorControllerPlayable**](https://docs.unity.cn/cn/2022.1/ScriptReference/Animations.AnimatorControllerPlayable.html)）时才会发生此回调。

  **注意：**状态机评估通常是多线程的，但添加某些回调（例如，**OnStateMachineEnter** 和 **OnStateMachineExit**）会禁用多线程。请参数上文的[动画更新循环](https://docs.unity.cn/cn/2022.1/Manual/ExecutionOrder.html#AnimationUpdateLoop)以了解详细信息。

- **ProcessGraph：**评估所有动画图。此过程包括对需要评估的所有动画剪辑进行采样以及计算根运动 (Root Motion)。

- **ProcessAnimation：**混合动画图的结果。

- **WriteTransforms：**将所有动画变换从工作线程写入场景。

  如果一个人形骨架的多个层启用了 **IK pass**，则该人形骨架可以有多个 **WriteTransforms** 通道（请参阅[脚本生命周期流程图](https://docs.unity.cn/cn/2022.1/Manual/ExecutionOrder.html#ScriptLifecycleFlowchart)）。

### 8. Rendering

- **OnPreCull：**在摄像机剔除场景之前调用。剔除操作将确定摄像机可以看到哪些对象。正好在进行剔除之前调用 OnPreCull。
- **OnBecameVisible/OnBecameInvisible：**对象变为对任何摄像机可见/不可见时调用。
- **OnWillRenderObject：**如果对象可见，则为每个摄像机调用**一次**。
- **OnPreRender：**在摄像机开始渲染场景之前调用。
- **OnRenderObject：**所有常规场景渲染完成之后调用。此时，可以使用 [GL](https://docs.unity.cn/cn/2022.1/ScriptReference/GL.html) 类或 [Graphics.DrawMeshNow](https://docs.unity.cn/cn/2022.1/ScriptReference/Graphics.DrawMeshNow.html) 来绘制自定义几何形状。
- **OnPostRender：**在摄像机完成场景渲染后调用。
- **OnRenderImage：**在场景渲染完成后调用以允许对图像进行后处理，请参阅[后期处理效果](https://docs.unity.cn/cn/2022.1/Manual/PostProcessingOverview.html)。
- **OnGUI：**每帧调用多次以响应 GUI 事件。首先处理布局和重新绘制事件，然后为每个输入事件处理布局和键盘/鼠标事件。
- **OnDrawGizmos** 用于在场景视图中绘制辅助图标以实现可视化。

**Note:** These callbacks only work with the Built-in Render Pipeline.

### 9. 协程

Update 函数返回后将运行正常协程更新。协程是一个可暂停执行 (yield) 直到给定的 YieldInstruction 达到完成状态的函数。 协程的不同用法：

- **yield** 在下一帧上调用所有 Update 函数后，协程将继续。
- **yield WaitForSeconds** 在为帧调用所有 Update 函数后，在指定的时间延迟后继续。
- **yield WaitForFixedUpdate** 在所有脚本上调用所有 FixedUpdate 后继续。如果协同程序在 FixedUpdate 之前生成，那么它会在当前帧的 FixedUpdate 之后继续运行。
- **yield WWW** 在 WWW 下载完成后继续。
- **yield StartCoroutine** 将协程链接起来，并会等待 MyFunc 协程先完成。

### 10. 销毁对象时

- **OnDestroy：**对象存在的最后一帧完成所有帧更新之后，调用此函数（可能应 Object.Destroy 要求或在场景关闭时销毁该对象）。

### 11. 退出时

在场景中的所有活动对象上调用以下函数：

- **OnApplicationQuit：**在退出应用程序之前在所有游戏对象上调用此函数。在编辑器中，用户停止播放模式时，调用函数。
- **OnDisable：**行为被禁用或处于非活动状态时，调用此函数。



## 1、常用工具（Package Manager & 内置工具插件）

1. 常用的摄像机管理 ：`Cinemachine`

   ![image-20230224163001478](/NoteIamges/1.常用工具/Cinemachine)

2. 动画任何通过简化的代码。`DOTween`是一个快速、高效、完全类型安全的面向对象动画引擎。

   ![image-20230224163047680](/NoteIamges/1.常用工具/DOTween)

3. `UI Toolkit`

  4. 在`Porject `下创建`Editor`文件夹，这个`Editor`文件夹不会经过打包

  5. 在`Editor`文件夹创建`UI Builder`文件夹

  6. 在`UI Builder`文件夹内，右键-> `UI Toolkit` -> `Editor Window` 输入对应的类名，这样会生成3个文件

     -  C#脚本
     -  样式文件
     -  `UI Builder`编辑器文件

  ![image-20230224163252755](/NoteIamges/1.常用工具/Toolkit_UI_Builder)



## 2、摄像机操作

### 1. 常用的摄像机管理工具`Cinemachine`，

在Hierarchy目录下右键使用`Ciemachine`创建对象（名字默认可能是`CM Vcam1`）

- `Main Camera` 对象：（主摄像机）

  > 在Main Camera 中添加组件 Pixel Perfect Camera，修改属性配置。

- `CM Vcam1`对象：（需要`Chinemachine`工具创建）

  > 在`CinemachineVirtualCamera`组件中的`Extensions`属性选择`Add Extension` 下拉框，选择 `Cinemachine Pixel Perfect`，他会根据`Pixel Perfect Camera`来处理
  > 最后`Lens Ortho Size`属性 改为9

  

### 2. 设置摄像机的缓冲（需要`Cinemachine`配合）

​	在`CM vcam1 `对象中， 在`CinemachineVirtualCamera`组件的`Body`属性设置参数，看个人喜好

### 3. 设置摄像机边界（需要`Cinemachine`配合）

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

![image-20230224163958999](/NoteIamges/2.摄像机操作/CM_vcam1)

## 3、图片操作

### 1. 图片统一预设处理

1. 当有N多个图片时，调整的参数配置都一样，那么就可以使用预设统一处理

2. 先在`Project`目录中点击图片，在`Inspector`窗口的右上角有个预设按钮![image-20230224154202679](/NoteIamges/3.图片操作/preset)，创建新的`Preset`

3. 然后多选图片，选中`Preset`内的预设，进行处理

### 2. `Player`与树跟（或者其它物品）的遮挡关系

### 3. 可以设置树根（或者其它物品）的`Sprite Renderer`组件的`Sprite Sort Point `属性改为`Pivot`

### 4. 找到`Sprite Renderer`组件的`Sprite`属性图片位置，选择图片点击`Sprite Editor`进入`Sprite Editor`窗口 

![image-20230224164428288](/NoteIamges/3.图片操作/image-2-2)

  1. 右下方的`Sprite`窗口，`Pivot`属性下拉选中`Custom`， 修改`Custom Pivot `的 `X || Y`，让其有遮挡的锚点位置![image-20230224164301095](/NoteIamges/3.图片操作/image-2-4)

### 5. 当`png`图片为`Button`时，设置马赛克区域不能被点击

​	![image-20230305042043284](NoteIamges/3.图片操作/5-2)

​	

>  选中图片，把`Advanced`属性的`Read/Write`勾选上

![image-20230305041739244](/NoteIamges/3.图片操作/5-1.png)


## 4、排序等级操作

### 1. `sorting group`组件

- 对象中添加`Sorting Group`组件，选中对应的`Sorting Layer`可以让父物体下的所有子物体，整体一起渲染来排序。

- 子物体也要选中对应的`Sorting Layer`

  ![image-20230224164714442](/NoteIamges/4.排序等级操作/sorting-1-2)

### 2. 遮挡的功能效果，设置整个项目按照 Y 轴渲染

1. `Unity`菜单栏：`Edit` -> `Project settings` -> `Graphics `

2. 找到`Camera Settings`配置项，`Transparency Sort Mode` 透明度排序模式，选择 `Custom Axis`就是按照自定义轴的模式

3. `Transparency Sort Axis`配置项为 `x=0,y=1,z=0` 这样根据Y轴就有遮挡关系

> 注意事项：在`Sprite Renderer`组件下`Sorting Layer`必须是相同的，且`Player`的图片切割锚点需要在底部

![image-20230224164714442](/NoteIamges/4.排序等级操作/sorting-2-3)

## 5、Tilemap 操作

### 1. `Tile Palette` 的翻转功能（需要新的版本）

1. 在 `Tile Palette ` 窗口中，在功能按钮旁边的区域，进行鼠标右键，点击  `open Tile palette Preferences`选项进行预设

   ![image-20230224165327011](/NoteIamges/5.Tilemap操作/Tilemap-1-1)

2. 在`Tile Palette`窗口找到 `Default Tile Palatte Tools`，添加新的功能，然后`save`

   ![image-20230224165901649](/NoteIamges/5.Tilemap操作/Tilemap-1-2)

### 2. `Scene` 窗口网格区域的对焦模式（显示当前访问的绘画区域）

1. `Scene `窗口网格区域最上方右边有个网格（对焦模式），选择`Tilemap`对焦
2. 当`Tile Palette` 窗口的`Active Tilemap`下拉栏选择了`Tilemap（Ground）`那么只会显示`Ground`区域的`Tile`

![image-20230224165943865](/NoteIamges/5.Tilemap操作/Tilemap-2-2)

### 3. 绘制区域可以使用![image-20230224161959530](/NoteIamges/5.Tilemap操作/TilePalette_box)快速绘制，也可以使用`shift`+ ![image-20230224161959530](/NoteIamges/5.Tilemap操作/TilePalette_box)进行快速删除

### 4. 笔刷可以绘画出随机`Tile`, `Tile Palette`窗口下方有`Default Brush`下拉框，可以进行下拉选择`Random Brush`进行配置

![image-20230224170036896](/NoteIamges/5.Tilemap操作/Tilemap-4-1)

### 5. `Rule Tile` 可以进行规则绘制，减少绘制量。

1. 在`Project`目录下鼠标右键 -> `Create` -> `2D` -> `Tiles`-> `Rule Tile`
2. 根据不同图片进行规则绘制

![image-20230224170200939](/NoteIamges/5.Tilemap操作/Tilemap-5-2)



### 6. 当瓦片地图`(Tilemap)`出现缝隙，可以使用`Sprite Atlas`功能进行处理，这样可以把所有图片素材打包成一张图片，然后根据不同平台进行压缩优化。

1. 在`Project`目录下鼠标右键 -> `Create` -> `2D` -> `Sprite Atlas` 新建`Sprite Atlas`
2. 把需要打包的图片素材的`文件夹`拖到 `Objects for packing `
3. 修改` Filter Mode` 为`Point `像素，` Compression` 为`None `最后进行`Pack preview`
4. 注意：图片素材可能会干扰其他图片，可以把`Padding` 改大点

![image-20230224170418595](/NoteIamges/5.Tilemap操作/Tilemap-6-4)

### 7. 瓦片地图碰撞（整体）

1. 给`Tilemap` 添加 `Tilemap Collider 2D`组件
2. 为了让`Tilemap`整体，添加`Composite Collider 2D`，（给`Rigidbody 2D`的属性`Type` 改为`Staitc` , 看游戏需要）
3. 最后`Tilemap Collider 2D`组件，勾选`Used By Composite`

![image-20230224171439947](/NoteIamges/5.Tilemap操作/Tilemap-7-3)

### 8. 对于不规则的碰撞`Tilemap`处理

1. 可以回到碰撞体图片，点击`Sprite Editor`进行编辑 , `Sprite Editor` 下拉菜单选中 `Custom Physics Shape`

2. 选中单独的`Tile`，点击上方的`Generate`，然后修改成自己想要的效果， 修改完成后点击`Apply`

3. 关闭`Collision`，然后重新打开，就会完成绘制

  ![image-20230224172133256](/NoteIamges/5.Tilemap操作/Tilemap-8-3)

## 6、组件操作

### 1. 组件复制

- 当组件配置好之后，可以右上角3点号进行Copy，可以复制到其他对象

![image-20230224173614771](/NoteIamges/6.组件操作/component-1-1)

### 2. Rect Transform （center位置）

> 点击矩阵center的多种操作

![image-20230228235218282](/NoteIamges/6.组件操作/7-component-1-1)

1. 默认情况下

   - 对象位置（白色边框）

   - 枢轴（黄色点）类似于原点

![image-20230301002500061](/NoteIamges/6.组件操作/7-component-1-2)

2. 按住`shift`键：设置枢轴位置

![image-20230301003613870](/NoteIamges/6.组件操作/7-component-1-3)

3. 按住`alt`键：设置物体位置

![image-20230301005259245](/NoteIamges/6.组件操作/2-3)

4. 同时按住`alt+shift`键：设置2个位置

   ![image-20230301005450767](/NoteIamges/6.组件操作/2-4)

### 3. Horizontal Layout Group（水平布局组）

- 水平布局排列
- 当父元素使用这个`Horizontal Layout Group`组件，子组件会受到`Layout`影响，可以生成对应的布局
- 子组件使用`Layout Element`组件，勾选上`ignore Layout`让其不受影响

### 4. Grid Layout Group（网格布局组）

​	可以设置网络布局。可用于背包的格子布局，设置对应的大小以及间距等等

### 5. enable 属性（组件是否激活）

​	这个属性是针对对象里面的组件是否勾选（激活）

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("拖拽图片")]
        public Image dragItem;
        
        // 不激活Image属性
        private void NotJiHuo(){
            dragItem.enable = false;
        }
    }

}


```

### 6. Vertical Layout Group（垂直布局组）

​	可以设置垂直的布局

### 7. Content Size Fitter（内容尺寸装饰）

​	可以根据实际内容扩展大小区域

### 8. Layout Element（布局元素）

​	布局元素： 可以设置对象的宽高，最大最小宽高，以及期望值等待

### 9. Mask （遮罩）


## 7、文件操作

### 1. （SO文件）创建`ScriptableObject`的`C#`文件，用于存储数据

```c#
[CreateAssetMenu(fileName ="ItemDataList_SO", menuName = "Inventory/ItemDataList")]

public class ItemDataList_SO: ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
}
```

使用方式：在`Project` 目录，`右键空白区域` -> `Inventory` -> `ItemDataList`，即可生成对应的`SO`文件



## 8、事件接口

### 1.  `UnityEngine.EventSystems` 

- > 建议去2019.3版本之前的手册观看
  >
  > 地址格式：`https://docs.unity.cn/cn/2019.3/ScriptReference/EventSystems.接口名字.html`
  >
  > 例如，想观看`IPointerClickHandler `接口
  >
  > https://docs.unity.cn/cn/2019.3/ScriptReference/EventSystems.IPointerClickHandler.html

  - IPointerEnterHandler - OnPointerEnter - 当指针进入对象时调用
  - IPointerExitHandler - OnPointerExit - 当指针退出对象时调用
  - IPointerDownHandler - OnPointerDown - 在对象上按下指针时调用
  - IPointerUpHandler - OnPointerUp - 松开指针时调用（在指针正在点击的游戏对象上调用）
  - IPointerClickHandler - OnPointerClick - 在同一对象上按下再松开指针时调用
  - IInitializePotentialDragHandler - OnInitializePotentialDrag - 在找到拖动目标时调用，可用于初始化值
  - IBeginDragHandler - OnBeginDrag - 即将开始拖动时在拖动对象上调用
  - IDragHandler - OnDrag - 发生拖动时在拖动对象上调用
  - IEndDragHandler - OnEndDrag - 拖动完成时在拖动对象上调用
  - IDropHandler - OnDrop - 在拖动目标对象上调用
  - IScrollHandler - OnScroll - 当鼠标滚轮滚动时调用
  - IUpdateSelectedHandler - OnUpdateSelected - 每次勾选时在选定对象上调用
  - ISelectHandler - OnSelect - 当对象成为选定对象时调用
  - IDeselectHandler - OnDeselect - 取消选择选定对象时调用
  - IMoveHandler - OnMove - 发生移动事件（上、下、左、右等）时调用
  - ISubmitHandler - OnSubmit - 按下 Submit 按钮时调用
  - ICancelHandler - OnCancel - 按下 Cancel 按钮时调用



## 8、Animator 动画制作器

### 1. 创建一个人物基础通用的动画控制器

1. 创建一个`BaseController`的`Animator`,打开`Animator`窗口
2. 鼠标在空白处右键新建一个`Blend Tree`，名为`idle`，这个状态是战力时候的状态
3. ![image-20230304010147135](/NoteIamges/8.Animator动画制作器/1-1.png)
4. 双击进入`idle`，进行编辑
5. 在左上方`Parameters`创建三个参数，后面需要
6. 选中`Blend Tree`，右边`Inspector`会生成对应的窗口，进行后续处理
7. ![image-20230304010740104](/NoteIamges/8.Animator动画制作器/1-2.png)
8. 左上方，点击`Base Layer`返回主页面
9. 鼠标右键创建一个`Walk Run`状态，用于处理行走、跑步。
10. ![image-20230304011156982](/NoteIamges/8.Animator动画制作器/1-3.png)
11. 连线根据`isMoving`参数进行连线
12. ![image-20230304011429834](/NoteIamges/8.Animator动画制作器/1-4.png)
13. 双击`Walk Run`进入编辑树，详细参数自行设计
14. ![image-20230304011556585](/NoteIamges/8.Animator动画制作器/1-5.png)
15. 回到`Project`目录窗口
16. 右键空白处 ->  `Create` -> `Animator Override Controll` -> 命名为`Body`
17. 点击`Body`，把`BaseController `拖拽进去，这样就会通用所有状态
18. ![image-20230304011909214](/NoteIamges/8.Animator动画制作器/1-6.png)
19. 制作`Animations`，把所有对应的`Animation`拖拽进去
20. ![image-20230304015035961](/NoteIamges/8.Animator动画制作器/1-7.png)
21. `Body`对象使用该`Animator`，后面的`Hair、Arm`也是类似的同样操作


## 功能代码实现

### 1. 让unity识别class对象，在Inspector显示的话，记得加上序列号

  ```c#
[System.Serializable]
public class ItemDetails（）{
    
}
  ```

### 2. Event 事件驱动

> EventHandler.cs 定义一个委托

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    // 定义一个委托：注册更新InventoryUI
    public static event Action<InventoryLocation, List<InventoryItem>> OnUpdateInventoryUI;


    // 用来被订阅的委托
    public static void CallOnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        OnUpdateInventoryUI?.Invoke(location, list);
    }
}

```

> InventoryUI.cs 订阅一个委托

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            // 订阅委托
            EventHandler.OnUpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            // 取消委托
            EventHandler.OnUpdateInventoryUI -= OnUpdateInventoryUI;   
        }


        private void Start()
        {
            // 给每一个格子赋值
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex= i;
            }
        }

        private void OnUpdateInventoryUI(InventoryLocation lcation, List<InventoryItem> list)
        {
            switch(lcation)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmout > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmout);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }

    }

}

```

> InventoryManager.cs 调用委托

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            // 初始化背包
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 通过ID，返回对应的物品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID== ID);
        }

        /// <summary>
        /// 添加物品到Player背包里
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否需要销毁物品</param>
        public void AddItem(Item item, bool toDestory)
        {
            // 背包是否有这个物品
            var index = GetItemIndexInBag(item.itemID);

            // 添加物品
            // 还没处理： 背包不存在物品，且背包没有空位的时候
            AddItemAtIndex(item.itemID, index, 1);

            if(toDestory)
            {
                Destroy(item.gameObject);
            }

            // 更新UI, 调用委托
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品的ID</param>
        /// <param name="index">物品在背包的序号</param>
        /// <param name="amount">添加的数量</param>
        private void AddItemAtIndex(int ID, int index, int amount =1)
        {
            // index == -1 背包没有这个物品
            if(index == -1)
            {
                // 1. 背包没有空位
                if (!CheckBagCapacity())
                {
                    return;
                }

                // 2. 有空位
                // 生成新的物品
                var item = new InventoryItem {itemID = ID, itemAmout = amount};

                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    // 当itemID为0，背包有位置，位置为 i
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item; // 添加物品到Bag
                        break;
                    }
                }

            }
            else  // 背包有这个物品
            {
                int currentAmount = playerBag.itemList[index].itemAmout + amount;
                var item = new InventoryItem { itemID= ID, itemAmout = currentAmount};
                playerBag.itemList[index] = item;
            }
        }
    }
}


```

### 3. 拖拽图片功能

1. 当拖动图片的时候，记得把`Image`组件下的`Raycast Target` 勾选取消掉

![image-20230302025221152](/NoteIamges/功能代码实现/3-1.png)

2. `Raycast Target`会阻止鼠标的射线判断，导致拖动图片下的物体获取不到（因为被图片挡住了）

3. 当拖拽结束时，鼠标射线会获取当前的位置的物体，如下（可能会随机获取到4个不同的物体）

![image-20230302024439310](/NoteIamges/功能代码实现/3-2.png)

4. 为了指定的获取到`Slot_Bag`对象，可以把`Slot_Bag`对象下的子元素的`Raycast Target`勾选取消掉，这样就能获取到`Slot_Bag`对象



### 4. 序列化属性（SerializeField）

​	`[SerializeField]` 可以让属性在unity上显示，和`public`不同的是，可以是一个`private`属性，`public`属性会被访问到

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;
}

```

### 5. RequireComponent（要求有一个属性在这个组件中）

​	`RequireComponent`属性自动添加所需的组件作为依赖项。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    // 要求有一个SlotUI属性在这个组件中
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour
    {
        private SlotUI slotUI;
    }

}
```

### 6. UI渲染有延迟处理

​	在布局中，Text可能会有单列、甚至多列，会导致布局不能及时更改，这时候需要强制刷新

```c#
public class Text(){
    private void UpdateUI(){
        // 计算 .......
        // UI 计算渲染
        // 计算结束
        // 强制立即重新构建受计算影响的布局元素和子布局元素。（记得为对应的对象）
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
```





## Unity 知识

### 1. 学习手册

​	组件右上方有圆圈问号，可以点击跳转到手册

### 2. UI Toolkit 知识

- 创建`UI Toolkit`编辑器窗口： 

  `Project`目录 -> `鼠标右键 `-> `Create` -> `UI Toolkit` -> `Editor Window`->`输入名字 Confirm` -> `生成三个文件`

  	- `c#` 脚本，控制渲染
  	- `UXML`  页面展示 ：双击文件，可以打开 `UI Builder`进行页面结构编辑
  	- `USS` 样式

- `C#`脚本中查询`UI Toolkit`的元素

  在`uxml (Visual Tree Asset )`所有编辑的`UI`中，用到的`Library`元素，都可以通过以下方式进行查找，如下

​		  ![image-20230224163638297](/NoteIamges/知识点/search_ui)

​		  `root.Q< Standard下的元素名字 >("Hierarchy下的元素的name")`

  ```
#Container
   #ItemList（VisualElement）
  ```

​		  最后的查询语法`root.Q<VisualElement>("ItemList")` : 根元素查询名为 `ItemList `的 `VisualElement`

  

## C# 知识

### 1. c# switch 语法糖

```c#
public class Text(){
    private string GetItemType(IItemType itemType)
    {
        return itemType switch
        {
            IItemType.Seed => "种子",
            IItemType.Commodity => "商品",
            IItemType.Furniture => "家具",
            IItemType.BreakTool => "工具",
            IItemType.ChopTool => "工具",
            IItemType.CollectTool => "工具",
            IItemType.HoeTool => "工具",
            IItemType.ReapTool => "工具",
            IItemType.WaterTool => "工具",
            _ => "无"
        };
    }
}
```





## 学习思路

1. `Player` 挂载`TriggerEnter`， 然后触发，调用其他对象所挂载的函数

## Unity 目录

```markdown
├└┴┘┤┼┬┐┌│─

Assets
	├─ Editor 不会被打包的目录，可以放置一些在开发过程使用的东西，例如UI-Tookit等
	├─ Prefabs 用于放置预制件
	├─ Scripts 用于放置脚本
	├─ Scenes 用于放置场景
```