
# 💻 프로젝트 이름
Legendary Fruit : The Ultimate Weapon

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [팀소개](#팀소개)
3. [프로젝트 계기](#프로젝트-계기)
4. [주요기능](#주요기능)
5. [개발기간](#개발기간)
6. [기술스택](#기술스택)
8. [와이어프레임](#와이어프레임)
10. [ERD](#ERD)
11. [클라이언트구조](#클라이언트-구조)
12. [Trouble Shooting](#trouble-shooting)
    
## 👨‍🏫 프로젝트 소개
스컬 모티브한 2D 로그라이크 게임

## 팀소개
팀 이름 유래 : 2승희보유, C# 2주 학습, 유니티 2달 학습, Unity 2D 프로젝트, 팀20조 // Team 222220
리더 : 손대오
부리더 : 이승희
팀원 : 박기찬

## 프로젝트 계기
스컬 게임을 한 경험을 바탕으로 스컬을 모티브로한 2D 로그라이크 게임 개발 팀 결성

## 😎 프로젝트 컨셉

- 무기가 과일로 된 컨셉
  
![25011704](https://github.com/user-attachments/assets/b2364e30-c8db-4328-bfe9-566f2d537864)


- 얻은 무기로 재화를 획득
  
![25011601](https://github.com/user-attachments/assets/ceace5ca-6db3-4e97-ba3a-ceb4299ac1b5)


- 얻은 재화를 강화를 진행

![25011607](https://github.com/user-attachments/assets/74da1fb0-0611-478c-aab7-ef2fe05a6a91)
![25011608](https://github.com/user-attachments/assets/10122835-650e-44f5-aeef-e0aae33528c3)

## ⚙️ Project 내 설정
- ⌨️ Input
    - 키보드 화살표 기반 이동으로 진행
    - 공격 : Z
    -  대쉬 : X
    -  점프 : C
    -  상호작용 : F
    -  사용자 정보 : Tab
    -  설정 : Esc
    -  무기 교체 : Spacebar

## 💜 주요기능

- 기능 1 - FSM
    ![image](https://github.com/user-attachments/assets/631bd0c3-935f-42cf-b942-fed3172a76f4)
  
    - 사용이유
      플레이어나 몬스터의 상태 전환과 동작을 체계적으로 관리하고 효율적으로 제어하기 위해 사용.
    - 구현방법
        1. Unity 기반의 몬스터 상태 머신(State Machine) 구현으로, 몬스터의 상태(Idle, Patrolling, Attack)를 관리하며 상태 전환과 실행 로직을 포함.
        2. `MonsterController`와 연동하여 상태별 행동 업데이트 및 실행, Unity의 `UnityAction`을 활용한 상태 통계 업데이트 기능 제공.
           
- 기능 2 - ObjectPooling
    ![image (1)](https://github.com/user-attachments/assets/bee96c90-ad29-412c-ab2d-901c03897cee)
  
    - 사용이유
    반복되서 사용되는 오브젝트를 생성파괴하지 않고 재사용하기 위해 사용.
    - 구현방법
    1. PoolManager는 Unity에서 제공하는 UnityEngine.Pool을 사용하여 다양한 타입의 풀링객체를 생성, 관리, 초기화하는 기능을 제공.
    2. 풀링 시스템은 GenericPooledObject를 사용해 특정 풀 타입에 대해 오브젝트를 생성, 활성화/비화성화, 제거하며, 필요한 경우 모든 풀을 초기화 하는 기능 제공.

- 기능 3 - 전략패턴(SO)
    ![image (2)](https://github.com/user-attachments/assets/d4953358-2233-40d2-a287-6b05e151fce5)
  
    - 사용이유
    몬스터의 공격 패턴과 애니메이션을 동저긍로 변경하여 다양한 몬스터 동작을 효율적으로 관리하기 위해 사용.
    - 구현방법
    1. MonsterSO에 RegularPatternData, PatternData를 통해 몬스터의 스탯, 공격 패턴, 애니메이션 데이터를 ScriptObejct로 관리.
    2. 런타임에 SO를 주입하거나 교체하여 몬스터의 공작과 애니메이션을 동적으로 설정.

- 기능 4 - InputSystem
    ![image (3)](https://github.com/user-attachments/assets/22fdc868-c9e9-45ba-81bf-5db23895e462)
  
    - 사용이유
    몬스터의 공격 패턴과 애니메이션을 동저긍로 변경하여 다양한 몬스터 동작을 효율적으로 관리하기 위해 사용.
    - 구현방법
    1. MonsterSO에 RegularPatternData, PatternData를 통해 몬스터의 스탯, 공격 패턴, 애니메이션 데이터를 ScriptObejct로 관리.
    2. 런타임에 SO를 주입하거나 교체하여 몬스터의 공작과 애니메이션을 동적으로 설정.

- 기능 5 - Animation Override Controller & AnimationEvent  
    ![image (4)](https://github.com/user-attachments/assets/fa0b6a57-ce1c-4a59-8a61-0bf4afca24db)
  
    - 사용이유
    AnimationOverrideController
    - 동일한 AnimationController를 여러 몬스터에게 적용시켜 메모리 사용량을 줄이고, 애니메이션 관리 효율성을 높이기 위해서 사용
    
    AnimationEvent
    - 애니메이션과 게임 로직을 간단하고 직관적으로 연결하기 위해 사용
    - 구현방법
    1. 기존 Controller의 State에 대응해 각자의 클립을 주입
       
    ![스크린샷 2025-01-19 142113](https://github.com/user-attachments/assets/42f04ba9-fce0-4167-8867-11c2f0d9f859)

    트렌지션은 유지된채 객체 생성.


    2. AnimationClip에 클립에 맞는 메서드를 이벤트 설정
       
    ![스크린샷 2025-01-19 143414](https://github.com/user-attachments/assets/c60e5414-7f59-4b0f-8f5e-6ecdd6b91178)

    설정한 메서드에 맞는 행동 구현 

## ⏲️ 개발기간
- 2024.11.25(월) ~ 2025.01.21(화)

## 📚️ 기술스택
![스크린샷 2025-01-16 192242](https://github.com/user-attachments/assets/48a8d90c-3715-49f9-9def-ada645f6565e)

| GitHub | Version 3.4.13 |
| --- | --- |
| Rider | 2024.3.3 |
| Visual Studio | 2022 |
| Unity  | 2022.3.17.f1 |
| Aseprite  | v1.3.11-x64 |

### ✔️ Language
C#

### ✔️ Version Control
Unity 2022.3.17.f1
Visual Studio 2022

### ✔️ IDE
Visual Studio 2022

### ✔️ Framework
.NET 9.0

### ✔️ Deploy 
https://nosoead.itch.io/legendaryfruit

## 📑 와이어프레임
https://www.figma.com/board/5LJiExFjudEpSSrfuhIrLa/22220%EC%A1%B0-%EC%99%80%EC%9D%B4%EC%96%B4%ED%94%84%EB%A0%88%EC%9E%84?node-id=0-1&node-type=canvas&t=WtP5c2lugbyUG3Cu-0

## 💾 ERD
https://www.figma.com/board/QcHAQbKkVd6qroLPvDi4Ax/22220%EC%A1%B0-%EC%B4%88%EC%95%88-%26-%EC%BB%A8%EC%85%89?node-id=180-752&node-type=section&t=1YphAOslbvR5QsqM-0

## ⚙️ 클라이언트 구조
- Managers
![image (5)](https://github.com/user-attachments/assets/9edbf890-f64d-4285-ab09-d3f3f7c83545)

- Player & Monster
![image (6)](https://github.com/user-attachments/assets/4825fbcf-9a53-4a16-8f9f-2d6fffacdffe)

    <details>
        <surmmary>Player</surmmary>
        ![image (9)](https://github.com/user-attachments/assets/c0dfc51a-8095-4b82-9394-360985b647e0)

        <surmmary>Monster</surmmary>
       ![image (8)](https://github.com/user-attachments/assets/467cd588-4ae2-4ada-afa8-5cc7d2077e93)

    </details>
      
      

## ⚽ Trouble Shooting





