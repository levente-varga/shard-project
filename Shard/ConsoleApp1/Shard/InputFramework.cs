/*
*
*   SDL provides an input layer, and we're using that.  This class tracks input, anchors it to the 
*       timing of the game loop, and converts the SDL events into one that is more abstract so games 
*       can be written more interchangeably.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;

namespace Shard
{

    // We'll be using SDL2 here to provide our underlying input system.
    class InputFramework : InputSystem
    {

        double tick, timeInterval;
        public override bool GetInput()
        {

            SDL.SDL_Event ev;
            int res;
            InputEvent ie;

            tick += Bootstrap.GetDeltaTime();

            if (tick < timeInterval)
            {
                return true;
            }

            while (tick >= timeInterval)
            {

                Debug.Log($"Handling input...");

                res = SDL.SDL_PollEvent(out ev);


                if (res != 1)
                {
                    return true;
                }

                ie = new InputEvent();

                ie.TimeStamp = tick;

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                {
                    SDL.SDL_MouseMotionEvent mot;

                    mot = ev.motion;

                    ie.X = mot.x;
                    ie.Y = mot.y;
                    ie.Type = InputEventType.MouseMotion;

                    InformListeners(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;
                    ie.Type = InputEventType.MouseDown;

                    InformListeners(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;
                    ie.Type = InputEventType.MouseUp;

                    InformListeners(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                {
                    SDL.SDL_MouseWheelEvent wh;

                    wh = ev.wheel;

                    ie.X = (int)wh.direction * wh.x;
                    ie.Y = (int)wh.direction * wh.y;
                    ie.Type = InputEventType.MouseWheel;

                    InformListeners(ie);
                }


                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    ie.Type = InputEventType.KeyDown;

                    InformListeners(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    ie.Type = InputEventType.KeyUp;

                    InformListeners(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_WINDOWEVENT && ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                {
                    return false;
                }

                tick -= timeInterval;
            }

            return true;
        }

        public override void Initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 60.0;
        }

    }
}