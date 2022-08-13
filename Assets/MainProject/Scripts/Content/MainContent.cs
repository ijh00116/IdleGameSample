using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.Contents
{
    public class MainContent : IContent
    {
        protected override void OnEnter()
        {
            //InGameManager.Instance.IsMainGameStart = true;

            UI.IDialog.RequestDialogEnter<SideButtonDialog>();
            UI.IDialog.RequestDialogEnter<MainDialog>();
            UI.IDialog.RequestDialogEnter<PlayerdataDialog>();
            UI.IDialog.RequestDialogEnter<EventDialog>();
            UI.IDialog.RequestDialogEnter<SideDialog>();
          

            //InGameManager.Instance.StartGame();
        }

        protected override void OnExit()
        {
            UI.IDialog.RequestDialogExit<SideButtonDialog>();
            UI.IDialog.RequestDialogExit<MainDialog>();
            UI.IDialog.RequestDialogExit<PlayerdataDialog>();
            UI.IDialog.RequestDialogExit<EventDialog>();
            UI.IDialog.RequestDialogExit<SideDialog>();
      
        }
    }

}
