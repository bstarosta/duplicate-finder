using duplicate_finder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace duplicate_finder.Controllers
{
    public class InputProcessingController : Controller
    {
        private readonly ILogger<InputProcessingController> _logger;

        public InputProcessingController(ILogger<InputProcessingController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InputProcessingResult(string input)
        {
            if(!isInputValid(input))
            {
                var errorViewModel = new InputProcessingResultViewModel()
                {
                    Output = "Error. Invalid input"
                };

                return View(errorViewModel);
            }

            var viewModel = new InputProcessingResultViewModel()
            {
                Output = processInput(input)
            };

            return View(viewModel);
        }

        private bool isInputValid(string input)
        {
            var regex = new Regex(@"\[(\d+,)*\d+\]");
            return regex.IsMatch(input);
        }

        private int[] stringToIntArray(string input)
        {
            string noBracketsInput = input.Substring(1, input.Length - 2);
            return noBracketsInput.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        }

        private int[] findRepeatingElements(int[] input, int numberOfRepetitions)
        {
            return input.GroupBy(x => x)
                .Where(g => g.Count() >= numberOfRepetitions)
                .Select(g => g.Key ).ToArray();
        }

        private string intArrayToString(int[] array)
        {
            string commaSeparatedIntegers = string.Join(",", array.Select(x => x.ToString()).ToArray());
            return "[" + commaSeparatedIntegers + "]";
        }

        private string processInput(string input)
        {
            int[] inputArray = stringToIntArray(input);
            int[] orderedRepeatingElements = findRepeatingElements(inputArray, 3).OrderByDescending(x=>x).ToArray();
            return intArrayToString(orderedRepeatingElements);
        }
    }
}
