using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.TwoD;
using EmberEngine.TwoD.Instances;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.Tools.Input;
using OpenTK.Input;

namespace Sample5
{
    /// <summary>
    /// Represents a basic instance that just rotates
    /// </summary>
    public class BasicCar : Instance2D
    {
        float maxSpeed = 20;
        float standardTurn = 10;
        float acceleration = 0.2F;
        float standardFriction = 0.05F;
        float brakeFriction = 0.05F;
        float eBrakeTurn = 20;
        float elipson = 0.00001F;

        float turn;
        float friction;

        /// <summary>
        /// Creates a new basic instance
        /// </summary>
        /// <param name="sprite">The sprite to use</param>
        /// <param name="pos">The position to begin at</param>
        public BasicCar(Sprite sprite, Vector2 pos)
            : base(sprite, pos) 
        {
            KeyWatcher w = new KeyWatcher(Key.W);
            w.AddDown(OnForwardPressed);
            KeyWatcher s = new KeyWatcher(Key.S);
            s.AddDown(OnReversePressed);
            KeyWatcher a = new KeyWatcher(Key.A);
            a.AddDown(OnLeftPressed);
            KeyWatcher d = new KeyWatcher(Key.D);
            d.AddDown(OnRightPressed);
            KeyWatcher space = new KeyWatcher(Key.Space);
            space.AddPressed(OnSpacePressed);
            space.AddReleased(OnSpaceReleased);

            InputKeys.AddKeyWatcher(w);
            InputKeys.AddKeyWatcher(a);
            InputKeys.AddKeyWatcher(s);
            InputKeys.AddKeyWatcher(d);
            InputKeys.AddKeyWatcher(space);

            turn = standardTurn;
            friction = standardFriction;
        }

        private void OnForwardPressed(KeyDownEventArgs e)
        {
            if (Speed < maxSpeed)
                Speed += acceleration;
        }

        private void OnReversePressed(KeyDownEventArgs e)
        {
            if (Speed > -maxSpeed)
                Speed -= acceleration;
        }

        private void OnLeftPressed(KeyDownEventArgs e)
        {
            Direction -= (Speed / maxSpeed) * turn;
        }

        private void OnRightPressed(KeyDownEventArgs e)
        {
            Direction += (Speed / maxSpeed) * turn;
        }

        private void OnSpacePressed(KeyDownEventArgs e)
        {
            turn = standardTurn + eBrakeTurn;
            friction = standardFriction + brakeFriction;
        }

        private void OnSpaceReleased(KeyDownEventArgs e)
        {
            turn = standardTurn;
            friction = standardFriction;
        }

        public override void Update(GameTime gameTime)
        {
            if (Speed != 0)
                Speed /= (1 + friction);

            if (Math.Abs(Speed) <= elipson)
                Speed = 0;

            base.Update(gameTime);
        }
    }
}
