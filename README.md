# unity_code
如何使用：
新建空物体AudioManager，并挂载同名脚本；
在AudioManager上设定AudioClipArray大小，并从Project中拖拽音频文件
创建Audiomixer ，在其中创建音频组对应名字：musicGroup,environmentGroup,playerFXGroup,playerVoiceGroup,FXGroup;
脚本中的PlayEffect(string acName) 、 PlayBGM(string acName) 方法将音源输出到对应的音频组中，
创建Slider组件,添加on valve change 事件 将AudioManager挂载进来，选择AudioManager.Setvolue()方法；
通过调用 PlayEffect(string acName) 、 PlayBGM(string acName) 、 StopBGMPlay() 等方法对音频播放进行操控
