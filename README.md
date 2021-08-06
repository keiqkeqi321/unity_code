
对象池的AudioManager：
新建空物体AudioManager，并挂载同名脚本；
在AudioManager上设定AudioClipArray大小，并从Project中拖拽音频文件
创建Audiomixer ，在其中创建音频组对应名字：musicGroup,environmentGroup,playerFXGroup,playerVoiceGroup,FXGroup;
脚本中的PlayEffect(string acName) 、 PlayBGM(string acName) 方法将音源输出到对应的音频组中，
创建Slider组件,添加on valve change 事件 将AudioManager挂载进来，选择AudioManager.Setvolue()方法；
通过调用 PlayEffect(string acName) 、 PlayBGM(string acName) 、 StopBGMPlay() 等方法对音频播放进行操控



Singleton:
范式基类，继承于它的子类会变成单例模式

继承于singleton的audiomagerNew：
封装了播放Music，播放ambient，播放FX,播放随机音高FX和随机音高随机片段的方法；外加包含audiosourse数据的类audiodata，在需要播放音乐的类中声明此类，通过audioManagerNew.instance.播放函数（AudioData audio）来播放指定音乐片段；



PlatformMoment：
封装平台移动，跳跃，下蹲，抓握上攀 的方法。
