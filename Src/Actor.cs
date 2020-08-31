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
    /// Represents a moving character within the game world. It is rendered as a simple coloured polygon.
    /// </summary>
    public class Actor
    {
        public Point TarPos { get; private set; }
        public Point Pos { get; private set; } // Current position of the actor.
        public float Rot { get; private set; } // Current rotation of the actor. 2PI per circle.
        public SolidColorBrush Colour { get; private set; } // Colour to render the actor.
        public SolidColorBrush StoredColour { get; private set; } // Colour to render the actor.

        readonly Controller m_controller; // Controller used to update the position and rotation of the actor.

        private bool playerControlled;
        public bool PlayerControlled
        {
            get => playerControlled;
            set
            {
                if (value) { Colour = Brushes.CornflowerBlue; StoredColour = Brushes.CornflowerBlue; }
                else { Colour = Brushes.LightSeaGreen; StoredColour = Brushes.LightSeaGreen; }
                playerControlled = value;
            }
        }

        //Storing frequently used rotation amounts.
        private readonly double FullSpin = Math.PI * 2;
        private readonly double HalfSpin = Math.PI;
        private readonly double QuaterSpin = Math.PI / 2;

        private float MoveSpeed = 100f;
        private float RotationSpeed = 1.0f;

        readonly Random random;

        /// <summary>
        /// Construct the actor, initialising its position, rotation and colour.
        /// </summary>
        public Actor(int x = 100, int y = 100)
        {
            Pos = new Point(x, y);
            random = new Random(x + y); //Have the seed be based on spawn position.
            TarPos = new Point(random.Next(0, 800), random.Next(0, 400));
            Rot = 0.0f;
            Colour = Brushes.LightSeaGreen;
            m_controller = new Controller();
            PlayerControlled = false;
            m_controller.MoveDir = Controller.Movement.Forwards;
            m_controller.RotateDir = Controller.Rotation.CW;
        }

        /// <summary>
        /// Update the position and rotation of the actor - advancing by deltaT.
        /// </summary>
        public void Update(float deltaT)
        {
            if (PlayerControlled)
            {
                PlayerStatsInput();
                m_controller.PlayerInput();
            }
            else
            {
                AutoMovement();
            }
            ApplyMovement(deltaT);
        }

        /// <summary>
        /// Add representation of the actor into the renderables collection.
        /// </summary>
        public void Draw(UIElementCollection renderables)
        {
            renderables.Add(RenderUtils.DrawActor(Pos, Rot, Colour));
        }

        public void Highlighted()
        {
            StoredColour = Colour;
            Colour = Brushes.HotPink;
        }

        public void UnHighlighted()
        {
            Colour = StoredColour;
        }

        /// <summary>
        /// Move the actor based on the current Controller variables.
        /// </summary>
        public void ApplyMovement(float deltaT)
        {
            // Apply current rotation to actors heading.
            switch (m_controller.IsRotating())
            {
                case Controller.Rotation.CW:
                    Rot += deltaT * RotationSpeed;
                    break;
                case Controller.Rotation.ACW:
                    Rot -= deltaT * RotationSpeed;
                    break;
                case Controller.Rotation.None:
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            // Update the position of the actor taking into account its current heading.
            var movement = new Point();
            switch (m_controller.IsMoving())
            {
                case Controller.Movement.Forwards:
                    movement.Y = deltaT * -MoveSpeed;
                    break;
                case Controller.Movement.Backwards:
                    movement.Y = deltaT * MoveSpeed;
                    break;
                case Controller.Movement.None:
                    break;
                default:
                    Debug.Assert(false);
                    break;

            }
            var rotatedPoint = new Point(
                movement.X * Math.Cos(Rot) - movement.Y * Math.Sin(Rot),
                movement.Y * Math.Cos(Rot) + movement.X * Math.Sin(Rot)
            );
            var point = Pos;
            point.X += rotatedPoint.X;
            point.Y += rotatedPoint.Y;
            Pos = point;
        }

        /// <summary>
        /// Changes the stats of the actor if player controlled.
        /// </summary>
        private void PlayerStatsInput()
        {
            #region SpeedControl
            if (Keyboard.IsKeyDown(Key.NumPad0))
            {
                MoveSpeed = 100.0f;
                RotationSpeed = 1.0f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad1))
            {
                MoveSpeed *= .9f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad2))
            {
                MoveSpeed = 100.0f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad3))
            {
                MoveSpeed *= 1.1f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad4))
            {
                RotationSpeed *= .9f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad5))
            {
                RotationSpeed = 1.0f;
            }
            if (Keyboard.IsKeyDown(Key.NumPad6))
            {
                RotationSpeed *= 1.1f;
            }
            #endregion SpeedControl

            #region MoveControl
            //if (Keyboard.IsKeyDown(Key.W))
            //{
            //    m_controller.MoveDir = Controller.Moving.Forward;
            //}
            //else if (Keyboard.IsKeyDown(Key.S))
            //{
            //    m_controller.MoveDir = Controller.Moving.Backward;
            //}
            //else
            //{
            //    m_controller.MoveDir = Controller.Moving.Stationary;
            //}
            //if (Keyboard.IsKeyDown(Key.D))
            //{
            //    m_controller.RotateDir = Controller.Rotating.Clockwise;
            //}
            //else if (Keyboard.IsKeyDown(Key.A))
            //{
            //    m_controller.RotateDir = Controller.Rotating.AntiClockwise;
            //}
            //else
            //{
            //    m_controller.RotateDir = Controller.Rotating.Stationary;
            //}
            #endregion MoveControl
        }

        /// <summary>
        /// Moves towards the current target point using MoveX and MoveY.
        /// </summary>
        private void AutoMovement()
        {
            if (!MoveX())
            {
                //Console.WriteLine("Moving X.");
            }
            else if (!MoveY())
            {
                //Console.WriteLine("Moving Y.");
            }
            else
            {
                //Console.WriteLine("Near enough to target position: " + TargetPos);
                TarPos = new Point(random.Next(0, 800), random.Next(0, 400));
                //Console.WriteLine("Getting new target position: " + TargetPos);

            }
        }

        /// <summary>
        /// Moves towards the current target point X and returns true if close enough.
        /// </summary>
        private bool MoveX()
        {
            double Xdiff = TarPos.X - Pos.X;
            if (Xdiff > 5)
            {
                //Console.WriteLine("Xdiff is positive: " + Xdiff);
                if (Rot % FullSpin > (QuaterSpin + .015f))
                {
                    m_controller.RotateDir = Controller.Rotation.ACW;
                }
                else if (Rot % FullSpin < (QuaterSpin - .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.CW;
                }
                else
                {
                    m_controller.RotateDir = Controller.Rotation.None;
                    m_controller.MoveDir = Controller.Movement.Forwards;
                }
            }
            else if (Xdiff < -5)
            {
                //Console.WriteLine("Xdiff is negative: " + Xdiff);
                if (Rot % FullSpin > -(QuaterSpin - .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.ACW;
                }
                else if (Rot % FullSpin < -(QuaterSpin + .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.CW;
                }
                else
                {
                    m_controller.RotateDir = Controller.Rotation.None;
                    m_controller.MoveDir = Controller.Movement.Forwards;
                }
            }
            else
            {
                //Console.WriteLine("X is close enough: " + Xdiff);
                m_controller.MoveDir = Controller.Movement.None;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves towards the current target point Y and returns true if close enough.
        /// </summary>
        private bool MoveY()
        {
            double Ydiff = TarPos.Y - Pos.Y;
            if (Ydiff > 5)
            {
                //Console.WriteLine("Ydiff is positive: " + Ydiff);
                if (Rot % FullSpin > (HalfSpin + .015f))
                {
                    m_controller.RotateDir = Controller.Rotation.ACW;
                }
                else if (Rot % FullSpin < (HalfSpin - .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.CW;
                }
                else
                {
                    m_controller.RotateDir = Controller.Rotation.None;
                    m_controller.MoveDir = Controller.Movement.Forwards;
                }
            }
            else if (Ydiff < -5)
            {
                //Console.WriteLine("Ydiff is negative: " + Ydiff);
                if (Rot % FullSpin > (0 + .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.ACW;
                }
                else if (Rot % FullSpin < (0 - .012f))
                {
                    m_controller.RotateDir = Controller.Rotation.CW;
                }
                else
                {
                    m_controller.RotateDir = Controller.Rotation.None;
                    m_controller.MoveDir = Controller.Movement.Forwards;
                }
            }
            else
            {
                //Console.WriteLine("Y is close enough: " + Ydiff);
                m_controller.MoveDir = Controller.Movement.None;
                return true;
            }
            return false;
        }
    }
}