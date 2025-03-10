
using UnityEngine;

using TMPro;

namespace GS.UI
{
    public class UIObjectPanel : MonoBehaviour
    {
        public TextMeshProUGUI objectName;

        public UIAObjectSubpanel activeSubpanel;

        public void HideActiveSubpanel()
        {
            //—крываем активную подпанель
            activeSubpanel.gameObject.SetActive(false);

            //”казываем, что активной подпанели нет
            activeSubpanel = null;
        }
    }
}
