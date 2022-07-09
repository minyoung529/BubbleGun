# Bubble Gun!
2학년 1학기 엔진 응용 시간에 만든 FPS를 기반으로 새롭게 만든 개인 FPS  프로젝트입니다.

![BubbleGun1](https://user-images.githubusercontent.com/77655318/178115798-0958b63e-4148-4d39-9e32-cf402d4a42d4.png)

[플레이 영상](https://www.youtube.com/watch?v=83_IZzUd3jE)

## 게임 소개

### 만들게 된 배경
![화면 캡처 2022-07-10 015524](https://user-images.githubusercontent.com/77655318/178115525-8238758d-e301-4a0d-a135-090bb3a5ef3e.png)

우연히 보게 된 위 껌 사진이 <b>총알</b>과 비슷하다는 생각을 하게 되었습니다. 또, Gum과 Gun의 발음이 비슷한 것을 느껴 ‘<b>껌을 쏘는 총</b>이 있다면 어떨까?’라는 생각으로 하게 되었습니다. 

그러다가 채소들에게 껌을 쏴 껌으로 만드는 게임의 핵심 시스템이 떠올랐습니다. 이 간단한 핵심 시스템으로 게임 경험이 없는 게임 유저 또한 간단하지만 재미있게 즐길 수 있는 게임을 만들어보고 싶었습니다.


### 콘셉트

**Chewing Gun!** Chewing Gum + Gun. 합쳐서 **껌을 쏘는 총**이다. 주인공이 꿈에서 평소에 맛이 없다고 느껴 싫어하는 음식, 즉 채소들에게 복수하는 스토리이다.

주인공이 편식하는 대상인 못된 **채소**들에게 껌을 쏘는데, 채소들에게 껌을 쏴 그들을 맛있는 **껌**으로 만드는 컨셉입니다. 야채들을 모두 껌으로 만들고, **최종 보스인 양파**까지 껌으로 만들어 채소들로부터 점령된 도시를 구하는 스토리입니다.

### 타겟
* 간단하고 단순하지만 중독성 있는 게임을 좋아하는 타겟층
* 귀엽고 만화같은 3D 그래픽을 좋아하는 타겟층
* FPS 게임을 좋아하는 타겟층


### 재미 요소
#### 1. 껌을 쏠 때의 타격감
껌을 쏠 때에 껌에 맞는 오브젝트에 이펙트와 사운드를 넣었습니다. 이펙트는 물감과 같은 재질에 쏜 껌의 색깔을 바탕이고 또, 이펙트가 닿은 곳은 물감처럼 다른 곳에 묻고 번지게 해 바닥이나 몬스터를 껌의 색깔로 물들게 했습니다.

<br>아이디어 참고: Splatoon

#### 2. 귀여운 그래픽과 캐릭터
게임의 스토리와 같이 그래픽 컨셉 또한 동화같은 카툰 그래픽이기 때문에 가볍고 귀엽습니다. 플레이어는 이러한 귀여운 그래픽을 보며 힐링하고 재미를 느낄 수 있습니다.

#### 3. 다양한 스킬
플레이어를 중심으로 특정 범위에 들어온 몬스터는 즉사하는 스킬, 몬스터 스턴 스킬 등 많은 스킬들이 있습니다. 이로 하여금 플레이어는 다채로운 플레이와 전략을 세울 수 있고, 타격감 있는 TPS 게임을 즐길 수 있습니다.


## 게임 이미지
![11](https://user-images.githubusercontent.com/77655318/178116093-1dff5c87-3565-4b4a-a8d4-a6e7957e8954.png)
![22](https://user-images.githubusercontent.com/77655318/178116098-a3f7d206-b95d-4eb8-bf5e-897b3f33c048.png)
![33](https://user-images.githubusercontent.com/77655318/178116104-fc702b5e-6105-4ff7-add1-aa5bcb76b224.png)
![44](https://user-images.githubusercontent.com/77655318/178116107-8f66563f-da6b-49a1-a890-7c50dbac4b34.png)
![555](https://user-images.githubusercontent.com/77655318/178116112-3d985f4c-ee83-424f-bf33-86e7a57b2d87.png)
![666](https://user-images.githubusercontent.com/77655318/178116114-fb15edc3-66f4-43d3-8832-f8ca0fb51419.png)
![777](https://user-images.githubusercontent.com/77655318/178116118-8fa77efc-3683-467a-a497-3f57412f7408.png)
![888](https://user-images.githubusercontent.com/77655318/178116122-1e852713-ee8d-4fe4-829b-dcd6819c8168.png)
![999](https://user-images.githubusercontent.com/77655318/178116126-f8ea08c1-117e-4148-b86d-ce57f314a8d9.png)
![10](https://user-images.githubusercontent.com/77655318/178116128-ada664f8-2172-4aa8-975b-8fa24bf02556.png)
![1111](https://user-images.githubusercontent.com/77655318/178116130-a24b92ca-f5d7-4adf-83a5-46f77f13a9bb.png)


## 구현

* 물감처럼 오브젝트에 묻고 번지는 이펙트를 구현하기 위해서 영상을 찾아보았습니다.<br>
[Splatoon - Painting Effect in Unity](https://www.youtube.com/watch?v=YUWfHX_ZNCw)<br><br>

* 카툰 그래픽을 구현하기 위해 [유니티짱 툰쉐이더](https://unity-chan.com/contents/guideline/?id=ssu_urp)를 사용했습니다.
