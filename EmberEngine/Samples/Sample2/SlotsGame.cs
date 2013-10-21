using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using System.Timers;

namespace Samples.Sample2
{
    public static class SlotsGame
    {
        static double AngleMulti = (Math.PI * 2) / 6.0;
        static Dictionary<string, int> IDS = new Dictionary<string, int>();
        static World world;

        static Timer Spinner1;
        static Timer Spinner2;
        static Timer Spinner3;
        static Timer Spinner4;

        static int[] Results = new int[4];

        static Random rand1 = new Random();
        static Random rand2 = new Random(DateTime.Now.Millisecond + rand1.Next());
        static Random rand3 = new Random(DateTime.Now.Hour + rand2.Next());
        static Random rand4 = new Random(DateTime.Now.Second + rand3.Next());

        /// <summary>
        /// Gets or sets the speen of the spinners
        /// </summary>
        public static float SpinnerSpeed = 0.4F;

        /// <summary>
        /// Initializes this Slots game
        /// </summary>
        /// <param name="World">The world to add the components to</param>
        /// <param name="SpinnerEffect">The shader to use for the spinners</param>
        public static void Intialize(ref World World, Shader SpinnerEffect)
        {
            world = World;

            float x = -5.5F;
            for (int i = 0; i < 4; i++)
            {
                Spinner temp = new Spinner(new Vector3(x, 0, 2), new Vector3(x + 2, 0, 2), SpinnerEffect, 1);
                temp.RotationSpeed = new Vector3(0, 0, 0);
                temp.Initialize(world);
                IDS.Add("Spinner" + (i + 1), temp.ID);

                x += 3F;
            }

            Spinner1 = new Timer(TimeSpan.FromSeconds(4).TotalMilliseconds);
            Spinner1.Elapsed += Spinner1_Elapsed;
            Spinner1.AutoReset = true;

            Spinner2 = new Timer(TimeSpan.FromSeconds(6).TotalMilliseconds);
            Spinner2.Elapsed += new ElapsedEventHandler(Spinner2_Elapsed);
            Spinner2.AutoReset = true;

            Spinner3 = new Timer(TimeSpan.FromSeconds(8).TotalMilliseconds);
            Spinner3.Elapsed += new ElapsedEventHandler(Spinner3_Elapsed);
            Spinner3.AutoReset = true;

            Spinner4 = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            Spinner4.Elapsed += new ElapsedEventHandler(Spinner4_Elapsed);
            Spinner4.AutoReset = true;
        }

        /// <summary>
        /// Starts a spine cycle
        /// </summary>
        public static void StartSpin()
        {
            foreach (string s in IDS.Keys)
            {
                if (s.Contains("Spinner"))
                    world.instances[IDS[s]].RotationSpeed = new Vector3(SpinnerSpeed, 0, 0);
            }

            Spinner1.Start();
            Spinner2.Start();
            Spinner3.Start();
            Spinner4.Start();
        }

        /// <summary>
        /// Gets the Angle in radians for the given result
        /// </summary>
        /// <param name="result"> a result that is >= 0 and < 6</param>
        /// <returns></returns>
        public static float GetAngleForResult(int result)
        {
            if (result >= 0 & result < 6)
            {
                return (float)AngleMulti * result;
            }
            else
                throw new IndexOutOfRangeException("The result " + result + "is out of range");
        }

        /// <summary>
        /// Updates this slots game
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
        }

        static void Spinner4_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner4"]].RotationSpeed = Vector3.Zero;
            Spinner4.Stop();

            Results[3] = rand4.Next(0, 6);
            world.instances[IDS["Spinner4"]].Rotation = new Vector3(GetAngleForResult(Results[3]), 0, 0);
        }

        static void Spinner3_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner3"]].RotationSpeed = Vector3.Zero;
            Spinner3.Stop();

            Results[2] = rand3.Next(0, 6);
            world.instances[IDS["Spinner3"]].Rotation = new Vector3(GetAngleForResult(Results[2]), 0, 0);
        }

        static void Spinner2_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner2"]].RotationSpeed = Vector3.Zero;
            Spinner2.Stop();

            Results[1] = rand2.Next(0, 6);
            world.instances[IDS["Spinner2"]].Rotation = new Vector3(GetAngleForResult(Results[1]), 0, 0);
        }

        static void Spinner1_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner1"]].RotationSpeed = Vector3.Zero;
            Spinner1.Stop();

            Results[0] = rand1.Next(0, 6);
            world.instances[IDS["Spinner1"]].Rotation = new Vector3(GetAngleForResult(Results[0]), 0, 0);
        }
    }
}
