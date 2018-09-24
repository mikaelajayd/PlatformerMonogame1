using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System;

namespace PlatformerMonogame1
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            // Compares the position of each rectangle edge against the other
            // Compares opposite edges of each rectangle, ie, the left edge
            // of one with the right of the other
            if (hero.rightEdge < otherSprite.leftEdge || 
                hero.leftEdge > otherSprite.rightEdge || 
                hero.bottomEdge < otherSprite.topEdge || 
                hero.topEdge > otherSprite.bottomEdge)
            {
                // These two rectangles are not colliding
                return false;
            }

            // Else, the two AABB rectangles overlap, therefore there is a collision
            return true;
        }

        public Sprite CollideWithPlatforms(Sprite hero, float deltaTime)
        {
            // Create a copy of the hero that will move to where the hero will be
            // next frame, to PREDICT if the hero will overlap an obstacle
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offset = hero.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltaTime;

            int playerColumn = (int)Math.Round(playerPrediction.position.X / game.tileHeight);
            int playerRow = (int)Math.Round(playerPrediction.position.Y / game.tileHeight);
            Vector2 playerTile = new Vector2(playerColumn, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y + 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y + 1);
            Vector2 topLeftTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);

            bool leftCheck = CheckForTile(game, leftTile);
            bool rightCheck = CheckForTile(game, rightTile);
            bool bottomCheck = CheckForTile(game, bottomTile);
            bool topCheck = CheckForTile(game, topTile);

            bool bottomLeftCheck = CheckForTile(game, bottomLeftTile);
            bool bottomRightCheck = CheckForTile(game, bottomRightTile);
            bool topLeftCheck = CheckForTile(game, topLeftTile);
            bool topRightCheck = CheckForTile(game, topRightTile);
            
            // Checks for collisions with tiles to the left of player
            if(leftCheck == true)
            {
                hero = CollideLeft(game, hero, leftTile, playerPrediction);
            }

            // Checks for collisions with tilesto theright of player
            if (rightCheck == true)
            {
                hero = CollideRight(game, hero, rightTile, playerPrediction);
            }

            // Checks for collisions under player
            if (bottomCheck == true)
            {
                hero = CollideBelow(game, hero, bottomTile, playerPrediction);
            }

            // Checks for collisions above player
            if (topCheck == true)
            {
                hero = CollideAbove(game, hero, topTile, playerPrediction);
            }
            return hero;
        }

        //Checks if there is a tile at the specified coordinates
        bool CheckForTile(Game1 game, Vector2 coordinates)
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if (column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }

            if (row < 0 || row > game.levelTileHeight - 1)
            {
                return false;
            }

            Sprite tileFound = game.levelGrid[column, row];

            if (tileFound != null)
            {
                return true;
            }

            return false;
        }

        Sprite CollideLeft(Game1 game, Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideRight(Game1 game, Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideAbove(Game1 game, Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y < 0)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        Sprite CollideBelow(Game1 game, Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y > 0)
            {
                hero.position.Y = tile.topEdge - hero.height + hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }
    }
}
