using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SColorRemote.Services
{
    public static class VoiceRecognition
    {
        public static IEnumerable<ColorAction> Recognize(string text)
        {
            text = text.ToLower().Replace("sang pour sang", "100%" /*eh oui...*/).Replace("pourcent", "%");
            string[] words = text.Split(new string[] { " " }, StringSplitOptions.None);

            KeyValuePair<Color, string[]> color = new KeyValuePair<Color, string[]>(Color.Transparent, null);
            int intensity = -1;

            string found = words.FirstOrDefault(x => ColorAction.ColorDict.FirstOrDefault(y => y.Value.Contains(x)).Value != null);
            if (!string.IsNullOrEmpty(found))
                color = ColorAction.ColorDict.FirstOrDefault(x => x.Value.Contains(found));

            if (Regex.IsMatch(text, @"(\d+)\ ?\%"))
            {
                MatchCollection matches = new Regex(@"(\d+)\ ?\%").Matches(text);
                intensity = Int32.Parse(matches[0].Groups[1].Value);
            }

            if (color.Value != null || intensity!= -1)
            {
                if (color.Value != null && intensity != -1)
                    yield return new ColorAction(ColorActionType.Color, new object[] { color.Key, intensity });
                else if (color.Value != null)
                    yield return new ColorAction(ColorActionType.Color, new object[] { color.Key });
                else if (intensity != 1)
                    yield return new ColorAction(ColorActionType.Intensity, intensity);
            }
            else
            {
                if (words.Any(x => x.StartsWith("allum")))
                    yield return new ColorAction(ColorActionType.Color, new object[] { Color.White });
                else if (words.Any(x => x.StartsWith("étein")))
                    yield return new ColorAction(ColorActionType.Color, new object[] { Color.Black });
            }

            if (words.Any(x => x.StartsWith("aléatoire")))
                yield return new ColorAction(ColorActionType.Random, null);
            if (Regex.IsMatch(text, @"vitesse (\d+)"))
            {
                MatchCollection matches = new Regex(@"vitesse (\d+)").Matches(text);
                yield return new ColorAction(ColorActionType.Speed, Int32.Parse(matches[0].Groups[1].Value));
            }
            if (words.Any(x => x.StartsWith("reconnaissance")))
                yield return new ColorAction(ColorActionType.SoundReco, words.Any(x => x.StartsWith("active")));
            if (text.Contains("arc-en-ciel") || text.Contains("toutes les couleurs"))
                yield return new ColorAction(ColorActionType.Rainbow);

            //if() le reste
        }
    }

    public class ColorAction
    {
        public static Dictionary<Color, string[]> ColorDict = new Dictionary<Color, string[]> {
            { Color.Lime, new string[] { "vert", "verre", "verres", "vers"} },
            { Color.Blue, new string[] { "bleu", "bleue", "bleus", "bleues" } },
            { Color.Red, new string[] { "rouge", "rouges"} },
            { Color.Yellow, new string[] { "jaune", "jaunes"} },
            { Color.Cyan, new string[] { "cyan", "cyans"} },
            { Color.Violet, new string[] { "violet", "violets"} },
            { Color.Orange, new string[] { "orange", "oranges"} },
            { Color.Pink, new string[] { "rose", "roses"} },
            { Color.White, new string[] { "blanc", "blancs"} },
            { Color.Indigo, new string[] { "indigo", "indigo"} }
        };

        public ColorActionType ActionType { get; set; }
        public object Detail { get; set; }

        public ColorAction(ColorActionType cat, object det = null)
        {
            this.ActionType = cat;
            this.Detail = det;
        }

        public override string ToString()
        {
            switch (this.ActionType)
            {
                case ColorActionType.Color:
                    Color col = (Color)((object[])this.Detail)[0];
                    return $"http://{Global.IP}/leds?red={(int)(col.R * 255)}&green={(int)(col.G * 255)}&blue={(int)(col.B * 255)}{(((object[])this.Detail).Length == 2 ? $"&int={(int)((object[])this.Detail)[1]}" : "")}";
                case ColorActionType.Intensity:
                    return $"http://{Global.IP}/intensity?int={this.Detail}";
                case ColorActionType.Speed:
                    return $"http://{Global.IP}/speed?speed={this.Detail}";
                case ColorActionType.Random:
                    return $"http://{Global.IP}/random";
                case ColorActionType.SoundReco:
                    return $"http://{Global.IP}/soundreco?reco={(((bool)this.Detail) ? 1 : 0)}";
                case ColorActionType.Rainbow:
                    return $"http://{Global.IP}/allcolors";
                default:
                    return null;
            }
        }
    }

    public enum ColorActionType
    {
        Color,
        Intensity,
        Speed,
        Random,
        SoundReco,
        Rainbow
    }

    public interface IVoiceRecognition
    {
        Task<string> LaunchRecognition();
    }
}
