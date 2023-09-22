﻿using ClassLibrary;
using Savanna.cons;

namespace Savannah
{
    public class GameFlow
    {
        public int Dimension;
        private readonly GridService _initializeGrid;
        private readonly Input _input;
        private readonly AnimalFinalizer _updateGame;
        private readonly Display _display;
        public GameFlow()
        {
            _initializeGrid = new();
            _input = new(Dimension);
            _updateGame = new(Dimension);
            _display = new(_updateGame.Animals);
        }
        public void Run()
        {
            Dimension = _input.GridSizeInput();
            _display.DisplayAnimalCount();
            _display.DisplayGameTitle();
            int cursorTop = Console.CursorTop;
            bool isGameRunning = true;
            bool isPredatorTurn = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (isGameRunning)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                _updateGame.MoveAnimals(Dimension, grid, ref isPredatorTurn);
                Thread.Sleep(250);

                _input.ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
    }
}
