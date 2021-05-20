using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataLibrary.Model
{
    public class Files
    {
        public static string AppPath() => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        public static string SolutionPath() => Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        public static string FormatPath(string path)
        {
            string slash = @"\";// check if path is ending with slash
            char c = path.Trim().ToString().Last();
            if (c.ToString() != slash) path += slash;
            return path;
        }

        public static List<string[]> VBCSVParser(string path)
        {
            //Parser CSV jeszcze z czasow VB (6.0) nadal najlepsze z wszystkiego co C# moze w tej
            // kwesti zaoferowac -Dariusz Tomczak
            if (File.Exists(path) == false) return null;
            List<string[]> tempList = new List<string[]>();
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    tempList.Add(fields);
                }
                tempList = tempList //Linq sort
                        .OrderBy(arr => arr[0])
                        .ThenBy(arr => arr[1])
                        .ToList();
                return tempList;
            }
        }

        public static void OpenFileFormPath(string path)
        {
            if (File.Exists(path) == false) return;
            System.Diagnostics.Process.Start(path);
        }

        public static string RemLetters(string myString)
        {
            string remLettersRet = default;
            string regPattern = "[a-zA-Z]+";

            var regx = new Regex(regPattern);
            remLettersRet = regx.Replace(myString, "");
            remLettersRet = Regex.Replace(remLettersRet, @"\,$", "");
            return remLettersRet;
        }

        public static void ParserCSV(Stream myStream, ref (long, long) counting)
        {
            List<string[]> tempList = new List<string[]>();
            myStream.Position = 0;
            using TextFieldParser csvParser = new TextFieldParser(myStream);
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { ";" });
            csvParser.HasFieldsEnclosedInQuotes = true;

            csvParser.ReadLine();
            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields();
                tempList.Add(fields);
            }
            tempList = tempList //Linq sort
                    .OrderBy(arr => arr[0])
                    .ThenBy(arr => arr[1])
                    .ToList();

            if (tempList.Count > 0)
            {
                foreach (var item in tempList)
                {
                    try
                    {
                        EventModel ev = new EventModel();
                        ev.EventName = item[0].ToString();
                        ev.EventDesc = item[1].ToString();
                        if (ev.EventName.Length > 32|| ev.EventDesc.Length > 255) { counting.Item2++; continue; }

                        if (DateTime.TryParse(item[2].ToString(), out DateTime rDate) == true && DateTime.TryParse(item[3].ToString(), out DateTime rEndDate) == true)
                        {
                            ev.EventStartDate = rDate;
                            ev.EventStartDate = rEndDate;

                            ISQLDataAccess dbSQL = new SQLDataAccess();
                            IEventData db = new EventData(dbSQL); ;
                            db.InsertEvent(ev);
                            counting.Item1++;
                        }
                        else counting.Item2++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Incorrect input from file: error msg {0}", ex.Message);
                        counting.Item2++;
                    }
                }
            }
        }

        public static bool CheckExtenstion(string fileName, string ext)
        {
            var sd = Path.GetExtension(fileName);
            bool result = (ext.ToUpper() == sd.ToString().ToUpper()) ? true : false;
            return result;
        }
    }
}