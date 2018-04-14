using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StealthGame
{

    public class GameUI : MonoBehaviour
    {

        public GameObject game_over_screen;
        public GameObject game_win_screen;

        private bool is_game_over;

        void Start ()
        {
            is_game_over = false;
            Guard.OnPlayerSpotted += OnGameOver;
            Player.OnGameWon += OnGameWon;
        }

        void OnGameOver ()
        {
            is_game_over = true;
            game_over_screen.SetActive ( true );
            Guard.OnPlayerSpotted -= OnGameOver;
        }

        void OnGameWon ()
        {
            is_game_over = true;
            game_win_screen.SetActive ( true );
            Player.OnGameWon -= OnGameWon;
        }

        private void Update ()
        {
            if(is_game_over)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene (0);
                }
            }
        }
    }

}