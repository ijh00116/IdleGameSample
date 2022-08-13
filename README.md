# KnightRush
- Valkirie 키우기 포트폴리오 입니다(해당 프로젝트는 리소스 제외 스크립트만 존재합니다)
- 방치형게임의 핵심 로직내용과 플레이팹(Playfab)연동 코드가 첨부되어 있습니다.


## Index

1. [JsonParsing](#jsonparsing)
2. [재화관리](#재화관리)
3. [미션관리](#미션관리)
4. [튜토리얼 시스템](#튜토리얼시스템)
5. [인벤토리 관리](#인벤토리관리)

## JsonParsing

### 소개 
- 각종 데이터 관리에 필요한 테이블을 json으로 관리하기에 json으로 게임에 필요한 데이터를 게임 시작시 세팅하여 사용합니다.
<details>
<summary>
    <span style="color:#008000">옵저버패턴 코드 내용 보기</span>
</summary>
    <div markdown="1">
     
json에 사용되는 엑셀 예시(미션)

| idx | name | mission_type | mission_value | reward_type | reward_count |
| --- | ---- | ------------ | ------------- | ----------- | ------------ |
|50001|	m_daily_name_001 |	MISSION_CLEAR|	6|	DIAMOND|	100|
|50002|	m_daily_name_002	|MONSTER_KILL|	30|	DIAMOND	|20|
|50003|	m_daily_name_003	|GACHA_COUNT	|5	|DIAMOND	|20|
|50004|	m_daily_name_004	|MONSTER_KILL	|2|	DIAMOND	|20|

```code
 T ReadData<T>(string fileName,BundleType bundleType=BundleType.None)
        {
            bundleType = BundleType.None;

            var path = new System.Text.StringBuilder();
            path.Append(bundleType != BundleType.None ? "" : "Tables/");
            path.Append(fileName);
            if (bundleType != BundleType.None)
            {
                 path.Append(".json");
            }

            TextAsset jsonString = Resources.Load<TextAsset>(path.ToString());

            if (jsonString != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString.text);
            }
            return default;
        }
```
</div>
</details>

## 재화관리

## 미션관리

## 튜토리얼시스템

## 인벤토리관리
