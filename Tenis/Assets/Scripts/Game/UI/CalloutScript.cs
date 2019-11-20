using FrameLord;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CalloutScript : MonoBehaviorSingleton<CalloutScript>
    {
        private GameObject _callout;

        private void Start()
        {
            _callout = gameObject;
        }

        public void TriggerCallout(string text)
        {
            _callout.GetComponentInChildren<TextMeshProUGUI>().SetText(text);
            _callout.GetComponent<Animator>().Play("Open");
        }
    }
}
