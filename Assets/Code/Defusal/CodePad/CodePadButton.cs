using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePadButton : MonoBehaviour
{
    public Animator buttonAnim;
        public Transform fingerAnchor;
        public int buttonValue;

        [SerializeField]
        public CodePadButton up;
        public CodePadButton down;
        public CodePadButton left;
        public CodePadButton right;
}
