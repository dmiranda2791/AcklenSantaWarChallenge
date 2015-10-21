﻿using System;
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
            }
        }
    }

    private string IronMan(string[] words){
        Array.Sort(words);
        printWords(words, lblOrderedWords);
        string[] newWords = new string[words.Length];
        string concatenatedString = "";

        newWords = ShiftVowels(words);

        for (int i = 0; i < newWords.Length; i++) {
            if (i == 0) {
                concatenatedString = newWords[i] + (int)newWords[newWords.Length - 1][0];
            } else {
                concatenatedString += newWords[i] + (int)newWords[i - 1][0];
            }
        }

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

        for (int i = 0; i < words.Length; i++) {
            concatenatedString += newWords[i] + (i == newWords.Length - 1 ? "" : "*");
        }

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
        List<int> fibonacciSerie = GetFibonacciSerieUpTo(fibonacciNumber);

        for (int i = 0; i < splittedWords.Count; i++) {
            splittedWord = new StringBuilder(splittedWords[i]);
            for (int j = 0; j < splittedWord.Length; j++) {
                if (!Char.IsNumber(splittedWord[j])) {
                    if (!IsVowel(splittedWord[j])) {
                        splittedWord[j] = (useUpper ? Char.ToUpper(splittedWord[j]) : Char.ToLower(splittedWord[j]));
                        useUpper = !useUpper;
                    } else {
                        fibonacciSerie.Add(fibonacciSerie[fibonacciSerie.Count - 1] + fibonacciSerie[fibonacciSerie.Count - 2]);
                        splittedWord.Remove(j, 1);
                        splittedWord.Insert(j, fibonacciSerie[fibonacciSerie.Count - 1].ToString());
                    }
                }
                
            }
            splittedWords[i] = splittedWord.ToString();
            concatenatedString += splittedWord.ToString() + (i == splittedWords.Count - 1 ? "" : "*");
        }

        lblConcatenatedWords.Text = concatenatedString;
        printWords(splittedWords.ToArray(), lblNewWords);
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(concatenatedString));
    }
    private void CaptainAmerica(string[] words) {
        
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

    private List<int> GetFibonacciSerieUpTo(int fibonacciNumber) {
        List<int> fibonacciSerie = new List<int>();

        int a = 0;
        int b = 1;

        fibonacciSerie.Add(a);
        fibonacciSerie.Add(b);

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