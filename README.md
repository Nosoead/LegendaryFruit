
# 💻 프로젝트 이름
Legendary Fruit : The Ultimate Weapon

## 📖 목차
1. [프로젝트 소개](#-프로젝트-소개)
2. [팀소개](#-팀소개)
4. [프로젝트 계기](#-프로젝트-계기)
5. [프로젝트 컨셉](#-프로젝트-컨셉)
6. [주요기능](#-주요기능)
7. [개발기간](#%EF%B8%8F-개발기간)
8. [기술스택](#%EF%B8%8F-기술스택)
9. [와이어프레임](#-와이어프레임)
10. [ERD](#-erd)
11. [클라이언트구조](#%EF%B8%8F-클라이언트-구조)
12. [Trouble Shooting](#-trouble-shooting)
    
## 👨‍🏫 프로젝트 소개
![165468431 (1)](https://github.com/user-attachments/assets/40c2634b-013e-4fee-be80-6170ff9d7d98)
스컬 모티브한 2D 로그라이크 게임


## 👥 팀소개
팀 이름 유래 : 2승희보유, C# 2주 학습, 유니티 2달 학습, Unity 2D 프로젝트, 팀20조 // Team 222220
리더 : 손대오
부리더 : 이승희
팀원 : 박기찬

## 🤝 프로젝트 계기
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

<details>
      <summary>자세히보기</summary>
- 사용이유<br>
    플레이어나 몬스터의 상태 전환과 동작을 체계적으로 관리하고 효율적으로 제어하기 위해 사용.<br>  
- 구현방법<br>
    1. Unity 기반의 몬스터 상태 머신(State Machine) 구현으로, 몬스터의 상태(Idle, Patrolling, Attack)를 관리하며 상태 전환과 실행 로직을 포함.<br>
    2. `MonsterController`와 연동하여 상태별 행동 업데이트 및 실행, Unity의 `UnityAction`을 활용한 상태 통계 업데이트 기능 제공.
</details>

           
- 기능 2 - ObjectPooling
    ![image (1)](https://github.com/user-attachments/assets/bee96c90-ad29-412c-ab2d-901c03897cee)
  
    <details>
      <summary>자세히보기</summary>
    - 사용이유<br>
    반복되서 사용되는 오브젝트를 생성파괴하지 않고 재사용하기 위해 사용.<br>
    - 구현방법<br>
    1. PoolManager는 Unity에서 제공하는 UnityEngine.Pool을 사용하여 다양한 타입의 풀링객체를 생성, 관리, 초기화하는 기능을 제공.<br>
    2. 풀링 시스템은 GenericPooledObject를 사용해 특정 풀 타입에 대해 오브젝트를 생성, 활성화/비화성화, 제거하며, 필요한 경우 모든 풀을 초기화 하는 기능 제공.
   </details>


- 기능 3 - 전략패턴(SO)
    ![image (2)](https://github.com/user-attachments/assets/d4953358-2233-40d2-a287-6b05e151fce5)
  
    <details>
      <summary>자세히보기</summary>
    - 사용이유<br>
    몬스터의 공격 패턴과 애니메이션을 동저긍로 변경하여 다양한 몬스터 동작을 효율적으로 관리하기 위해 사용.<br>
    - 구현방법<br>
    1. MonsterSO에 RegularPatternData, PatternData를 통해 몬스터의 스탯, 공격 패턴, 애니메이션 데이터를 ScriptObejct로 관리.<br>
    2. 런타임에 SO를 주입하거나 교체하여 몬스터의 공작과 애니메이션을 동적으로 설정.
   </details>

- 기능 4 - InputSystem
    ![image (3)](https://github.com/user-attachments/assets/22fdc868-c9e9-45ba-81bf-5db23895e462)

    <details>
      <summary>자세히보기</summary>
    - 사용이유<br>
    몬스터의 공격 패턴과 애니메이션을 동저긍로 변경하여 다양한 몬스터 동작을 효율적으로 관리하기 위해 사용.<br>
    - 구현방법<br>
    1. MonsterSO에 RegularPatternData, PatternData를 통해 몬스터의 스탯, 공격 패턴, 애니메이션 데이터를 ScriptObejct로 관리.<br>
    2. 런타임에 SO를 주입하거나 교체하여 몬스터의 공작과 애니메이션을 동적으로 설정.
   </details>
  
- 기능 5 - Animation Override Controller & AnimationEvent  
    ![image (4)](https://github.com/user-attachments/assets/fa0b6a57-ce1c-4a59-8a61-0bf4afca24db)

    <details>
      <summary>자세히보기</summary>
    - 사용이유<br>
    AnimationOverrideController<br>
    - 동일한 AnimationController를 여러 몬스터에게 적용시켜 메모리 사용량을 줄이고, 애니메이션 관리 효율성을 높이기 위해서 사용<br>
    
    AnimationEvent<br>
    - 애니메이션과 게임 로직을 간단하고 직관적으로 연결하기 위해 사용<br>
    - 구현방법<br>
    1. 기존 Controller의 State에 대응해 각자의 클립을 주입<br>
       
    ![스크린샷 2025-01-19 142113](https://github.com/user-attachments/assets/42f04ba9-fce0-4167-8867-11c2f0d9f859)

    트렌지션은 유지된채 객체 생성.<br>


    2. AnimationClip에 클립에 맞는 메서드를 이벤트 설정<br>
       
    ![스크린샷 2025-01-19 143414](https://github.com/user-attachments/assets/c60e5414-7f59-4b0f-8f5e-6ecdd6b91178)

    설정한 메서드에 맞는 행동 구현 
   </details>
  

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
    <summary>Player</summary>

![image (9)](https://github.com/user-attachments/assets/48c1276e-e699-4be8-a908-ac1ed6447ce7)

</details>
<details>
    <summary>Monster</summary>

![image (8)](https://github.com/user-attachments/assets/3059a179-0a5e-4ac5-a775-0c3b0270c0bf)

</details>
      

## ⚽ Trouble Shooting
---
<details>
    <summary>ObjectPool 제네릭 명시화 문제</summary>
- 문제제기
    
 제네릭으로 생성된 ObjectPool 객체들이 명시화가 되지않아 전역적으로 같은 함수가 호출 되지 않는 문제로 게임을 다시 시도할 때 ObjectPool이 초기화가 되지 않는 문제 발생.
    
- 해결방법
    
    ```csharp
    private Dictionary<PoolType, Action> resetDictionary = new Dictionary<PoolType, Action>();
        
    //딕셔너리 추가하는 곳
    public GenericPooledObject(PoolType poolType, T prefab, bool collectionCheck, int defaultCapacity, int maxSize)
    {
        if (PoolManager.Instance.poolDictionary.ContainsKey(poolType))
        {
            return;
        }
        this.gameObjectPrefab = prefab;
        objectPoolT = new ObjectPool<T>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionCheck, defaultCapacity, maxSize);
        if (!PoolManager.Instance.poolDictionary.ContainsKey(poolType))
        {
            PoolManager.Instance.poolDictionary.Add(poolType, objectPoolT);
            PoolManager.Instance.resetDictionary.Add(poolType, objectPoolT.Clear);
        }
    }
        
    // 사용하는 곳
     public void ResetAllObjectPool()
     {
         foreach (var pool in resetDictionary.Values)
         {
             pool();
         }
     }
    ```
    
  함수를 딕셔너리로 담아서 `UnityEngine.Pool`에서 `IObjectPool`인터페이스에서 제공하는 `Clear()`Clear()함수를 `SomeIObjectPool.Clear` 라는 함수 형태로 호출.
    
- 결과
    
    새로운 스테이지를 실행할 때, `ResetAllObjectPool`을 호출하는 것으로 ObjectPool 초기화 전역 접근이 되어 명시화가 되지 않던 문제 해결.
    
    각각의 pooling된 객체의 함수 접근에 유연성이 더 좋아짐.
</details>

---
<details>
    <summary>Scene전환시 Manager 초기화 문제</summary>
    - 문제제기
    
Awake와 Start문의 호출 순서가 일정하지 않아 Scene전환시 Manager의 초기화가 정상적으로 이뤄지지 않는 문제 발생
    
- 해결방법
    
    ```csharp
    public class SceneManagerExtension : Singleton<SceneManagerExtension>
    {
        public void LoadScene(SceneType sceneType)
        {
            ResetWork();
            SceneManager.LoadScene(sceneType.ToString());
            SceneManager.sceneLoaded += HandleSceneLoaded;
            
            void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if (scene.name == sceneType.ToString())
                {
                    scenes[(int)sceneType].Init();
                    SceneManager.sceneLoaded -= HandleSceneLoaded;
                }
            }
        }
    }
    ```

  `SceneManagerExtension` 을 만들어 `LoadScene` 에 `HandleSceneLoaded` 를 구독해서 각 씬에서 필요한 초기화를 할 수 있도록 SceneManager를 확장.
    
- 결과
    
    ```csharp
    public class OneCycleScene : BaseScene
    {
        public override void Init()
        {
            ItemManager.Instance.Init();
            StageManager.Instance.Init();
            UIManager.Instance.ForeInit();
            GameManager.Instance.Init();
            UIManager.Instance.PostInit();
            GameManager.Instance.GameStart();
            PlayerInfoManager.Instance.SetCurrency();
        }
    }
    ```
    
   씬전환시 각 싱글톤에서 필요한 초기화 순서를 정하는 것으로 초기화 오류 문제 해결.
    
   한 매니저에서 전후로 초기화 순서를 나눌 수 있어 Manager초기화의 유연성 확장.
</details>

---
<details>
    <summary>Input 입력 관리 문제</summary>
    - 문제제기<br>
     **입력 충돌 문제:**<br>
         플레이어 조작 중 UI 입력이 동시에 활성화되어 원치 않는 동작 발생.<br>
     **비활성화된 입력 이벤트 호출:**<br>
         특정 상태에서 비활성화된 입력이 여전히 호출되는 현상.
    
- 해결방법
    
    ```csharp
     public class GatherInputManager : Singleton<GatherInputManager>
     {
        protected override void Awake()
        {
            base.Awake();
            input = new PlayerInput();
     
        }
        
        public void SetInput()
        {
            input.Player.Enable();
            input.UI.Disable();
        }
    }    
    ```
    
    상태 기반 입력 제어:
    
    `GatherInputManager`의 플래그(`isPlay`, `isTab` 등)를 사용해 현재 활성화된 입력 상태를 명확히 구분.
    
    상황에 따라 `input.Player.Enable()` 또는 `input.UI.Disable()`을 명확히 호출.
    
- 결과
    
    입력 충돌이 해결되어 플레이어 조작과 UI 상호작용이 독립적으로 작동.
    
    비활성화된 입력 이벤트 호출 문제 제거로 안정적인 입력 처리 가능.
</details>

---
<details>
    <summary>Dialogue UI 출력 문제</summary>
    - 문제제기<br>
    게임 내에서 CSV 파일을 통해 NPC와 대화 데이터를 불러오고, 이를 UI에 맞게 출력하는 시스템에서 대화 UI가 제대로 표시되지 않는 경우<br>
    
- 해결방법
    
    ```csharp
    IEnumerator LoadCSV(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string result;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = new UnityWebRequest(filePath);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        }
        else
        {
            result = File.ReadAllText(filePath);
        }
    
        StringReader reader = new StringReader(result);
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
        string[] headers = reader.ReadLine().Split(',');
    
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            if (line == null) break;
            string[] fields = line.Split(',');
            Dictionary<string, string> entry = new Dictionary<string, string>();
            for (int i = 0; i < headers.Length; i++)
            {
                entry[headers[i]] = fields[i];
            }
            data.Add(entry);
        }
    
        switch (fileName)
        {
            case "Npc.csv":
                SetNpcTable(data);
                break;
            case "Dialogue.csv":
                SetDialogueTable(data);
                break;
            case "DialogueList.csv":
                SetDialogueListTable(data);
                break;
        }
    
        loadData++;
    }
    ```
    
    `LoadCSV` 메서드를 하나의 통합된 방식으로 개선하고, 각 CSV 파일 처리 로직을 공통화하여 중복을 제거
    
- 결과
    
    CSV 파일 로딩, 데이터 매핑, UI 리소스 로딩 및 UI 업데이트 로직을 점검하여 대화 시스템이 정상적으로 동작하도록 구현.
</details>

---
<details>
    <summary>ScriptableObject 직렬화 문제</summary>
    - 문제제기
    
![스크린샷 2025-01-19 134852.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/df5d4818-1c49-4beb-8da5-f9148dfd03dd/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7_2025-01-19_134852.png)
    
![스크린샷 2025-01-19 135855.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/54d54b8c-a2d2-41a6-b9ce-208d0d1a44a5/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7_2025-01-19_135855.png)
    
Unity 2022.3.17f1 버전에서 ScriptableObject에 ParticleSystem.MinMaxCurve를 
직렬화하지 못하는 문제 발생
    
- 해결방법
    
    데이터를 `List<EffectData>`로 감싸는 방식으로 직렬화 가능하게 만들었다. 이를 통해 Unity Inspector와의 호환성을 유지하면서 데이터를 처리할 수 있게 되었다.
    
    ![스크린샷 2025-01-19 135936.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/57d83634-8497-48e3-adfe-560292ace2b3/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7_2025-01-19_135936.png)
    
    `List<EffectData>`  캡술화를 통해 직렬화를 가능하게 하였음.
    
- 결과
    
    Unity Inspector와의 호환성을 유지하면서 데이터를 처리할 수 있게 됨
</details>

---
<details>
    <summary>ParticleSystem Module 접근 문제 </summary>
    - 문제제기
    
 ScriptableObejct에서 ParticleModule는 접근이 안되기에 다양한 파티클의 모듈에 접근이 어려운 문제 발생
    
- 해결방법
    
    ParticleHelper 을 통해 파티클의 모듈에 접근을 용이하게 만듬
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/efbd0db5-8b71-4844-9d54-bf1c52ecb635/image.png)
    
- 결과
    
    ParticleSystem의 설정메서드를 Helper클래스에 집중시켜 함수 재사용성이 높아짐.
    Helper클래스에 집중되어있어 유지보수성 높아짐.
    
    모듈화(Modularity) 를 통해 특정모듈에 접근하는 작업이 분리되어있어 독립적으로 수정하거나 확장 가능
</details>
