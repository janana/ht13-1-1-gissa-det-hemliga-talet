using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NumberGuessingGame.Models;
using NumberGuessingGame.ViewModels;

namespace NumberGuessingGame.Controllers
{
    public class GuessController : Controller
    {
        private SecretNumber _secretNumber;

        //
        // GET: /Guess/
        public ActionResult Index()
        {
            if (Session.IsNewSession)
            {
                _secretNumber = new SecretNumber();
                Session.Add("SecretNumber", _secretNumber);
            }
            else
            {
                if (Session.Count > 0)
                {
                    _secretNumber = (SecretNumber)Session["SecretNumber"];
                    // Check if secret nr should be rebooted
                    if (!_secretNumber.CanMakeGuess)
                    {
                        _secretNumber.Initialize();
                    }
                }
                else
                {
                    return View("Error", string.Empty, "Sessionen är felaktig");
                }
            }
            var viewModel = new ViewModel
            {
                SecretNumber = _secretNumber
            };
            return View("Index", viewModel);
        }

        //
        // POST: /Guess/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index([Bind(Include="Guess")] ViewModel viewModel)
        {
            try
            {
                if (Session.Count > 0)
                {
                    _secretNumber = (SecretNumber)Session["SecretNumber"];
                    viewModel.SecretNumber = _secretNumber;
                    if (ModelState.IsValid)
                    {
                        var outcome = _secretNumber.MakeGuess(viewModel.Guess);

                        switch (outcome) 
                        {
                            case Outcome.Indefinite:
                                break;
                            case Outcome.High:
                                viewModel.Status = _secretNumber.LastGuessedNumber.Number + " är för högt.";
                                break;
                            case Outcome.Low:
                                viewModel.Status = _secretNumber.LastGuessedNumber.Number + " är för lågt";
                                break;
                            case Outcome.OldGuess:
                                viewModel.Status = "Du har redan gissat på talet " + _secretNumber.LastGuessedNumber.Number;
                                break;
                            case Outcome.Right:
                                viewModel.Status = "Grattis, du klarade det på " + _secretNumber.Count + " försök!";
                                break;
                        }
                        if (!_secretNumber.CanMakeGuess)
                        {
                            viewModel.Status += " Det hemliga talet var " + _secretNumber.Number;
                        }
                    }
                    return View("Index", viewModel);
                }
                else
                {
                    throw new ApplicationException("Sessionen är felaktig");
                }
            }
            catch (Exception e)
            {
                return View("Error", string.Empty, e.Message);
            }
        }
        
        
        // Inits the secret nr again and sends back to index-method and page
        public ActionResult Reload()
        {
            if (Session.Count > 0)
            {
                _secretNumber = (SecretNumber)Session["SecretNumber"];
                _secretNumber.Initialize();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", String.Empty, "Sessionen är felaktig");
            }
        }
    }
}
