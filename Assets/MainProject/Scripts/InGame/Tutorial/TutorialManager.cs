using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BlackTree
{
    public class TutorialManager : MonoSingleton<TutorialManager>
    {
        private Queue<TutorialTouch> _currentTutorials = null;
        private UnityAction _callbackTutorialFinish;
        private Coroutine _coTutorial;
        public bool IsPlayingTutorial => _currentTutorials != null && _currentTutorials.Count > 0;

        public void ClearTutorial()
        {
            if (_currentTutorials != null)
                _currentTutorials.Clear();

            _callbackTutorialFinish?.Invoke();
            _callbackTutorialFinish = null;
        }

        public void SkipTutorial()
        {
            if (_currentTutorials != null)
            {
                if (_coTutorial != null)
                {
                    StopCoroutine(_coTutorial);
                    _coTutorial = null;
                }
                while (_currentTutorials.Count > 1)
                    _currentTutorials.Dequeue();
                NextTutorialStep();
            }
        }


        public int GetTutorialIdx(eTutorialDivision division)
        {
            for (var i = 0; i <InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList.Count; i++)
            {
                if (InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].division == division)
                    return i;
            }
            return -1;
        }

        public void StartTutorial(eTutorialDivision division,UnityAction callbackFinish)
        {
            if (IsPlayingTutorial)
            {
#if UNITY_EDITOR
                Debug.LogError($"<color=green>튜토리얼 진행 중이라 {division}가 취소 됨!!</color>");
#endif
                return;
            }
            var idx = GetTutorialIdx(division);
            if (idx == -1)
            {
                idx = InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList.Count;
                InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList.Add(new Tutorial { division = division, step = 1 });
            }
            //튜토리얼 진행 정보 가지고 튜토리얼터치 정보 가져옴
            _currentTutorials = GetTutorialTouch(division, InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[idx].step);

            _callbackTutorialFinish = () => {
                callbackFinish?.Invoke();
            };

            if (_currentTutorials.Count > 1)
            {
                //튜토리얼 띄워줘야함 다른 UI예외처리
            }
            _coTutorial = StartCoroutine(IeStartTutorialStep());
        }

        private IEnumerator IeStartTutorialStep()
        {
            var nowTuto = _currentTutorials.Dequeue();
            
            switch (nowTuto.target_ui)
            {
                case eTargetUI.NONE:
                    break;
                case eTargetUI.SHOW_CURTAIN:
                    break;
                case eTargetUI.INTRO_SCREEN_SWITCHING_01:
                    break;
                case eTargetUI.INTRO_SCREEN_SWITCHING_02:
                    break;
                case eTargetUI.INTRO_SCREEN_SWITCHING_03:
                    break;
                case eTargetUI.INTRO_SCREEN_SWITCHING_04:
                    break;
                case eTargetUI.HIDE_CURTAIN:
                    break;
                case eTargetUI.GIFT_BASIC_TUTORIAL_START:
                    break;
                case eTargetUI.SEL_CM:
                    break;
                case eTargetUI.SEL_CM_CHAR:
                    break;
                case eTargetUI.SEL_CM_TAB_SKILL:
                    break;
                case eTargetUI.SEL_CM_CHAR_UPGRADE:
                    break;
                case eTargetUI.SEL_CM_CHAR_UPGRADE_UP:
                    break;
                case eTargetUI.SEL_CM_CHAR_UPGRADE_CLOSE:
                    break;
                case eTargetUI.SEL_CM_SKILL_LEVEL_UP:
                    break;
                case eTargetUI.SEL_CM_TAB_WEAPON:
                    break;
                case eTargetUI.SEL_CM_WEAPON_ITEM:
                    break;
                case eTargetUI.SEL_CM_WEAPON_UPGRADE:
                    break;
                case eTargetUI.SEL_CM_WEAPON_UPGRADE_UP:
                    break;
                case eTargetUI.SEL_CM_WEAPON_UPGRADE_CLOSE:
                    break;
                case eTargetUI.SEL_CM_TAB_COSTUME:
                    break;
                case eTargetUI.SEL_CM_CLOSE:
                    break;
                case eTargetUI.SEL_SHOP:
                    break;
                case eTargetUI.SEL_SHOP_ADVENTURE_2X_UP:
                    break;
                case eTargetUI.SEL_SHOP_TAB_FAIRY:
                    break;
                case eTargetUI.SEL_SHOP_MAX_LEVEL_UP:
                    break;
                case eTargetUI.SEL_SHOP_MAX_LEVEL_BUY:
                    break;
                case eTargetUI.SEL_SHOP_CLOSE:
                    break;
                case eTargetUI.GIFT_BASIC_TUTORIAL_END:
                    break;
                case eTargetUI.SEL_SHOP_TAB_BOX:
                    break;
                case eTargetUI.SEL_SHOP_ANGELA_RARE_BOX:
                    break;
                case eTargetUI.SEL_SHOP_BOX_BUY:
                    break;
                case eTargetUI.SEL_SHOP_BOX_OKAY:
                    break;
                case eTargetUI.SEL_CM_ANGELA_RARE_WEAPON:
                    break;
                case eTargetUI.SEL_CM_ANGELA_RARE_WEAPON_EQUIP:
                    break;
                case eTargetUI.SEL_REB:
                    break;
                case eTargetUI.SEL_REB_BASIC:
                    break;
                case eTargetUI.SEL_POPUP_YES:
                    break;
                default:
                    break;
            }

            bool tutorialTouched = false;
            //방향 튜토리얼 ui 버튼 선택유도인데 아직 기능 추가 할 예정 ㅇ없어서 걍 다음으로 넘기거나 끝내버림
            if(nowTuto.name_id.Equals("0"))
            {
                if (_currentTutorials.Count == 0)
                {
                    for (int i = 0; i < InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList.Count; i++)
                    {
                        if (InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].division == nowTuto.tutorial_division)
                        {
                            if (InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].step == nowTuto.save_step)
                                break;

                            InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].step = nowTuto.save_step;
#if UNITY_EDITOR
                            Debug.Log($"<color=green>튜토리얼 진행중</color> \n division : {nowTuto.tutorial_division} save_step : {nowTuto.save_step}");
#endif
                            break;
                        }
                    }
                    ClearTutorial();
                }
                else
                {
                    NextTutorialStep();
                }
            }
            else
            {
                //튜토 버튼에 튜토리얼 터치 정보 보내주기
                Message.Send<UI.Event.TutorialUIpopup>(new UI.Event.TutorialUIpopup(nowTuto, () => tutorialTouched = true));
            }
            //튜토리얼 읽음 처리
            yield return new WaitUntil(() => tutorialTouched==true);

            if (nowTuto.save_step != 0)
            {
                for (int i = 0; i < InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList.Count; i++)
                {
                    if (InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].division == nowTuto.tutorial_division)
                    {
                        if (InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].step == nowTuto.save_step)
                            break;

                        InGameManager.Instance.GetPlayerData.tutorialInfo.TutorialList[i].step = nowTuto.save_step;
#if UNITY_EDITOR
                        Debug.Log($"<color=green>튜토리얼 진행중</color> \n division : {nowTuto.tutorial_division} save_step : {nowTuto.save_step}");
#endif
                        break;
                    }
                }
            }

            //튜토리얼 끝났을때
            if (_currentTutorials.Count==0)
            {
                ClearTutorial();
            }else
            {
                NextTutorialStep();
            }
        }

        private void NextTutorialStep()
        {
            _coTutorial = StartCoroutine(IeStartTutorialStep());
        }

        //튜토리얼 테이블에서 스텝 순서대로 (스텝 이후로 튜토리얼만)
        Queue<TutorialTouch> GetTutorialTouch(eTutorialDivision division,int step)
        {
            List<TutorialTouch> tutotouchlist = InGameDataTableManager.TutorialTableList.tutorial_touch;
            return new Queue<TutorialTouch>(tutotouchlist.FindAll(x => x.tutorial_division.Equals(division) && x.step >= step).OrderBy((a) => a.step).ToList());
        }
    }

}
