﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mono_test_android2
{
    public class AsteroidEngine
    {
        List<Asteroid> asteroidList;
        ParticleEngine asteroidParticle;
        Random random;
        Matrix[] asteroidTransforms;
        Vector3 bulletVector;
        int asteroids;

        public AsteroidEngine(Model currentTexture, Camera camera, List<Model> particleModel, int level)
        {
            asteroids = GameConstants.NumAsteroids * level;
            asteroidList = new List<Asteroid>(GameConstants.NumAsteroids * level);
            asteroidParticle = new ParticleEngine(particleModel);
            asteroids = asteroidList.Count();
            asteroidTransforms = SetupEffectDefaults(currentTexture, camera);
            random = new Random();
        }

        public List<Asteroid> getAsteroidList()
        {
            return asteroidList;
        }

        public void setBulletVector(Vector3 v)
        {
            bulletVector = v;
        }

        private Matrix[] SetupEffectDefaults(Model myModel, Camera camera)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.Projection = camera.getProjection();
                    effect.View = camera.getView();
                }
            }
            return absoluteTransforms;
        }

        public void ResetAsteroids(Model currentTexture, Camera camera, int level)
        {
            float x; // x for position
            float y; // y for position
            float x2; // x for direction
            float y2; // y for direction
            for (int i = 0; i < GameConstants.NumAsteroids * level; i++)
            {
                if (random.Next(2) == 0)
                {
                    x = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    x = (float)GameConstants.PlayfieldSizeX;
                }
                y = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                //Console.WriteLine("size: "+asteroidList.Count());
                double angle = random.NextDouble() * 2 * Math.PI;
                x2 = -(float)Math.Sin(angle);
                y2 = (float)Math.Cos(angle);
                AddAsteroid(currentTexture, x, y, x2, y2, GameConstants.AsteroidMinSpeed + (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed, 3, 30);
            }
        }

        public void AddAsteroid(Model currentTexture, float x, float y, float x2, float y2, float velocity, int life, int mass)
        {
            asteroidList.Add(new Asteroid(currentTexture, new Vector3(x, y, 0), x2, y2, velocity, life, mass, random));
        }

        public void Update(float timeDelta, Model[] asteroidModel, Camera camera)
        {
            for (int i = 0; i < asteroidList.Count(); i++)
            {
                asteroidList[i].Update(timeDelta);
                if (asteroidList[i].IsActive() == false)
                {
                    asteroidList[i].addLife(-1);
                    if (asteroidList[i].getLife() > 0)
                    {                       
                        //(-Y, X)
                        //(Y, -X)
                        //adds two more asteroids when one is destroyed
                        Vector3 perpendicularVector1 = new Vector3(-asteroidList[i].getVelocity().Y, asteroidList[i].getVelocity().X, 0);//maybe we should add bullet vector?
                        Vector3 perpendicularVector2 = new Vector3(asteroidList[i].getVelocity().Y, -asteroidList[i].getVelocity().X, 0);//maybe we should add bullet vector?
                        Vector3 newVector1 = ((asteroidList[i].getVelocity()*2) + perpendicularVector1 + bulletVector)/4;
                        Vector3 newVector2 = ((asteroidList[i].getVelocity()*2) + perpendicularVector2 + bulletVector)/4;
                        AddAsteroid(
                            asteroidModel[asteroidList[i].getLife() - 1], //pass in smaller roid
                            asteroidList[i].getPosition().X, //pass in x position of parent roid
                            asteroidList[i].getPosition().Y, //pass in y position of parent roid

                            newVector1.X,   
                            newVector1.Y,  
                            asteroidList[i].getSpeed(), 
                            asteroidList[i].getLife(), 10 * asteroidList[i].getLife());// adds perpendicular vector (-y, x)

                        AddAsteroid(
                            asteroidModel[asteroidList[i].getLife() - 1], //pass in smaller model
                            asteroidList[i].getPosition().X, //pass in x position of parent roid
                            asteroidList[i].getPosition().Y, //pass in y position of parent roid
                            newVector2.X, 
                            newVector2.Y, 
                            asteroidList[i].getSpeed(), 
                            asteroidList[i].getLife(), 10 * asteroidList[i].getLife());// adds perpendicular vector (y, -x)
                    }
                    int total = 25;
                    for (int j = 0; j < total; j++)
                    {
                        Vector3 velocity = new Vector3(1f * (float)(random.NextDouble() * 2 - 1), 1f * (float)(random.NextDouble() * 2 - 1), 0);
                        asteroidParticle.getParticles().Add(asteroidParticle.GenerateNewParticle(asteroidList[i].getPosition(), Vector3.Multiply(velocity, 0.1f), camera));
                    }
                    asteroidList.RemoveAt(i); // removes old asteroid
                    i--;                    
                }
            }
            asteroidParticle.Update();
        }

        public void Draw(Camera camera)
        {
            for (int i = 0; i < asteroidList.Count(); i++)
            {
                if (asteroidList[i].IsActive() == true)
                {
                    asteroidList[i].Draw(camera, asteroidTransforms);
                }
            }
            asteroidParticle.Draw(camera);
        }
    }
}
