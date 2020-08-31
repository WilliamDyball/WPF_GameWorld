using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameWorld
{
    /// <summary>
    /// Main game world that contains a single actor.
    /// </summary>
    public class World
    {
        public List<Actor> actors = new List<Actor>();
        private int SelectedActor = 0;
        public bool SwitchingControl { get; private set; }
        public bool SwitchingSelected { get; private set; }

        /// <summary>
        /// Construct the world and actors with actors[0] being player controlled.
        /// </summary>
        public World()
        {
            for (int i = 0; i < 10; i++)
            {
                actors.Add(new Actor(i * 30));
            }
            actors[SelectedActor].PlayerControlled = true;
            actors[SelectedActor].Highlighted();
        }

        /// <summary>
        /// Update the world with deltaT.
        /// </summary>
        public void Update(float deltaT)
        {
            ChangeSelectedActor();
            ControlCheck();
            foreach (Actor actor in actors)
            {
                actor.Update(deltaT);
            }
        }

        /// <summary>
        /// Draw the world.
        /// </summary>
        public void Draw(UIElementCollection Drawables)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(Drawables);
            }
        }

        /// <summary>
        /// Changes selected actor between player controlled and auto movement.
        /// </summary>
        private void ControlCheck()
        {
            if (Keyboard.IsKeyDown(Key.Space) & !SwitchingControl)
            {
                SwitchingControl = true;
                actors[SelectedActor].PlayerControlled = !actors[SelectedActor].PlayerControlled;
            }
            if (Keyboard.IsKeyUp(Key.Space) & SwitchingControl)
            {
                SwitchingControl = false;
            }
        }

        /// <summary>
        /// Changes the currently selected actor.
        /// </summary>
        private void ChangeSelectedActor()
        {
            if (Keyboard.IsKeyDown(Key.Up) & !SwitchingSelected)
            {
                SwitchingSelected = true;
                actors[SelectedActor].UnHighlighted();
                if (SelectedActor == (actors.Count) - 1)
                {
                    SelectedActor = 0;
                }
                else
                {
                    SelectedActor++;
                }
                actors[SelectedActor].Highlighted();
            }
            if (Keyboard.IsKeyUp(Key.Up) & SwitchingSelected)
            {
                SwitchingSelected = false;
            }
            if (Keyboard.IsKeyDown(Key.Down) & !SwitchingSelected)
            {
                SwitchingSelected = true;
                actors[SelectedActor].UnHighlighted();
                if (SelectedActor == 0)
                {
                    SelectedActor = (actors.Count) - 1;
                }
                else
                {
                    SelectedActor--;
                }
                actors[SelectedActor].Highlighted();
            }
            if (Keyboard.IsKeyUp(Key.Down) & SwitchingSelected)
            {
                SwitchingSelected = false;
            }
        }
    }
}