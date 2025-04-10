using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatingInstructions : MonoBehaviour
{
    public static bool isCheating;
    static Queue<KeyCode> inputKeys;

    private void Awake()
    {
        inputKeys = new Queue<KeyCode>();
    }

    private void Update()
    {
        foreach (var key in InputManager.GetCurKeyDown_CharNum())
        {
            inputKeys.Enqueue(key);
        }

        while (inputKeys.Count > 100)
        {
            inputKeys.Dequeue();
        }
        if (CompareInstruction("UGIONCHEAT"))
        {
            if (!isCheating)
            {
                MonoFunction.Instance.boxFade.PopLog("开启作弊", 3);
            }
            else
            {
                MonoFunction.Instance.boxFade.PopLog("关闭作弊", 3);
            }
            isCheating = !isCheating;
        }
    }

    public static bool CompareInstruction(KeyCode[] instruction)
    {
        if (instruction.Length > inputKeys.Count) return false;
        int index = 0;
        foreach (var key in inputKeys)
        {
            if (key == instruction[index])
            {
                index++;
            }
            else
            {
                index = 0;
            }
            if (index == instruction.Length)
            {
                inputKeys.Clear();
                return true;
            }
        }
        return false;
    }

    public static bool CompareInstruction(string instruction)
    {
        KeyCode[] keys = new KeyCode[instruction.Length];
        for (int i = 0; i < instruction.Length; i++)
        {
            if (instruction[i] == 'A' || instruction[i] == 'a') keys[i] = KeyCode.A;
            if (instruction[i] == 'B' || instruction[i] == 'b') keys[i] = KeyCode.B;
            if (instruction[i] == 'C' || instruction[i] == 'c') keys[i] = KeyCode.C;
            if (instruction[i] == 'D' || instruction[i] == 'd') keys[i] = KeyCode.D;
            if (instruction[i] == 'E' || instruction[i] == 'e') keys[i] = KeyCode.E;
            if (instruction[i] == 'F' || instruction[i] == 'f') keys[i] = KeyCode.F;
            if (instruction[i] == 'G' || instruction[i] == 'g') keys[i] = KeyCode.G;
            if (instruction[i] == 'H' || instruction[i] == 'h') keys[i] = KeyCode.H;
            if (instruction[i] == 'I' || instruction[i] == 'i') keys[i] = KeyCode.I;
            if (instruction[i] == 'J' || instruction[i] == 'j') keys[i] = KeyCode.J;
            if (instruction[i] == 'K' || instruction[i] == 'k') keys[i] = KeyCode.K;
            if (instruction[i] == 'L' || instruction[i] == 'l') keys[i] = KeyCode.L;
            if (instruction[i] == 'M' || instruction[i] == 'm') keys[i] = KeyCode.M;
            if (instruction[i] == 'N' || instruction[i] == 'n') keys[i] = KeyCode.N;
            if (instruction[i] == 'O' || instruction[i] == 'o') keys[i] = KeyCode.O;
            if (instruction[i] == 'P' || instruction[i] == 'p') keys[i] = KeyCode.P;
            if (instruction[i] == 'Q' || instruction[i] == 'q') keys[i] = KeyCode.Q;
            if (instruction[i] == 'R' || instruction[i] == 'r') keys[i] = KeyCode.R;
            if (instruction[i] == 'S' || instruction[i] == 's') keys[i] = KeyCode.S;
            if (instruction[i] == 'T' || instruction[i] == 't') keys[i] = KeyCode.T;
            if (instruction[i] == 'U' || instruction[i] == 'u') keys[i] = KeyCode.U;
            if (instruction[i] == 'V' || instruction[i] == 'v') keys[i] = KeyCode.V;
            if (instruction[i] == 'W' || instruction[i] == 'w') keys[i] = KeyCode.W;
            if (instruction[i] == 'X' || instruction[i] == 'x') keys[i] = KeyCode.X;
            if (instruction[i] == 'Y' || instruction[i] == 'y') keys[i] = KeyCode.Y;
            if (instruction[i] == 'Z' || instruction[i] == 'z') keys[i] = KeyCode.Z;
            if (instruction[i] == '0') keys[i] = KeyCode.Alpha0;
            if (instruction[i] == '1') keys[i] = KeyCode.Alpha1;
            if (instruction[i] == '2') keys[i] = KeyCode.Alpha2;
            if (instruction[i] == '3') keys[i] = KeyCode.Alpha3;
            if (instruction[i] == '4') keys[i] = KeyCode.Alpha4;
            if (instruction[i] == '5') keys[i] = KeyCode.Alpha5;
            if (instruction[i] == '6') keys[i] = KeyCode.Alpha6;
            if (instruction[i] == '7') keys[i] = KeyCode.Alpha7;
            if (instruction[i] == '8') keys[i] = KeyCode.Alpha8;
            if (instruction[i] == '9') keys[i] = KeyCode.Alpha9;
        }
        return CompareInstruction(keys);
    }
}
