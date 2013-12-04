using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NumberGuessingGame.Models
{
    public class SecretNumber
    {
        public const int MaxNumberOfGuesses = 7;

        private int? _number;
        private List<GuessedNumber> _guessedNumbers;
        private GuessedNumber _lastGuessedNumber;

        public bool CanMakeGuess
        {
            get
            {
                if (Count >= MaxNumberOfGuesses || LastGuessedNumber.Outcome == Outcome.Right || LastGuessedNumber.Outcome == Outcome.NoMoreGuesses)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int Count
        {
            get
            {
                return _guessedNumbers.Count;
            }
        }
        public IList<GuessedNumber> GuessedNumbers
        {
            get
            {
                return _guessedNumbers.AsReadOnly();
            }
        }
        public GuessedNumber LastGuessedNumber
        {
            get
            {
                return _lastGuessedNumber;
            }
        }
        public int? Number
        {
            get
            {
                if (!CanMakeGuess)
                {
                    return _number;
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                if (value > 0 && value < 100)
                {
                    _number = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Gissningen måste vara mellan 1-100.");
                }
            }
        }

        public SecretNumber()
        {
            _guessedNumbers = new List<GuessedNumber>();
            Initialize();
        }
        public void Initialize()
        {
            _guessedNumbers.Clear();
            _lastGuessedNumber = new GuessedNumber { Number = null, Outcome = Outcome.Indefinite };

            Random random = new Random();
            _number = random.Next(1, 100);
        }
        public Outcome MakeGuess(int guess)
        {
            if (guess < 1 || guess > 100)
            {
                throw new ArgumentOutOfRangeException("Gissningen måste vara mellan 1-100.");
            }
            foreach (GuessedNumber nr in _guessedNumbers)
            {
                if (nr.Number == guess)
                {
                    return Outcome.OldGuess;
                }
            }

            if (Count >= MaxNumberOfGuesses)
            {
                return  Outcome.NoMoreGuesses;
            }

            _lastGuessedNumber.Number = guess;
            
            if (guess == _number)
            {
                _lastGuessedNumber.Outcome = Outcome.Right;
            }
            else if (guess > _number)
            {
                _lastGuessedNumber.Outcome = Outcome.High;
            }
            else if (guess < _number)
            {
                _lastGuessedNumber.Outcome = Outcome.Low;
            }
            
            _guessedNumbers.Add(_lastGuessedNumber);

            return _lastGuessedNumber.Outcome;
        }
    }
}