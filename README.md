# Game Producer 2｜雙人合作動作解謎

由三人團隊製作的雙人合作動作解謎遊戲，使用 Unity、C# 與 Photon PUN 開發。

兩名玩家分別操作「美術」與「程式」角色，透過繪圖生成物件、操控場景物件等能力，合作通過關卡。

[作品介紹與技術解析](https://tsukimi123.github.io/GameProducer2.html) · [觀看宣傳影片](https://tsukimi123.github.io/Project/GameProducer2/video/gameproducer2-promo.mp4)

[![Game Producer 2 遊玩畫面](https://tsukimi123.github.io/Project/GameProducer2/hero-gameplay.webp)](https://tsukimi123.github.io/GameProducer2.html)

## 專案資訊

- 開發期間：2021.09–2022.06
- 製作團隊：三人
- 遊玩形式：PC、雙人連線合作
- 開發工具：Unity 2019.4.41f2、C#
- 連線技術：Photon PUN
- 專案成果：2022 金點新秀設計獎入圍

## 負責項目與團隊分工

團隊共同討論玩法、角色能力與關卡需求，分別負責企劃與 3D 美術、2D 美術，以及程式開發。

我負責程式實作與系統整合，主要包含：

- 美術角色的繪製輸入、路徑取樣與物件生成。
- 程式角色的物件選取、移動、縮放與投擲。
- Photon PUN 房間流程、物件控制權與角色位置同步。
- 關卡互動、檢查點與重生流程整合。

## 核心玩法

| 角色 | 能力 | 合作方式 |
| --- | --- | --- |
| 美術 | 繪製路徑並生成可互動的場景物件 | 利用生成物件協助通過機關 |
| 程式 | 選取並移動、縮放或投擲場景物件 | 操控物件，與另一位玩家配合解謎 |

## 技術實作

### 繪製輸入與物件生成

使用 Input System 的輸入事件管理繪製流程：開始操作時建立繪製物件，持續操作時取樣位置，放開後停止繪製。

繪製物件透過 `PhotonNetwork.Instantiate` 生成，路徑由 `LineRenderer` 記錄並呈現，整合進多人關卡的互動流程。

相關程式：[ArtistSkill.cs](Assets/Scripts/Artist/ArtistSkill.cs)

### 場景物件操作

拖曳與投擲時，我用 Raycast 選取可操作的物件，透過 `ChangeOwner` 請求控制權，再用 Rigidbody 移動或投擲物件。

縮放模式會發射 `ScaleBullet`。命中允許縮放的物件時，`AddScale` 會將它放大，並限制放大後的尺寸。

相關程式：[ProgrammerSkill.cs](Assets/Scripts/Programmer/ProgrammerSkill.cs) · [ChangeOwner.cs](Assets/Scripts/Programmer/ChangeOwner.cs) · [AddScale.cs](Assets/Scripts/Programmer/AddScale.cs)

### 房間流程與角色同步

使用 Photon PUN 實作建立房間、加入房間、玩家列表及場景同步。

角色位置由本機端傳送，遠端副本接收後，依目前位置與接收位置的差距修正移動；差距過大時直接校正位置。本機角色不套用這段遠端修正，避免覆蓋玩家當下的操作。

開發期間，我先在同一台電腦開啟兩個客戶端，比對兩邊看到的角色位置與物件操作，再和團隊成員連線遊玩，確認兩名玩家能合作通過關卡。

相關程式：[Launcher.cs](Assets/Scripts/Lobby/Launcher.cs) · [NetworkPlayer.cs](Assets/Scripts/Artist/NetworkPlayer.cs)

### 檢查點與重生

角色進入檢查點時，先排除遠端角色副本的觸發，再依角色類型透過 RPC 更新重生座標，分別保存兩名玩家的重生位置。

相關程式：[SpawnPoint.cs](Assets/Scripts/System/SpawnPoint.cs)




## 素材來源

遊戲素材由團隊成員製作；我負責程式開發與系統整合。
