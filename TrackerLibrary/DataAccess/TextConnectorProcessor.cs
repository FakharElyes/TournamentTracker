﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        //public static string FullFilePath(this string fileName) => $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        public static string FullFilePath(this string fileName) => $"D:\\Khedma\\TournamentTracker\\data\\{fileName}";
        public static List<string> LoadFile(this string file)
        {
            // * Load the text file
            // * convert the text to List<PrizeModel>
            // find the last ID
            // Add the new record with the new ID (last +1)
            // convert the prizes to list
            // Save the list<string> to the text file
            if (!File.Exists(file)) 
            {
                return new List<string>();
            }
            
            return File.ReadLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModel(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }

            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(),lines);
        }

    }
}