1.如果镜子反射出现错位现象，把镜子的父物体全部干掉
2.如果报错 no more space in the planar reflection probe atlas（有的镜子会什么也不反射），需注意每个镜子有一个resolution，它们相加不能超过2048（hdrp的设置里可以改）
3.IPointerClick触发条件有三，一是摄像机有physics raycast，二是脚本所挂物体有collider，三是有eventsystem
4.RObj=RotatableObject，即可旋转的物体 S=Single，即单个 SRObj=SinglRotatableObject
5.按钮碰撞体搞大点，不然人物旋转时没接触就会无法旋转
6.robj的状态进入finished后，要把switchstates里切换至correct的语句关掉
7.NavMeshManager使用相关 需要满足以下条件：
一：摄像机有physics raycast
二：路径挂有collider
8.楼梯上不去的话，调大agent的size，或者调下楼梯的scale
9.rotation是个四元数，最大值是1.0，不要把它同eulerangels弄混
10.设置平移是注意translate的相对参考系，是space.self还是space.world