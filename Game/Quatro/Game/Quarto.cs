﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using Game.GameBase;
using Client.AIAlgorithmBase;
using GameBase;

namespace Game.Quatro.Game
{
    public class Quarto
    {
        static Board activeBoard;
        QuartoHeuristic heuristic;
        //actuális pálya lekérdezése
        internal static Board ActiveBoard
        {
            get { return Quarto.activeBoard; }

        }
        static Player[] player;
        //játékos tömb lekérdezése
        internal static Player[] Player
        {
            get { return Quarto.player; }

        }
        static Piece[] activePieces;

        //a bábuk tömbjének lekérdése
        internal static Piece[] ActivePieces
        {
            get { return Quarto.activePieces; }

        }
        static int activePlayer;
        // az aktuális játékos indexe a Player tömbbne
        public static int ActivePlayer
        {
            get { return Quarto.activePlayer; }

        }
        // az ellenfél számára választott bábú
        static Piece selectedPiece;

        internal static Piece SelectedPiece
        {
            get { return Quarto.selectedPiece; }
            
        }

        static BackgroundWorker gameThread = new BackgroundWorker();
        internal static void initGame(string playerName)
        {
            // ezzel a függvénnyel inicializáljuk és indítjuk a játékot

            activeBoard = new Board();
            player = new Player[2];
            player[0] = new Player();
            player[1] = new Player();
            

            //a bábuk tömbje 1 0 kommentezve a Pieaces osztálynál
            activePieces = new Piece[16];
            activePieces[0] = new Piece();
            activePieces[0].setPiece(1, 1, 1, 1);
            activePieces[1] = new Piece();
            activePieces[1].setPiece(1, 1, 1, 0);
            activePieces[2] = new Piece();
            activePieces[2].setPiece(1, 1, 0, 1);
            activePieces[3] = new Piece();
            activePieces[3].setPiece(1, 1, 0, 0);
            activePieces[4] = new Piece();
            activePieces[4].setPiece(1, 0, 1, 1);
            activePieces[5] = new Piece();
            activePieces[5].setPiece(1, 0, 1, 0);
            activePieces[6] = new Piece();
            activePieces[6].setPiece(1, 0, 0, 1);
            activePieces[7] = new Piece();
            activePieces[7].setPiece(1, 0, 0, 0);
            activePieces[8] = new Piece();
            activePieces[8].setPiece(0, 1, 1, 1);
            activePieces[9] = new Piece();
            activePieces[9].setPiece(0, 1, 1, 0);
            activePieces[10] = new Piece();
            activePieces[10].setPiece(0, 1, 0, 1);
            activePieces[11] = new Piece();
            activePieces[11].setPiece(0, 1, 0, 0);
            activePieces[12] = new Piece();
            activePieces[12].setPiece(0, 0, 1, 1);
            activePieces[13] = new Piece();
            activePieces[13].setPiece(0, 0, 1, 0);
            activePieces[14] = new Piece();
            activePieces[14].setPiece(0, 0, 0, 1);
            activePieces[15] = new Piece();
            activePieces[15].setPiece(0, 0, 0, 0);

            //eldöntjük hogy ki kezd
            Random random = new Random();
            activePlayer = random.Next(0, 1000) % 2;
           

        }

       
        //az elenfél által választott bábut lerakjuk pálya x y korrdinátára 0 tól indexelve
        internal static void updateGameState(int x, int y)
        {
            try
            {
                if (activeBoard.checkIsEmpty(x, y))
                {
                    activeBoard.insertPiece(x, y, selectedPiece);

                    selectedPiece = null;
                    activeBoard.checkWinningState();
                }

            }
            catch (Exception)
            { }
        }
        internal static void selectPiece(PlayerType type,Piece select)
        {
            //kiválasztjuk a bábút amit az ellenfélnek le kell tenni majd kivesszük a bábúk tömbbjéből
            if (type == PlayerType.PlayerOne && activePlayer == 0)
            {
                try
                {
                    selectedPiece = select;
                    activePieces = activePieces.Where(w => w != selectedPiece).ToArray();
                    activePlayer = (activePlayer + 1) % 2;
                }
                catch (Exception)
                { }
            }
            else if (type == PlayerType.PlayerTwo && activePlayer == 1)
            {
                try
                {
                    selectedPiece = select;
                    activePieces = activePieces.Where(w => w != selectedPiece).ToArray();
                    activePlayer = (activePlayer + 1) % 2;
                }
                catch (Exception)
                { }
            }

        }
    
        private static void startGameProcess(object sender, DoWorkEventArgs e)
        {
            //egy külön szálként indítjuk el a játékot ez maga a háttérfolyamat

            //addig mig a nem nyert valaki vagy nem raktak le minden bábút
            while (activeBoard.Winstate == false || !activeBoard.checkIsFull())
            {


                if (selectedPiece == null)
                {
                    if (Player[activePlayer].PlayerEntity == EntityType.HumanPlayer)
                    {

                        Thread.Sleep(1000);
                        ///megvárjuk míg a játékos lerak egy bábut

                    }
                    else
                    {
                        //itt kell meghivni az ai algoritmust
                        Thread.Sleep(1000);
                    }
                }
                if (selectedPiece != null)
                {
                    if (Player[activePlayer].PlayerEntity == EntityType.HumanPlayer)
                    {


                        Thread.Sleep(1000);
                        //várunk míg a játékos lerak egy bábut


                    }
                    else
                    {
                        // az AI lerakja a bábút
                        Thread.Sleep(1000);

                    }


                }
            }
        }


        public override void StartGame()
        {
           
            try
            { //ezzel indítjuk a háttérszálat automatikusan meghívásra kerül az initGame-ben
                gameThread.WorkerSupportsCancellation = true;

                gameThread.DoWork += new DoWorkEventHandler(startGameProcess);
                gameThread.RunWorkerAsync();
            }
            catch (Exception)
            { }


        }
        public override void SetHeuristic<Board>(IHeuristic<Board> heur)
        {
            if (!(heur is QuartoHeuristic))
                throw new ArgumentException("Not valid heuristic type for this game!");

            heuristic = (QuartoHeuristic)heur;
        }

        public override string GetGameTypeInfo()
        {
            return "Quarto";
        }

        public override void RegisterAsPlayer<TAlgorithm>(ref GameBase.AbstractGame.StepHandler onStep, GameBase.PlayerType playerType, GameBase.EntityType controller, TAlgorithm algorithm)
        {
            if (!(algorithm is IAIAlgorithm))
                throw new Exception("Not valid algorithm type!");

            if (Player == null)
            {
                int index = 1;
                // Index in the players list 
                if (playerType == PlayerType.PlayerOne)
                {
                    index = 0;
                }
                Player[index].PlayerEntity = controller;
                Player[index].PlayerType = playerType;
                Player[index].Algorithm = (IAIAlgorithm)algorithm;
            }
           
        }
        public override AbstractStep.Result DoStep(AbstractStep step, GameBase.PlayerType playerType)
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");

            QuartoStep cStep = (QuartoStep)step;
            if (selectedPiece == null)
            {
                selectPiece( playerType,cStep.P); ;
            }
            else
            {
                updateGameState(cStep.X, cStep.Y);
                
            }

            

            return AbstractStep.Result.Success;
        }
       





    }
}