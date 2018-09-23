using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerMonogame1
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            //Compares the position of each rectangle edge against the other
            //Compares opposite edges of each rectangle, ie, the left edge
            //of one with the right of the other
            if (hero.rightEdge < otherSprite.leftEdge || 
                hero.leftEdge > otherSprite.rightEdge || 
                hero.bottomEdge < otherSprite.topEdge || 
                hero.topEdge > otherSprite.bottomEdge)
            {
                //These two rectangles are not colliding
                return false;
            }
            //Else, the two AABB rectangles overlap, therefore there is a collision
            return true;
        }
    }
}
