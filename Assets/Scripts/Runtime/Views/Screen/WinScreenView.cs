using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Function;
using Rich.Base.Runtime.Abstract.View;
using UnityEngine.Events;

namespace Runtime.Views.Screen
{
    public class WinScreenView : RichView, IPanel
    {
        public IPanelVo vo { get; set; }

        //We want this to be initialized by RichScreenManager
        public override bool autoRegisterWithContext
        {
            get => false;
        }

        public UnityAction onNextLevel = delegate { };

        public void OnClick()
        {
            onNextLevel?.Invoke();
        }
    }
}