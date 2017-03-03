using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids_Android
{
    public class BulletEngine
    {
        Model texture;
        List<Bullet> bullets;
        Matrix[] bulletTransforms;
        SoundEffect laserSound;

        public BulletEngine(Model texture, Camera camera, SoundEffect laserSound)
        {
            this.laserSound = laserSound;
            this.texture = texture;
            bullets = new List<Bullet>(GameConstants.NumBullets);
            bulletTransforms = camera.SetupEffectDefaults(texture, camera);
        }

        public List<Bullet> getBullets()
        {
            return bullets;
        }

        private Bullet GenerateNewBullet(Vector3 direction, float velocity, Vector3 position, Camera camera, int ttl)
        {
            return new Bullet(texture, velocity, direction,  position, camera, ttl);
        }

        public void Update(KeyboardState state, KeyboardState lastState, Vector3 direction, float velocity, Vector3 position, Camera camera, float timeDelta)
        {
           if ((state.IsKeyDown(Keys.Space)) && lastState.IsKeyDown(Keys.Space) == false)
            {
                shootBullet(direction, velocity, position, camera);
            }
            for (int i = 0; i < bullets.Count; i++)
           {
                bullets[i].Update(timeDelta);
                if (bullets[i].getTTL() <= 0 || bullets[i].IsActive() == false)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
           }
        }

        public void shootBullet(Vector3 direction, float velocity, Vector3 position, Camera camera)
        {
            for (int index = 0; index < GameConstants.NumBullets; index++)
            {
                int ttl = 150;
                bullets.Add(GenerateNewBullet(direction, velocity, position, camera, ttl));
                //float volume = 0.75f;
                laserSound.Play(0.05f, 0, 0);
            }
        }

        public void Draw(Camera camera)
        {
            for (int index = 0; index < bullets.Count; index++)
            {
                bullets[index].Draw(camera, bulletTransforms);
            }
        }
    }
}
