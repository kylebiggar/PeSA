﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeSA.Engine
{
    public class Settings
    {
        //MotifSettings
        public Dictionary<char, Color> AminoAcidMotifColors;
        private Dictionary<char, Color> DefaultAminoAcidMotifColors;
        public int MotifWidth, MotifHeight;
        public int MotifMaxAAPerColumn = 10;
        public double MotifThreshold = 0.1;

        //PeptideArraySettings
        public int PeptideArrayColumns, PeptideArrayRows;
        public bool PeptideArrayRowsFirst;

        //PermutationArraySettings
        public bool WildTypeYAxisTopToBottom = true;

        //ColorMatrixSettings - set as private for now; not editable
        private Dictionary<string, ColorMatrixTheme> ColorMatrixThemes;

        public Settings()
        {
            this.DefaultAminoAcidMotifColors = new Dictionary<char, Color>() {
                    {'A', Color.Fuchsia },
                    {'R',Color.Green },
                    {'N', Color.FromArgb(128,255,255) },
                    {'D', Color.Red },
                    {'C', Color.Black },
                    {'E', Color.Red },
                    {'Q', Color.FromArgb(128,255,255) },
                    {'G', Color.Black },
                    {'H', Color.Green },
                    {'O', Color.Green },
                    {'I', Color.Fuchsia },
                    {'L', Color.Fuchsia },
                    {'K', Color.Green },
                    {'M', Color.Fuchsia },
                    {'F', Color.Fuchsia },
                    {'P', Color.Black },
                    {'U', Color.Green },
                    {'S', Color.FromArgb(128,255,255) },
                    {'T', Color.FromArgb(128,255,255) },
                    {'W', Color.Fuchsia },
                    {'Y', Color.FromArgb(128,255,255) },//Fuchsia },
                    {'V', Color.Fuchsia },
                    {'X', Color.Black }
                };

        }
        public Color GetColorOfAminoAcid(char aa)
        {
            if (AminoAcidMotifColors.ContainsKey(aa))
                return AminoAcidMotifColors[aa];
            if (DefaultAminoAcidMotifColors.ContainsKey(aa))
                return DefaultAminoAcidMotifColors[aa];
            return Color.Black;
        }
        public void SetDefaultColors()
        {
            AminoAcidMotifColors = DefaultAminoAcidMotifColors;
        }

        public List<char> AminoAcidExcludeList;

        private void SetDefaultColorMatrixThemes()
        {
            ColorMatrixThemes = new Dictionary<string, ColorMatrixTheme>();

            ColorMatrixTheme theme = new ColorMatrixTheme();
            theme.ThemeName = "Grayscale";
            theme.BackgroundColor = theme.StartColor = Color.White;
            theme.FontColor = theme.EndColor = Color.Black;
            theme.Dia = 20;
            theme.Flip = false;
            ColorMatrixThemes.Add(theme.ThemeName, theme);

            theme = new ColorMatrixTheme();
            theme.ThemeName = "Blue-Red scale";
            theme.BackgroundColor = Color.Black;
            theme.StartColor = Color.Blue;
            theme.EndColor = Color.Red;
            theme.FontColor = Color.White;
            theme.Dia = 20;
            theme.Flip = true;
            ColorMatrixThemes.Add(theme.ThemeName, theme);

            theme = new ColorMatrixTheme();
            theme.ThemeName = "Custom";
            theme.BackgroundColor = Color.Black;
            theme.StartColor = Color.LightGreen;
            theme.EndColor = Color.Red;
            theme.FontColor = Color.White;
            theme.Dia = 20;
            theme.Flip = false;
            ColorMatrixThemes.Add(theme.ThemeName, theme);
        }
        public ColorMatrixTheme GetMatrixTheme(bool color)
        {
            if (color)
                return ColorMatrixThemes["Blue-Red scale"];
            return ColorMatrixThemes["Grayscale"];
        }

        public static Settings Load(string name)
        {
            Settings settings = null;
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/PeSA";
                if (Directory.Exists(folder) && File.Exists(folder + "/" + name))
                {
                    settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(folder + "/" + name));
                    if (settings.ColorMatrixThemes == null)
                        settings.SetDefaultColorMatrixThemes();
                }
                else
                {
                    settings = new Settings();
                    settings.MotifHeight = 200;
                    settings.MotifWidth = 800;
                    settings.SetDefaultColors();
                    settings.SetDefaultColorMatrixThemes();
                    settings.PeptideArrayColumns = 30;
                    settings.PeptideArrayRows = 20;
                    settings.PeptideArrayRowsFirst = true;

                    string json = JsonConvert.SerializeObject(settings);

                    Directory.CreateDirectory(folder);
                    File.WriteAllText(folder + "/" + name, json);
                }
                return settings;
            }
            catch
            {
                if (settings == null)
                {
                    settings = new Settings();
                    settings.MotifHeight = 200;
                    settings.MotifWidth = 800;
                    settings.SetDefaultColors();
                    settings.SetDefaultColorMatrixThemes();
                    settings.PeptideArrayColumns = 30;
                    settings.PeptideArrayRows = 20;
                    settings.PeptideArrayRowsFirst = true;
                }
                return settings;
            }
        }

        public bool Save(string name)
        {
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/PeSA";
                if (Directory.Exists(folder) && File.Exists(folder + "/" + name))
                {
                    string json = JsonConvert.SerializeObject(this);
                    File.WriteAllText(folder + "/" + name, json);
                }
                return true;
            }
            catch { return false; }
        }
    }

}
