using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;

namespace GameWorld
{
    /// <summary>
    /// Input control for an actor. 
    /// </summary>
    public class Controller
    {
        public Controller()
        {
            MoveDir = Movement.None;
            RotateDir = Rotation.None;
        }

        public enum Rotation
        {
            None,
            CW,
            ACW
        }

        public enum Movement
        {
            None,
            Forwards,
            Backwards
        }

        public Movement MoveDir { get; set; }
        public Rotation RotateDir { get; set; }

        /// <summary>
        /// Check if the actor is currently rotating, and the direction they are rotating.
        /// </summary>
        public Rotation IsRotating()
        {
            return RotateDir;
        }

        /// <summary>
        /// Check if the actor is currently moving, and the direction they are moving.
        /// </summary>
        public Movement IsMoving()
        {
            return MoveDir;
        }

        /// <summary>
        /// Player input to move the player actors.
        /// </summary>
        public void PlayerInput()
        {
            #region MoveControl
            if (Keyboard.IsKeyDown(Key.W))
            {
                MoveDir = Movement.Forwards;
            }
            else if (Keyboard.IsKeyDown(Key.S))
            {
                MoveDir = Movement.Backwards;
            }
            else
            {
                MoveDir = Movement.None;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                RotateDir = Rotation.CW;
            }
            else if (Keyboard.IsKeyDown(Key.A))
            {
                RotateDir = Rotation.ACW;
            }
            else
            {
                RotateDir = Rotation.None;
            }
            #endregion MoveControl
        }

    }
}