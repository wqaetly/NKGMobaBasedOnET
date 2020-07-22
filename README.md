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

 **编辑器：Unity 2019.4.0 LTS** 

 **客户端：.Net Framework 4.7.2** 

 **IDE：JetBrain Rider 2020**

 **服务端：.Net Core 2.2** 


## 已实现功能列表

- 整合FairyGUI 2019.4.27
- 丰富AB编辑器接口，专门为FairyGUI增加了AB标签工具   2019.4.27
- 资源的下载更新界面和逻辑开发完成   2019.4.28
- 登录注册，接入MongoDB数据库  2019.5.1
- 游戏大厅UI，账号之间冲突处理，心跳开发  2019.5.6
- 人物同步，自动寻路（包含点击小地图寻路）2019.5.11
- 整合可视化结点编辑器（配置树状数据） 2019.5.19
- 将人物和技能数据转移到服务端并解析  2019.5.27
- 整合Box2D作为服务端碰撞方案  2019.6.10
- 提取LOL资源作为项目主要资源 2019.6.20
- 整合行为树到客户端和服务端，将配合技能编辑器制作技能系统 2019.6.25
- 新增贴图（dds）工作流，提高开发效率  2019.7.2
- 选定状态同步为游戏同步策略，初步实现多玩家动画同步工作 2019.7.3
- 完成Box2D可视化编辑器的制作  2019.7.19
- 重构可视化结点编辑器代码，将用它做更多的事情 2019.7.24
- 完成碰撞关系可视化编辑器的制作，附带自动生成代码功能 2019.8.1
- 完成服务端Box2D相关架构的搭建 2019.8.14
- 移除传统行为树，完成NPBehave行为树可视化编辑器制作(dev分支) 2019.8.26
- 基本实现基于NPBehave的可视化技能系统编辑器 2019.9.28
- 基于NPBehave的技能系统完成，附诺克Q技能完整流程Demo 2019.10.3 
- 基本完成客户端与服务端技能系统的通信与同步 2020.1.25

## 开发计划

1. 使用Shader实现人物描边效果
2. 使用Shader实现人物阴影效果
3. 使用Shader实现人物在河道行走时的水波纹效果
4. 使用Shader实现战争迷雾效果

## 开发进度展示

![image-20200722083928209](https://images.gitee.com/uploads/images/2020/0722/084147_fc1f9a7c_2253805.png)
![image-20200722083940466](https://images.gitee.com/uploads/images/2020/0722/084147_70475065_2253805.png)
![image-20200722083952197](https://images.gitee.com/uploads/images/2020/0722/084147_e41d6ac7_2253805.png)
![image-20200722084012352](https://images.gitee.com/uploads/images/2020/0722/084147_079e755b_2253805.png)
![image-20200722084025787](https://images.gitee.com/uploads/images/2020/0722/084147_be3e8764_2253805.png)
![image-20200722084036165](https://images.gitee.com/uploads/images/2020/0722/084147_22c5cb4d_2253805.png)
![image-20200722084047541](https://images.gitee.com/uploads/images/2020/0722/084148_6d7d9888_2253805.png)
![163758_138e22e9_2253805](https://images.gitee.com/uploads/images/2020/0722/084148_1f2eb6b1_2253805.png)