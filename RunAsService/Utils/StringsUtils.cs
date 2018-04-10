using System.Text;

namespace RunAsService.Utils
{
    /// <summary>
    /// работа со строками
    /// </summary>
    public class StringsUtils
    {
        /// <summary>
        /// <para>Извлекает строку из кавычек.</para>
        /// <para>Если неправильно "закавычена" - просто возвращает исходную строку.</para>
        /// <para>Пример: Передали: s = 'строка''подстрока'''конец  -> Получили: строка'подстрока' и s = конец</para>
        /// <para>Кавычками считаются либой из символов строки, переданной в параметре quotes.</para>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="quotes"></param>
        /// <returns></returns>
        private static string DequotedStr(ref string s, string quotes)
        {
            var res = s.Trim();
            s = "";
            if (res.Length < 2 || quotes.IndexOf(res[0]) == -1) return res;
            var sb = new StringBuilder();
            var quote = res[0];
            var ss = res.Substring(1);
            var idx = 0;
            do
            {
                while (idx < ss.Length && ss[idx] != quote)
                {
                    sb.Append(ss[idx++]);
                } // Поиск кавычки
                // Если достигли конца цикла - конец (неверная строка в кавычках)
                if (idx >= ss.Length) return res;
                // Если следующий символ последний или не кавычка  - конец 
                if (idx + 1 >= ss.Length || ss[idx + 1] != quote)
                {
                    s = ss.Substring(idx + 1);
                    return sb.ToString();
                }
                // Следующий символ кавычка 
                sb.Append(quote);
                idx += 2;
            } while (idx < ss.Length);
            return res;
        }

        private static string GetPartStr(string s, char separator, out int pos, string strBrackets)
        {
            pos = 0;
            s = s.Trim();
            if (s == "") return "";
            var sb = new StringBuilder();
            do
            {
                if (strBrackets.IndexOf(s[pos]) != -1)
                {
                    var s1 = s.Substring(pos);
                    var ss = DequotedStr(ref s1, s[pos].ToString());
                    sb.Append(s[pos]).Append(ss).Append(s[pos]);
                    pos += ss.Length + 2;
                }
                else if (s[pos] != separator)
                    sb.Append(s[pos++]);
            } while (pos < s.Length && s[pos] != separator);
            return sb.ToString();
        }

        internal static string SeparateText(ref string s, char separator)
        {
            s = s.Trim();
            var res = GetPartStr(s, separator, out var i, "'\"");
            s = s.RightPart(s.Length - i - 1).Trim();
            return res.Trim();
        }
    }
}
