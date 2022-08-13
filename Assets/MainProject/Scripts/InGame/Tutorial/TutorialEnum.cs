using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public enum eTutorialDivision
    {
        TEST_TUTORIAL = -1,
        /// <summary>
        /// 기본 튜토리얼
        /// </summary>
        BASIC_TUTORIAL = 0,

        END
    }
    public enum eTargetUI
    {
        NONE = 0,

        /// <summary>
        /// Eanble Curtain
        /// </summary>
        SHOW_CURTAIN,
        /// <summary>
        /// 1번이미지 연출
        /// </summary>
        INTRO_SCREEN_SWITCHING_01,
        /// <summary>
        /// 2번 이미지 연출
        /// </summary>
        INTRO_SCREEN_SWITCHING_02,
        /// <summary>
        /// 3번 이미지 연출
        /// </summary>
        INTRO_SCREEN_SWITCHING_03,
        /// <summary>
        /// 4번 이미지 연출
        /// </summary>
        INTRO_SCREEN_SWITCHING_04,
        /// <summary>
        /// Disable Curtain
        /// </summary>
        HIDE_CURTAIN,
        /// <summary>
        /// 튜토 시작 선물 지급
        /// </summary>
        GIFT_BASIC_TUTORIAL_START,
        /// <summary>
        /// 캐릭터 정보창 클릭
        /// </summary>
        SEL_CM,
        /// <summary>
        /// 2번 슬롯 클릭 [안젤라]
        /// </summary>
        SEL_CM_CHAR,
        /// <summary>
        /// 스킬 탭 클릭
        /// </summary>
        SEL_CM_TAB_SKILL,
        /// <summary>
        /// 승급 팝업 열기 클릭
        /// </summary>
        SEL_CM_CHAR_UPGRADE,
        /// <summary>
        /// 승급 실행
        /// </summary>
        SEL_CM_CHAR_UPGRADE_UP,
        /// <summary>
        /// 승급 팝업 닫기 클릭
        /// </summary>
        SEL_CM_CHAR_UPGRADE_CLOSE,
        /// <summary>
        /// 스킬 배우기
        /// </summary>
        SEL_CM_SKILL_LEVEL_UP,
        /// <summary>
        /// 무기 탭 클릭
        /// </summary>
        SEL_CM_TAB_WEAPON,
        /// <summary>
        /// 무기 클릭
        /// </summary>
        SEL_CM_WEAPON_ITEM,
        /// <summary>
        /// 무기 강화 팝업 열기
        /// </summary>
        SEL_CM_WEAPON_UPGRADE,
        /// <summary>
        /// 무기 강화 실행
        /// </summary>
        SEL_CM_WEAPON_UPGRADE_UP,
        /// <summary>
        /// 무기 강화 팝업 닫기
        /// </summary>
        SEL_CM_WEAPON_UPGRADE_CLOSE,
        /// <summary>
        /// 코스튬 탭 클릭
        /// </summary>
        SEL_CM_TAB_COSTUME,
        /// <summary>
        /// 뒤로가기 클릭
        /// </summary>
        SEL_CM_CLOSE,

        /// <summary>
        /// 상점 버튼 클릭
        /// </summary>
        SEL_SHOP,
        /// <summary>
        /// 2배속 클릭
        /// </summary>
        SEL_SHOP_ADVENTURE_2X_UP,
        /// <summary>
        /// 요정 탭 클릭
        /// </summary>
        SEL_SHOP_TAB_FAIRY,
        /// <summary>
        /// 최대 레벨 증가 클릭
        /// </summary>
        SEL_SHOP_MAX_LEVEL_UP,
        /// <summary>
        /// 구매 버튼 클릭
        /// </summary>
        SEL_SHOP_MAX_LEVEL_BUY,
        /// <summary>
        /// 뒤로가기 클릭
        /// </summary>
        SEL_SHOP_CLOSE,
        /// <summary>
        /// 튜토 종료 선물 지급
        /// </summary>
        GIFT_BASIC_TUTORIAL_END,
        /// <summary>
        /// 상점 박스 탭 클릭
        /// </summary>
        SEL_SHOP_TAB_BOX,
        /// <summary>
        /// 안젤라 희귀 박스 상품 클릭
        /// </summary>
        SEL_SHOP_ANGELA_RARE_BOX,
        /// <summary>
        /// 박스 상품 구매
        /// </summary>
        SEL_SHOP_BOX_BUY,
        /// <summary>
        /// 상품 구매 확인
        /// </summary>
        SEL_SHOP_BOX_OKAY,
        /// <summary>
        /// 안젤라 희귀 무기 선택
        /// </summary>
        SEL_CM_ANGELA_RARE_WEAPON,
        /// <summary>
        /// 안젤라 희귀 무기 장착
        /// </summary>
        SEL_CM_ANGELA_RARE_WEAPON_EQUIP,
        /// <summary>
        /// 환생 버튼
        /// </summary>
        SEL_REB,
        /// <summary>
        /// 기본 환생 선택
        /// </summary>
        SEL_REB_BASIC,
        /// <summary>
        /// 선택 팝업에서 YES 선택
        /// </summary>
        SEL_POPUP_YES,

    }

    public enum eArrowDirection
    {
        NONE = 0,
        RIGHT,
        LEFT,
        TOP,
        BOTTOM,
    }

    [System.Serializable]
    public class Tutorial
    {
        public eTutorialDivision division;
        public int step;

    }
}
