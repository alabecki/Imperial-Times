using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp {
    public class NationDropdownStarter : MonoBehaviour {

        [Tooltip("Please select a nation with which to play")]

        public Dropdown NationDropdown;

        void Start()
        {
            NationDropdown.ClearOptions();

        }
    }
}