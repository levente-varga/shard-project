/*
*
*   This is about a simple an input system as you can have, and it's horrible.
*       Only used for illustrative purposes.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    class InputBasic : InputSystem
    {
        public override bool GetInput()
        {
            InputEvent ie;
            ConsoleKeyInfo cki;
            if (!Console.KeyAvailable)
            {
                return true;
            }

            cki = Console.ReadKey(true);

            ie = new InputEvent();
            ie.Key = (int)cki.KeyChar;
            ie.Type = InputEventType.KeyDown;

            InformListeners(ie);

            ie = new InputEvent();
            ie.Key = (int)cki.KeyChar;
            ie.Type = InputEventType.KeyUp;

            InformListeners(ie);

            Debug.GetInstance().log("Key is " + ie.Key);

            return true;
        }
    }
}
