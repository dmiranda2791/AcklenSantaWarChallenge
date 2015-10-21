using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    private HttpWebRequest request;
    private String url = "http://internal-devchallenge-2-dev.apphb.com/";
    private string[] ENGLISH_WORDS = {"drool", "cats", "clean", "code", "dogs", "materials", "needed", "this", "is", "hard", "what", "are"
                                            ,"you", "smoking", "shot", "gun", "down", "river", "super", "man", "rule", "acklen", "developers"
                                            , "are", "amazing"};
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid guid = Guid.NewGuid();
        lblGuid.Text = guid.ToString();
        request = WebRequest.Create(url + "values/" + guid.ToString()) as HttpWebRequest;
        
        if (request != null) {
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            string responseStream = new StreamReader(response.GetResponseStream()).ReadToEnd();
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            EncriptionValues encriptionValues = deserializer.Deserialize<EncriptionValues>(responseStream);
            lblAlgorithm.Text = encriptionValues.algorithm;
            lblFibonacciNumber.Text = encriptionValues.startingFibonacciNumber.ToString();
            printWords(encriptionValues.words, lblWords);

            switch (encriptionValues.algorithm){
                case "IronMan":
                    lblEncodedString.Text = IronMan(encriptionValues.words);
                    break;
                case "TheIncredibleHulk":
                    lblEncodedString.Text = TheIncredibleHulk(encriptionValues.words);
                    break;
                case "Thor":
                    lblEncodedString.Text = Thor(encriptionValues.words, encriptionValues.startingFibonacciNumber);
                    break;
                case "CaptainAmerica":
                    lblEncodedString.Text = CaptainAmerica(encriptionValues.words, encriptionValues.startingFibonacciNumber);
                    break;
            }
        }
    }

    private string IronMan(string[] words){
        Array.Sort(words);
        printWords(words, lblOrderedWords);
        string[] newWords = new string[words.Length];
        string concatenatedString = "";

        newWords = ShiftVowels(words);
        concatenatedString = ConcatenateWithASCII(newWords);

        printWords(newWords, lblNewWords);
        lblConcatenatedWords.Text = concatenatedString;
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(concatenatedString));
    }

    private string TheIncredibleHulk(string[] words) {
        string concatenatedString = "";
        string[] newWords = ShiftVowels(words);
        Array.Sort(newWords);
        Array.Reverse(newWords);
        printWords(words, lblOrderedWords);

        concatenatedString = ConcatenateWithAsterisk(newWords);

        printWords(newWords, lblNewWords);
        lblConcatenatedWords.Text = concatenatedString;
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(concatenatedString));
    }

    public string Thor(string[] words, int fibonacciNumber) {
        List<String> splittedWords = new List<String>(words);
        string wordToSplit;
        string concatenatedString = "";
        for (int i = 0; i < splittedWords.Count; i++) {
            wordToSplit = splittedWords[i];
            int j = 0;
            while(j < ENGLISH_WORDS.Length && wordToSplit != "") {

                if (wordToSplit.StartsWith(ENGLISH_WORDS[j], true, new CultureInfo("en-US"))) {
                    splittedWords.Add(wordToSplit.Substring(0, ENGLISH_WORDS[j].Length));
                    wordToSplit = wordToSplit.Remove(0, ENGLISH_WORDS[j].Length);
                    if (wordToSplit == "") {
                        splittedWords.RemoveAt(i);
                    }
                    j = -1;
                }
                j++;
            }
        }
        splittedWords.Sort();
        printWords(splittedWords.ToArray(), lblOrderedWords);

        bool useUpper = Char.IsUpper(splittedWords[0][0]);
        StringBuilder splittedWord;
        List<long> fibonacciSerie = GetFibonacciSerieUpTo(fibonacciNumber);

        for (int i = 0; i < splittedWords.Count; i++) {
            splittedWord = new StringBuilder(splittedWords[i]);
            for (int j = 0; j < splittedWord.Length; j++) {
                if (!Char.IsNumber(splittedWord[j])) {
                    if (!IsVowel(splittedWord[j])) {
                        splittedWord[j] = (useUpper ? Char.ToUpper(splittedWord[j]) : Char.ToLower(splittedWord[j]));
                        useUpper = !useUpper;
                    } 
                }
                
            }
            splittedWords[i] = splittedWord.ToString();
        }
        splittedWords = new List<string>(ReplaceVowelsWithFibonacci(splittedWords.ToArray(), fibonacciNumber));
        concatenatedString = ConcatenateWithAsterisk(splittedWords.ToArray());

        lblConcatenatedWords.Text = concatenatedString;
        printWords(splittedWords.ToArray(), lblNewWords);
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(concatenatedString));
    }
    private string CaptainAmerica(string[] words, int fibonacciNumber) {
        string[] newWords = ShiftVowels(words);
        string concatenatedString;
        Array.Sort(newWords);
        Array.Reverse(newWords);
        newWords = ReplaceVowelsWithFibonacci(newWords, fibonacciNumber);
        concatenatedString = ConcatenateWithASCII(newWords);
        lblConcatenatedWords.Text = concatenatedString;
        printWords(newWords.ToArray(), lblNewWords);
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(concatenatedString));
    }

    private string ConcatenateWithASCII(string[] words) {
        string concatenatedString = "";
        for (int i = 0; i < words.Length; i++) {
            if (i == 0) {
                concatenatedString = words[i] + (int)words[words.Length - 1][0];
            } else {
                concatenatedString += words[i] + (int)words[i - 1][0];
            }
        }
        return concatenatedString;
    }

    private string ConcatenateWithAsterisk(string[] words) {
        string concatenatedString = "";
        for (int i = 0; i < words.Length; i++) {
            concatenatedString += words[i] + (i == words.Length - 1 ? "" : "*");
        }

        return concatenatedString;
    }

    public string[] ReplaceVowelsWithFibonacci(string[] words, int fibonacciNumber) {
        List<long> fibonacciSerie = GetFibonacciSerieUpTo(fibonacciNumber);

        for (int i = 0; i < words.Length; i++) {
            StringBuilder word = new StringBuilder(words[i]);
            for (int j = 0; j < word.Length; j++) {
                if (IsVowel(word[j])) {
                    fibonacciSerie.Add(fibonacciSerie[fibonacciSerie.Count - 1] + fibonacciSerie[fibonacciSerie.Count - 2]);
                    word.Remove(j, 1);
                    word.Insert(j, fibonacciSerie[fibonacciSerie.Count - 1].ToString());
                }
            }
            words[i] = word.ToString();
        }
        return words;
    }

    private string[] ShiftVowels(string[] words) {
        string[] newWords = new string[words.Length];
        char vowel;
        char charToReplace;

        for (int i = 0; i < words.Length; i++) {
            StringBuilder word = new StringBuilder(words[i]);

            for (int j = 0; j < word.Length; j++) {
                if (IsVowel(word[j])) {
                    vowel = word[j];
                    if (j == word.Length - 1) {
                        word.Remove(word.Length - 1, 1);
                        word = word.Insert(0, vowel);
                    } else {
                        charToReplace = word[j + 1];
                        word[j + 1] = vowel;
                        word[j] = charToReplace;
                        j++;
                    }
                }
            }
            newWords[i] = word.ToString();
        }
        return newWords;
    }

    private Boolean IsVowel(char c) {
        return (c == 'a' || c == 'A' || c == 'e' || c == 'E' || c == 'i' || c == 'I' || c == 'o' || c == 'O' || c == 'u' || c == 'U' || c == 'y' || c == 'Y');
    }

    private void printWords(string[] words, Label label) {
        for (int i = 0; i < words.Length; i++) {
            label.Text += words[i] + ", ";
        }

        label.Text = label.Text.Remove(label.Text.Length - 2);
    }

    private List<long> GetFibonacciSerieUpTo(int fibonacciNumber) {
        List<long> fibonacciSerie = new List<long>();

        fibonacciSerie.Add(0);
        fibonacciSerie.Add(1);

        if (fibonacciNumber > 1) {
            while (fibonacciSerie[fibonacciSerie.Count - 1] < fibonacciNumber) {
                fibonacciSerie.Add(fibonacciSerie[fibonacciSerie.Count - 1] + fibonacciSerie[fibonacciSerie.Count - 2]);
            }
        }
        fibonacciSerie.RemoveAt(fibonacciSerie.Count - 1);
        return fibonacciSerie;
    }

    private class EncriptionValues {
        public string[] words{ get; set; }
        public int startingFibonacciNumber{ get; set; }
        public string algorithm{ get; set; }
    }
}