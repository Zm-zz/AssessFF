using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(0)]
public class InputManager : MonoBehaviour
{
    static List<KeyCode> downKeys;
    static List<KeyCode> holdKeys;
    static List<KeyCode> upKeys;

    static List<KeyCode> downKeys_charNum;
    static List<KeyCode> holdKeys_charNum;
    static List<KeyCode> upKeys_charNum;

    private void Awake()
    {
        downKeys = new List<KeyCode>();
        holdKeys = new List<KeyCode>();
        upKeys = new List<KeyCode>();
        downKeys_charNum = new List<KeyCode>();
        holdKeys_charNum = new List<KeyCode>();
        upKeys_charNum = new List<KeyCode>();
    }

    void Update()
    {
        CollectAllKey();
        Collect_CharNum();
    }

    void CollectAllKey()
    {
        downKeys.Clear();
        holdKeys.Clear();
        upKeys.Clear();

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                holdKeys.Add(keyCode);
            }

            if (Input.GetKeyDown(keyCode))
            {
                downKeys.Add(keyCode);
            }

            if (Input.GetKeyUp(keyCode))
            {
                upKeys.Add(keyCode);
            }
        }
    }

    void Collect_CharNum()
    {
        downKeys_charNum = new List<KeyCode>();
        holdKeys_charNum = new List<KeyCode>();
        upKeys_charNum = new List<KeyCode>();

        for (KeyCode keyCode = KeyCode.A; keyCode <= KeyCode.Z; keyCode++)
        {
            CheckKey(keyCode);
        }

        // 数字键 (0-9)
        for (KeyCode keyCode = KeyCode.Alpha0; keyCode <= KeyCode.Alpha9; keyCode++)
        {
            CheckKey(keyCode);
        }

        // 小键盘上的数字键 (Keypad 0-9)
        for (KeyCode keyCode = KeyCode.Keypad0; keyCode <= KeyCode.Keypad9; keyCode++)
        {
            CheckKey(keyCode);
        }

        void CheckKey(KeyCode keyCode)
        {
            if (Input.GetKey(keyCode))
            {
                holdKeys_charNum.Add(keyCode);
            }

            if (Input.GetKeyDown(keyCode))
            {
                downKeys_charNum.Add(keyCode);
            }

            if (Input.GetKeyUp(keyCode))
            {
                upKeys_charNum.Add(keyCode);
            }
        }
    }

    public static bool GetKeyDown(KeyCode keyCode)
    {
        return downKeys.Contains(keyCode);
    }
    public static bool GetKeyUp(KeyCode keyCode)
    {
        return upKeys.Contains(keyCode);
    }
    public static bool GetKey(KeyCode keyCode)
    {
        return holdKeys.Contains(keyCode);
    }
    public static bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }
    public static bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }
    public static bool GetMouseButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public static List<KeyCode> GetCurKeyDown_CharNum()
    {
        return downKeys_charNum;
    }
    public static List<KeyCode> GetCurKeyUp_CharNum()
    {
        return upKeys_charNum;
    }
    public static List<KeyCode> GetCurKeyHold_CharNum()
    {
        return holdKeys_charNum;
    }

    public static bool GetNumberKeyDown(out int numIndex)
    {
        if (GetKeyDown(KeyCode.Alpha0) || GetKeyDown(KeyCode.Keypad0))
        {
            numIndex = 0;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha1) || GetKeyDown(KeyCode.Keypad1))
        {
            numIndex = 1;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha2) || GetKeyDown(KeyCode.Keypad2))
        {
            numIndex = 2;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha3) || GetKeyDown(KeyCode.Keypad3))
        {
            numIndex = 3;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha4) || GetKeyDown(KeyCode.Keypad4))
        {
            numIndex = 4;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha5) || GetKeyDown(KeyCode.Keypad5))
        {
            numIndex = 5;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha6) || GetKeyDown(KeyCode.Keypad6))
        {
            numIndex = 6;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha7) || GetKeyDown(KeyCode.Keypad7))
        {
            numIndex = 7;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha8) || GetKeyDown(KeyCode.Keypad8))
        {
            numIndex = 8;
            return true;
        }
        if (GetKeyDown(KeyCode.Alpha9) || GetKeyDown(KeyCode.Keypad9))
        {
            numIndex = 9;
            return true;
        }
        numIndex = -1;
        return false;
    }
    public static bool GetNumberKey(out int numIndex)
    {
        if (GetKey(KeyCode.Alpha0) || GetKey(KeyCode.Keypad0))
        {
            numIndex = 0;
            return true;
        }
        if (GetKey(KeyCode.Alpha1) || GetKey(KeyCode.Keypad1))
        {
            numIndex = 1;
            return true;
        }
        if (GetKey(KeyCode.Alpha2) || GetKey(KeyCode.Keypad2))
        {
            numIndex = 2;
            return true;
        }
        if (GetKey(KeyCode.Alpha3) || GetKey(KeyCode.Keypad3))
        {
            numIndex = 3;
            return true;
        }
        if (GetKey(KeyCode.Alpha4) || GetKey(KeyCode.Keypad4))
        {
            numIndex = 4;
            return true;
        }
        if (GetKey(KeyCode.Alpha5) || GetKey(KeyCode.Keypad5))
        {
            numIndex = 5;
            return true;
        }
        if (GetKey(KeyCode.Alpha6) || GetKey(KeyCode.Keypad6))
        {
            numIndex = 6;
            return true;
        }
        if (GetKey(KeyCode.Alpha7) || GetKey(KeyCode.Keypad7))
        {
            numIndex = 7;
            return true;
        }
        if (GetKey(KeyCode.Alpha8) || GetKey(KeyCode.Keypad8))
        {
            numIndex = 8;
            return true;
        }
        if (GetKey(KeyCode.Alpha9) || GetKey(KeyCode.Keypad9))
        {
            numIndex = 9;
            return true;
        }
        numIndex = -1;
        return false;
    }
    public static bool GetNumberKeyUp(out int numIndex)
    {
        if (GetKeyUp(KeyCode.Alpha0) || GetKeyUp(KeyCode.Keypad0))
        {
            numIndex = 0;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha1) || GetKeyUp(KeyCode.Keypad1))
        {
            numIndex = 1;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha2) || GetKeyUp(KeyCode.Keypad2))
        {
            numIndex = 2;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha3) || GetKeyUp(KeyCode.Keypad3))
        {
            numIndex = 3;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha4) || GetKeyUp(KeyCode.Keypad4))
        {
            numIndex = 4;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha5) || GetKeyUp(KeyCode.Keypad5))
        {
            numIndex = 5;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha6) || GetKeyUp(KeyCode.Keypad6))
        {
            numIndex = 6;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha7) || GetKeyUp(KeyCode.Keypad7))
        {
            numIndex = 7;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha8) || GetKeyUp(KeyCode.Keypad8))
        {
            numIndex = 8;
            return true;
        }
        if (GetKeyUp(KeyCode.Alpha9) || GetKeyUp(KeyCode.Keypad9))
        {
            numIndex = 9;
            return true;
        }
        numIndex = -1;
        return false;
    }
}
