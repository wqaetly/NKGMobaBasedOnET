# NKGMobaBasedOnET

## 介绍
基于ET框架致敬LOL的Moba游戏，包含完整的客户端与服务端交互，热更新，基于双端行为树的技能系统，更多精彩等你发现！

如果你对这个开源项目有好的想法或者想和大家一起交流，可以提Issues或者加QQ群：959572557

对于想系统学习本项目而无从下手的，推荐去看看本项目的Wiki，里面有运行指南和基础教程以及常见问题，相关技术点讲解（如果运行出现问题请先看Wiki，解决99%问题）。
[这是Wiki地址](https://gitee.com/NKG_admin/NKGMobaBasedOnET/wikis)

**引导演示视频：[视频链接](https://www.bilibili.com/video/av74833675)** 

**基于行为树的技能系统架构讲解视频：[视频链接](https://www.bilibili.com/video/av85318986)** 

本项目中所有的插件仅供学习交流使用，请务必支持正版！

最后，大家一起加油！



## 特别鸣谢

感谢JetBrains公司提供的使用许可证！

<p><a href="https://www.jetbrains.com/?from=NKGMobaBasedOnET ">
<img src="https://images.gitee.com/uploads/images/2020/0722/084147_cc1c0a4a_2253805.png" alt="JetBrains的Logo" width="20%" height="20%"></a></p>


## 运行环境

 **编辑器：Unity 2019.4.8 LTS** 

 **客户端：.Net Framework 4.7.2** 

 **IDE：JetBrain Rider 2020**

 **服务端：.Net Core 2.2** 

## 已实现功能列表


| 功能内容                                                 | 时间节点       |
| -------------------------------------------------------- | -------------- |
| 整合FairyGUI作为UI方案                                   | 2019.4.22      |
| 丰富资源模块功能，完成打包工具制作                       | 2019.4.27      |
| 登录注册，接入MongoDB数据库，账号之间冲突处理，心跳开发  | 2019.5.1       |
| 人物同步，寻路（包含点击小地图寻路）                     | 2019.5.11      |
| 整合可视化节点编辑器（配置树状数据）                     | 2019.5.19      |
| 整合Box2D作为服务端碰撞方案                              | 2019.6.10      |
| 提取LOL资源作为项目主要资源                              | 2019.6.20      |
| 整合行为树到客户端和服务端，将配合技能编辑器制作技能系统 | 2019.6.25      |
| 选定状态同步为游戏同步策略，初步实现多玩家动画同步工作   | 2019.7.3       |
| 完成Box2D可视化编辑器的制作                              | 2019.7.19      |
| 完成碰撞关系可视化编辑器的制作，附带自动生成代码功能     | 2019.8.1       |
| 完成服务端Box2D相关架构的搭建                            | 2019.8.14      |
| 完成NPBehave行为树可视化编辑器制作v0.0.1                 | 2019.8.26      |
| 实现基于NPBehave的可视化技能系统编辑器v0.0.2             | 2019.9.28      |
| 诺克萨斯之手Q技能完整流程Demo                            | 2019.10.3      |
| 基本完成客户端与服务端技能系统的通信与同步               | 2020.1.25      |
| 准备发行版的技能编辑器和技能系统v0.0.2-v0.10.0           | 2020.3~2020.8  |
| 更新FGUI至2020最新版，并升级插件                         | 2020.8.9       |
| 重构优化技能编辑器GUI和技能系统运行时逻辑v0.10.1         | 2020.8.17~当前 |
| 接入Universal Render Pipeline                            | 2020.10.5      |
|                                                          |                |


## 开发计划

1. 重构优化完善技能编辑器GUI，技能系统Runtime
2. 使用Shader Graph实现人物描边效果，人物阴影效果，人物在河道行走时的水波纹效果，战争迷雾效果
3. 使用Visusl Effect Graph重新制作特效
4. 接入C++版本的Recast寻路
5. 加入寒冰，盖伦，赵信
6. 开发匹配系统



## 开发进度展示
资源热更新界面
![image-20200722083928209](https://images.gitee.com/uploads/images/2020/0722/084147_fc1f9a7c_2253805.png)
登录界面
![image-20200722083940466](https://images.gitee.com/uploads/images/2020/0722/084147_70475065_2253805.png)
大厅界面
![image-20200722083952197](https://images.gitee.com/uploads/images/2020/0722/084147_e41d6ac7_2253805.png)
战斗界面
![image-20200722084012352](https://images.gitee.com/uploads/images/2020/0722/084147_079e755b_2253805.png)
基于Monkey Commander改造的编辑器拓展，按F呼出界面，输入关键字，选中之后点击/回车即可运行
![基于Monkey Commander改造的编辑器拓展，按F呼出界面，输入关键字，选中之后点击/回车即可运行](https://images.gitee.com/uploads/images/2020/1029/194658_b5dee162_2253805.png "QQ截图20201029192331.png")
Box2D编辑器
![image-20200722084025787](https://images.gitee.com/uploads/images/2020/0722/084147_be3e8764_2253805.png)
技能编辑器v0.0.3
![技能编辑器v0.0.3](https://images.gitee.com/uploads/images/2020/1002/235719_dd64d0a5_2253805.png "QQ截图20201002234438.png")
技能系统架构图
![163758_138e22e9_2253805](https://images.gitee.com/uploads/images/2020/0722/084148_1f2eb6b1_2253805.png)