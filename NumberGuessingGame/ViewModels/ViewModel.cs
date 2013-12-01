using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NumberGuessingGame.Models;

namespace NumberGuessingGame.ViewModels
{
    public class ViewModel
    {
        public SecretNumber SecretNumber { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage="Du måste göra en gissning.")]
        [Range(1, 100, ErrorMessage="Talet måste vara mellan 1 och 100.")]
        public int Guess { get; set; }
    }
}