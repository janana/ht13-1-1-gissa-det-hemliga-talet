using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum Outcome
{
    Indefinite,
    Low,
    High,
    Right,
    NoMoreGuesses,
    OldGuess
}

namespace NumberGuessingGame.Models
{
    public struct GuessedNumber
    {
        public int? Number;
        public Outcome Outcome;
    }
}